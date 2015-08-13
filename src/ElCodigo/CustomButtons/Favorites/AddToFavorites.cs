using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
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

            //Ignore for now
            var favorites = UserOptions.Favorites.Root;

            //Get User, Set favorite item, save user profile
            User user = Sitecore.Context.User;
            user.Profile.SetCustomProperty("UserFavorite", item.ID.ToString());
            user.Profile.Save();
        }
    }
}