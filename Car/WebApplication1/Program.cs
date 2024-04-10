using DataEntity;
using IServer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Model;
using Server;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    #region Swagger配置支持Token参数传递 
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "请输入token,格式为 Bearer jwtToken(注意中间必须有空格)",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });//添加安全定义

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {   //添加安全要求
                    new OpenApiSecurityScheme
                    {
                        Reference =new OpenApiReference()
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id ="Bearer"
                        }
                    },
                    new string[]{ }
                }
                });
    #endregion
});

#region JWT鉴权授权

builder.Services.Configure<JWTTokenOptions>(builder.Configuration.GetSection("JWTTokenOptions"));

JWTTokenOptions tokenOptions = new JWTTokenOptions();//初始化
builder.Configuration.Bind("JWTTokenOptions", tokenOptions);//实现调用

//鉴权
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //JWT有一些默认的属性，就是给鉴权时就可以筛选了
            ValidateIssuer = true,//是否验证Issuer
            ValidateAudience = true,//是否验证Audience
            ValidateLifetime = false,//是否雅正失效时间
            ValidateIssuerSigningKey = true,//是否验证SecurityKey
            ValidAudience = tokenOptions.Audience,
            ValidIssuer = tokenOptions.Isuser,//Issuer,这两项和前面签发jwt的设置一致
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))
        };
    });
//授权
builder.Services.AddAuthorization(options =>
{
    //要求有个角色字段 --- 指定Admin //[Authorize(Roles = "Admin")] //[Authorize(Policy = "AdminPolicy")]
    options.AddPolicy("AdminPolicy", policyBuilder => policyBuilder.RequireRole("Admin"));

    //要求用户信息里面必须有Role字段,他的信息必须是Admian  //[Authorize(Policy = "AssertionAdminPolicy")]
    options.AddPolicy("AssertionAdminPolicy", policyBuilder =>
        policyBuilder.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == ClaimTypes.Role)
                && context.User.Claims.First(c => c.Type.Equals(ClaimTypes.Role)).Value == "Admin")//Claim的Role是User
     );
});

#endregion


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDirectoryBrowser();// 用于支持目录浏览功能（可选）
builder.Services.AddScoped<DbContext, CarConsignmentContext>();
builder.Services.AddScoped<ILoginService, LoginServer>();
builder.Services.AddScoped<ICarInfoSerivce, CarInfoServer>();
builder.Services.AddTransient<IJwtService, JwtService>(); 
builder.Services.AddTransient<ISmsService, SmsService>(); 

//配置文档获取连接字符串注入DbContext
builder.Services.AddDbContext<CarConsignmentContext>(options =>
{
    var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
    //options.UseSqlServer(Configuration.GetConnectionString("SQLConnection"));
});

//跨域

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("AllowAll");

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(
           Path.Combine(builder.Environment.ContentRootPath, "File")),
    RequestPath = ""
});


app.UseAuthentication();// 启用认证（先认证后授权，否则带了token也认证不通过）

app.UseAuthorization();//授权

app.MapControllers();

app.Run();
