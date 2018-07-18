namespace Plugin.NishTech.IntegrationFramework
{
    using System;
    using System.Threading.Tasks;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Core.Commands;
    using Sitecore.Framework.Conditions;

    /// <inheritdoc />
    /// <summary>
    /// Defines the AddJobInstanceCommand command.
    /// </summary>
    public class AddJobInstanceCommand : CommerceCommand
    {
        private readonly IAddJobInstancePipeline _pipeline;

        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Plugin.NishTech.IntegrationFramework.AddJobInstanceCommand" /> class.
        /// </summary>
        /// <param name="pipeline">
        /// The pipeline.
        /// </param>
        /// <param name="serviceProvider">The service provider</param>
        public AddJobInstanceCommand(IAddJobInstancePipeline pipeline, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this._pipeline = pipeline;
        }

        /// <summary>
        /// The process of the command
        /// </summary>
        /// <param name="commerceContext">
        /// The commerce context
        /// </param>
        /// <param name="addJobInstanceArgument"></param>
        /// <returns>
        /// The <see cref="Task"/>.
        /// </returns>
        public async Task<JobInstance> Process(CommerceContext commerceContext, AddJobInstanceArgument addJobInstanceArgument)
        {
            Condition.Requires(addJobInstanceArgument).IsNotNull("AddJobInstanceCommand: addJobInstanceArgument cannot be null.");
            using (var activity = CommandActivity.Start(commerceContext, this))
            {
                var arg = addJobInstanceArgument;
                var result = await this._pipeline.Run(arg, new CommercePipelineExecutionContextOptions(commerceContext));
                return result;
            }
        }
    }
}