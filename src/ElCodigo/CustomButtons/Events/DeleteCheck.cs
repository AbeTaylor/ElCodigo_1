using ElCodigo.CustomButtons.Helper;
using Sitecore.Data;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using Sitecore.Events;
using Sitecore.Security.Accounts;
using System;
using System.Linq;

namespace ElCodigo.CustomButtons.Events
{
    public class DeleteCheck
    {

        private string _favoriteTemplateId = "{F52BCA1A-0A34-46DC-AE5F-B3BB97389F75}";
        private string _favoriteItemField = "FavoriteItem";

        protected void OnDelete(object sender, EventArgs args)
        {
            if (args == null)
                return;
            SitecoreEventArgs scea = args as SitecoreEventArgs;
            if (scea.Parameters == null || scea.Parameters.Count() <= 0)
                return;
            Item obj = scea.Parameters[0] as Item;
            if (obj == null)
                return;
            var FavoriteContainer = obj.Database.GetItem("{871D33CC-00C7-4CAA-A293-6D81B0D2C0AD}");
            if (InFavoritesItem(FavoriteContainer, obj.ID.ToString()))
            {
                deleteFromFavorites(FavoriteContainer, obj.ID.ToString());
            }
            
        }

        private bool InFavoritesItem(Item FavoriteContainer, string itemId)
        {
            return FavoriteContainer.Axes.GetDescendants().Where(x => x.Security.CanRead(User.Current) && x.TemplateID.Equals(new ID(_favoriteTemplateId))).Any(y => y.Fields[_favoriteItemField].Value.Equals(itemId));
        }

        private void deleteFromFavorites(Item FavoriteContainer, string itemId)
        {
            FavoriteContainer.Axes.GetDescendants().Where(x => x.Security.CanRead(User.Current) && x.TemplateID.Equals(new ID(_favoriteTemplateId))).Where(y => y.Fields[_favoriteItemField].Value.Equals(itemId)).First().Delete();
        }

        
    }
}