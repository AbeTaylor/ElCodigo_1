using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.InsertRenderings.Processors;
using Sitecore.Security.Accounts;
using Sitecore.Shell;
using Sitecore.Shell.Framework.Commands;

namespace ElCodigo.CustomButtons.Favorites
{
    public class AddToFavorites : Command
    {
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

            //Get the context Item
            var item = context.Items[0];
            var FavoriteContainer = item.Database.GetItem("{871D33CC-00C7-4CAA-A293-6D81B0D2C0AD}");
            var newFavoriteItem = FavoriteContainer.Add(item.Name, new TemplateID(new ID("{F52BCA1A-0A34-46DC-AE5F-B3BB97389F75}")));
            newFavoriteItem.Editing.BeginEdit();
            newFavoriteItem.Fields["FavoriteItem"].Value = item.ID.ToString();
            newFavoriteItem.Editing.EndEdit();

            //Ignore for now
            //var favorites = UserOptions.Favorites.Root;

            ////Get User, Set favorite item, save user profile
            //User user = Sitecore.Context.User;
            //user.Profile.SetCustomProperty("UserFavorite", item.ID.ToString());
            //user.Profile.Save();
        }
    }
}