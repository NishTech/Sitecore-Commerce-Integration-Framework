namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;

    /// <summary>
    /// Defines the ISamplePipeline interface
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Plugin.NishTech.IntegrationFramework.IAddJobInstancePipeline{Plugin.NishTech.IntegrationFramework.AddJobInstanceArgument,
    ///         Plugin.NishTech.IntegrationFramework.JobInstance, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>

    [PipelineDisplayName("JobScheduler.pipeline.addjobinstance")]
    public interface IAddJobInstancePipeline : IPipeline<AddJobInstanceArgument, JobInstance, CommercePipelineExecutionContext>
    {
    }
}
