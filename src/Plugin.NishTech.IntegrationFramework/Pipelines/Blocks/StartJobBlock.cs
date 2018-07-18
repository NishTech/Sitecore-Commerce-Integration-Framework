namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a block
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Sitecore.Framework.Pipelines.StartJobBlock{Plugin.NishTech.IntegrationFramework.JobSchedulerArgument,
    ///         Plugin.NishTech.IntegrationFramework.JobInfoArgument, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>
    [PipelineDisplayName("JobScheduler.block.startjobinfo")]
    public class StartJobBlock : PipelineBlock<JobSchedulerArgument, JobInfoArgument, CommercePipelineExecutionContext>
    {
        private readonly IFindEntityPipeline _findEntityPipeline;

        public StartJobBlock(IFindEntityPipeline findEntityPipeline)
          : base((string)null)
        {
            this._findEntityPipeline = findEntityPipeline;
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="arg">
        /// The JobSchedulerArgument argument.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="JobInfoArgument"/>.
        /// </returns>
        public override async Task<JobInfoArgument> Run(JobSchedulerArgument arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull("RunJobMinionArgument: The argument cannot be null.");
            Condition.Requires(arg.QueuedJobInstance).IsNotNull("QueuedJobInstance cannot be null.");
            var job = await this._findEntityPipeline.Run(new FindEntityArgument(typeof(Job), $"{arg.QueuedJobInstance.Job.EntityTarget}"), context) as Job;
            if (job==null)
            {
                var commerceContext = context.CommerceContext;
                var validationError = context.GetPolicy<KnownResultCodes>().ValidationError;
                const string commerceTermKey = "JobNotFound";
                var args = new object[1] { arg.QueuedJobInstance.Name };
                var defaultMessage = $"Job not found.";
                context.Abort("Ok|" + await commerceContext.AddMessage(validationError, commerceTermKey, args, defaultMessage), context);
                context = null;
                return null;
            }
            var jobConnection = await this._findEntityPipeline.Run(new FindEntityArgument(typeof(JobConnection), $"{job.JobConnection.EntityTarget}"), context) as JobConnection;
            if (jobConnection == null)
            {
                var commerceContext = context.CommerceContext;
                var validationError = context.GetPolicy<KnownResultCodes>().ValidationError;
                const string commerceTermKey = "JobConnectionNotFound";
                var args = new object[1] { arg.QueuedJobInstance.Name };
                var defaultMessage = $"Job connection not found.";
                context.Abort("Ok|" + await commerceContext.AddMessage(validationError, commerceTermKey, args, defaultMessage), context);
                context = null;
                return null;
            }
            return new JobInfoArgument(jobConnection, job, arg.QueuedJobInstance);
        }
    }
}
