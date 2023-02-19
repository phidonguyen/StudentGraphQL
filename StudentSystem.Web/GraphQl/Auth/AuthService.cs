using System.Security.Authentication;
using System.Security.Claims;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentSystem.DataAccess.EntityFramework;
using StudentSystem.DataAccess.EntityFramework.Entities;
using StudentSystem.Web.Common.Constants;
using StudentSystem.Web.Common.Helpers;
using StudentSystem.Web.Configurations;
using StudentSystem.Web.GraphQl._Base;
using SystemTech.Core.JwtManager;
using SystemTech.Core.Utils;

namespace StudentSystem.Web.GraphQl.Auth
{
    public class AuthService : BaseService
    {
        private readonly IMapper _mapper;
        private readonly IJwtManagerService _jwtManagerService;
        private readonly double _refreshTokenExpiryDuration;
        private readonly double _accessTokenExpiryDuration;

        public AuthService(IMapper mapper,
            IHttpContextAccessor httpContextAccessor,
            IJwtManagerService jwtManagerService,
            IConfiguration configuration,
            StudentSystemDbContextFactory dbContextFactory) : base(httpContextAccessor, dbContextFactory)
        {
            var appSettings = configuration.Get<AppSettings>();
            _mapper = mapper;
            _jwtManagerService = jwtManagerService;
            _refreshTokenExpiryDuration = appSettings.Jwt.RefreshTokenExpiryDuration;
            _accessTokenExpiryDuration = appSettings.Jwt.AccessTokenExpiryDuration;
        }

        #region Write

        public async Task<Token> Login(LoginInput loginInput, CancellationToken cancellationToken)
        {
            User user = await DbContext.Users.FirstOrDefaultAsync(_ => _.Email == loginInput.Email, cancellationToken);

            if (user == null)
                throw new ValidationException($"Email [{loginInput.Email}] does not exist.");

            await ValidateLoggedUser(user, loginInput, cancellationToken);

            var token = await CreateTokenForUser(user, cancellationToken);
            
            return token;
        }

        public async Task<Token> RefreshToken(RefreshTokenInput input, CancellationToken cancellationToken)
        {
            ClaimsPrincipal claimsPrincipal = _jwtManagerService.GetPrincipalFromExpiredToken(input.AccessToken);
            string userId = claimsPrincipal.GetCurrentUserId();

            Token token = await DbContext.Tokens.FirstOrDefaultAsync(_ => _.RefreshToken == input.RefreshToken 
                                                                          && _.UserId == userId, cancellationToken);
            if (token == null || token.RefreshTokenExpired <= DateTime.Now)
                throw new AuthenticationException("Refresh token expired.");

            User user = AuthHelpers.ExtractCurrentUser(claimsPrincipal);
            ClaimsIdentity claimsIdentity = AuthHelpers.ArchiveCurrentUser(user);

            token.AccessToken = _jwtManagerService.GenerateAccessToken(claimsIdentity);
            token.AccessTokenExpired = DateTime.Now.AddMinutes(_accessTokenExpiryDuration);
            token.RefreshTokenExpired = DateTime.Now.AddMinutes(_refreshTokenExpiryDuration);

            var accessTokenEntry = DbContext.Update(token);

            await DbContext.SaveChangesAsync(cancellationToken);
            return accessTokenEntry.Entity;
        }

        #endregion

        #region Helper

        private async Task ValidateLoggedUser(User userDb, LoginInput input, CancellationToken cancellationToken)
        {
            if (userDb.Status == (int)UserStatusEnum.Banned)
                throw new UnauthorizedAccessException("Your account is locked. Contact admin.");

            if (!CryptoHelpers.VerifyPassword(input.Password, userDb.Password))
            {
                var numberLoginAttempt = await LoginFailedHandler(userDb, cancellationToken);
                int remainingAttemptTimes = GeneralConstants.MaxLoginAttempt - numberLoginAttempt;
                string message = "The password is incorrect.";

                if (numberLoginAttempt > 0 && remainingAttemptTimes > 0)
                    message = $"The password is incorrect. You have {remainingAttemptTimes} attempts remaining.";
                if (numberLoginAttempt > 0 && remainingAttemptTimes == 0)
                    message = "Your account is locked.";

                throw new AuthenticationException(message);
            }
        }
        
        private async Task<int> LoginFailedHandler(User user, CancellationToken cancellationToken)
        {
            // tracking user login failed
            HistoryLogin historyLogin = new()
            {
                Ip = IpAddress,
                UserAgent = UserAgent,
                UserId = user.Id,
                IsSuccess = false
            };
            DbContext.Add(historyLogin);
            await DbContext.SaveChangesAsync(cancellationToken);
            
            // checking user login attempt
            var loginAttempts = DbContext.HistoryLogins
                .Where(_ => _.UserId == user.Id)
                .OrderByDescending(_ => _.CreatedDate)
                .AsEnumerable()
                .TakeWhile(_ => !_.IsSuccess)
                .Take(GeneralConstants.MaxLoginAttempt);

            int numberLoginAttempt = loginAttempts.Count();

            if (numberLoginAttempt == GeneralConstants.MaxLoginAttempt)
            {
                user.Status = (int)UserStatusEnum.Banned;
                DbContext.Update(user);
            }

            return numberLoginAttempt;
        }
        
        private async Task<Token> CreateTokenForUser(User user, CancellationToken cancellationToken)
        {
            ClaimsIdentity claimsIdentity = AuthHelpers.ArchiveCurrentUser(user);
            string accessToken = _jwtManagerService.GenerateAccessToken(claimsIdentity);
            string refreshToken = _jwtManagerService.GenerateRefreshToken();
            
            Token token = new()
            {
                UserId = user.Id,
                AccessToken = accessToken,
                AccessTokenExpired = DateTime.Now.AddMinutes(_accessTokenExpiryDuration),
                RefreshToken = refreshToken,
                RefreshTokenExpired = DateTime.Now.AddMinutes(_refreshTokenExpiryDuration)
            };
            var tokenEntry = DbContext.Add(token);

            // tracking login
            var historyLogin = new HistoryLogin
            {
                Ip = IpAddress,
                UserAgent = UserAgent,
                UserId = user.Id,
                TokenId = tokenEntry.Entity.Id,
                IsSuccess = true
            };
            DbContext.Add(historyLogin);
            
            await DbContext.SaveChangesAsync(cancellationToken);
            return tokenEntry.Entity;
        }
        
        #endregion
    }
}