using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.InsertRenderings.Processors;
using Sitecore.Security.AccessControl;
using Sitecore.Security.Accounts;
using Sitecore.Shell;
using Sitecore.Shell.Framework.Commands;
using System.Linq;

namespace ElCodigo.CustomButtons.Favorites
{
    public class AddToFavorites : Command
    {
        private string _favoriteTemplateId = "{F52BCA1A-0A34-46DC-AE5F-B3BB97389F75}";
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

            //TODO break down code into executable chunks
            //Get the context Item
            Role everyone = Role.FromName("sitecore\\Sitecore Client Users");
            var item = context.Items[0];
            User curUser = User.Current;
            var FavoriteContainer = item.Database.GetItem("{871D33CC-00C7-4CAA-A293-6D81B0D2C0AD}");
                     
            if (InFavoritesItem(FavoriteContainer, item.ID.ToString())) return;

            var newFavoriteItem = FavoriteContainer.Add(item.Name, new TemplateID(new ID(_favoriteTemplateId)));
            newFavoriteItem.Editing.BeginEdit();
            newFavoriteItem.Fields["FavoriteItem"].Value = item.ID.ToString();
            newFavoriteItem.Appearance.Icon = item.Appearance.Icon;            
            AccessRuleCollection accessRules = newFavoriteItem.Security.GetAccessRules();
            accessRules.Helper.AddAccessPermission(
                curUser, 
                AccessRight.ItemRead, 
                PropagationType.Any, 
                AccessPermission.Allow);
            accessRules.Helper.AddAccessPermission(
                everyone,
                AccessRight.ItemRead,
                PropagationType.Any,
                AccessPermission.Deny
                );
            newFavoriteItem.Security.SetAccessRules(accessRules);
            
            newFavoriteItem.Editing.EndEdit();

            //Ignore for now
            //var favorites = UserOptions.Favorites.Root;

            ////Get User, Set favorite item, save user profile
            //User user = Sitecore.Context.User;
            //user.Profile.SetCustomProperty("UserFavorite", item.ID.ToString());
            //user.Profile.Save();
        }

        public override CommandState QueryState(CommandContext context)
        {
            if (context == null || context.Items == null || context.Items.Length == 0)
            {
                return CommandState.Disabled;
            }

            var item = context.Items[0];

            if (item.Database.Name.ToLower().Equals("web") || item.Database.Name.ToLower().Equals("core") || item.ID.ToString().Equals("{871D33CC-00C7-4CAA-A293-6D81B0D2C0AD}") || item.Axes.GetAncestors().Any(x => x.ID.ToString().Equals("{871D33CC-00C7-4CAA-A293-6D81B0D2C0AD}")))
            {
                return CommandState.Hidden;
            }

            var FavoriteContainer = item.Database.GetItem("{871D33CC-00C7-4CAA-A293-6D81B0D2C0AD}");

            if (FavoriteContainer == null)
            {
                return CommandState.Hidden;
            }
            
            if (InFavoritesItem(FavoriteContainer, item.ID.ToString()))
            {
                return CommandState.Disabled;
            }
            
            
            return CommandState.Enabled;
        }

        private bool InFavoritesItem(Item FavoriteContainer, string itemId)
        {
            return FavoriteContainer.Axes.GetDescendants().Where(x => x.Security.CanRead(User.Current) && x.TemplateID.Equals(new ID(_favoriteTemplateId))).Any(y => y.Fields[_favoriteItemField].Value.Equals(itemId));
        }
    }
}