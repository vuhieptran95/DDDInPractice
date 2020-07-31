﻿using System.Collections.Generic;

 namespace DDDInPractice.Persistence.Infrastructure.Requests.RequestContexts
{
    public interface IRequestContext
    {
        string UserId { get; set; }
        string Username { get; set; }
        string UserRole { get; set; }
        string UserEmail { get; set; }
        Dictionary<string, object> TempData { get; }
        List<string> AccessRights { get; set; }
    }

    public class RequestContext : IRequestContext
    {
        public RequestContext()
        {
            TempData = new Dictionary<string, object>();
        }
        public string UserId { get; set; } = "anonymous";
        public string Username { get; set; } = "anonymous";
        public string UserRole { get; set; } = "anonymous";
        public string UserEmail { get; set; } = "anonymous";
        public Dictionary<string, object> TempData { get; }
        public List<string> AccessRights { get; set; } = new List<string>();
    }
}