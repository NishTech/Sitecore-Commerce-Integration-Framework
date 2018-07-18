namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    [PipelineDisplayName("JobScheduler.block.registeredplugin")]
    public class RegisteredPluginBlock : PipelineBlock<IEnumerable<RegisteredPluginModel>, IEnumerable<RegisteredPluginModel>, CommercePipelineExecutionContext>
    {
        public override Task<IEnumerable<RegisteredPluginModel>> Run(IEnumerable<RegisteredPluginModel> arg, CommercePipelineExecutionContext context)
        {
            if (arg == null)
                return Task.FromResult((IEnumerable<RegisteredPluginModel>) null);
            var list = arg.ToList();
            PluginHelper.RegisterPlugin(this, list);
            return Task.FromResult(list.AsEnumerable());
        }

        public RegisteredPluginBlock()
          : base((string)null)
        {
        }
    }
}
