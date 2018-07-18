namespace Plugin.NishTech.IntegrationFramework
{
    using System;
    using Sitecore.Commerce.Core;

    public class JobPolicy : Policy
    {
        public bool IsRecurringJob { get; set; }
        public DateTime? StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public string RecurrenceRepeatType { get; set; }
        public int RepeatValue { get; set; }
    }
}
