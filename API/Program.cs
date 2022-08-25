using System.Text;using BLL.DELETE;
using BLL.DTO.Mapper;
using BLL.DTO.Models.JWTManager;
using BLL.DTO.Models.UserModels.Password;
using BLL.Services;
using DAL.EF;
using DAL.UOW;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddControllers();

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme()
    {
        Description = "Standard Authorization header using Bearer scheme (\"bearer {token}\")",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey
    });

    options.OperationFilter<SecurityRequirementsOperationFilter>();
});

builder.Services.AddSwaggerGen();

builder.Services.AddAntiforgery();

// TODO: CHANGE DB CONNECTION
builder.Services.AddDbContext<ScheduleContext>(optionsBuilder => {
    optionsBuilder.UseSqlite(new ScheduleSqlLiteFactory("TestDB").ConnectionString);
});

// FOR: Test only!!
new TestWorker();

builder.Services.AddScoped<IUnitOfWork, EfUnitOfWork>();
builder.Services.AddScoped<IJwtManagerRepository, JwtManagerRepository>();
builder.Services.AddScoped<UserService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);


builder.Services.AddAuthentication(options => {
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options => {
        var key = Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]);
        options.SaveToken = true;
        options.TokenValidationParameters = new TokenValidationParameters() {
            ValidateIssuerSigningKey = true,
            ValidateLifetime = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();

app.Run();