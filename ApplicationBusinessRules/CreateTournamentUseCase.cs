using Model.Entities;
using Model.EnterpriseBusinessRules;

namespace ApplicationBusinessRules
{
    public class CreateTournamentUseCase
    {
        private readonly GetAllClubs _getAllClubs;
        private readonly InsertTournament _insertTournament;
        private Tournament _tournament;

        public CreateTournamentUseCase(GetAllClubs getAllClubs, InsertTournament insertTournament)
        {
            this._getAllClubs = getAllClubs;
            this._insertTournament = insertTournament;
        }

        public async Task<int> ExecuteAsync()
        {
            List<Club> clubs = await this._getAllClubs.ExecuteAsync();
            clubs = clubs.Take(4).ToList();
            List<Match> matches = this.GetDayMatchs(clubs);

            this._tournament = new Tournament()
            {
                Start = matches.Min(x => x.Date),
                Standings = new List<Standing>(),
                Matches = matches,
                End = matches.Max(x => x.Date)
            };

            foreach (Club club in clubs)
            {
                this._tournament.Standings.Add(new Standing()
                {
                    Tournament = this._tournament,
                    ClubId = club.Id
                });
            }

            return await this._insertTournament.ExecuteAsync(this._tournament);
        }

        private List<Match> GetDayMatchs(List<Club> clubs)
        {
            List<Match> matches = new List<Match>();

            DateTime matchDay1 = DateTime.Now;
            DateTime matchDay2 = DateTime.Now.AddDays(7);
            DateTime matchDay3 = DateTime.Now.AddDays(14);

            matches.Add(new Match()
            {
                LocalClubId = clubs.First().Id,
                VisitingClubId = clubs.Skip(1).First().Id,
                Date = matchDay1
            });

            matches.Add(new Match()
            {
                LocalClubId = clubs.Skip(2).First().Id,
                VisitingClubId = clubs.Skip(3).First().Id,
                Date = matchDay1
            });

            matches.Add(new Match()
            {
                LocalClubId = clubs.First().Id,
                VisitingClubId = clubs.Skip(2).First().Id,
                Date = matchDay2
            });

            matches.Add(new Match()
            {
                LocalClubId = clubs.Skip(1).First().Id,
                VisitingClubId = clubs.Skip(3).First().Id,
                Date = matchDay2
            });

            matches.Add(new Match()
            {
                LocalClubId = clubs.First().Id,
                VisitingClubId = clubs.Skip(3).First().Id,
                Date = matchDay3
            });

            matches.Add(new Match()
            {
                LocalClubId = clubs.Skip(1).First().Id,
                VisitingClubId = clubs.Skip(2).First().Id,
                Date = matchDay3
            });

            return matches;
        }
    }
}