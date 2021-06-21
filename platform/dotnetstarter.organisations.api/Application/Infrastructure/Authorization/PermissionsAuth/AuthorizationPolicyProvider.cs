using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace dotnetstarter.organisations.api.Application.Infrastructure.Authorization.PermissionsAuth
{
    internal class AuthorizationPolicyProvider : IAuthorizationPolicyProvider
    {
        const string PERMISSIONSALL_POLICY = "PermissionsAll";
        const string PERMISSIONSANY_POLICY = "PermissionsAny";

        public DefaultAuthorizationPolicyProvider FallbackPolicyProvider { get; }

        public AuthorizationPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            // ASP.NET Core only uses one authorization policy provider, so if the custom implementation
            // doesn't handle all policies (including default policies, etc.) it should fall back to an
            // alternate provider.
            //
            // Here a default authorization policy provider (constructed with options from the 
            // dependency injection container) is used if this custom provider isn't able to handle a given
            // policy name.
            //
            // If a custom policy provider is able to handle all expected policy names then, of course, this
            // fallback pattern is unnecessary.
            FallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync() => FallbackPolicyProvider.GetDefaultPolicyAsync();

        // Policies are looked up by string name, so expect 'parameters'
        // to be embedded in the policy names. 
        public Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
        {
            if (policyName.StartsWith(PERMISSIONSALL_POLICY, StringComparison.OrdinalIgnoreCase) &&
                !String.IsNullOrEmpty(policyName.Substring(PERMISSIONSALL_POLICY.Length)))
            {
                var policy = new AuthorizationPolicyBuilder();
                var permissions = policyName.Substring(PERMISSIONSALL_POLICY.Length).Split('#');
                policy.AddRequirements(new PermissionAllRequirement(permissions));
                return Task.FromResult(policy.Build());
            }

            if (policyName.StartsWith(PERMISSIONSANY_POLICY, StringComparison.OrdinalIgnoreCase) &&
                !String.IsNullOrEmpty(policyName.Substring(PERMISSIONSANY_POLICY.Length)))
            {
                var policy = new AuthorizationPolicyBuilder();
                var permissions = policyName.Substring(PERMISSIONSANY_POLICY.Length).Split('#');
                policy.AddRequirements(new PermissionAnyRequirement(permissions));
                return Task.FromResult(policy.Build());
            }



            //TODO add other custom policies here

            // If the policy name doesn't match the format expected by this policy provider,
            // try the fallback provider. If no fallback provider is used, this would return 
            // Task.FromResult<AuthorizationPolicy>(null) instead.
            return FallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
