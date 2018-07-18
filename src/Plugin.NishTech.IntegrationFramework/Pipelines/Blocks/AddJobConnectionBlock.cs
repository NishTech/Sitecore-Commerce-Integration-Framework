namespace Plugin.NishTech.IntegrationFramework
{
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Plugin.ManagedLists;
    using Sitecore.Framework.Conditions;
    using Sitecore.Framework.Pipelines;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading.Tasks;

    /// <summary>
    /// Defines a block
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Sitecore.Framework.Pipelines.PipelineBlock{Plugin.NishTech.IntegrationFramework.AddJobConnectionArgument,
    ///         Plugin.NishTech.IntegrationFramework.JobConnection, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>
    [PipelineDisplayName("JobScheduler.block.addjobconnection")]
    public class AddJobConnectionBlock : PipelineBlock<AddJobConnectionArgument, JobConnection, CommercePipelineExecutionContext>
    {
        private readonly IDoesEntityExistPipeline _findEntityPipeline;

        public AddJobConnectionBlock(IDoesEntityExistPipeline findEntityPipeline)
          : base((string)null)
        {
            this._findEntityPipeline = findEntityPipeline;
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="arg">
        /// The AddJobConnectionArgument argument.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="JobConnection"/>.
        /// </returns>
        public override async Task<JobConnection> Run(AddJobConnectionArgument arg, CommercePipelineExecutionContext context)
        {
            Condition.Requires<AddJobConnectionArgument>(arg).IsNotNull<AddJobConnectionArgument>("AddJobConnectionArgument The argument cannot be null.");
            Condition.Requires<string>(arg.Name).IsNotNullOrEmpty("The Job Connection name cannot be null or empty.");
            Condition.Requires<string>(arg.Type).IsNotNullOrEmpty("The Job Connection type cannot be null or empty.");
            TaskAwaiter<bool> awaiter = _findEntityPipeline.Run(new FindEntityArgument(typeof(JobConnection),
                $"{(object) CommerceEntity.IdPrefix<JobConnection>()}{ arg.Name}"), context).GetAwaiter();
            if (awaiter.GetResult())
            {
                CommerceContext commerceContext = context.CommerceContext;
                string validationError = context.GetPolicy<KnownResultCodes>().ValidationError;
                string commerceTermKey = "JobConnectionNameAlreadyInUse";
                object[] args = new object[1] { (object)arg.Name };
                string defaultMessage = $"Job Connection name {(object) arg.Name} is already in use.";
                context.Abort("Ok|" + await commerceContext.AddMessage(validationError, commerceTermKey, args, defaultMessage), context);
                context = null;
                return null;
            }
            JobConnection jobConnection = new JobConnection(arg.Name, arg.Type);
            jobConnection.Id = $"{ CommerceEntity.IdPrefix<JobConnection>()}{ arg.Name}";
            jobConnection.FriendlyId = $"{arg.Name}"; ;
            jobConnection.Description = arg.Description; ;
            jobConnection.DisplayName = arg.DisplayName;
            var jobConnectionPolicy=context.GetPolicy<JobConnectionPolicy>();
            jobConnectionPolicy.DbConnectionString = arg.DbConnectionString;
            jobConnectionPolicy.WebServiceUrl = arg.WebServiceUrl;
            jobConnectionPolicy.Username = arg.Username;
            jobConnectionPolicy.Password = arg.Password;
            jobConnection.SetPolicy(jobConnectionPolicy);
            jobConnection.SetComponent(new ListMembershipsComponent()
            {
                Memberships = new List<string>()
                {
                    CommerceEntity.ListName<JobConnection>()
                }
            });
            JobConnectionAdded jobConnectionAdded = new JobConnectionAdded(jobConnection.FriendlyId);
            jobConnectionAdded.Name = jobConnection.Name;
            context.CommerceContext.AddModel(jobConnectionAdded);
            return jobConnection;

        }
    }
}
