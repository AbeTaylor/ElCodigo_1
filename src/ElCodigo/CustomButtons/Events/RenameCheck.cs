using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore.Security.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElCodigo.CustomButtons.Events
{
    public class RenameCheck
    {
        private string _favoriteTemplateId = "{F52BCA1A-0A34-46DC-AE5F-B3BB97389F75}";
        private string _favoriteItemField = "FavoriteItem";

        protected void OnRename(object sender, EventArgs args)
        {
            if (args == null)
                return;
            SitecoreEventArgs scea = args as SitecoreEventArgs;
            if (scea.Parameters == null || scea.Parameters.Count() <= 0)
                return;
            if (scea.Parameters[0] == null || scea.Parameters[1] == null)
                return;
            Item renamedItem = scea.Parameters[0] as Item;
            string oldName = scea.Parameters[1] as string;
            var FavoriteContainer = renamedItem.Database.GetItem("{871D33CC-00C7-4CAA-A293-6D81B0D2C0AD}");
            if (InFavoritesItem(FavoriteContainer, renamedItem.ID.ToString()))
            {
                Item favItem = FavoriteContainer.Axes.GetDescendants().Where(x => x.Security.CanRead(User.Current) && x.TemplateID.Equals(new ID(_favoriteTemplateId))).First(y => y.Fields[_favoriteItemField].Value.Equals(renamedItem.ID.ToString()));
                if (favItem.Name.Equals(oldName))
                {
                    favItem.Editing.BeginEdit();
                    favItem.Name = renamedItem.Name;
                    favItem.Editing.EndEdit();
                }
            }
        }

        private bool InFavoritesItem(Item FavoriteContainer, string itemId)
        {
            return FavoriteContainer.Axes.GetDescendants().Where(x => x.Security.CanRead(User.Current) && x.TemplateID.Equals(new ID(_favoriteTemplateId))).Any(y => y.Fields[_favoriteItemField].Value.Equals(itemId));
        }
    }
}