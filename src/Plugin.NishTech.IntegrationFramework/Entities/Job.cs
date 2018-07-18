using System;
using System.Collections.Generic;
using Sitecore.Commerce.Core;

namespace Plugin.NishTech.IntegrationFramework
{


    /// <inheritdoc />
    /// <summary>
    /// Job model.
    /// </summary>
    public class Job : CommerceEntity
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Plugin.NishTech.IntegrationFramework.Job" /> class.
        /// </summary>
        public Job(string name)
        {
            this.Name = name;
            this.Type = string.Empty;
            this.DisplayName = string.Empty;
            this.Description = string.Empty;
            this.NotificationEmail = string.Empty;
            this.Components = new List<Component>();
            this.DateCreated = DateTime.UtcNow;
            this.DateUpdated = this.DateCreated;
        }

        public string Type { get; set; }
        public string Description { get; set; }
        public string SqlQuery { get; set; }
        public string NotificationEmail { get; set; }
        public EntityReference JobConnection { get; set; }
    }
}