namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;

    [PipelineDisplayName("JobScheduler.pipeline.jobscheduler")]
    public interface IJobSchedulerPipeline : IPipeline<JobSchedulerArgument, JobInfoArgument, CommercePipelineExecutionContext>
    {
    }
}
