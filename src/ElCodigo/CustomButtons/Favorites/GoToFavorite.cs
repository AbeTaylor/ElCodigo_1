using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using Sitecore.Shell.Framework.Commands;
using System;

namespace ElCodigo.CustomButtons.Favorites
{
    public class GoToFavorite : Command
    {
        public override void Execute(CommandContext context)
        {
            if (context == null || context.Items == null || context.Items.Length == 0)
            {
                return;
            }
            var item = context.Items[0];

            User user = Sitecore.Context.User;
            Item myItem = item.Database.GetItem(new ID(user.Profile.GetCustomProperty("UserFavorite")));
            string load = String.Concat(new object[] { "item:load(id=", myItem.ID, ",language=", myItem.Language, ",version=", myItem.Version, ")" });
            Sitecore.Context.ClientPage.SendMessage(this, load);
        }
    }
}