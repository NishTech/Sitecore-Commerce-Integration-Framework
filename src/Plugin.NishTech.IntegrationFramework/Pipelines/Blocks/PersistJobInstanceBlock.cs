namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;
    using System.Threading.Tasks;

    [PipelineDisplayName("JobScheduler.block.persistjobinstance")]
    public class PersistJobInstanceBlock : PipelineBlock<JobInstance, JobInstance, CommercePipelineExecutionContext>
    {
        private readonly IPersistEntityPipeline _persistEntityPipeline;

        public PersistJobInstanceBlock(IPersistEntityPipeline persistEntityPipeline)
          : base((string)null)
        {
            this._persistEntityPipeline = persistEntityPipeline;
        }

        public override async Task<JobInstance> Run(JobInstance arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull("The JobInstance can not be null");
            var persistEntityArgument = await _persistEntityPipeline.Run(new PersistEntityArgument(arg), context);
            context.CommerceContext.AddEntity(arg);
            return arg;
        }
    }
}
