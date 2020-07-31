﻿using ResponsibilityChain;

 namespace DDDInPractice.Persistence.Infrastructure.Requests.RequestContexts
{
    public interface IRequiredRequestContext
    {
        IRequestContext RequestContext { get; set; }
    }

    public interface IRequiredAuthorization
    {
        bool AuthorizationIsHandled { get; set; }
    }
    
    public abstract class Request<TResponse> : IRequest<TResponse>, IRequiredRequestContext, IRequiredAuthorization
    {
        public TResponse Response { get; set; }
        public IRequestContext RequestContext { get; set; }
        public bool AuthorizationIsHandled { get; set; }
    }
}