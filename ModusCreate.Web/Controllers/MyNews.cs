using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using ModusCreate.Core.DAL.Repositories;
using ModusCreate.Core.Models;
using ModusCreate.Core.Services;
using System.Linq;

namespace ModusCreate.Web.Controllers
{
    public class MyNewsController : ODataController
    {
        private readonly IRepository<News> _newsRepository;
        private readonly IFeedsService _feedService;

        public MyNewsController(IRepository<News> newsRepository, IFeedsService feedService)
        {
            _newsRepository = newsRepository;
            _feedService = feedService;
        }

        [HttpGet]
        [EnableQuery(PageSize = 10)]
        public IQueryable<News> Get()
        {
            return _feedService
                    .ListSubscribedNews()
                    .OrderByDescending(x => x.CreatedOn);
        }
    }
}
