namespace Plugin.NishTech.IntegrationFramework
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.OData.Builder;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Core.Commands;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;

    /// <summary>
    /// Defines a block which configures the OData model
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Plugin.NishTech.IntegrationFramework.ConfigureServiceApiBlock{Microsoft.AspNetCore.OData.Builder.ODataConventionModelBuilder,
    ///         Microsoft.AspNetCore.OData.Builder.ODataConventionModelBuilder,
    ///         Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>
    [PipelineDisplayName("JobScheduler.block.configureserviceapi")]
    public class ConfigureServiceApiBlock : PipelineBlock<ODataConventionModelBuilder, ODataConventionModelBuilder, CommercePipelineExecutionContext>
    {
        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="modelBuilder">
        /// The argument.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="ODataConventionModelBuilder"/>.
        /// </returns>
        public override Task<ODataConventionModelBuilder> Run(ODataConventionModelBuilder modelBuilder, CommercePipelineExecutionContext context)
        {

            Condition.Requires(modelBuilder).IsNotNull($"{this.Name}: The argument cannot be null.");
            // Add the entities
            modelBuilder.AddEntityType(typeof(JobConnection));
            // Add complex types
            modelBuilder.AddComplexType(typeof(JobConnectionAdded));
            // Add the entity sets
            modelBuilder.EntitySet<JobConnection>("JobConnections");

            modelBuilder.AddEntityType(typeof(Job));
            // Add complex types
            modelBuilder.AddComplexType(typeof(JobAdded));
            // Add the entity sets
            modelBuilder.EntitySet<Job>("Jobs");

            modelBuilder.AddEntityType(typeof(JobInstance));
            // Add complex types
            modelBuilder.AddComplexType(typeof(JobInstanceAdded));
            // Add the entity sets
            modelBuilder.EntitySet<Job>("JobInstances");

            return Task.FromResult<ODataConventionModelBuilder>(modelBuilder);
        }

        public ConfigureServiceApiBlock()
          : base((string)null)
        {
        }
    }
}
