using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace StalTradeUI.Helpers
{
    public class JwtAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _config;

        public JwtAuthorizationMiddleware(RequestDelegate next, IConfiguration config)
        {
            _next = next;
            _config = config;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Cookies["JWTToken"];

            if (string.IsNullOrEmpty(token))
            {
                if (context.Request.Path == "/" || context.Request.Path == "/UserUI/LoginView" || context.Request.Path == "/UserUI/Login")
                {
                    await _next(context);
                    return;
                }
                else
                {
                    context.Response.Redirect("/UserUI/LoginView");
                    return;
                }               
            } else
            {
                if (IsTokenValid(token))
                {
                    var newToken = UpdateTokenExpiration(token, 60);
                    if (!string.IsNullOrEmpty(newToken))
                    {
                        var jwtCookie = new CookieOptions
                        {
                            HttpOnly = true,
                            Secure = true,
                            SameSite = SameSiteMode.Strict,
                            Expires = DateTime.UtcNow.AddMinutes(60)
                        };
                        context.Response.Cookies.Append("JWTToken", newToken, jwtCookie);
                        AttachUserToContext(context, newToken);
                        if (context.Request.Path == "/")
                        {
                            context.Response.Redirect("/UserUI/Index");
                            return;
                        }
                    }
                    else
                    {
                        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                        context.Response.Redirect("/UserUI/LoginView");
                        return;
                    }
                }
            }
          
            await _next(context);
        }

        private void AttachUserToContext(HttpContext context, string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_config["Jwt:Key"]);
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                var principal = tokenHandler.ValidateToken(token, parameters, out var validatedToken);
                context.User = principal;
            }
            catch
            {
                context.Response.Redirect("/UserUI/LoginView");
            }
        }

        private bool IsTokenValid(string token)
        {
            if(token == null)
                return false;

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var parameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    ValidateIssuer = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidateAudience = true,
                    ValidAudience = _config["Jwt:Audience"],
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                SecurityToken validatedToken;
                var principal = tokenHandler.ValidateToken(token, parameters, out validatedToken);
                return true;
            }
            catch (SecurityTokenExpiredException)
            {
                return false;
            }
        }


        private string UpdateTokenExpiration(string token, int minutesToAdd)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var expiration = DateTime.UtcNow.AddMinutes(minutesToAdd);
                var tokenS = tokenHandler.ReadToken(token) as JwtSecurityToken;

                var newToken = new JwtSecurityToken(
                    _config["Jwt:Issuer"],
                    _config["Jwt:Audience"],
                    tokenS.Claims,
                    DateTime.UtcNow,
                    expiration,
                    signingCredentials: credentials);

                var tokenString = tokenHandler.WriteToken(newToken);
                return tokenString;
            }
            catch
            {
                return token;
            }
        }
    }


    public static class JwtAuthorizationExtensions
    {
        public static IApplicationBuilder UseJwtAuthorization(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<JwtAuthorizationMiddleware>();
        }
    }
}
