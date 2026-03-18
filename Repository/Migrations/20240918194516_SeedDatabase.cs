using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Repository.Migrations
{
    /// <inheritdoc />
    public partial class SeedDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            int tournamentId = 980;
            int standingId = 4839;
            int minClubId = 5;
            int maxClubId = 10;

            InsertTournament(migrationBuilder, tournamentId);

            for (int clubId = minClubId; clubId <= maxClubId; clubId++)
            {
                InsertClub(migrationBuilder, clubId);
                InsertStanding(migrationBuilder, tournamentId, clubId);
                InsertPlayers(migrationBuilder, clubId);
                InsertStadiums(migrationBuilder, clubId);
            }

            InsertMatchs(migrationBuilder, tournamentId, minClubId, maxClubId);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM Matchs");
            migrationBuilder.Sql("DELETE FROM Players");
            migrationBuilder.Sql("DELETE FROM Standings");
            migrationBuilder.Sql("DELETE FROM Tournaments");
            migrationBuilder.Sql("DELETE FROM Stadiums");
            migrationBuilder.Sql("DELETE FROM Clubs");
        }

        private static void InsertClub(MigrationBuilder mb, int clubId)
        {
            string query = "SET IDENTITY_INSERT Clubs ON;";
            query += $"INSERT INTO Clubs (Id, Name, Birthday, City, Email, NumberOfPartners, Phone, Address)" +
               $"VALUES ({clubId}, '{GetFakedName()}', GETDATE(), '{RemoveUnwantedCharacters(Faker.Address.City())}', 'club1@mail.com', {new Random().Next(1, 15849)}, '{Faker.Phone.Number()}', '{RemoveUnwantedCharacters(Faker.Address.StreetAddress())}')";
            query += "SET IDENTITY_INSERT Clubs OFF;";

            mb.Sql(query);
        }

        private static void InsertPlayers(MigrationBuilder mb, int clubId)
        {
            for (int i = 1; i <= 23; i++)
            {
                string query = $"INSERT INTO Players (Name, Birthday, ClubId)" +
                    $"VALUES ('{GetFakedName()}', GETDATE(), {clubId})";
                mb.Sql(query);
            }
        }

        private static void InsertStadiums(MigrationBuilder mb, int clubId)
        {
            string query = $"INSERT INTO Stadiums (Name, Capacity, ClubId)" +
                $"VALUES('{GetFakedName()}', {new Random().Next(1, 5849)}, {clubId})";
            mb.Sql(query);
        }

        private static void InsertTournament(MigrationBuilder mb, int tournamentId)
        {
            string query = "SET IDENTITY_INSERT Tournaments ON;";
            query += $"INSERT INTO Tournaments (Id, Start, [End])" +
                   $"VALUES ({tournamentId}, GETDATE(), GETDATE())";
            query += "SET IDENTITY_INSERT Tournaments OFF;";

            mb.Sql(query);
        }

        private static void InsertStanding(MigrationBuilder mb, int tournamentId, int clubId)
        {
            string query = "INSERT INTO Standings (TournamentId, ClubId,  Win, Draw, Loss)" +
                $"VALUES ({tournamentId}, {clubId}, 0, 0, 0)";

            mb.Sql(query);
        }

        private static void InsertMatchs(MigrationBuilder mb, int tournamentId, int minClubId, int maxClubId)
        {
            Dictionary<string, KeyValuePair<int, int>> combinations = new Dictionary<string, KeyValuePair<int, int>>();

            for (int club1 = minClubId; club1 <= maxClubId; club1++)
            {
                for (int club2 = minClubId; club2 <= maxClubId; club2++)
                {
                    if (club1 != club2 && !combinations.ContainsKey(GetKey(club1, club2)))
                    {
                        combinations.Add(GetKey(club1, club2),
                            new KeyValuePair<int, int>(club1, club2));
                    }
                }
            }

            DateTime start = DateTime.Now;
            int i = 0;
            foreach (var combination in combinations)
            {
                string query = $"INSERT INTO Matchs (Date, LocalClubId, VisitingClubId, TournamentId)" +
                   $"VALUES (GETDATE(), {combination.Value.Key}, {combination.Value.Value}, {tournamentId})";
                mb.Sql(query);
                i++;
            }
        }

        private static string GetKey(int club1, int club2)
        {
            if (club2 < club1)
            {
                return string.Format("{0}-{1}", club2, club1);
            }
            return string.Format("{0}-{1}", club1, club2);
        }

        private static string GetFakedName()
        {
            return RemoveUnwantedCharacters(Faker.Company.Name());
        }

        private static string RemoveUnwantedCharacters(string text)
        {
            return text.Replace("'", string.Empty)
                .Replace("\"", string.Empty)
                .Replace(")", string.Empty);
        }
    }
}
