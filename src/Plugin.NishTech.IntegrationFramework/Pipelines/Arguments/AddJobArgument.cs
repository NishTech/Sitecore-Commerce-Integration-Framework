using System;
using Sitecore.Framework.Conditions;

namespace Plugin.NishTech.IntegrationFramework
{
    /// <inheritdoc />
    /// <summary>
    /// Defines an argument
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.PipelineArgument" />
    public class AddJobArgument : JobConnectionArgument
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Plugin.NishTech.IntegrationFramework.JobConnectionArgument" /> class.
        /// </summary>
        /// <param name="jobConnection"></param>
        /// <param name="name"></param>
        public AddJobArgument(JobConnection jobConnection, string name) : base(jobConnection)
        {
            Condition.Requires<string>(name).IsNotNullOrEmpty("The job name can not be null or empty");
            this.Name = name;
            this.Description = string.Empty;
            this.DisplayName = string.Empty;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string NotificationEmail { get; set; }
        public string SqlQuery { get; set; }
        public string JobProcessingPipeline { get; set; }

        //Policy arguments
        public bool IsRecurringJob { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string RecurrenceRepeatType { get; set; }
        public int RepeatValue { get; set; }
    }
}
