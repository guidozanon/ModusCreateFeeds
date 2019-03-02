using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Mvc;
using ModusCreate.Core.DAL.Repositories;
using ModusCreate.Core.Models;
using System.Linq;

namespace ModusCreate.Web.Controllers
{
    public class CategoriesController : ODataController
    {
        private readonly IRepository<FeedCategory> _categoryRepository;

        public CategoriesController(IRepository<FeedCategory> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [EnableQuery(PageSize = 50)]
        public IQueryable<FeedCategory> Get()
        {
            return _categoryRepository
                .List()
                .OrderBy(x => x.Name);
        }
    }
}
