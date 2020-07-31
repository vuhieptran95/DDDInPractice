﻿using System.Threading.Tasks;

namespace ResponsibilityChain
{
    public readonly struct Nothing
    {
        
    }
    
    public interface IRequest<TResponse>
    {
        TResponse Response { get; set; }
    }
}