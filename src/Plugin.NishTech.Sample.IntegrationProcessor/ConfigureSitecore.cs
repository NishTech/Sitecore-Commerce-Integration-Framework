
namespace Plugin.NishTech.Sample.IntegrationProcessor
{
    using System;
    using System.Reflection;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Configuration;
    using Sitecore.Framework.Pipelines.Definitions.Extensions;
    using Plugin.NishTech.IntegrationFramework;
    using Sitecore.Framework.Pipelines;
    using Sitecore.Framework.Pipelines.Definitions;
    using Sitecore.Framework.Rules;
    /// <summary>
    /// The configure sitecore class.
    /// </summary>
    public class ConfigureSitecore : IConfigureSitecore
    {
        /// <summary>
        /// The configure services.
        /// </summary>
        /// <param name="services">
        /// The services.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            services.RegisterAllPipelineBlocks(assembly);
            services.Sitecore().Rules(config => config.Registry(registry => registry.RegisterAssembly(assembly)));
            services.Sitecore().Pipelines(config =>
            {
                SitecorePipelinesConfigBuilder pipelinesConfigBuilder1 = config;
                string section1 = "main";
                int order1 = 1000;
                SitecorePipelinesConfigBuilder pipelinesConfigBuilder2 = pipelinesConfigBuilder1.ConfigurePipeline((Action<PipelineDefinition<IRunningPluginsPipeline>>)(c => c.Add<RegisteredPluginBlock>().After<RunningPluginsBlock>()), section1, order1);
                string section2 = "main";
                int order2 = 1000;
                SitecorePipelinesConfigBuilder pipelinesConfigBuilder3 = pipelinesConfigBuilder2.ConfigurePipeline((Action<PipelineDefinition<IConfigureServiceApiPipeline>>)(c => c.Add<ConfigureServiceApiBlock>()), section2, order2);
                string section3 = "main";
                int order3 = 1000;
                SitecorePipelinesConfigBuilder pipelinesConfigBuilder4 = pipelinesConfigBuilder3.ConfigurePipeline<IJobSchedulerPipeline>(c => c.Add<CustomerSqlProcessorBlock>().After<StartJobBlock>().Add<CustomerFlatFileProcessorBlock>().After<CustomerSqlProcessorBlock>(), section3, order3);
            });
            services.RegisterAllCommands(assembly);
        }
    }
}