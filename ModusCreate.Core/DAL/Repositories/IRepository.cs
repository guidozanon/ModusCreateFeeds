using AutoMapper;
using AutoMapper.QueryableExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModusCreate.Core.DAL.Repositories
{
    public interface IRepository<TModel>
    {
        IQueryable<TModel> List();

        Task<TModel> Get(object id);
    }

    class Repository<TEntity, TModel> : IRepository<TModel>
        where TEntity : class
        where TModel : class
    {
        private readonly NewsFeedContext _context;
        private readonly IMapper _mapper;

        public Repository(NewsFeedContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<TModel> Get(object id)
        {
            return _mapper.Map<TModel>(await _context.Set<TEntity>()
                .FindAsync(id));
        }

        public IQueryable<TModel> List()
        {
            return _context.Set<TEntity>()
                .ProjectTo<TModel>(_mapper.ConfigurationProvider);
        }
    }
}
