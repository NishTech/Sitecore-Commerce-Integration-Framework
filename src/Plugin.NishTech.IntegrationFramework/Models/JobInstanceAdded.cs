namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;

    public class JobInstanceAdded : Model
    {
        public JobInstanceAdded(string friendlyId)
        {
            this.JobInstanceFriendlyId = friendlyId;
        }

        public string JobInstanceFriendlyId { get; set; }
    }

}
