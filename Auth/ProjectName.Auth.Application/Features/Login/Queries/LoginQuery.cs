using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ProjectName.Auth.Application.DTOs.Account;
using ProjectName.Auth.Application.Exceptions;
using ProjectName.Auth.Application.Wrappers;
using ProjectName.Auth.Domain.Entities.Identity;
using ProjectName.Auth.Domain.Settings;

namespace ProjectName.Auth.Application.Features.Login.Queries
{
    public sealed record LoginQuery(string Email, string Password) : IRequest<Response<AuthenticationResponse>>;

    public class LoginQueryHandler : IRequestHandler<LoginQuery, Response<AuthenticationResponse>>
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        //private readonly ILogger<LoginQueryHandler> _logger;
        private readonly JWTSettings _jwtSettings;

        public LoginQueryHandler(UserManager<User> userManager, SignInManager<User> signInManager, /*ILogger<LoginQueryHandler> logger,*/ IOptions<JWTSettings> jwtSettings)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            //_logger = logger;
            _jwtSettings = jwtSettings.Value;
        }
        public async Task<Response<AuthenticationResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException($"No accounts registered with {request.Email}.");
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

            if (result.Succeeded == false)
            {
                throw new ApiException($"Invalid Credentials for '{request.Email}'.");
            }
            if (user.EmailConfirmed == false)
            {
                throw new ApiException($"Account Not Confirmed for '{request.Email}'.");
            }
            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);
            var response = new AuthenticationResponse
            {
                Id = user.Id,
                JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                IsVerified = user.EmailConfirmed,
                Roles = (await _userManager.GetRolesAsync(user)
                .ConfigureAwait(false))
                .ToList(),
            };
            return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
        }

        private async Task<JwtSecurityToken> GenerateJWToken(User user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            //string ipAddress = IpHelper.GetIpAddress();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", (user.Id).ToString()),
                //new Claim("ip", ipAddress)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}
