namespace Plugin.NishTech.IntegrationFramework
{
    using System;
    using System.Collections.Generic;
    using Sitecore.Commerce.Core;

    /// <inheritdoc />
    /// <summary>
    /// JobConnection model.
    /// </summary>
    public class JobConnection : CommerceEntity
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Plugin.NishTech.IntegrationFramework.JobConnection" /> class.
        /// </summary>
        public JobConnection(string name, string type)
        {
            this.Name = name;
            this.DisplayName = string.Empty;
            this.Description = string.Empty;
            this.Type = type;
            this.Components = new List<Component>();
            this.DateCreated = DateTime.UtcNow;
            this.DateUpdated = this.DateCreated;
        }

        public string Description { get; set; }
        public string Type { get; set; }

    }
}