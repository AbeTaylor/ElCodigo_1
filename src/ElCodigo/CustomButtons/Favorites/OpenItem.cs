using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using Sitecore.Shell.Framework;
using Sitecore.Shell.Framework.Commands;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Specialized;

namespace ElCodigo.CustomButtons.Favorites
{
    public class OpenItem : Sitecore.Shell.Framework.Commands.ContentEditor.OpenItem
    {
        public override void Execute(CommandContext context)
        {
            Assert.IsNotNull((object)context, "context");
            Assert.IsNotNull((object)context.Items, "items");
            if (context.Items.Length < 1)
                return;
            Item obj = context.Items[0];

            if (!obj.TemplateID.ToString().Equals("{F52BCA1A-0A34-46DC-AE5F-B3BB97389F75}"))
                base.Execute(context);

            NameValueCollection parameters = new NameValueCollection();
            parameters["newItem"] = obj.Fields["FavoriteItem"].Value.ToString();
            parameters["ItemID"] = obj.ID.ToString();
            Context.ClientPage.Start((object)this, "Run", parameters);
        }

        protected void Run(ClientPipelineArgs args)
        {
            Assert.ArgumentNotNull((object)args, "args");
            if (args.IsPostBack)
            {
                if (!args.HasResult)
                    return;
                Context.ClientPage.SendMessage((object)this, "item:load(id=" + args.Result + ")");
            }
            else
            {
                string str = args.Parameters["ItemID"];
                if (str == null)
                    return;
                Dialogs.BrowseItem("Open Item.", string.Empty, "Core3/16x16/open_document.png", "Open", "/", str + "/");
                args.WaitForPostBack();
            }
        }
    }
}