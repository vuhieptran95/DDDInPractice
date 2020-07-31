﻿#nullable enable
 using System;

 namespace DDDInPractice.Persistence.Infrastructure.Requests.Authorizations
{
    public class AuthorizationConfigException : Exception
    {
        public AuthorizationConfigException(string? message) : base(message)
        {
            
        }
    }
}