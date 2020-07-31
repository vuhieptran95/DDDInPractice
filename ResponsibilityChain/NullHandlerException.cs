﻿using System;

namespace ResponsibilityChain
{
    public class NullHandlerException : Exception
    {
        public NullHandlerException(string? message) : base(message)
        {
        }
    }
}