using ElCodigo.CustomButtons.Helper;
using Sitecore.Data.Events;
using Sitecore.Data.Items;
using Sitecore.Events;
using System;
using System.Linq;

namespace ElCodigo.CustomButtons.Events
{
    public class CreateFavoriteFolderProcessor
    {
        protected void OnFavoriteFolderCreated(object sender, EventArgs args)
        {
            if (args == null)
                return;
            SitecoreEventArgs scea = args as SitecoreEventArgs;
            if (scea.Parameters == null || scea.Parameters.Count() <= 0)
                return;
            ItemCreatedEventArgs icea = scea.Parameters[0] as ItemCreatedEventArgs;
            if (icea == null)
                return;
            Item obj = icea.Item;
            if (obj == null)
                return;
            SetFolderSecurity(obj);
        }

        protected void SetFolderSecurity(Item favoriteFolder)
        {
            if (favoriteFolder.TemplateID.ToString().Equals("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}") && favoriteFolder.Axes.GetAncestors().Any(a => a.ID.ToString().Equals("{871D33CC-00C7-4CAA-A293-6D81B0D2C0AD}")))
            {
                FavoritesHelper.SetItemSecurity(favoriteFolder);
            }
        }
    }
}