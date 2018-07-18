using System;
using System.Threading.Tasks;
using Sitecore.Commerce.Core;
using Sitecore.Commerce.Core.Commands;
using Sitecore.Framework.Conditions;

namespace Plugin.NishTech.IntegrationFramework
{


    /// <inheritdoc />
    /// <summary>
    /// Defines the AddJobConnectionCommand command.
    /// </summary>
    public class AddJobConnectionCommand : CommerceCommand
    {
        private readonly IAddJobConnectionPipeline _pipeline;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Plugin.NishTech.IntegrationFramework.AddJobConnectionCommand" /> class.
        /// </summary>
        /// <param name="pipeline">
        /// The pipeline.
        /// </param>
        /// <param name="serviceProvider">The service provider</param>
        public AddJobConnectionCommand(IAddJobConnectionPipeline pipeline, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._pipeline = pipeline;
        }

        /// <summary>
        /// The process of the command
        /// </summary>
        /// <param name="commerceContext">
        /// The commerce context
        /// </param>
        /// <param name="addJobConnectionArgument"></param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<JobConnection> Process(CommerceContext commerceContext, AddJobConnectionArgument addJobConnectionArgument)
        {
            Condition.Requires(addJobConnectionArgument).IsNotNull("AddJobConnectionCommand: addJobConnectionArgument cannot be null.");
            using (var activity = CommandActivity.Start(commerceContext, this))
            {
                var arg = addJobConnectionArgument;
                var result = await this._pipeline.Run(arg, new CommercePipelineExecutionContextOptions(commerceContext));
                return result;
            }
        }
    }
}