namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.ManagedLists;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a block
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Sitecore.Framework.Pipelines.PipelineBlock{Plugin.NishTech.IntegrationFramework.AddJobArgument,
    ///         Plugin.NishTech.IntegrationFramework.Job, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>
    [PipelineDisplayName("JobScheduler.block.addjob")]
    public class AddJobBlock : PipelineBlock<AddJobArgument, Job, CommercePipelineExecutionContext>
    {
        private readonly IDoesEntityExistPipeline _findEntityPipeline;

        public AddJobBlock(IDoesEntityExistPipeline findEntityPipeline)
          : base((string)null)
        {
            this._findEntityPipeline = findEntityPipeline;
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="arg">
        /// The AddJobArgument argument.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="Job"/>.
        /// </returns>
        public override async Task<Job> Run(AddJobArgument arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull("AddJobArgument The argument cannot be null.");
            Condition.Requires(arg.Name).IsNotNullOrEmpty("The Job name cannot be null or empty.");
            Condition.Requires(arg.JobConnection).IsNotNull("The Job Connection cannot be null.");
            var awaiter = _findEntityPipeline.Run(new FindEntityArgument(typeof(Job),
                $"{(object) CommerceEntity.IdPrefix<Job>()}{ arg.Name}"), context).GetAwaiter();
            if (awaiter.GetResult())
            {
                CommerceContext commerceContext = context.CommerceContext;
                string validationError = context.GetPolicy<KnownResultCodes>().ValidationError;
                string commerceTermKey = "JobNameAlreadyInUse";
                object[] args = new object[1] { arg.Name };
                string defaultMessage = $"Job name { arg.Name} is already in use.";
                context.Abort("Ok|" + await commerceContext.AddMessage(validationError, commerceTermKey, args, defaultMessage), context);
                context = null;
                return null;
            }
            var job = new Job(arg.Name)
            {
                Id = $"{ CommerceEntity.IdPrefix<Job>()}{ arg.JobConnection.Name}-{ arg.Name}",
                FriendlyId = $"{arg.JobConnection.Name}-{arg.Name}"
            };
            job.JobConnection= new EntityReference()
            {
                EntityTarget = arg.JobConnection.Id,
                Name = arg.JobConnection.Name
            };
            job.Description = arg.Description; ;
            job.DisplayName = arg.DisplayName;
            job.Type = arg.Type;
            job.NotificationEmail = arg.NotificationEmail;
            job.SqlQuery = arg.SqlQuery;

            //Assign Policy data
            var jobPolicy = context.GetPolicy<JobPolicy>();
            jobPolicy.IsRecurringJob = arg.IsRecurringJob;
            jobPolicy.StartDateTime = arg.StartDateTime;
            jobPolicy.EndDateTime = arg.EndDateTime;
            jobPolicy.RecurrenceRepeatType = arg.RecurrenceRepeatType;
            jobPolicy.RepeatValue = arg.RepeatValue;
            job.SetPolicy(jobPolicy);

            job.SetComponent(new ListMembershipsComponent()
            {
                Memberships = new List<string>()
                {
                    CommerceEntity.ListName<Job>()
                }
            });
            var jobAdded = new JobAdded(job.FriendlyId)
            {
                Name = job.Name
            };
            context.CommerceContext.AddModel(jobAdded);
            return job;

        }
    }
}
