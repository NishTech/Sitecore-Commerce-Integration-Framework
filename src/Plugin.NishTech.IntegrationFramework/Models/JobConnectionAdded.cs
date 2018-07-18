using Sitecore.Commerce.Core;

namespace Plugin.NishTech.IntegrationFramework
{
    public class JobConnectionAdded : Model
    {
        public JobConnectionAdded(string bookFriendlyId)
        {
            this.JobConnectionFriendlyId = bookFriendlyId;
        }

        public string JobConnectionFriendlyId { get; set; }
    }

}
