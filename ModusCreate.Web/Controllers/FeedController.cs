using Microsoft.AspNet.OData;
using Microsoft.AspNet.OData.Routing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ModusCreate.Core.Models;
using ModusCreate.Core.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ModusCreate.Web.Controllers
{
    [Authorize]
    [ODataRoutePrefix("Feeds")]
    public class FeedsController : ODataController
    {
        private readonly IFeedsService _feedService;

        public FeedsController(IFeedsService feedService)
        {
            _feedService = feedService;
        }

        [HttpGet]
        [EnableQuery()]
        [ODataRoute("({id})")]
        public Feed Get([FromODataUri]Guid id)
        {
            return _feedService.ListAll().Where(x => x.Id == id).FirstOrDefault();
        }

        [HttpGet]

        [EnableQuery(PageSize = 50)]
        public IQueryable<Feed> Get()
        {
            return _feedService.ListAll();
        }


        // GET /Feed(id)/News
        [ODataRoute("({id})/News")]
        [EnableQuery(PageSize = 10)]
        public IQueryable<News> GetNews([FromODataUri] Guid id)
        {
            return _feedService.List(id);
        }


        [HttpGet]
        [EnableQuery(PageSize = 50)]
        public IQueryable<Feed> MyFeeds()
        {
            return _feedService.List();
        }

        [HttpPost]
        [ODataRoute("({id})")]
        public async Task<IActionResult> Subscribe([FromODataUri]Guid id)
        {
            await _feedService.SubscribeAsync(new Feed { Id = id });

            return Ok();
        }

        [HttpDelete]
        [ODataRoute("({id})")]
        public async Task<IActionResult> Unsubscribe([FromODataUri]Guid id)
        {
            await _feedService.UnubscribeAsync(new Feed { Id = id });

            return Ok();
        }
    }
}
