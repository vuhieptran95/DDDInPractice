﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ResponsibilityChain
{
    public interface IHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        Task HandleAsync(TRequest request);
    }

    public interface IPreHandler<TRequest, TResponse> : IHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        
    }
    
    public interface IPostHandler<TRequest, TResponse> : IHandler<TRequest, TResponse>
        where TRequest : IRequest<TResponse>
    {
        
    }

    public class Handler<TRequest, TResponse> : IHandler<TRequest, TResponse> where TRequest : IRequest<TResponse>
    {
        protected readonly List<IPreHandler<TRequest, TResponse>> ListPreHandlers = new List<IPreHandler<TRequest, TResponse>>();
        protected readonly List<IPostHandler<TRequest, TResponse>> ListPostHandlers = new List<IPostHandler<TRequest, TResponse>>();
        public virtual async Task HandleAsync(TRequest request)
        {
            if (request == null)
            {
                throw new NullReferenceException("Cannot handle null request");
            }
            
            if (Next == null)
            {
                throw new NullHandlerException($"The next handler of {this.GetType()} is null");
            }

            // Console.WriteLine($"Type of Next is {Next.GetType()}");
            foreach (var preHandler in ListPreHandlers)
            {
                await preHandler.HandleAsync(request);
            }
            
            await Next.HandleAsync(request);
            
            foreach (var postHandler in ListPostHandlers)
            {
                await postHandler.HandleAsync(request);
            }
        }

        public void AddHandler(IPreHandler<TRequest, TResponse> handler)
        {
            ListPreHandlers.Add(handler);
        }
        public void AddHandler(IPostHandler<TRequest, TResponse> handler)
        {
            ListPostHandlers.Add(handler);
        }
        public void AddHandler(Handler<TRequest, TResponse> handler)
        {
            Next = AddHandler(Next, handler);
        }

        Handler<TRequest, TResponse> AddHandler(Handler<TRequest, TResponse> handler, Handler<TRequest, TResponse> handlerToAdd)
        {
            if (handlerToAdd == null)
            {
                throw new NullHandlerException("Cannot add null handler.");
            }
            
            if (handler == null)
            {
                handler = handlerToAdd;
            }
            else
            {
                var temp = handler;
                while (temp.Next != null)
                {
                    temp = temp.Next;
                }

                temp.Next = handlerToAdd;
            }

            return handler;
        }

        public Handler<TRequest, TResponse> Next { get; set; }
    }
}