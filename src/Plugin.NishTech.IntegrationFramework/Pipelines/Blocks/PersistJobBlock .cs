
namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;
    using System.Threading.Tasks;

    [PipelineDisplayName("JobScheduler.block.persistjob")]
    public class PersistJobBlock : PipelineBlock<Job, Job, CommercePipelineExecutionContext>
    {
        private readonly IPersistEntityPipeline _persistEntityPipeline;

        public PersistJobBlock(IPersistEntityPipeline persistEntityPipeline)
          : base((string)null)
        {
            this._persistEntityPipeline = persistEntityPipeline;
        }

        public override async Task<Job> Run(Job arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires(arg).IsNotNull("The Job can not be null");
            var persistEntityArgument = await _persistEntityPipeline.Run(new PersistEntityArgument(arg), context);
            context.CommerceContext.AddEntity(arg);
            return arg;
        }
    }
}
