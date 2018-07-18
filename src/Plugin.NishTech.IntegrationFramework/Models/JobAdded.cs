using Sitecore.Commerce.Core;

namespace Plugin.NishTech.IntegrationFramework
{
    public class JobAdded : Model
    {
        public JobAdded(string friendlyId)
        {
            this.JobFriendlyId = friendlyId;
        }

        public string JobFriendlyId { get; set; }
    }

}
