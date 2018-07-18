namespace Plugin.NishTech.IntegrationFramework
{
    using Microsoft.Extensions.Logging;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;

    /// <inheritdoc cref="" />
    /// <summary>
    ///  Defines the AddJobPipeline pipeline.
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Plugin.NishTech.IntegrationFramework.JobSchedulerPipeline{Plugin.NishTech.IntegrationFramework.JobSchedulerArgument,
    ///         Plugin.NishTech.IntegrationFramework.JobInfoArgument}
    ///     </cref>
    /// </seealso>
    /// <seealso cref="T:Plugin.NishTech.IntegrationFramework.IAddJobPipeline" />
    public class JobSchedulerPipeline : CommercePipeline<JobSchedulerArgument, JobInfoArgument>, IJobSchedulerPipeline
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Plugin.NishTech.IntegrationFramework.AddJobPipeline" /> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        public JobSchedulerPipeline(IPipelineConfiguration<IJobSchedulerPipeline> configuration, ILoggerFactory loggerFactory)
            : base(configuration, loggerFactory)
        {
        }
    }
}

