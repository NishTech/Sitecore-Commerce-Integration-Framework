namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Conditions;

    /// <inheritdoc />
    /// <summary>
    /// Defines an argument
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.PipelineArgument" />
    public class JobArgument : PipelineArgument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobArgument"/> class.
        /// </summary>
        /// <param name="job"></param>
        public JobArgument(Job job)
        {
            Condition.Requires(job).IsNotNull("The job cannot be null.");
            Condition.Requires<string>(job.Name).IsNotNullOrEmpty("The job name cannot be null or empty.");
            this.Job = job;
        }

        /// <summary>
        /// Gets or sets the parameter.
        /// </summary>
        /// <value>
        /// The parameter.
        /// </value>
        public Job Job { get; set; }
    }
}
