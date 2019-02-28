using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using ModusCreate.Core.DAL.Repositories;
using ModusCreate.Core.Models;
using System.Linq;

namespace ModusCreate.Web.Controllers
{
    public class NewsController : ODataController
    {
        private readonly IRepository<News> _newsRepository;

        public NewsController(IRepository<News> newsRepository)
        {
            _newsRepository = newsRepository;
        }

        [HttpGet]
        [EnableQuery(PageSize = 50)]
        public IQueryable<News> Get()
        {
            return _newsRepository
                .List()
                .OrderByDescending(x => x.CreatedOn);
        }
    }
}
