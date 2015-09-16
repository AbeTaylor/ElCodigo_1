using Sitecore.Data.Items;
using Sitecore.Security.AccessControl;
using Sitecore.Security.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElCodigo.CustomButtons.Helper
{
    public static class FavoritesHelper
    {
        public static void SetItemSecurity(Item favoriteItem)
        {
            var arc = new AccessRuleCollection();
            arc.Helper.AddAccessPermission(Sitecore.Context.User, AccessRight.ItemRead, PropagationType.Entity, AccessPermission.Allow);
            arc = CreateMultipleAccessRules(Role.FromName("sitecore\\Sitecore Client Users"), new AccessRight[] { AccessRight.ItemRead, AccessRight.ItemWrite, AccessRight.ItemRename, AccessRight.ItemDelete }, AccessPermission.Deny, arc);
            favoriteItem.Security.SetAccessRules(arc);
        }

        private static AccessRuleCollection CreateMultipleAccessRules(Account account, AccessRight[] rights, AccessPermission permission, AccessRuleCollection arc = null)
        {
            arc = arc ?? new AccessRuleCollection();

            foreach (AccessRight right in rights)
            {
                arc.Helper.AddAccessPermission(account, right, PropagationType.Entity, permission);
            }

            return arc;
        }
    }
}