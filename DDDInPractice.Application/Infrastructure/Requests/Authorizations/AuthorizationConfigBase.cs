using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DDDInPractice.Persistence.Infrastructure.Requests.RequestContexts;
using ResponsibilityChain;

namespace DDDInPractice.Persistence.Infrastructure.Requests.Authorizations
{
    public class AuthorizationConfigBase<TRequest, TResponse> : Handler<TRequest, TResponse>
        where TRequest : Request<TResponse>
    {
        private readonly IAuthorizationConfig<TRequest> _config;

        public AuthorizationConfigBase(RequestContext requestContext, IAuthorizationConfig<TRequest> config)
        {
            _config = config;
            // AccessRights.Add(("item1 item2", "read create"));
            // AccessRights.Add(("item3 item2", "edit"));
            // AccessRights.Add(("item4", "*"));
        }

        public override Task HandleAsync(TRequest request)
        {
            var accessRights = CalculateRights(_config.GetAccessRights());

            var violatedRights = accessRights.Except(request.RequestContext.AccessRights).ToList();

            if (violatedRights.Count > 0)
            {
                var violated = string.Join(' ', violatedRights);
                throw new UnauthorizedAccessException(
                    $"{violated} right(s) are not permitted for this user: {request.RequestContext.UserEmail}");
            }

            return base.HandleAsync(request);
        }

        private List<string> CalculateRights(List<(string[] Resources, string[] Actions)> rights)
        {
            var listRight = new List<string>();
            if (rights == null)
            {
                return listRight;
            }

            foreach (var right in rights)
            {
                var resources = right.Resources.Where(i => !string.IsNullOrWhiteSpace(i)).Distinct().ToList();
                var actions = right.Actions.Where(i => !string.IsNullOrWhiteSpace(i)).Distinct().ToList();
                if (resources.Contains("*"))
                {
                    resources = new List<string> {"read", "create", "delete", "update"};
                }

                var cartesianProducts = actions
                    .SelectMany(action => resources, (action, resource) => resource + ":" + action);

                listRight.AddRange(cartesianProducts);
            }

            return listRight;
        }
    }

}