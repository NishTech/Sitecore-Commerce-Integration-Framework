namespace Plugin.NishTech.Sample.IntegrationProcessor
{
    using System;
    using System.Linq;
    using Sitecore.Commerce.EntityViews;
    using System.Collections.Generic;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;
    using System.Threading.Tasks;
    using Plugin.NishTech.IntegrationFramework;
    /// <summary>
    /// Defines a block
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Plugin.NishTech.Sample.IntegrationProcessor.CustomerFlatFileProcessorBlock{Plugin.NishTech.IntegrationFramework.ProductProcessorBlock,
    ///         Plugin.NishTech.IntegrationFramework.JobInfoArgument, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>
    [PipelineDisplayName("IntegrationProcessor.block.flatfile.customer")]
    public class CustomerFlatFileProcessorBlock : PipelineBlock<JobInfoArgument, JobInfoArgument, CommercePipelineExecutionContext>
    {
        private readonly IGetEntityViewPipeline _getEntityViewPipeline;
        private readonly IDoActionPipeline _doActionPipeline;

        public CustomerFlatFileProcessorBlock(IGetEntityViewPipeline getEntityViewPipeline, IDoActionPipeline doActionPipeline)
            : base((string)null)
        {
            _getEntityViewPipeline = getEntityViewPipeline;
            _doActionPipeline = doActionPipeline;
        }

        /// <summary>
        /// The execute.
        /// </summary>
        /// <param name="arg">
        /// The SampleArgument argument.
        /// </param>
        /// <param name="context">
        /// The context.
        /// </param>
        /// <returns>
        /// The <see cref="JobInfoArgument"/>.
        /// </returns>
        public override async Task<JobInfoArgument> Run(JobInfoArgument arg, CommercePipelineExecutionContext context)
        {
            if (arg == null || arg.JobConnection ==null || !arg.JobConnection.Type.Equals("flat file", StringComparison.OrdinalIgnoreCase))
                return arg;
            try
            {
                var customersFromERP = GetCustomersFromFlatFile(arg.JobConnection);
                var entityViewArg = new EntityViewArgument();
                entityViewArg.ViewName = "Details";
                entityViewArg.ForAction = "AddCustomer";
                var view = await _getEntityViewPipeline.Run(entityViewArg, context);
                view.Properties.FirstOrDefault(v => v.Name.Equals("FirstName", StringComparison.OrdinalIgnoreCase)).Value =
                    customersFromERP.FirstName;
                view.Properties.FirstOrDefault(v => v.Name.Equals("LastName", StringComparison.OrdinalIgnoreCase)).Value =
                    customersFromERP.LastName;
                view.Properties.FirstOrDefault(v => v.Name.Equals("email", StringComparison.OrdinalIgnoreCase)).Value =
                    customersFromERP.Email;
                view.Properties.FirstOrDefault(v => v.Name.Equals("AccountStatus", StringComparison.OrdinalIgnoreCase)).Value =
                    customersFromERP.AccountStatus;
                //view.Properties.FirstOrDefault(v => v.Name.Equals("Language", StringComparison.OrdinalIgnoreCase)).Value =context.CommerceContext.CurrentLanguage();
                view = await _doActionPipeline.Run(view, context);
                if (context.CommerceContext.HasErrors())
                {
                    arg.JobInstance.Status = "Failure";
                }
                else
                {
                    arg.JobInstance.Status = "Success";
                }
                //var messages = context.CommerceContext.GetMessages();
            }
            catch (Exception ex)
            {
                string str = await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Error, "Error", new object[1]
                {
                    arg.JobInstance.FriendlyId,
                }, ex.Message);
                arg.JobInstance.Status = "Failure";
            }
            return arg ;
        }

        private ErpCustomer GetCustomersFromFlatFile(JobConnection argJobConnection)
        {
            var erpCustomerAddress=new ErpCustomerAddress()
            {
                Address1 = "1234 xyz street",
                Address2 = string.Empty,
                AddressName = "my address",
                City = "Dublin",
                Company = "1",
                Country = "USA",
                CountryCode = "US",
                IsPrimary = true,
                PhoneNumber = "9999999999",
                State = "Ohio",
                StateCode = "OH",
                ZipPostalCode = "43034",
                Email = "test@yahoo.com"
            };
            var erpCustomerAddressList = new List<ErpCustomerAddress>();
            erpCustomerAddressList.Add(erpCustomerAddress);
            var erpCustomer = new ErpCustomer()
            {
                AccountNumber = "0002",
                AccountStatus = "Active",
                Email = "test@gmail.com",
                ErpCustomerAddressList = erpCustomerAddressList,
                FirstName = "Test",
                LastName = "Test",
            };
            return erpCustomer;
        }
    }
}
