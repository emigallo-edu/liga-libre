using Model.Entities;
using Model.Repositories;
using Model.Services;
using NetWebApi.Context;
using NetWebApi.Model;
using NetWebApi.Utils;
using Repository;
using Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(

// Manejo de Exception por Filter
//    options =>
//{
//    options.Filters.Add<CustomExceptionFilter>();
//}

);


builder.Services.AddScoped<StandingRepository, StandingRepository>();
builder.Services.AddScoped<IClubRepository, ClubDbRepository>();
builder.Services.AddScoped<ITournamentRepository, TournamentRepository>();
builder.Services.AddScoped<CreateTournamentService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInMemoryApplicationDbContext();
builder.Services.AddRepositories(DatabaseType.SqlServer);
builder.Services.AddSecurityDbContext();
builder.Services.AddSecurity();

//builder.Services.AddCors(options =>
//{
//    options.AddPolicy(name: "AcceptLocalHost",
//                      policy =>
//                      {
//                          policy.WithOrigins("http://127.0.0.1:5500");
//                      });
//});

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
//app.UseCors("AcceptLocalHost");

// Maneja de Exception por Middleware
//app.UseMiddleware<ExceptionMiddleware>();

app.Run();