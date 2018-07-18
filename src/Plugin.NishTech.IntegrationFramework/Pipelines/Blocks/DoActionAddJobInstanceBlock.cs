namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Framework.Pipelines;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [PipelineDisplayName("JobScheduler.block.doactionaddjobinstance")]
    public class DoActionAddJobInstanceBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly AddJobInstanceCommand _addJobInstanceCommand;

        public DoActionAddJobInstanceBlock(AddJobInstanceCommand addJobInstanceCommand)
          : base((string)null)
        {
            this._addJobInstanceCommand = addJobInstanceCommand;
        }

        public override async Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(entityView?.Action) || !entityView.Action.Equals(context.GetPolicy<KnownJobSchedulerActionsPolicy>().AddJobInstance, StringComparison.OrdinalIgnoreCase))
                return entityView;
            var jobInstanceName = entityView.Properties.FirstOrDefault(p => p.Name.Equals("Name", StringComparison.OrdinalIgnoreCase));
            if (string.IsNullOrEmpty(jobInstanceName?.Value))
            {
                var str1 = jobInstanceName == null ? "Name" : jobInstanceName.DisplayName;
                var str2 = await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().ValidationError, "InvalidOrMissingPropertyValue", new object[1]
                {
                     str1
                }, "Invalid or missing value for property 'Name'.");
                return entityView;
            }

            if (string.IsNullOrEmpty(entityView.EntityId))
            {
                var str = await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().ValidationError, "InvalidOrMissingPropertyValue", new object[1]
                {
                    "EntityId"
                }, "Invalid or missing value for property 'EntityId'.");
                return entityView;
            }
            var job = context.CommerceContext.GetObject((Func<Job, bool>)(p => p.Id.Equals(entityView.EntityId, StringComparison.OrdinalIgnoreCase)));
            if (job == null)
            {
                var str = await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().ValidationError, "InvalidOrMissingPropertyValue", new object[1]
                {
                    "EntityId"
                }, "Job doesn't exists.");
                return entityView;
            }
            var displayNameProperty = entityView.Properties.FirstOrDefault(p => p.Name.Equals("DisplayName", StringComparison.OrdinalIgnoreCase));
            var displayName = displayNameProperty!=null ? displayNameProperty.Value : null as string;
            var descriptionProperty= entityView.Properties.FirstOrDefault(p => p.Name.Equals("Description", StringComparison.OrdinalIgnoreCase));
            var description = descriptionProperty!=null ? descriptionProperty.Value : null as string;
            var scheduleDateTimeProperty = entityView.Properties.FirstOrDefault(p => p.Name.Equals("ScheduleDateTime", StringComparison.OrdinalIgnoreCase));
            DateTime? scheduleDateTime = scheduleDateTimeProperty != null ? Convert.ToDateTime(scheduleDateTimeProperty.Value) : (DateTime?)null;
            var statusProperty = entityView.Properties.FirstOrDefault(p => p.Name.Equals("Status", StringComparison.OrdinalIgnoreCase));
            var status = statusProperty != null ? statusProperty.Value : "Queued";

            var addJobInstanceArgument = new AddJobInstanceArgument(job, jobInstanceName.Value)
            {
                Description = description,
                DisplayName = displayName,
                ScheduleDateTime = scheduleDateTime,
                Status = status,
            };
            var jobInstance = await _addJobInstanceCommand.Process(context.CommerceContext, addJobInstanceArgument);
            return entityView;
        }
    }
}
