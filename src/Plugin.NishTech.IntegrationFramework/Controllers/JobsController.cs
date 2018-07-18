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
    [Route("api/Jobs")]
    public class JobsController : CommerceController
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Plugin.NishTech.IntegrationFramework.JobsController" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="globalEnvironment">The global environment.</param>
        public JobsController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment) : base(serviceProvider, globalEnvironment)
        {
        }

        [EnableQuery]
        [Route("Jobs")]
        public async Task<IEnumerable<Job>> Get()
        {
            CommerceList<Job> commerceList = await Command<FindEntitiesInListCommand>().Process<Job>(CurrentContext, CommerceEntity.ListName<Job>(), 0, int.MaxValue);
            return commerceList?.Items.ToList() ?? new List<Job>();
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
            var entityId = $"{ CommerceEntity.IdPrefix<Job>()}{id}";
            var commerceEntity = await Command<FindEntityCommand>().Process(CurrentContext, typeof(Job), entityId, false);
            return commerceEntity != null ? new ObjectResult(commerceEntity) : (IActionResult)NotFound();
        }
    }
}
