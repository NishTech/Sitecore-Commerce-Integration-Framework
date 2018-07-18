namespace Plugin.NishTech.IntegrationFramework
{
    using System;
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
    ///         Sitecore.Framework.Pipelines.FinishJobBlock{Plugin.NishTech.IntegrationFramework.JobInfoArgument,
    ///         Plugin.NishTech.IntegrationFramework.JobInstance, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>
    [PipelineDisplayName("JobScheduler.block.finishjob")]
    public class FinishJobBlock : PipelineBlock<JobInfoArgument, JobInstance, CommercePipelineExecutionContext>
    {
        private readonly IPersistEntityPipeline _persistEntityPipeline;
        private readonly IRemoveListEntitiesPipeline _removeListEntitiesPipeline;
        public FinishJobBlock(IPersistEntityPipeline persistEntityPipeline, IRemoveListEntitiesPipeline removeListEntitiesPipeline)
            : base((string)null)
        {
            this._persistEntityPipeline = persistEntityPipeline;
            _removeListEntitiesPipeline = removeListEntitiesPipeline;
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="arg">
        /// The JobInfoArgument argument.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="JobInstance"/>.
        /// </returns>
        public override async Task<JobInstance> Run(JobInfoArgument arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull("JobInfoArgument: The argument cannot be null.");
            Condition.Requires(arg.JobInstance).IsNotNull("JobInstance cannot be null.");
            Condition.Ensures(arg.JobInstance.Status.Equals("queued", StringComparison.OrdinalIgnoreCase))
                .IsFalse("Job status has not changed. Make sure job processor exists.");
            var jobInstance = arg.JobInstance;
            //Remove the JobInstance from WatchList
            var knownJobInstanceListsPolicy = context.GetPolicy<KnownJobInstanceListsPolicy>();
            var component = jobInstance.GetComponent<TransientListMembershipsComponent>();
            if (jobInstance.Status.Equals("Success", StringComparison.OrdinalIgnoreCase))
            {
                component.Memberships.Add(knownJobInstanceListsPolicy.SuccessJobs);
            }
            else
            {
                component.Memberships.Add(knownJobInstanceListsPolicy.FailedJobs);
            }
            var listEntitiesArgument = new ListEntitiesArgument(new string[1]
            {
                arg.JobInstance.Id
            }, knownJobInstanceListsPolicy.QueuedJobs);

            listEntitiesArgument = await _removeListEntitiesPipeline.Run(listEntitiesArgument, context);
            jobInstance.EndDateTime = DateTime.Now;
            jobInstance.ElapsedTime = (jobInstance.EndDateTime - jobInstance.StartDateTime).GetValueOrDefault(TimeSpan.Zero);
            var persistEntityArgument = await _persistEntityPipeline.Run(new PersistEntityArgument(jobInstance), context);
            context.CommerceContext.AddEntity(jobInstance);
            return jobInstance;
        }
    }
}
