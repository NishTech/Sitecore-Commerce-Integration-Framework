namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;

    public class KnownJobSchedulerActionsPolicy : Policy
    {
        public KnownJobSchedulerActionsPolicy()
        {
            this.AddJobConnection = nameof(AddJobConnection);
            this.AddJob = nameof(AddJob);
            this.AddJobInstance = nameof(AddJobInstance);
        }

        public string AddJobConnection { get; set; }
        public string AddJob { get; set; }
        public string AddJobInstance { get; set; }
        public string PaginateJobConnections { get; set; } = nameof(PaginateJobConnections);
    }
}
