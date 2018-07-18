namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;

    public class JobInfoArgument : PipelineArgument
    {
        public JobInfoArgument(JobConnection jobConnection, Job job, JobInstance jobInstance)
        {
            this.JobConnection = jobConnection;
            this.Job = job;
            this.JobInstance = jobInstance;
        }

        public JobConnection JobConnection { get; set; }
        public Job Job { get; set; }
        public JobInstance JobInstance { get; set; }
    }
}
