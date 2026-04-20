using Model.Entities;
using Model.EnterpriseBusinessRules;

namespace ApplicationBusinessRules
{
    public class CreateSingleLegTournamentUseCase
    {
        private readonly GetAllClubs _getAllClubs;
        private readonly InsertTournament _insertTournament;

        public CreateSingleLegTournamentUseCase(GetAllClubs getAllClubs, InsertTournament insertTournament)
        {
            this._getAllClubs = getAllClubs;
            this._insertTournament = insertTournament;
        }

        public async Task<int> ExecuteAsync()
        {
            List<Club> clubs = await this._getAllClubs.ExecuteAsync();
            List<Match> matches = this.BuildFixture(clubs);

            var tournament = new Tournament()
            {
                Start = matches.Min(x => x.Date),
                Standings = new List<Standing>(),
                Matches = matches,
                End = matches.Max(x => x.Date)
            };

            foreach (Club club in clubs)
            {
                tournament.Standings.Add(new Standing()
                {
                    Tournament = tournament,
                    ClubId = club.Id
                });
            }

            return await this._insertTournament.ExecuteAsync(tournament);
        }

        private List<Match> BuildFixture(List<Club> clubs)
        {
            List<int?> rotation = clubs.Select(c => (int?)c.Id).ToList();
            if (rotation.Count % 2 != 0)
            {
                rotation.Add(null);
            }

            int teamCount = rotation.Count;
            int rounds = teamCount - 1;
            int matchesPerRound = teamCount / 2;
            DateTime seasonStart = DateTime.Now;

            List<Match> matches = new List<Match>();

            for (int round = 0; round < rounds; round++)
            {
                DateTime matchDay = seasonStart.AddDays(7 * round);

                for (int i = 0; i < matchesPerRound; i++)
                {
                    int? firstId = rotation[i];
                    int? secondId = rotation[teamCount - 1 - i];

                    if (firstId == null || secondId == null)
                    {
                        continue;
                    }

                    bool firstIsLocal = Random.Shared.Next(2) == 0;
                    matches.Add(new Match()
                    {
                        LocalClubId = firstIsLocal ? firstId.Value : secondId.Value,
                        VisitingClubId = firstIsLocal ? secondId.Value : firstId.Value,
                        Date = matchDay
                    });
                }

                int? last = rotation[teamCount - 1];
                for (int i = teamCount - 1; i > 1; i--)
                {
                    rotation[i] = rotation[i - 1];
                }
                rotation[1] = last;
            }

            return matches;
        }
    }
}
