using Notus.Models.User;
using Notus.Models.User.Dto;
using Notus.Utils;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

namespace Notus.Services
{
    public class AuthServices
    {
        private readonly UserServices _userServices;
        private readonly IEncoderServices _encoderServices;
        private readonly IMapper _mapper;
        private readonly IConfiguration _config;
        internal readonly string _secret;

        public AuthServices(UserServices userServices, IEncoderServices encoderServices, IConfiguration config, IMapper mapper)
        {
            _userServices = userServices;
            _encoderServices = encoderServices;
            _config = config;
            _secret = _config.GetSection("Secrets:JWT")?.Value?.ToString() ?? string.Empty;
            _mapper = mapper;
        }

        async public Task<List<UserWithoutPassDTO>> GetUsers()
        {
            return await _userServices.GetAll();
        }

        async public Task<UserWithoutPassDTO> Register(RegisterDTO register)
        {
            var user = await _userServices.GetOneByEmail(register.Email);
            if (user != null) {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "User already exists");
            }

            var created = await _userServices.CreateOne(register);
            return created;
        }

        async public Task<LoginResponseDTO> Login(LoginDTO login, HttpContext context)
        {
            string email = login.Email;
            var user = await _userServices.GetOneByEmail(email);

            if (user == null)
            {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "Invalid credentials");
            }

            bool IsMatch = _encoderServices.Verify(login.Password, user.Password);

            if (!IsMatch) {
                throw new HttpResponseError(HttpStatusCode.BadRequest, "Invalid credentials");
            }

            await SetCookie(user, context);
            string token = GenerateJwt(user);

            return new LoginResponseDTO {
                Token = token,
                User = _mapper.Map<UserWithoutPassDTO>(user)
            };
        }

        async public Task Logout(HttpContext context)
        {
            await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        }

        async public Task SetCookie(User user, HttpContext context)
        {
            var claims = new List<Claim>
            {
                new Claim("id", user.Id.ToString())
            };

            if (user.Roles != null || user.Roles?.Count > 0)
            {
                foreach (var role in user.Roles)
                {
                    var claim = new Claim(ClaimTypes.Role, role.Name);
                    claims.Add(claim);
                }
            }
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await context.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = true,
                    ExpiresUtc = DateTime.UtcNow.AddDays(1)
                });
        }

        public string GenerateJwt(User user)
        {
            var key = Encoding.UTF8.GetBytes(_secret);
            var symmetricKey = new SymmetricSecurityKey(key);

            var credentials = new SigningCredentials(
                symmetricKey,
                SecurityAlgorithms.HmacSha256Signature
            );

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim("id", user.Id.ToString()));

            if (user.Roles != null || user.Roles?.Count > 0)
            {
                foreach (var role in user.Roles) {
                    var claim = new Claim(ClaimTypes.Role, role.Name);
                    claims.AddClaim(claim);
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = credentials
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfig = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(tokenConfig);
            return token;
        }

    }   
}
