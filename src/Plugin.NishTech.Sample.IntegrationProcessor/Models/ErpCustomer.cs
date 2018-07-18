namespace Plugin.NishTech.Sample.IntegrationProcessor
{
    using System.Collections.Generic;
    public class ErpCustomer
    {
        public string AccountNumber { get; set; }
        public string AccountStatus { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<ErpCustomerAddress> ErpCustomerAddressList {get; set;}
    }
}
