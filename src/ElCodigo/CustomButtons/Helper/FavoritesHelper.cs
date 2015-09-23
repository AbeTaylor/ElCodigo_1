using Sitecore.Data.Items;
using Sitecore.Security.AccessControl;
using Sitecore.Security.Accounts;


namespace ElCodigo.CustomButtons.Helper
{
    public static class FavoritesHelper
    {
        public static void SetItemSecurity(Item favoriteItem)
        {
            var arc = new AccessRuleCollection();
            arc = CreateMultipleAccessRules(Sitecore.Context.User, new AccessRight[] { AccessRight.ItemRead, AccessRight.ItemWrite, AccessRight.ItemRename, AccessRight.ItemDelete, AccessRight.ItemCreate }, AccessPermission.Allow, arc);
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