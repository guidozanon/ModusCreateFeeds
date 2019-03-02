using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ModusCreate.Core.DAL;
using ModusCreate.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModusCreate.Core.Services
{
    public interface IFeedsService
    {
        /// <summary>
        /// List the user feeds
        /// </summary>
        /// <returns></returns>
        IQueryable<Feed> List();

        /// <summary>
        /// List All the feeds
        /// </summary>
        /// <returns></returns>
        IQueryable<Feed> ListAll();

        /// <summary>
        /// List of News for the specified Feed
        /// </summary>
        /// <param name="feedId"></param>
        /// <returns></returns>
        IQueryable<News> List(Guid feedId);

        /// <summary>
        /// List of News for the current user subscribed Feeds.
        /// </summary>
        /// <returns></returns>
        IQueryable<News> ListSubscribedNews();

        /// <summary>
        /// Unsubscribe the current user to the feed
        /// </summary>
        /// <param name="feed"></param>
        Task SubscribeAsync(Feed feed);
        /// <summary>
        /// Subscribe the current user to a feed
        /// </summary>
        /// <param name="feed"></param>
        Task UnubscribeAsync(Feed feed);
    }

    class FeedService : IFeedsService
    {
        private readonly IInternalUserService _userService;
        private readonly NewsFeedContext _context;
        private readonly IMapper _mapper;
        public FeedService(IInternalUserService userService, NewsFeedContext context, IMapper mapper)
        {
            _userService = userService;
            _context = context;
            _mapper = mapper;
        }

        public IQueryable<Feed> List()
        {
            if (_userService.CurrentUser != null)
                return _context.Subscriptions
                        .Where(u => u.UserId == _userService.CurrentUser.Id)
                        .Select(u => u.Feed)
                        .Where(f => !f.IsDeleted)
                        .OrderBy(x => x.Name)
                        .ProjectTo<Feed>(_mapper.ConfigurationProvider);

            return new List<Feed>().AsQueryable();
        }

        public IQueryable<News> ListSubscribedNews()
        {
            return _context.Subscriptions
                        .Where(u => u.UserId == _userService.CurrentUser.Id)
                        .Select(u => u.Feed)
                        .Where(f => !f.IsDeleted)
                        .SelectMany(f => f.News)
                        .OrderByDescending(n => n.CreatedOn)
                        .ProjectTo<News>(_mapper.ConfigurationProvider);
        }

        public IQueryable<News> List(Guid feedId)
        {
            return _context.News
                      .Where(u => u.FeedId == feedId)
                      .OrderByDescending(x => x.CreatedOn)
                      .ProjectTo<News>(_mapper.ConfigurationProvider);
        }

        public IQueryable<Feed> ListAll()
        {
            return _context.Feeds
                    .Select(x => new Feed
                    {
                        Id = x.Id,
                        Name = x.Name,
                        Category = x.Category.Name,
                        CreatedOn = x.CreatedOn,
                        DeletedOn = x.DeletedOn,
                        Description = x.Description,
                        IsDeleted = x.IsDeleted,
                        IsSubscribed = x.Subscriptions.Where(s => s.UserId == _userService.CurrentUser.Id).Any(),
                        News = _mapper.Map<ICollection<News>>(x.News)

                    });
        }

        public async Task SubscribeAsync(Feed feed)
        {
            if (_userService.CurrentUser == null)
                throw new InvalidOperationException("A user should be logger in to subscribe to a feed");

            var feedtoAdd = await _context.Feeds.FindAsync(feed.Id);

            if (feedtoAdd != null)
            {
                _context.Subscriptions.Add(new DAL.Domain.SubscriptionEntity
                {
                    FeedId = feedtoAdd.Id,
                    UserId = _userService.CurrentUserInternal.Id
                });

                await _context.SaveChangesAsync();
            }
        }

        public async Task UnubscribeAsync(Feed feed)
        {
            if (_userService.CurrentUser == null)
                throw new InvalidOperationException("A user should be logger in to unsubscribe to a feed");

            var feedtoRemove = await _context.Subscriptions.Where(s => s.UserId == _userService.CurrentUser.Id && s.FeedId == feed.Id).FirstOrDefaultAsync();
            if (feedtoRemove != null)
            {
                _context.Subscriptions.Remove(feedtoRemove);

                await _context.SaveChangesAsync();
            }
        }
    }
}
