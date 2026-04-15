using DotNetEnv;
using Model.Entities;
using NetWebApi.Context;
using NetWebApi.Model;
using Security;

var builder = WebApplication.CreateBuilder(args);
Env.Load();
builder.Services.AddControllers();
builder.Services.AddRepositories(DatabaseType.SqlServer);
builder.Services.AddEnterpriseBusinessRules();
builder.Services.AddApplicationBusinessRules();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSecurityDbContext();
builder.Services.AddSecurity();

builder.Services.AddAutoMapper(configuration =>
{
    configuration.CreateMap<ClubDTOV2, Club>()
        .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.NombreClub))
        .ReverseMap();
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

ApplicationDbContextFactoryConfig.SetProvider(app.Services);

app.UseHttpsRedirection();
app.MapControllers();
app.UseAuthentication();
app.UseAuthorization();
app.Run();