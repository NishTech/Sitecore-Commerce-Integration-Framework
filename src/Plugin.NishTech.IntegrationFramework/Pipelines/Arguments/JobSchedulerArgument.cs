
namespace Plugin.NishTech.IntegrationFramework
{
    using System;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Conditions;

    /// <inheritdoc />
    /// <summary>
    /// Defines an argument
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.PipelineArgument" />
    public class JobSchedulerArgument : PipelineArgument
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Plugin.NishTech.IntegrationFramework.JobSchedulerArgument" /> class.
        /// </summary>
        /// <param name="jobInstance"></param>
        public JobSchedulerArgument(JobInstance jobInstance)
        {
            Condition.Requires(jobInstance).IsNotNull("Job Instance cannot be null.");
            this.QueuedJobInstance = jobInstance;
        }

        public JobInstance QueuedJobInstance { get; set; }
    }
}
