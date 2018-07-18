namespace Plugin.NishTech.Sample.IntegrationProcessor
{
    using System;
    using System.Linq;
    using System.Data.SqlClient;
    using Sitecore.Framework.Conditions;
    using Sitecore.Commerce.EntityViews;
    using System.Collections.Generic;
    using Sitecore.Commerce.Core;
    using Sitecore.Framework.Pipelines;
    using System.Threading.Tasks;
    using Plugin.NishTech.IntegrationFramework;
    using Sitecore.Commerce.Plugin.Customers;

    /// <summary>
    /// Defines a block
    /// </summary>
    /// <seealso>
    ///     <cref>
    ///         Plugin.NishTech.Sample.IntegrationProcessor.CustomerSqlProcessorBlock{Plugin.NishTech.IntegrationFramework.ProductProcessorBlock,
    ///         Plugin.NishTech.IntegrationFramework.JobInfoArgument, Sitecore.Commerce.Core.CommercePipelineExecutionContext}
    ///     </cref>
    /// </seealso>
    [PipelineDisplayName("IntegrationProcessor.block.sql.customer")]
    public class CustomerSqlProcessorBlock : PipelineBlock<JobInfoArgument, JobInfoArgument, CommercePipelineExecutionContext>
    {
        private readonly IGetEntityViewPipeline _getEntityViewPipeline;
        private readonly IDoActionPipeline _doActionPipeline;
        private readonly IFindEntityPipeline _findEntityPipeline;

        public CustomerSqlProcessorBlock(IGetEntityViewPipeline getEntityViewPipeline, IDoActionPipeline doActionPipeline, IFindEntityPipeline findEntityPipeline)
            : base((string)null)
        {
            _getEntityViewPipeline = getEntityViewPipeline;
            _doActionPipeline = doActionPipeline;
            _findEntityPipeline = findEntityPipeline;
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
            if (arg == null || arg.JobConnection ==null || !arg.JobConnection.Type.Equals("sql server", StringComparison.OrdinalIgnoreCase))
                return arg;
            Condition.Requires(arg).IsNotNull("JobInfoArgument: The argument cannot be null.");
            Condition.Requires(arg.JobConnection).IsNotNull("JobInfoArgument: JobConnection cannot be null.");
            Condition.Requires(arg.Job).IsNotNull("JobInfoArgument: Job cannot be null.");
            Condition.Requires(arg.JobInstance).IsNotNull("JobInfoArgument: JobInstance cannot be null.");
            try
            {
                var customersFromERP = GetCustomersFromERP(arg);

                if (customersFromERP.Any())
                {
                    foreach (var erpCustomer in customersFromERP)
                    {
                        var customer = await this._findEntityPipeline.Run(new FindEntityArgument(typeof(Customer),
                            $"{(object)CommerceEntity.IdPrefix<Customer>()}{erpCustomer.AccountNumber}"), context) as Customer;
                        if (customer == null)
                        {
                            await AddCustomer(erpCustomer, context);
                        }
                        else
                        {
                            await EditCustomer(erpCustomer, customer, context);
                        }

                        if (context.CommerceContext.HasErrors())
                        {
                            arg.JobInstance.Status = "Failure";
                            return arg;
                        }
                    }
                    arg.JobInstance.Status = "Success";
                }
                //var messages = context.CommerceContext.GetMessages();
            }
            catch (Exception ex)
            {
                string str = await context.CommerceContext.AddMessage(context.GetPolicy<KnownResultCodes>().Error, "Error", new object[1]
                {
                    arg.JobInstance.Id,
                }, $"Error occurred for {arg.JobInstance.Id}, Error Message: {ex.Message}");
                arg.JobInstance.Status = "Failure";
            }
            return arg ;
        }


        private async Task AddCustomer(ErpCustomer erpCustomer, CommercePipelineExecutionContext context)
        {
            var entityView=new EntityView();
            entityView.EntityId = $"{(object)CommerceEntity.IdPrefix<Customer>()}{erpCustomer.AccountNumber}";
            entityView.Name = "Details";
            entityView.DisplayName = "Details";
            entityView.Action = "AddCustomer";
            entityView.ChildViews=new List<Model>();
            entityView.DisplayRank = 500;
            entityView.UiHint = "Flat";
            entityView.Icon = "chart_column_stacked";
            entityView.Properties=new List<ViewProperty>()
            {
                new ViewProperty()
                {
                    Name = "Version",
                    DisplayName = "Version",
                    Value = "1",
                    IsHidden = true,
                    OriginalType = "System.Int32",
                    IsRequired = true,
                    IsReadOnly = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                },
                new ViewProperty()
                {
                    Name = "FirstName",
                    DisplayName = "First Name",
                    Value = erpCustomer.FirstName,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "e9150513558d4ba080accc7fa84fad00"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "LastName",
                    DisplayName = "Last Name",
                    Value = erpCustomer.LastName,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "70fdc77e288143c5ae050fe171ffffd3"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "Email",
                    DisplayName = "Email",
                    Value = erpCustomer.Email,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "960bae42b5084ae48b3c0770260587aa"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "AccountStatus",
                    DisplayName = "Account Status",
                    Value = erpCustomer.AccountStatus,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new AvailableSelectionsPolicy()
                        {
                            Models = new List<Model>(),
                            PolicyId = "3250891c884c4ba08c5fff2835cdc914",
                            AllowMultiSelect = false,
                            List = new List<Selection>()
                            {
                                new Selection()
                                {
                                    Name = "ActiveAccount",
                                    Policies = new List<Policy>(),
                                    DisplayName = "Active",
                                    IsDefault = false
                                },
                                new Selection()
                                {
                                    Name = "InactiveAccount",
                                    Policies = new List<Policy>(),
                                    DisplayName = "Inactive",
                                    IsDefault = false
                                },
                                new Selection()
                                {
                                    Name = "RequiresApproval",
                                    Policies = new List<Policy>(),
                                    DisplayName = "Requires Approval",
                                    IsDefault = false
                                }
                            }
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "PhoneNumber",
                    DisplayName = "Phone",
                    Value = erpCustomer.PhoneNumber,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 50,
                            Models = new List<Model>(),
                            PolicyId = "30b9ad5b9e014b3a8697967280cd2596"
                        }
                    }
                }
                //new ViewProperty()
                //{
                //    Name = "Language",
                //    DisplayName = "Language",
                //    Value = "en",
                //    IsHidden = false,
                //    OriginalType = "System.String",
                //    IsRequired = true,
                //    UiType = string.Empty,
                //    Policies = new List<Policy>()
                //    {
                //        new AvailableSelectionsPolicy()
                //        {
                //            Models = new List<Model>(),
                //            PolicyId = "beaebd811a8747db8b0577c4165f4118",
                //            AllowMultiSelect = false,
                //            List = new List<Selection>()
                //            {
                //                new Selection()
                //                {
                //                    Name = "en",
                //                    Policies = new List<Policy>(),
                //                    DisplayName = "en",
                //                    IsDefault = false
                //                },
                //                new Selection()
                //                {
                //                    Name = "de-DE",
                //                    Policies = new List<Policy>(),
                //                    DisplayName = "de-DE",
                //                    IsDefault = false
                //                },
                //                new Selection()
                //                {
                //                    Name = "fr-FR",
                //                    Policies = new List<Policy>(),
                //                    DisplayName = "fr-FR",
                //                    IsDefault = false
                //                }
                //            }
                //        }
                //    }
                //}
            };

            var viewCustomer = await _doActionPipeline.Run(entityView, context);

            foreach (var erpCustomerAddress in erpCustomer.ErpCustomerAddressList)
            {
                await AddAddress(erpCustomerAddress, viewCustomer.EntityId, context);
            }
        }

        private async Task EditCustomer(ErpCustomer erpCustomer, Customer customer, CommercePipelineExecutionContext context)
        {
            var entityView = new EntityView();
            entityView.EntityId = customer.Id;
            entityView.Name = "Details";
            entityView.DisplayName = "Details";
            entityView.Action = "EditCustomer";
            entityView.ChildViews = new List<Model>();
            entityView.DisplayRank = 500;
            entityView.UiHint = "Flat";
            entityView.Icon = "chart_column_stacked";
            entityView.Properties = new List<ViewProperty>()
            {
                new ViewProperty()
                {
                    Name = "Version",
                    DisplayName = "Version",
                    Value = customer.Version.ToString(),
                    IsHidden = true,
                    OriginalType = "System.Int32",
                    IsRequired = true,
                    IsReadOnly = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                },
                new ViewProperty()
                {
                    Name = "FirstName",
                    DisplayName = "First Name",
                    Value = erpCustomer.FirstName,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "e9150513558d4ba080accc7fa84fad00"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "LastName",
                    DisplayName = "Last Name",
                    Value = erpCustomer.LastName,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "70fdc77e288143c5ae050fe171ffffd3"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "Email",
                    DisplayName = "Email",
                    Value = erpCustomer.Email,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "960bae42b5084ae48b3c0770260587aa"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "AccountStatus",
                    DisplayName = "Account Status",
                    Value = erpCustomer.AccountStatus,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new AvailableSelectionsPolicy()
                        {
                            Models = new List<Model>(),
                            PolicyId = "3250891c884c4ba08c5fff2835cdc914",
                            AllowMultiSelect = false,
                            List = new List<Selection>()
                            {
                                new Selection()
                                {
                                    Name = "ActiveAccount",
                                    Policies = new List<Policy>(),
                                    DisplayName = "Active",
                                    IsDefault = false
                                },
                                new Selection()
                                {
                                    Name = "InactiveAccount",
                                    Policies = new List<Policy>(),
                                    DisplayName = "Inactive",
                                    IsDefault = false
                                },
                                new Selection()
                                {
                                    Name = "RequiresApproval",
                                    Policies = new List<Policy>(),
                                    DisplayName = "Requires Approval",
                                    IsDefault = false
                                }
                            }
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "PhoneNumber",
                    DisplayName = "Phone",
                    Value = erpCustomer.PhoneNumber,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 50,
                            Models = new List<Model>(),
                            PolicyId = "30b9ad5b9e014b3a8697967280cd2596"
                        }
                    }
                }
                //new ViewProperty()
                //{
                //    Name = "Language",
                //    DisplayName = "Language",
                //    Value = "en",
                //    IsHidden = false,
                //    OriginalType = "System.String",
                //    IsRequired = true,
                //    UiType = string.Empty,
                //    Policies = new List<Policy>()
                //    {
                //        new AvailableSelectionsPolicy()
                //        {
                //            Models = new List<Model>(),
                //            PolicyId = "beaebd811a8747db8b0577c4165f4118",
                //            AllowMultiSelect = false,
                //            List = new List<Selection>()
                //            {
                //                new Selection()
                //                {
                //                    Name = "en",
                //                    Policies = new List<Policy>(),
                //                    DisplayName = "en",
                //                    IsDefault = false
                //                },
                //                new Selection()
                //                {
                //                    Name = "de-DE",
                //                    Policies = new List<Policy>(),
                //                    DisplayName = "de-DE",
                //                    IsDefault = false
                //                },
                //                new Selection()
                //                {
                //                    Name = "fr-FR",
                //                    Policies = new List<Policy>(),
                //                    DisplayName = "fr-FR",
                //                    IsDefault = false
                //                }
                //            }
                //        }
                //    }
                //}
            };

            var viewCustomer = await _doActionPipeline.Run(entityView, context);
            foreach (var erpCustomerAddress in erpCustomer.ErpCustomerAddressList)
            {
                await EditAddress(erpCustomerAddress, customer, context);
            }
        }

        private async Task AddAddress(ErpCustomerAddress erpCustomerAddress, string customerId, CommercePipelineExecutionContext context)
        {
            var entityView = new EntityView();
            entityView.EntityId = customerId;
            entityView.ItemId = $"{(object)CommerceEntity.IdPrefix<AddressComponent>()}{customerId}-{erpCustomerAddress.AddressName}";
            entityView.Name = "AddressDetails";
            entityView.DisplayName = "Address Details";
            entityView.Action = "AddAddress";
            entityView.ChildViews = new List<Model>();
            entityView.DisplayRank = 500;
            entityView.UiHint = "Flat";
            entityView.Properties = new List<ViewProperty>()
            {
                new ViewProperty()
                {
                    Name = "Version",
                    DisplayName = "Version",
                    Value = "1",
                    IsHidden = true,
                    OriginalType = "System.Int32",
                    IsRequired = true,
                    IsReadOnly = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                },
                new ViewProperty()
                {
                    Name = "Country",
                    DisplayName = "Country",
                    Value = erpCustomerAddress.Country,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                },
                new ViewProperty()
                {
                    Name = "CountryCode",
                    DisplayName = "Country Code",
                    Value = erpCustomerAddress.CountryCode,
                    IsHidden = true,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                },
                new ViewProperty()
                {
                    Name = "AddressName",
                    DisplayName = "Address Name",
                    Value = erpCustomerAddress.AddressName,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "af3a53fe862d4bb190da30b5f9e5b9f7"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "FirstName",
                    DisplayName = "First Name",
                    Value = erpCustomerAddress.FirstName,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "5e4005cba57c4fee97d60db399be02ba"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "LastName",
                    DisplayName = "Last Name",
                    Value = erpCustomerAddress.LastName,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "ad94b1f4f4554e168da7a3d9ae935507"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "Address1",
                    DisplayName = "Address1",
                    Value = erpCustomerAddress.Address1,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = true,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "59bf5f09efb8495db4b5be8247351a86"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "Address2",
                    DisplayName = "Address2",
                    Value = erpCustomerAddress.Address2,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = true,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "3dd39459b4d049099fe0cd50762e3267"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "City",
                    DisplayName = "City",
                    Value = erpCustomerAddress.City,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "c4c3217dd6714714b591a46b44bd9360"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "State",
                    DisplayName = "State",
                    Value = erpCustomerAddress.State,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 50,
                            Models = new List<Model>(),
                            PolicyId = "50cf776c3082414ba7d036e6ff387e23"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "StateCode",
                    DisplayName = "State Code",
                    Value = erpCustomerAddress.StateCode,
                    IsHidden = true,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                },
                new ViewProperty()
                {
                    Name = "ZipPostalCode",
                    DisplayName = "Zip/Postal Code",
                    Value = erpCustomerAddress.ZipPostalCode,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "f6267895a15442938fdb156ac54c730a"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "IsPrimary",
                    DisplayName = "IsPrimary",
                    Value = erpCustomerAddress.IsPrimary.ToString(),
                    IsHidden = false,
                    OriginalType = "System.Boolean",
                    IsRequired = true,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                },
                new ViewProperty()
                {
                    Name = "PhoneNumber",
                    DisplayName = "Phone",
                    Value = erpCustomerAddress.PhoneNumber,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 50,
                            Models = new List<Model>(),
                            PolicyId = "07a47a75dda0433e9f2bfcccceadb6aa"
                        }
                    }
                }
            };

            var viewAddress = await _doActionPipeline.Run(entityView, context);
        }

        private async Task EditAddress(ErpCustomerAddress erpCustomerAddress, Customer customer, CommercePipelineExecutionContext context)
        {
            var entityView = new EntityView();
            entityView.EntityId = customer.Id;
            entityView.ItemId = customer.Components[2].Id;
            entityView.Name = "AddressDetails";
            entityView.DisplayName = "Address Details";
            entityView.Action = "EditAddress";
            entityView.ChildViews = new List<Model>();
            entityView.DisplayRank = 500;
            entityView.UiHint = "Flat";
            entityView.Properties = new List<ViewProperty>()
            {
                new ViewProperty()
                {
                    Name = "Version",
                    DisplayName = "Version",
                    Value = customer.Version.ToString(),
                    IsHidden = true,
                    OriginalType = "System.Int32",
                    IsRequired = true,
                    IsReadOnly = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                },
                new ViewProperty()
                {
                    Name = "Country",
                    DisplayName = "Country",
                    Value = erpCustomerAddress.Country,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                },
                new ViewProperty()
                {
                    Name = "CountryCode",
                    DisplayName = "Country Code",
                    Value = erpCustomerAddress.CountryCode,
                    IsHidden = true,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                },
                new ViewProperty()
                {
                    Name = "AddressName",
                    DisplayName = "Address Name",
                    Value = erpCustomerAddress.AddressName,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "af3a53fe862d4bb190da30b5f9e5b9f7"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "FirstName",
                    DisplayName = "First Name",
                    Value = erpCustomerAddress.FirstName,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "5e4005cba57c4fee97d60db399be02ba"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "LastName",
                    DisplayName = "Last Name",
                    Value = erpCustomerAddress.LastName,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "ad94b1f4f4554e168da7a3d9ae935507"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "Address1",
                    DisplayName = "Address1",
                    Value = erpCustomerAddress.Address1,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = true,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "59bf5f09efb8495db4b5be8247351a86"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "Address2",
                    DisplayName = "Address2",
                    Value = erpCustomerAddress.Address2,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = true,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "3dd39459b4d049099fe0cd50762e3267"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "City",
                    DisplayName = "City",
                    Value = erpCustomerAddress.City,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "c4c3217dd6714714b591a46b44bd9360"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "State",
                    DisplayName = "State",
                    Value = erpCustomerAddress.State,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 50,
                            Models = new List<Model>(),
                            PolicyId = "50cf776c3082414ba7d036e6ff387e23"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "StateCode",
                    DisplayName = "State Code",
                    Value = erpCustomerAddress.StateCode,
                    IsHidden = true,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = true,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                },
                new ViewProperty()
                {
                    Name = "ZipPostalCode",
                    DisplayName = "Zip/Postal Code",
                    Value = erpCustomerAddress.ZipPostalCode,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 100,
                            Models = new List<Model>(),
                            PolicyId = "f6267895a15442938fdb156ac54c730a"
                        }
                    }
                },
                new ViewProperty()
                {
                    Name = "IsPrimary",
                    DisplayName = "IsPrimary",
                    Value = erpCustomerAddress.IsPrimary.ToString(),
                    IsHidden = false,
                    OriginalType = "System.Boolean",
                    IsRequired = true,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                },
                new ViewProperty()
                {
                    Name = "PhoneNumber",
                    DisplayName = "Phone",
                    Value = erpCustomerAddress.PhoneNumber,
                    IsHidden = false,
                    OriginalType = "System.String",
                    IsRequired = false,
                    IsReadOnly = false,
                    UiType = string.Empty,
                    Policies = new List<Policy>()
                    {
                        new MaxLengthPolicy()
                        {
                            MaxLengthAllow = 50,
                            Models = new List<Model>(),
                            PolicyId = "07a47a75dda0433e9f2bfcccceadb6aa"
                        }
                    }
                }
            };

            var viewAddress = await _doActionPipeline.Run(entityView, context);
        }
        private IList<ErpCustomer> GetCustomersFromERP(JobInfoArgument jobInfoArgument)
        {
            var connectionString = jobInfoArgument.JobConnection.GetPolicy<JobConnectionPolicy>().DbConnectionString;
            Condition.Requires(connectionString).IsNotNullOrEmpty("GetCustomersFromERP: connectionString cannot be null or empty.");
            Condition.Requires(jobInfoArgument.Job.SqlQuery).IsNotNullOrEmpty("GetCustomersFromERP: Job.SqlQuery cannot be null or empty.");
            var customerList = new List<ErpCustomer>();
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                var query = jobInfoArgument.Job.SqlQuery;

                using (var command = new SqlCommand(query, sqlConnection))
                {
                    command.CommandTimeout = 90;
                    var reader=command.ExecuteReader();
                    while (reader.Read())
                    {
                        var erpCustomerAddress = new ErpCustomerAddress()
                        {
                            Address1 = reader.GetString(reader.GetOrdinal("Address1")),
                            Address2 = reader.GetString(reader.GetOrdinal("Address2")),
                            AddressName = reader.GetString(reader.GetOrdinal("AddressName")),
                            FirstName = reader.GetString(reader.GetOrdinal("AddressFirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("AddressLastName")),
                            City = reader.GetString(reader.GetOrdinal("City")),
                            Company = reader.GetString(reader.GetOrdinal("Company")),
                            Country = reader.GetString(reader.GetOrdinal("Country")),
                            CountryCode = reader.GetString(reader.GetOrdinal("CountryCode")),
                            IsPrimary = reader.GetBoolean(reader.GetOrdinal("IsPrimary")),
                            PhoneNumber = reader.GetString(reader.GetOrdinal("PhoneNumber")),
                            State = reader.GetString(reader.GetOrdinal("State")),
                            StateCode = reader.GetString(reader.GetOrdinal("StateCode")),
                            ZipPostalCode = reader.GetString(reader.GetOrdinal("ZipCode")),
                            Email = reader.GetString(reader.GetOrdinal("Email"))
                        };
                        var erpCustomerAddressList = new List<ErpCustomerAddress>
                        {
                            erpCustomerAddress
                        };
                        var erpCustomer = new ErpCustomer()
                        {
                            AccountNumber = reader.GetString(reader.GetOrdinal("CustomerAccountNumber")),
                            AccountStatus = reader.GetString(reader.GetOrdinal("CustomerAccountStatus")),
                            Email = reader.GetString(reader.GetOrdinal("CustomerEmail")),
                            PhoneNumber = reader.GetString(reader.GetOrdinal("CustomerPhoneNumber")),
                            FirstName = reader.GetString(reader.GetOrdinal("CustomerFirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("CustomerLastName")),
                            ErpCustomerAddressList = erpCustomerAddressList
                        };
                        customerList.Add(erpCustomer);
                    }
                }
            }
            return customerList;
        }
    }
}
