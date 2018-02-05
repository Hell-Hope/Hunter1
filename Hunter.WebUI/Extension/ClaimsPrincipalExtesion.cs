using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hunter
{
    public static class ClaimsPrincipalExtesion
    {
        public static Models.ApplicationUser GetApplicationUser(this System.Security.Claims.ClaimsPrincipal claimsPrincipal)
        {
            return GetApplicationUser(claimsPrincipal?.Claims?.ToList());
        }

        public static Models.ApplicationUser GetApplicationUser(IEnumerable<System.Security.Claims.Claim> claims)
        {
            if (claims == null)
                return null;
            var result = new Models.ApplicationUser();
            foreach (var item in claims.ToList())
            {
                if (item.Type == nameof(Models.ApplicationUser.ID))
                    result.ID = item.Value;
                else if (item.Type == nameof(Models.ApplicationUser.Account))
                    result.Account = item.Value;
                else if (item.Type == nameof(Models.ApplicationUser.Name))
                    result.Name = item.Value;
            }
            return result;
        }

    }
}
