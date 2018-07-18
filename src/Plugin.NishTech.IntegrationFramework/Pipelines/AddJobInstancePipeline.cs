namespace Plugin.NishTech.IntegrationFramework
{
    using Microsoft.Extensions.Logging;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;

    /// <inheritdoc cref="" />
    /// <summary>
    ///  Defines the AddJobInstancePipeline pipeline.
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Sitecore.Commerce.Core.CommercePipeline{Plugin.NishTech.IntegrationFramework.AddJobInstanceArgument,
    ///         Plugin.NishTech.IntegrationFramework.JobInstance}
    ///     </cref>
    /// </seealso>
    /// <seealso cref="T:Plugin.NishTech.IntegrationFramework.IAddJobInstancePipeline" />
    public class AddJobInstancePipeline : CommercePipeline<AddJobInstanceArgument, JobInstance>, IAddJobInstancePipeline
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Plugin.NishTech.IntegrationFramework.AddJobInstancePipeline" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public AddJobInstancePipeline(IPipelineConfiguration<IAddJobInstancePipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}

