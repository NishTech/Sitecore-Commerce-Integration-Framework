using System.Linq;

namespace Plugin.NishTech.IntegrationFramework
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Sitecore.Commerce.Core;
    public class JobSchedulerMinion : Minion
    {
        protected IJobSchedulerPipeline JobSchedulerPipeline { get; set; }
        public override void Initialize(IServiceProvider serviceProvider, ILogger logger, MinionPolicy policy, CommerceEnvironment environment, CommerceContext globalContext)
        {
            base.Initialize(serviceProvider, logger, policy, environment, globalContext);
            this.JobSchedulerPipeline = serviceProvider.GetService<IJobSchedulerPipeline>();
        }
        public override async Task<MinionRunResultsModel> Run()
        {
            long listCount = await GetListCount(Policy.ListToWatch);
            if (listCount > 0L)
            {
                var jobInstanceList = (await this.GetListItems<JobInstance>(this.Policy.ListToWatch, this.Policy.ItemsPerBatch)).ToList();
                foreach (var jobInstance in jobInstanceList)
                {
                    if (jobInstance.ScheduleDateTime < DateTime.Now)
                    {
                        this.Logger.LogInformation($"Starting job {jobInstance.Job.Name}");
                        jobInstance.StartDateTime = DateTime.Now;
                        var jobsSchedulerArgument = new JobSchedulerArgument(jobInstance);
                        var commerceContext = new CommerceContext(this.Logger, this.MinionContext.TelemetryClient)
                        {
                            Environment = this.Environment
                        };
                        CommercePipelineExecutionContextOptions executionContextOptions = new CommercePipelineExecutionContextOptions(commerceContext);
                        var job = await JobSchedulerPipeline.Run(jobsSchedulerArgument, executionContextOptions);
                    }
                }
            }
            return new MinionRunResultsModel();
        }
    }
}
