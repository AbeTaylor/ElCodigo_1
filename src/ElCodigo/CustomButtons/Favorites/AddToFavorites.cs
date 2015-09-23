using ElCodigo.CustomButtons.Helper;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Security.Accounts;
using Sitecore.Shell.Framework.Commands;
using System;
using System.Linq;

namespace ElCodigo.CustomButtons.Favorites
{
    public class AddToFavorites : Command
    {
        private string _favoriteContainerId = "{871D33CC-00C7-4CAA-A293-6D81B0D2C0AD}";
        private string _favoriteTemplateId = "{F52BCA1A-0A34-46DC-AE5F-B3BB97389F75}";
        private string _folderTemplateId = "{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}";
        private string _favoriteItemField = "FavoriteItem";

        /// <summary>
        /// Method called on click of the command (MUST EXIST if inheriting from command)
        /// </summary>
        /// <param name="context">Context that the command is running on</param>
        public override void Execute(CommandContext context)
        {
            //Make sure the context isnt null, and that there are context items
            if (context == null || context.Items == null || context.Items.Length == 0)
            {
                return;
            }

            CreateFavoriteItem(context.Items[0]);
        }

        public override CommandState QueryState(CommandContext context)
        {
            if (context == null || context.Items == null || context.Items.Length == 0)
            {
                return CommandState.Disabled;
            }

            var item = context.Items[0];

            if (item.Database.Name.ToLower().Equals("web") || item.Database.Name.ToLower().Equals("core") || item.ID.ToString().Equals(_favoriteContainerId) || item.Axes.GetAncestors().Any(x => x.ID.ToString().Equals(_favoriteContainerId)))
            {
                return CommandState.Hidden;
            }

            var favoriteContainer = item.Database.GetItem(_favoriteContainerId);

            if (favoriteContainer == null)
            {
                return CommandState.Hidden;
            }

            if (FavoritesItemExists(favoriteContainer, item.ID.ToString()))
            {
                return CommandState.Disabled;
            }


            return CommandState.Enabled;
        }

        public bool FavoritesItemExists(Item favoriteContainer, string itemId)
        {
            return favoriteContainer.Axes.GetDescendants().Where(x => x.Security.CanRead(User.Current) && x.TemplateID.Equals(new ID(_favoriteTemplateId))).Any(y => y.Fields[_favoriteItemField].Value.Equals(itemId));
        }

        private void CreateFavoriteItem(Item contextItem)
        {
            var favoriteContainer = contextItem.Database.GetItem(_favoriteContainerId);
            Item userFolder;

            var hasUserFolder = favoriteContainer.Children.Any(c => c.Name.Equals(User.Current.Name.Replace("\\", "_")));

            if (!hasUserFolder)
            {
                userFolder = CreateUserFolder(favoriteContainer);
            }
            else
            {
                userFolder = favoriteContainer.Children.First(c => c.Name.Equals(User.Current.Name.Replace("\\", "_")));
            }

            var favoriteItem = userFolder.Add(contextItem.Name, new TemplateID(new ID(_favoriteTemplateId)));
            favoriteItem.Editing.BeginEdit();
            favoriteItem.Fields[_favoriteItemField].Value = contextItem.ID.ToString();
            favoriteItem.Appearance.Icon = contextItem.Appearance.Icon;
            FavoritesHelper.SetItemSecurity(favoriteItem);
            favoriteItem.Editing.EndEdit();
        }

        private Item CreateUserFolder(Item favoriteContainer)
        {
            var folder = favoriteContainer.Add(User.Current.Name.Replace("\\","_"), new TemplateID(new ID(_folderTemplateId)));
            FavoritesHelper.SetItemSecurity(folder);
            return folder;
        }
    }
}