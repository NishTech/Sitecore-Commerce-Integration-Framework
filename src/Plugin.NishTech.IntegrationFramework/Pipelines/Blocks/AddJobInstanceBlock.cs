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
    ///         Sitecore.Framework.Pipelines.PipelineBlock{Plugin.NishTech.IntegrationFramework.AddJobInstanceArgument,
    ///         Plugin.NishTech.IntegrationFramework.JobInstance, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>
    [PipelineDisplayName("JobScheduler.block.addjobinstance")]
    public class AddJobInstanceBlock : PipelineBlock<AddJobInstanceArgument, JobInstance, CommercePipelineExecutionContext>
    {
        private readonly IDoesEntityExistPipeline _findEntityPipeline;

        public AddJobInstanceBlock(IDoesEntityExistPipeline findEntityPipeline)
          : base((string)null)
        {
            this._findEntityPipeline = findEntityPipeline;
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="arg">
        /// The AddJobInstanceArgument argument.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="JobInstance"/>.
        /// </returns>
        public override async Task<JobInstance> Run(AddJobInstanceArgument arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull("AddJobArgument The argument cannot be null.");
            Condition.Requires(arg.Name).IsNotNullOrEmpty("The Job Instance name cannot be null or empty.");
            Condition.Requires(arg.Job).IsNotNull("The Job cannot be null.");
            var awaiter = _findEntityPipeline.Run(new FindEntityArgument(typeof(JobInstance),
                $"{(object) CommerceEntity.IdPrefix<Job>()}{ arg.Name}"), context).GetAwaiter();
            if (awaiter.GetResult())
            {
                CommerceContext commerceContext = context.CommerceContext;
                string validationError = context.GetPolicy<KnownResultCodes>().ValidationError;
                string commerceTermKey = "JobNameAlreadyInUse";
                object[] args = new object[1] { arg.Name };
                string defaultMessage = $"Job Instance name { arg.Name} is already in use.";
                context.Abort("Ok|" + await commerceContext.AddMessage(validationError, commerceTermKey, args, defaultMessage), context);
                context = null;
                return null;
            }

            var jobInstance = new JobInstance(arg.Name)
            {
                Id = $"{CommerceEntity.IdPrefix<JobInstance>()}{arg.Job.Name}-{arg.Name}",
                FriendlyId = $"{arg.Job.Name}-{arg.Name}",
                Job = new EntityReference()
                {
                    EntityTarget = arg.Job.Id,
                    Name = arg.Job.Name
                },
                Description = arg.Description
            };
            ;
            jobInstance.DisplayName = arg.DisplayName;
            jobInstance.ScheduleDateTime = arg.ScheduleDateTime;
            jobInstance.Status = arg.Status;

            jobInstance.SetComponent(new ListMembershipsComponent()
            {
                Memberships = new List<string>()
                {
                    CommerceEntity.ListName<JobInstance>()
                }
            });
            var knownJobInstanceListsPolicy = context.GetPolicy<KnownJobInstanceListsPolicy>();
            jobInstance.SetComponent(new TransientListMembershipsComponent()
            {
                Memberships = new List<string>()
                {
                    knownJobInstanceListsPolicy.QueuedJobs
                }
            });

            var jobInstanceAdded = new JobInstanceAdded(jobInstance.FriendlyId)
            {
                Name = jobInstance.Name
            };
            context.CommerceContext.AddModel(jobInstanceAdded);
            return jobInstance;
        }
    }
}
