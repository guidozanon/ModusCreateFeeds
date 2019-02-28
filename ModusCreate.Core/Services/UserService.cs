using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModusCreate.Core.DAL;
using ModusCreate.Core.DAL.Domain;
using ModusCreate.Core.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ModusCreate.Core.Services
{
    public interface IUserService
    {
        User CurrentUser { get; }

        void SetCurrentUser(string username);

        Task<bool> IsValidToken(string token, TokenType type);

        Task AddToken(string token, TokenType type);

        Task RevokeToken(string token, TokenType type);

        Task<User> GetUser(string refreshToken);

        Task<IdentityResult> CreateAsync(User newUser, string password);

        Task<User> FindByEmailAsync(string email);
        Task<SignInResult> CheckPasswordSignInAsync(User user, string password);
    }

    class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly NewsFeedContext _context;
        private readonly UserManager<UserEntity> _userManager;
        private readonly SignInManager<UserEntity> _signinManager;

        public User CurrentUser { get; private set; }
        internal UserEntity CurrentUserInternal { get; private set; }

        public UserService(NewsFeedContext context, IMapper mapper, UserManager<UserEntity> userManager, SignInManager<UserEntity> signinManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _signinManager = signinManager;
        }

        public void SetCurrentUser(string username)
        {
            if (CurrentUser == null)
            {
                CurrentUserInternal = _context.Users.Where(u => u.UserName == username).FirstOrDefault();

                CurrentUser = _mapper.Map<User>(CurrentUserInternal);
            }
        }

        public async Task<bool> IsValidToken(string token, TokenType type)
        {
            if (CurrentUser == null || string.IsNullOrEmpty(token))
                return false;

            return await _context.UserJwtTokens
                .Where(t =>
                    t.UserId == CurrentUser.Id &&
                    t.Token == token &&
                    t.Type == type)
                .CountAsync() == 1;
        }

        public async Task AddToken(string token, TokenType type)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("token cannot be null or empty");

            if (CurrentUser == null)
                throw new InvalidOperationException("Cannot save a token without logged user");

            _context.UserJwtTokens.Add(new UserTokenEntity
            {
                Token = token,
                Type = type,
                UserId = CurrentUser.Id
            });

            await _context.SaveChangesAsync();
        }

        public async Task RevokeToken(string token, TokenType type)
        {
            if (string.IsNullOrEmpty(token))
                throw new ArgumentException("token cannot be null or empty");

            if (CurrentUser == null)
                throw new InvalidOperationException("Cannot revoke a token without logged user");

            var remove = await _context.UserJwtTokens
                                    .Where(t =>
                                        t.UserId == CurrentUser.Id &&
                                        t.Token == token &&
                                        t.Type == type)
                                    .FirstOrDefaultAsync();

            if (remove != null)
            {
                _context.UserJwtTokens.Remove(remove);

                await _context.SaveChangesAsync();
            }

        }

        public async Task<User> GetUser(string refreshToken)
        {
            if (!string.IsNullOrEmpty(refreshToken))
            {
                var token = await _context.UserJwtTokens
                    .Where(t => t.Token == refreshToken && t.Type == TokenType.RefreshToken)
                    .Include(t => t.User)
                    .FirstOrDefaultAsync();

                if (token != null)
                {
                    return _mapper.Map<User>(token.User);
                }
            }

            return null;
        }

        public async Task<IdentityResult> CreateAsync(User newUser, string password)
        {
            return await _userManager.CreateAsync(_mapper.Map<UserEntity>(newUser), password);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            return _mapper.Map<User>(await _userManager.FindByEmailAsync(email));
        }

        public async Task<SignInResult> CheckPasswordSignInAsync(User user, string password)
        {
            var loginUser = _mapper.Map<UserEntity>(user);

            return await _signinManager.CheckPasswordSignInAsync(loginUser, password, false);
        }
    }
}
