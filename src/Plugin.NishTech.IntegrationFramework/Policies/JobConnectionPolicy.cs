namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;

    public class JobConnectionPolicy : Policy
    {
        public string DbConnectionString { get; set; }
        public string WebServiceUrl { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
