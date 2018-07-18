namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Conditions;

    /// <inheritdoc />
    /// <summary>
    /// Defines an argument
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.PipelineArgument" />
    public class JobConnectionArgument : PipelineArgument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JobConnectionArgument"/> class.
        /// </summary>
        /// <param name="jobConnection">
        /// The parameter.
        /// </param>
        public JobConnectionArgument(JobConnection jobConnection)
        {
            Condition.Requires<JobConnection>(jobConnection).IsNotNull<JobConnection>("The job connection cannot be null.");
            Condition.Requires<string>(jobConnection.Name).IsNotNullOrEmpty("The job connection name cannot be null or empty.");
            Condition.Requires<string>(jobConnection.Type).IsNotNullOrEmpty("The job connection type cannot be null or empty.");
            this.JobConnection = jobConnection;
        }

        /// <summary>
        /// Gets or sets the parameter.
        /// </summary>
        /// <value>
        /// The parameter.
        /// </value>
        public JobConnection JobConnection { get; set; }
    }
}
