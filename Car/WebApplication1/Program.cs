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
    #region Swagger����֧��Token�������� 
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "������token,��ʽΪ Bearer jwtToken(ע���м�����пո�)",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = JwtBearerDefaults.AuthenticationScheme
    });//��Ӱ�ȫ����

    options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {   //��Ӱ�ȫҪ��
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

#region JWT��Ȩ��Ȩ

builder.Services.Configure<JWTTokenOptions>(builder.Configuration.GetSection("JWTTokenOptions"));

JWTTokenOptions tokenOptions = new JWTTokenOptions();//��ʼ��
builder.Configuration.Bind("JWTTokenOptions", tokenOptions);//ʵ�ֵ���

//��Ȩ
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            //JWT��һЩĬ�ϵ����ԣ����Ǹ���Ȩʱ�Ϳ���ɸѡ��
            ValidateIssuer = true,//�Ƿ���֤Issuer
            ValidateAudience = true,//�Ƿ���֤Audience
            ValidateLifetime = false,//�Ƿ�����ʧЧʱ��
            ValidateIssuerSigningKey = true,//�Ƿ���֤SecurityKey
            ValidAudience = tokenOptions.Audience,
            ValidIssuer = tokenOptions.Isuser,//Issuer,�������ǰ��ǩ��jwt������һ��
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenOptions.SecurityKey))
        };
    });
//��Ȩ
builder.Services.AddAuthorization(options =>
{
    //Ҫ���и���ɫ�ֶ� --- ָ��Admin //[Authorize(Roles = "Admin")] //[Authorize(Policy = "AdminPolicy")]
    options.AddPolicy("AdminPolicy", policyBuilder => policyBuilder.RequireRole("Admin"));

    //Ҫ���û���Ϣ���������Role�ֶ�,������Ϣ������Admian  //[Authorize(Policy = "AssertionAdminPolicy")]
    options.AddPolicy("AssertionAdminPolicy", policyBuilder =>
        policyBuilder.RequireAssertion(context =>
            context.User.HasClaim(c => c.Type == ClaimTypes.Role)
                && context.User.Claims.First(c => c.Type.Equals(ClaimTypes.Role)).Value == "Admin")//Claim��Role��User
     );
});

#endregion


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDirectoryBrowser();// ����֧��Ŀ¼������ܣ���ѡ��
builder.Services.AddScoped<DbContext, CarConsignmentContext>();
builder.Services.AddScoped<ILoginService, LoginServer>();
builder.Services.AddScoped<ICarInfoSerivce, CarInfoServer>();
builder.Services.AddTransient<IJwtService, JwtService>(); 
builder.Services.AddTransient<ISmsService, SmsService>(); 

//�����ĵ���ȡ�����ַ���ע��DbContext
builder.Services.AddDbContext<CarConsignmentContext>(options =>
{
    var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
    //options.UseSqlServer(Configuration.GetConnectionString("SQLConnection"));
});

//����

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


app.UseAuthentication();// ������֤������֤����Ȩ���������tokenҲ��֤��ͨ����

app.UseAuthorization();//��Ȩ

app.MapControllers();

app.Run();
