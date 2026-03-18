using Model.Entities;

namespace NetWebApi.Context
{
    public static class DataSeeder
    {
        public static void SeedLigaLibreData(
        out List<Club> clubes,
        out Tournament torneo,
        out List<Match> partidos,
        out List<MatchResult> resultados)
        {
            var random = new Random();

            // Nombres ficticios
            var nombresClubes = new[]
            {
            "Tigre", "Estudiantes", "Belgrano", "San Lorenzo", "Lanús", "Ferro", "All Boys", "Atlanta",
            "Defensores", "Quilmes", "Temperley", "Chicago", "Platense", "Almagro", "Arsenal", "Talleres",
            "Gimnasia", "Central Córdoba", "Chacarita", "Huracán"
        };

            clubes = new List<Club>();

            for (int i = 0; i < nombresClubes.Length; i++)
            {
                var club = new Club
                {
                    Id = i + 1,
                    Name = $"Club Atlético {nombresClubes[i]}",
                    Birthday = new DateTime(random.Next(1900, 2000), 1, 1),
                    City = random.Next(2) == 0 ? "BsAs" : "Interior",
                    Email = $"{nombresClubes[i].ToLower()}@ligalibre.com",
                    NumberOfPartners = random.Next(100, 5000),
                    Phone = $"11-4{random.Next(1000000, 9999999)}",
                    Address = $"Calle {random.Next(1, 999)}",
                    StadiumName = $"{nombresClubes[i]} Arena",
                    Players = new List<Player>(),
                    Stadium = new Stadium
                    {
                        Name = $"{nombresClubes[i]} Arena",
                        Capacity = random.Next(500, 3000),
                        ClubId = i + 1
                    }
                };

                // Jugadores
                for (int j = 0; j < 15; j++)
                {
                    var player = new Player
                    {
                        Id = club.Id * 100 + j,
                        Name = $"Jugador {nombresClubes[i]} {j + 1}",
                        Birthday = DateTime.Today.AddYears(-random.Next(18, 40)).AddDays(-random.Next(365)),
                        Address = $"Barrio {random.Next(1, 100)}",
                        ClubId = club.Id,
                        Club = club
                    };
                    club.Players.Add(player);
                }

                clubes.Add(club);
            }

            // Crear torneo
            torneo = new Tournament
            {
                Id = 1,
                Start = DateTime.Today.AddMonths(-1),
                End = DateTime.Today.AddMonths(2),
                Standings = new List<Standing>(),
                Matches = new List<Match>()
            };

            // Standings iniciales
            foreach (var club in clubes)
            {
                torneo.Standings.Add(new Standing
                {
                    TournamentId = torneo.Id,
                    ClubId = club.Id,
                    Club = club,
                    Tournament = torneo,
                    Win = 0,
                    Draw = 0,
                    Loss = 0
                });
            }

            // Partidos todos contra todos (una sola ronda)
            partidos = new List<Match>();
            resultados = new List<MatchResult>();
            int matchId = 1;

            for (int i = 0; i < clubes.Count; i++)
            {
                for (int j = i + 1; j < clubes.Count; j++)
                {
                    var local = clubes[i];
                    var visitante = clubes[j];

                    var match = new Match
                    {
                        Id = matchId,
                        TournamentId = torneo.Id,
                        Date = DateTime.Today.AddDays(matchId),
                        LocalClubId = local.Id,
                        LocalClub = local,
                        VisitingClubId = visitante.Id,
                        VisitingClub = visitante
                    };

                    var golesLocal = random.Next(0, 5);
                    var golesVisitante = random.Next(0, 5);

                    var result = new MatchResult
                    {
                        Matchid = matchId,
                        Match = match,
                        LocalClubGoals = golesLocal,
                        VisitingClubGoals = golesVisitante
                    };

                    partidos.Add(match);
                    torneo.Matches.Add(match);
                    resultados.Add(result);
                    matchId++;
                }
            }
        }
    }
}
