using System.Collections.Generic;

namespace DDDInPractice.Persistence.Infrastructure.Requests.Authorizations
{
    public interface IAuthorizationConfig<TRequest>
    {
        List<(string[] Resources, string[] Actions)> GetAccessRights();
    }
    
    public class DefaultAuthorizationConfig<TRequest> : IAuthorizationConfig<TRequest>
    {
        public List<(string[] Resources, string[] Actions)> GetAccessRights()
        {
            return new List<(string[] Resources, string[] Actions)>();
        }
    }
}