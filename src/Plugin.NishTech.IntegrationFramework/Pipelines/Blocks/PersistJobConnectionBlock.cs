namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;
    using System.Threading.Tasks;

    [PipelineDisplayName("JobScheduler.block.persistjobconnection")]
    public class PersistJobConnectionBlock : PipelineBlock<JobConnection, JobConnection, CommercePipelineExecutionContext>
    {
        private readonly IPersistEntityPipeline _persistEntityPipeline;

        public PersistJobConnectionBlock(IPersistEntityPipeline persistEntityPipeline)
          : base((string)null)
        {
            this._persistEntityPipeline = persistEntityPipeline;
        }

        public override async Task<JobConnection> Run(JobConnection arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull("The JobConnection can not be null");
            var persistEntityArgument = await _persistEntityPipeline.Run(new PersistEntityArgument(arg), context);
            context.CommerceContext.AddEntity(arg);
            return arg;
        }
    }
}
