namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;
    /// <summary>
    /// Defines the IAddJobPipeline interface
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Plugin.NishTech.IntegrationFramework.IAddJobPipeline{Plugin.NishTech.IntegrationFramework.AddJobArgument,
    ///         Plugin.NishTech.IntegrationFramework.Job, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>

    [PipelineDisplayName("JobScheduler.pipeline.addjob")]
    public interface IAddJobPipeline : IPipeline<AddJobArgument, Job, CommercePipelineExecutionContext>
    {
    }
}
