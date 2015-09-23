using Sitecore;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.ExperienceEditor.Speak.Server.Contexts;
using Sitecore.ExperienceEditor.Speak.Server.Requests;
using Sitecore.ExperienceEditor.Speak.Server.Responses;
using Sitecore.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ElCodigo.CustomButtons.Pipeline
{
    public class GetFavoriteItem : PipelineProcessorRequest<ValueItemContext>
    {
        public override PipelineProcessorResponseValue ProcessRequest()
        {
            string path = this.RequestContext.Value;
            Assert.IsNotNullOrEmpty(path, "The request value is null or empty.");
            Database database = Factory.GetDatabase(this.RequestContext.Database);
            Assert.IsNotNull((object)database, "The database is null.");
            Item obj = database.GetItem(path);
            Assert.IsNotNull((object)obj, "The item is null.");

            string favItemId = obj.Fields["FavoriteItem"].Value;

            return new PipelineProcessorResponseValue()
            {
                Value = (object) favItemId
            };
        }
    }
}