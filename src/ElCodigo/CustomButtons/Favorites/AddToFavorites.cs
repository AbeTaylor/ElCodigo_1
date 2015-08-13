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
        public override void Execute(CommandContext context)
        {
            if (context == null || context.Items == null || context.Items.Length == 0)
            {
                return;
            }
            var item = context.Items[0];

            var favorites = UserOptions.Favorites.Root;

            User user = Sitecore.Context.User;
            user.Profile.SetCustomProperty("UserFavorite", item.ID.ToString());
            user.Profile.Save();
        }
    }
}