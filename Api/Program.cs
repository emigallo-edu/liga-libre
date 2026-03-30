using ApplicationBusinessRules;
using Model.EnterpriseBusinessRules;
using Model.Entities;
using NetWebApi.Context;
using NetWebApi.Model;
using Security;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddRepositories(DatabaseType.SqlServer);

// EnterpriseBusinessRules
builder.Services.AddScoped<GetClubById>();
builder.Services.AddScoped<GetAllClubs>();
builder.Services.AddScoped<GetAllClubsShort>();
builder.Services.AddScoped<InsertClub>();
builder.Services.AddScoped<Model.EnterpriseBusinessRules.ChangeClubName>();
builder.Services.AddScoped<UpdateClub>();
builder.Services.AddScoped<UpdateClubWithStadium>();
builder.Services.AddScoped<GetClubsWithRegulations>();
builder.Services.AddScoped<InsertTournament>();
builder.Services.AddScoped<GetTournamentById>();
builder.Services.AddScoped<GetAllTournaments>();
builder.Services.AddScoped<GetMatch>();
builder.Services.AddScoped<GetMatchesByTournament>();
builder.Services.AddScoped<InsertMatchResult>();
builder.Services.AddScoped<GetStandingsByTournament>();
builder.Services.AddScoped<GetStanding>();
builder.Services.AddScoped<UpdateStanding>();
builder.Services.AddScoped<GetStadium>();
builder.Services.AddScoped<GetAllResponseAudits>();
builder.Services.AddScoped<InsertResponseAudit>();

// ApplicationBusinessRules (Use Cases)
builder.Services.AddScoped<CreateTournamentUseCase>();
builder.Services.AddScoped<CreateClubUseCase>();
builder.Services.AddScoped<GetClubByIdUseCase>();
builder.Services.AddScoped<GetAllClubsUseCase>();
builder.Services.AddScoped<GetAllClubsShortUseCase>();
builder.Services.AddScoped<ApplicationBusinessRules.ChangeClubNameUseCase>();
builder.Services.AddScoped<UpdateClubUseCase>();
builder.Services.AddScoped<UpdateClubWithStadiumUseCase>();
builder.Services.AddScoped<GetClubsWithRegulationsUseCase>();
builder.Services.AddScoped<GetAllTournamentsUseCase>();
builder.Services.AddScoped<GetTournamentByIdUseCase>();
builder.Services.AddScoped<GetMatchesByTournamentUseCase>();
builder.Services.AddScoped<RegisterMatchResultUseCase>();
builder.Services.AddScoped<GetStandingsByTournamentUseCase>();
builder.Services.AddScoped<GetAllResponseAuditsUseCase>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddInMemoryApplicationDbContext();
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