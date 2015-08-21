using Sitecore.Diagnostics;
using Sitecore.Shell.Applications.ContentManager;
using Sitecore.Web.UI.Sheer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Data.Items;
using Sitecore.Data;
using Sitecore.Globalization;

namespace ElCodigo.CustomButtons.Editor
{
    public class ContentEditor : ContentEditorForm
    {
        public override void HandleMessage(Message message)
        {
            Assert.ArgumentNotNull((object)message, "message");
            if (message["id"] == "ContentEditorDataContext")
            {
                Item item = this.ContentEditorDataContext.CurrentItem;
                if (item.TemplateID.ToString().Equals("{F52BCA1A-0A34-46DC-AE5F-B3BB97389F75}"))
                {
                    string itemId = item.Fields["FavoriteItem"].Value;
                    if (!String.IsNullOrWhiteSpace(itemId))
                    {
                        string load = String.Concat(new object[] { "item:load(id=", itemId, ",language=", Language.Parse(this.ContentEditorDataContext.Language.ToString()), ")" });
                        Sitecore.Context.ClientPage.SendMessage(this, load);
                        return;
                    }
                }
            }
            base.HandleMessage(message);
        }
    }
}