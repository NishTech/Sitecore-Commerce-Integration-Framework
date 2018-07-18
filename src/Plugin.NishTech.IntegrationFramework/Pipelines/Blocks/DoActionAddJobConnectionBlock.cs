using Sitecore.Commerce.Core;
using Sitecore.Commerce.EntityViews;
using Sitecore.Framework.Pipelines;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Plugin.NishTech.IntegrationFramework
{
    [PipelineDisplayName("JobScheduler.block.doactionaddjobconnection")]
    public class DoActionAddJobConnectionBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly AddJobConnectionCommand _addJobConnectionCommand;

        public DoActionAddJobConnectionBlock(AddJobConnectionCommand addJobConnectionCommand)
          : base((string)null)
        {
            _addJobConnectionCommand = addJobConnectionCommand;
        }

        public override async Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(entityView?.Action) || !entityView.Action.Equals(context.GetPolicy<KnownJobSchedulerActionsPolicy>().AddJobConnection, StringComparison.OrdinalIgnoreCase))
                return entityView;
            var jobConnectionName = entityView.Properties.FirstOrDefault(p => p.Name.Equals("Name", StringComparison.OrdinalIgnoreCase));
            if (string.IsNullOrEmpty(jobConnectionName?.Value))
            {
                var str1 = jobConnectionName == null ? "Name" : jobConnectionName.DisplayName;
                var str2 = await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().ValidationError, "InvalidOrMissingPropertyValue", new object[1]
                {
                     str1
                }, "Invalid or missing value for property 'Name'.");
                return entityView;
            }
            var type = entityView.Properties.FirstOrDefault(p => p.Name.Equals("Type", StringComparison.OrdinalIgnoreCase))?.Value;
            if (string.IsNullOrEmpty(type))
            {
                var str = await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().ValidationError, "InvalidOrMissingPropertyValue", new object[1]
                {
                    "Type"
                }, "Invalid or missing value for property 'Type'.");
                return entityView;
            }
            var displayNameProperty = entityView.Properties.FirstOrDefault(p => p.Name.Equals("DisplayName", StringComparison.OrdinalIgnoreCase));
            var displayName = displayNameProperty?.Value;
            var descriptionProperty= entityView.Properties.FirstOrDefault(p => p.Name.Equals("Description", StringComparison.OrdinalIgnoreCase));
            var description = descriptionProperty?.Value;
            var dbConnectionStringProperty= entityView.Properties.FirstOrDefault(p => p.Name.Equals("DbConnectionString", StringComparison.OrdinalIgnoreCase));
            var dbConnectionString = dbConnectionStringProperty?.Value ;
            var webServiceUrlProperty= entityView.Properties.FirstOrDefault(p => p.Name.Equals("WebServiceUrl", StringComparison.OrdinalIgnoreCase));
            var webServiceUrl = webServiceUrlProperty?.Value;
            var usernameProperty= entityView.Properties.FirstOrDefault(p => p.Name.Equals("Username", StringComparison.OrdinalIgnoreCase));
            var username = usernameProperty?.Value;
            var passwordProperty = entityView.Properties.FirstOrDefault(p => p.Name.Equals("Password", StringComparison.OrdinalIgnoreCase));
            var password = passwordProperty?.Value;
            var addJobConnectionArgument = new AddJobConnectionArgument(jobConnectionName.Value, type)
            {
                Description = description,
                DisplayName = displayName,
                DbConnectionString = dbConnectionString,
                WebServiceUrl = webServiceUrl,
                Username = username,
                Password = password
            };
            var jobConnection = await _addJobConnectionCommand.Process(context.CommerceContext, addJobConnectionArgument);
            return entityView;
        }
    }
}
