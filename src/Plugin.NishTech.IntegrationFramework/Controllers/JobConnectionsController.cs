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
    [Route("api/JobConnections")]
    public class JobConnectionsController : CommerceController
    {
        /// <inheritdoc />
        /// <summary>
        /// Initializes a new instance of the <see cref="T:Plugin.NishTech.IntegrationFramework.JobConnectionsController" /> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <param name="globalEnvironment">The global environment.</param>
        public JobConnectionsController(IServiceProvider serviceProvider, CommerceEnvironment globalEnvironment) : base(serviceProvider, globalEnvironment)
        {
        }

        [EnableQuery]
        [Route("JobConnections")]
        public async Task<IEnumerable<JobConnection>> Get()
        {
            CommerceList<JobConnection> commerceList = await Command<FindEntitiesInListCommand>().Process<JobConnection>(CurrentContext, CommerceEntity.ListName<JobConnection>(), 0, int.MaxValue);
            return (commerceList?.Items.ToList()) ?? new List<JobConnection>();
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
            string entityId = $"{ CommerceEntity.IdPrefix<JobConnection>()}{id}";
            CommerceEntity commerceEntity = await Command<FindEntityCommand>().Process(CurrentContext, typeof(JobConnection), entityId, false);
            return commerceEntity != null ? new ObjectResult(commerceEntity) : (IActionResult)NotFound();
        }
    }
}
