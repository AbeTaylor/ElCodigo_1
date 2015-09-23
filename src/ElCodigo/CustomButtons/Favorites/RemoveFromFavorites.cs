using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using Sitecore.Shell.Framework.Commands;
using System.Linq;

namespace ElCodigo.CustomButtons.Favorites
{
    public class RemoveFromFavorites : Command
    {
        private ID _favoritesContainerId = new ID("{871D33CC-00C7-4CAA-A293-6D81B0D2C0AD}");
        private string _favoriteTemplateId = "{F52BCA1A-0A34-46DC-AE5F-B3BB97389F75}";
        private string _favoriteItemField = "FavoriteItem";
        private string[] _unallowedDbs = new string[] { "core", "web" };

        public override void Execute(CommandContext context)
        {
            if (context == null || context.Items == null || context.Items.Length == 0)
            {
                return;
            }

            var item = context.Items[0];

            Item favoriteItem = item.Database.GetItem(_favoritesContainerId).Children.First(c => c.Name.Equals(User.Current.Name.Replace("\\", "_"))).Axes.GetDescendants().First(c => c.Security.CanRead(Sitecore.Context.User) && c.Fields[_favoriteItemField].Value.Equals(item.ID.ToString()));

            if (favoriteItem == null)
            {
                favoriteItem.Delete();
            }
        }

        public override CommandState QueryState(CommandContext context)
        {
            if (context == null || context.Items == null || context.Items.Length == 0)
            {
                return CommandState.Disabled;
            }

            var item = context.Items[0];

            if (_unallowedDbs.Any(db => item.Database.Name.ToLower().Contains(db)))
            {
                return CommandState.Disabled;
            }

            var favoritesContainer = item.Database.GetItem(_favoritesContainerId);

            if (favoritesContainer == null)
            {
                return CommandState.Disabled;
            }

            Item userFolder;

            var hasUserFolder = favoritesContainer.Children.Any(c => c.Name.Equals(User.Current.Name.Replace("\\", "_")));

            if (!hasUserFolder)
            {
                return CommandState.Disabled;
            }
            else
            {
                userFolder = favoritesContainer.Children.First(c => c.Name.Equals(User.Current.Name.Replace("\\", "_")));
            }

            if (FavoritesItemExists(userFolder, item.ID.ToString()))
            {
                return CommandState.Enabled;
            }

            return CommandState.Disabled;
        }

        public bool FavoritesItemExists(Item favoriteContainer, string itemId)
        {
            return favoriteContainer.Axes.GetDescendants().Where(x => x.Security.CanRead(User.Current) && x.TemplateID.Equals(new ID(_favoriteTemplateId))).Any(y => y.Fields[_favoriteItemField].Value.Equals(itemId));
        }
    }
}