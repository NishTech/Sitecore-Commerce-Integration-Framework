
namespace Plugin.NishTech.IntegrationFramework
{
    using System;
    using Sitecore.Framework.Conditions;

    /// <inheritdoc />
    /// <summary>
    /// Defines an argument
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.PipelineArgument" />
    public class AddJobInstanceArgument : JobArgument
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Plugin.NishTech.IntegrationFramework.AddJobInstanceArgument" /> class.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="name"></param>
        public AddJobInstanceArgument(Job job, string name) : base(job)
        {
            Condition.Requires<string>(name).IsNotNullOrEmpty("The job name can not be null or empty");
            this.Name = name;
            this.Description = string.Empty;
            this.DisplayName = string.Empty;
        }

        public string Name { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public DateTime? ScheduleDateTime { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public string Status { get; set; }
    }
}
