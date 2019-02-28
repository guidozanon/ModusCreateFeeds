using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using ModusCreate.Core.DAL.Repositories;
using ModusCreate.Core.Models;
using System.Linq;

namespace ModusCreate.Web.Controllers
{
    public class FeedController : ODataController
    {
        private readonly IRepository<Feed> _feedRepository;

        public FeedController(IRepository<Feed> feedRepository)
        {
            _feedRepository = feedRepository;
        }

        [HttpGet]
        [EnableQuery(PageSize = 50)]
        public IQueryable<Feed> Get()
        {
            return _feedRepository
                .List()
                .Where(x => !x.IsDeleted)
                .OrderBy(x => x.Name);
        }

        
    }
}
