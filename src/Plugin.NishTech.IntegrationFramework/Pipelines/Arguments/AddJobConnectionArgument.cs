namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Conditions;

    /// <inheritdoc />
    /// <summary>
    /// Defines an argument
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.PipelineArgument" />
    public class AddJobConnectionArgument : PipelineArgument
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddJobConnectionArgument"/> class.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="type"></param>
        public AddJobConnectionArgument(string name, string type)
        {
            Condition.Requires<string>(name).IsNotNullOrEmpty("The job connection name can not be null or empty");
            Condition.Requires<string>(type).IsNotNullOrEmpty("The job connection type can not be null or empty");
            this.Name = name;
            this.Type = type;
            this.Description = string.Empty;
            this.DisplayName = string.Empty;
            this.DbConnectionString = string.Empty;
            this.WebServiceUrl = string.Empty;
            this.Username = string.Empty;
            this.Password = string.Empty;
        }

        public string Name { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public string DisplayName { get; set; }
        public string DbConnectionString { get; set; }
        public string WebServiceUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
