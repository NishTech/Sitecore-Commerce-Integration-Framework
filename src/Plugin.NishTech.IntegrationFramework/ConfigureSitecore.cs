namespace Plugin.NishTech.IntegrationFramework
{
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.EntityViews;
    using Sitecore.Framework.Configuration;
    using Sitecore.Framework.Pipelines;
    using Sitecore.Framework.Pipelines.Definitions;
    using Sitecore.Framework.Pipelines.Definitions.Extensions;
    using Sitecore.Framework.Rules;
    using System;
    using System.Reflection;

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
                SitecorePipelinesConfigBuilder pipelinesConfigBuilder3 = pipelinesConfigBuilder2.ConfigurePipeline<IConfigureServiceApiPipeline>(c => c.Add<ConfigureServiceApiBlock>(), section2, order2);
                string section3 = "main";
                int order3 = 1000;
                SitecorePipelinesConfigBuilder pipelinesConfigBuilder4 = pipelinesConfigBuilder3.ConfigurePipeline<IDoActionPipeline>(c => c.Add<DoActionAddJobConnectionBlock>().After<ValidateEntityVersionBlock>().Add<DoActionAddJobBlock>().After<DoActionAddJobConnectionBlock>().Add<DoActionAddJobInstanceBlock>().After<DoActionAddJobBlock>(), section3, order3);
                string section4 = "main";
                int order4 = 1000;
                SitecorePipelinesConfigBuilder pipelinesConfigBuilder5 = pipelinesConfigBuilder4.AddPipeline<IAddJobConnectionPipeline, AddJobConnectionPipeline>(c => c.Add<AddJobConnectionBlock>().Add<PersistJobConnectionBlock>(), section4, order4);
                string section5 = "main";
                int order5 = 1000;
                SitecorePipelinesConfigBuilder pipelinesConfigBuilder6 = pipelinesConfigBuilder5.AddPipeline<IAddJobPipeline, AddJobPipeline>(c => c.Add<AddJobBlock>().Add<PersistJobBlock>(), section5, order5);
                string section6 = "main";
                int order6 = 1000;
                SitecorePipelinesConfigBuilder pipelinesConfigBuilder7 = pipelinesConfigBuilder6.AddPipeline<IAddJobInstancePipeline, AddJobInstancePipeline>(c => c.Add<AddJobInstanceBlock>().Add<PersistJobInstanceBlock>(), section6, order6);
                string section7 = "main";
                int order7 = 1000;
                SitecorePipelinesConfigBuilder pipelinesConfigBuilder8 = pipelinesConfigBuilder7.AddPipeline<IJobSchedulerPipeline, JobSchedulerPipeline>(c => c.Add<StartJobBlock>().Add<FinishJobBlock>(), section7, order7);
            });
            services.RegisterAllCommands(assembly);
        }
    }
}