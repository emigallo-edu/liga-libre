using Model.Entities;
using Model.Repositories;

namespace Model.Services
{
    public class CreateTournamentService
    {
        private readonly IClubRepository _clubRepository;
        private readonly ITournamentRepository _tournamentRepository;
        private Tournament _tournament;

        public CreateTournamentService(IClubRepository clubRepository, ITournamentRepository tournamentRepository)
        {
            this._clubRepository = clubRepository;
            this._tournamentRepository = tournamentRepository;
        }

        public async Task<int> ExecuteAsync()
        {
            List<Club> clubs = await this._clubRepository.GetAllAsync();
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

            // Devuelvo la cantidad de registros afectados en el insert (en este caso siempre va a ser 0 o 1)
            return await this._tournamentRepository.InsertAsync(this._tournament);
        }

        public int GetTournamentIdCreated()
        {
            // El Id del torneo is del tipo Identity, al momento de hacer el insert de la LN: 42, lo setea el mismo SQL
            // Como el atributo '_tournament' es privado, quien consume este service no tiene acceso al id generado
            // Expongo este método para que pueda consultarlo en caso de que lo necesite
            return this._tournament.Id;
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