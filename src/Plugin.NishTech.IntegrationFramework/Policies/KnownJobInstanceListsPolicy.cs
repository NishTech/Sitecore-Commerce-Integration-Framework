namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;
    public class KnownJobInstanceListsPolicy : Policy
    {
        public KnownJobInstanceListsPolicy()
        {
            this.QueuedJobs = nameof(QueuedJobs);
            this.SuccessJobs = nameof(SuccessJobs);
            this.FailedJobs = nameof(FailedJobs);
        }

        public string QueuedJobs { get; set; }
        public string SuccessJobs { get; set; }
        public string FailedJobs { get; set; }
    }
}
