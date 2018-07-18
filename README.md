# Sitecore Commerce Integration Framework
In most online ecommerce implementations, integration with backend systems like ERP, PIM etc. plays an important role. Most companies spend years building these systems and want to keep using them. A modern ecommerce platform like Sitecore Experience Commerce helps customers to enter in the much needed digital commerce space, but it needs a communication link to the backend system to complete  ecommerce transactions. This open source integration framework for Sitecore Commerce will give a jump start for your Sitecore Commerce project, that needs to integrate with backend system.

## How It Works
There are two plugins in this solution
- Plugin.NishTech.IntegrationFramework
This is the core framework project.
- Plugin.NishTech.Sample.IntegrationProcessor
This is a sample project that shows, how to create an integration processor.

Add these two plugins in your Sitecore Commerce solution. In the Postman folder, you will find the postman collection, that you can use to create job scheduler entities and also review them. 
In **src\Plugin.NishTech.Sample.IntegrationProcessor\SQL Scripts**, you will find some SQL scripts that can be used to load sample customer data in FakeERP database.

Add the following in the **PlugIn.AdventureWorks.CommerceMinions-1.0.0.json** and **PlugIn.Habitat.CommerceMinions-1.0.0.json** for creating the minion.
```json
      {
        "$type": "Sitecore.Commerce.Core.MinionPolicy, Sitecore.Commerce.Core",
        "WakeupInterval": "00:05:00",
        "ListToWatch": "QueuedJobs",
        "FullyQualifiedName": "Plugin.NishTech.IntegrationFramework.JobSchedulerMinion, Plugin.NishTech.IntegrationFramework",
        "ItemsPerBatch": 10,
        "SleepBetweenBatches": 500
      }
```
Bootstrap the application before using the framework.

Tested with Sitecore Commerce 9 Update 1.