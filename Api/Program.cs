using Model.Entities;
using NetWebApi.Context;
using NetWebApi.Model;
using Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
var dbType = Enum.TryParse<DatabaseType>(Environment.GetEnvironmentVariable("DATABASE_TYPE"), true, out var parsed)
    ? parsed
    : DatabaseType.SqlServer;
builder.Services.AddRepositories(dbType);
builder.Services.AddEnterpriseBusinessRules();
builder.Services.AddApplicationBusinessRules();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSecurityDbContext();
builder.Services.AddSecurity();

builder.Services.AddAutoMapper(configuration =>
{
    configuration.CreateMap<ClubDTO, Club>()
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