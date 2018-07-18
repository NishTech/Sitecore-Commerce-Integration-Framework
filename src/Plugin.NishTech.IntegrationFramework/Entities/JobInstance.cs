using System;
using System.Collections.Generic;
using Sitecore.Commerce.Core;

namespace Plugin.NishTech.IntegrationFramework
{

    /// <inheritdoc />
    /// <summary>
    /// JobInstance model.
    /// </summary>
    public class JobInstance : CommerceEntity
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Plugin.NishTech.IntegrationFramework.JobInstancey" /> class.
        /// </summary>
        public JobInstance(string name)
        {
            this.Name = name;
            this.StartDateTime = null;
            this.EndDateTime = null;
            this.ElapsedTime = TimeSpan.Zero;
            this.Status = string.Empty;
            this.DisplayName = string.Empty;
            this.Description = string.Empty;
            this.Components = new List<Component>();
            this.DateCreated = DateTime.UtcNow;
            this.DateUpdated = this.DateCreated;
        }

        public string Description { get; set; }
        public DateTime? ScheduleDateTime { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public TimeSpan ElapsedTime { get; set; }
        public string Status { get; set; }
        public EntityReference Job { get; set; }

    }
}