using System.Text;
using API.JwtCustomValidate;
using BLL.DELETE;
using BLL.DTO.Mapper;
using BLL.DTO.Models.JWTManager;
using BLL.Services;
using BLL.Services.RolesHandler;
using DAL.EF;
using DAL.UOW;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

// TEST
bool updateDb = false;

var th1 = new Thread((async () => await (new TestWorker()).Run()));
if (updateDb) {
    th1.Start();
}


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


builder.Services.AddSwaggerGen(options => {
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme {
        Description = "Standard Authorization header using Bearer scheme (\"bearer {token}\")",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddAuthentication(options => {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options => {
        options.Events = new JwtBearerEvents {
            OnTokenValidated = ValidateModel.ValidateUserModel
        };
        options.TokenValidationParameters = new TokenValidationParameters {
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
            ValidateIssuer = false,
            ValidateAudience = false
        };
    });

builder.Services.AddAntiforgery();

// TODO: CHANGE DB CONNECTION
builder.Services.AddDbContext<ScheduleContext>(optionsBuilder => {
    optionsBuilder.UseSqlite(new ScheduleSqlLiteFactory("TestDB").ConnectionString);
});

builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<IJwtManagerRepository, JwtManagerRepository>();
builder.Services.AddScoped<IRoleHandler, UserRoleHandler>();
builder.Services.AddScoped<UserService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

if (updateDb) th1.Join();