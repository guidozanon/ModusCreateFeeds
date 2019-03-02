using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using ModusCreate.Core.Models;

namespace ModusCreate.Web
{
    public class ODataRegistry
    {
        public static IEdmModel GetEdmModel()
        {
            var builder = new ODataConventionModelBuilder();
            builder.EntitySet<Feed>("Feeds")
                .EntityType
                .HasKey(x=>x.Id)
                .HasMany(x=>x.News)
                .Expand();

            builder.EntitySet<News>("News")
                .EntityType
                .Expand();

            builder.EntitySet<News>("MyNews")
                .EntityType
                .Expand();

            builder.EntitySet<FeedCategory>("Categories")
               .EntityType
               .Expand();

            return builder.GetEdmModel();
        }
    }
}
