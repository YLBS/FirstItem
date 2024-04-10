using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Model;
using System.Text;

namespace WebApplication1.tool
{
    public static class ServiceExtension
    {
        public static void JwtAuthentication(this WebApplicationBuilder builder)
        {
            JWTTokenOptions tokenOptions = new JWTTokenOptions();//初始化
            builder.Configuration.Bind("JWTTokenOptions", tokenOptions);
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //JWT有一些默认的属性，就是给鉴权时就可以筛选了
                        ValidateIssuer = true, //是否验证Issuer
                        ValidateAudience = true, //是否验证Audience
                        ValidateIssuerSigningKey = true, //是否验证SecurityKey
                        ValidAudience = tokenOptions.Audience,
                        ValidIssuer = tokenOptions.Isuser, //Issuer,这两项和前面签发jwt的设置一致
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))
                    };
                });
        }
    }
}
