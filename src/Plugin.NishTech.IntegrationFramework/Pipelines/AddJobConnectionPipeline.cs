namespace Plugin.NishTech.IntegrationFramework
{
    using Microsoft.Extensions.Logging;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;

    /// <inheritdoc cref="" />
    /// <summary>
    ///  Defines the AddJobConnectionPipeline pipeline.
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Plugin.NishTech.IntegrationFramework.AddJobConnectionPipeline{Plugin.NishTech.IntegrationFramework.AddJobConnectionArgument,
    ///         Plugin.NishTech.IntegrationFramework.JobConnection}
    ///     </cref>
    /// </seealso>
    /// <seealso cref="T:Plugin.NishTech.IntegrationFramework.ISamplePipeline" />
    public class AddJobConnectionPipeline : CommercePipeline<AddJobConnectionArgument, JobConnection>, IAddJobConnectionPipeline, IPipeline<AddJobConnectionArgument, JobConnection, CommercePipelineExecutionContext>, IPipelineBlock<AddJobConnectionArgument, JobConnection, CommercePipelineExecutionContext>, IPipelineBlock, IPipeline
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Plugin.NishTech.IntegrationFramework.AddJobConnectionPipeline" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public AddJobConnectionPipeline(IPipelineConfiguration<IAddJobConnectionPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}

