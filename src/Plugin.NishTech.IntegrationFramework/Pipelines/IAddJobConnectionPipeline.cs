namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;

    /// <summary>
    /// Defines the IAddJobConnectionPipeline interface
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Plugin.NishTech.IntegrationFramework.IAddJobConnectionPipeline{Plugin.NishTech.IntegrationFramework.AddJobConnectionArgument,
    ///         Plugin.NishTech.IntegrationFramework.JobConnection, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>

    [PipelineDisplayName("JobScheduler.pipeline.addjobconnection")]
    public interface IAddJobConnectionPipeline : IPipeline<AddJobConnectionArgument, JobConnection, CommercePipelineExecutionContext>, IPipelineBlock<AddJobConnectionArgument, JobConnection, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
    }
}
