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

            //Get User, get favorite item from the DB, create load string, send load message to UI
            User user = Sitecore.Context.User;
            Item myItem = item.Database.GetItem(new ID(user.Profile.GetCustomProperty("UserFavorite")));
            string load = String.Concat(new object[] { "item:load(id=", myItem.ID, ",language=", myItem.Language, ",version=", myItem.Version, ")" });
            Sitecore.Context.ClientPage.SendMessage(this, load);
        }
    }
}