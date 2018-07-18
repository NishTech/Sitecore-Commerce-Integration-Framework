namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Framework.Pipelines;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    [PipelineDisplayName("JobScheduler.block.doactionaddjob")]
    public class DoActionAddJobBlock : PipelineBlock<EntityView, EntityView, CommercePipelineExecutionContext>
    {
        private readonly AddJobCommand _addJobCommand;

        public DoActionAddJobBlock(AddJobCommand addJobCommand)
          : base((string)null)
        {
            _addJobCommand = addJobCommand;
        }

        public override async Task<EntityView> Run(EntityView entityView, CommercePipelineExecutionContext context)
        {
            if (string.IsNullOrEmpty(entityView?.Action) || !entityView.Action.Equals(context.GetPolicy<KnownJobSchedulerActionsPolicy>().AddJob, StringComparison.OrdinalIgnoreCase))
                return entityView;
            var jobName = entityView.Properties.FirstOrDefault(p => p.Name.Equals("Name", StringComparison.OrdinalIgnoreCase));
            if (string.IsNullOrEmpty(jobName?.Value))
            {
                var str1 = jobName == null ? "Name" : jobName.DisplayName;
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
            var jobConnection = context.CommerceContext.GetObject((Func<JobConnection, bool>)(p => p.Id.Equals(entityView.EntityId, StringComparison.OrdinalIgnoreCase)));
            if (jobConnection == null)
            {
                var str = await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().ValidationError, "InvalidOrMissingPropertyValue", new object[1]
                {
                    "EntityId"
                }, "JobConnection doesn't exists.");
                return entityView;
            }
            var typeProperty = entityView.Properties.FirstOrDefault(p => p.Name.Equals("Type", StringComparison.OrdinalIgnoreCase));
            var type = typeProperty?.Value;
            var displayNameProperty = entityView.Properties.FirstOrDefault(p => p.Name.Equals("DisplayName", StringComparison.OrdinalIgnoreCase));
            var displayName = displayNameProperty?.Value;
            var descriptionProperty= entityView.Properties.FirstOrDefault(p => p.Name.Equals("Description", StringComparison.OrdinalIgnoreCase));
            var description = descriptionProperty?.Value;
            var sqlQueryProperty = entityView.Properties.FirstOrDefault(p => p.Name.Equals("SqlQuery", StringComparison.OrdinalIgnoreCase));
            var sqlQuery = sqlQueryProperty?.Value;
            var jobProcessingPipelineProperty = entityView.Properties.FirstOrDefault(p => p.Name.Equals("JobProcessingPipeline", StringComparison.OrdinalIgnoreCase));
            var jobProcessingPipeline = jobProcessingPipelineProperty?.Value;
            var notificatioEmailProperty = entityView.Properties.FirstOrDefault(p => p.Name.Equals("NotificationEmail", StringComparison.OrdinalIgnoreCase));
            var notificationEmail = notificatioEmailProperty?.Value;

            //Policy data
            var isRecurringJobProperty = entityView.Properties.FirstOrDefault(p => p.Name.Equals("IsRecurringJob", StringComparison.OrdinalIgnoreCase));
            var isRecurringJob = isRecurringJobProperty != null && Convert.ToBoolean(isRecurringJobProperty.Value);
            var startDateTimeProperty = entityView.Properties.FirstOrDefault(p => p.Name.Equals("StartDateTime", StringComparison.OrdinalIgnoreCase));
            DateTime? startDateTime = startDateTimeProperty != null ? Convert.ToDateTime(startDateTimeProperty.Value) : (DateTime?) null;
            var endDateTimeProperty = entityView.Properties.FirstOrDefault(p => p.Name.Equals("EndDateTime", StringComparison.OrdinalIgnoreCase));
            DateTime? endDateTime = endDateTimeProperty != null ? Convert.ToDateTime(endDateTimeProperty.Value) : (DateTime?)null;
            var recurrenceRepeatTypeProperty = entityView.Properties.FirstOrDefault(p => p.Name.Equals("RecurrenceRepeatType", StringComparison.OrdinalIgnoreCase));
            var recurrenceRepeatType = recurrenceRepeatTypeProperty?.Value; ;
            var repeatValueProperty = entityView.Properties.FirstOrDefault(p => p.Name.Equals("RepeatValue", StringComparison.OrdinalIgnoreCase));
            var repeatValue = repeatValueProperty != null ? Convert.ToInt32(repeatValueProperty.Value) : 0;

            var addJobArgument = new AddJobArgument(jobConnection, jobName.Value)
            {
                Description = description,
                DisplayName = displayName,
                Type = type,
                NotificationEmail = notificationEmail,
                SqlQuery = sqlQuery,
                JobProcessingPipeline = jobProcessingPipeline,
                IsRecurringJob = isRecurringJob,
                StartDateTime = startDateTime,
                EndDateTime = endDateTime,
                RecurrenceRepeatType = recurrenceRepeatType,
                RepeatValue = repeatValue,
            };

            var job = await _addJobCommand.Process(context.CommerceContext, addJobArgument);
            return entityView;
        }
    }
}
