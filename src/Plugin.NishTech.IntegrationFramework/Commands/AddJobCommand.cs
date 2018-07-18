using System;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Framework.Conditions;

namespace Plugin.NishTech.IntegrationFramework
{


    /// <inheritdoc />
    /// <summary>
    /// Defines the AddJobCommand command.
    /// </summary>
    public class AddJobCommand : CommerceCommand
    {
        private readonly IAddJobPipeline _pipeline;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Plugin.NishTech.IntegrationFramework.AddJobCommand" /> class.
        /// </summary>
        /// <param name="pipeline">
        /// The pipeline.
        /// </param>
        /// <param name="serviceProvider">The service provider</param>
        public AddJobCommand(IAddJobPipeline pipeline, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._pipeline = pipeline;
        }

        /// <summary>
        /// The process of the command
        /// </summary>
        /// <param name="commerceContext">
        /// The commerce context
        /// </param>
        /// <param name="addJobArgument"></param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<Job> Process(CommerceContext commerceContext, AddJobArgument addJobArgument)
        {
            Condition.Requires(addJobArgument).IsNotNull("AddJobCommand: addJobArgument cannot be null.");
            using (var activity = CommandActivity.Start(commerceContext, this))
            {
                var arg = addJobArgument;
                var result = await this._pipeline.Run(arg, new CommercePipelineExecutionContextOptions(commerceContext));
                return result;
            }
        }
    }
}