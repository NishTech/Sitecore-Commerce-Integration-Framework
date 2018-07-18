namespace Plugin.NishTech.IntegrationFramework
{

    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.OData;
    using Sitecore.Commerce.Core;
    using Sitecore.Commerce.Core.Commands;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <inheritdoc />
    /// <summary>
    /// Defines a controller
    /// </summary>
    /// <seealso cref="T:Sitecore.Commerce.Core.CommerceController" />
    [Microsoft.AspNetCore.OData.EnableQuery]
    [Route("api/JobInstances")]
    public class JobInstancesController : CommerceController
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Plugin.NishTech.IntegrationFramework.JobInstancesController" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="globalEnvironment">The global environment.</param>
        public JobInstancesController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment) : base(serviceProvider, globalEnvironment)
        {
        }

        [EnableQuery]
        [Route("JobInstances")]
        public async Task<IEnumerable<JobInstance>> Get()
        {
            CommerceList<JobInstance> commerceList = await Command<FindEntitiesInListCommand>().Process<JobInstance>(CurrentContext, CommerceEntity.ListName<JobInstance>(), 0, int.MaxValue);
            return commerceList?.Items.ToList() ?? new List<JobInstance>();
        }

        /// <summary>
        /// Gets the specified identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>A <see cref="IActionResult"/></returns>
        [HttpGet]
        [Route("(Id={id})")]
        [Microsoft.AspNetCore.OData.EnableQuery]
        public async Task<IActionResult> Get(string id)
        {
            if (!ModelState.IsValid || string.IsNullOrEmpty(id))
                return NotFound();
            var entityId = $"{ CommerceEntity.IdPrefix<JobInstance>()}{id}";
            var commerceEntity = await Command<FindEntityCommand>().Process(CurrentContext, typeof(JobInstance), entityId, false);
            return commerceEntity != null ? new ObjectResult(commerceEntity) : (IActionResult)NotFound();
        }
    }
}
