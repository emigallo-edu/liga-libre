using Model.Entities;
using Model.EnterpriseBusinessRules;

namespace ApplicationBusinessRules
{
    public enum MatchResultEnum
    {
        LocalClubWon,
        VisitingClubWon,
        Draw
    }

    public class RegisterMatchResultUseCase
    {
        private readonly InsertMatchResult _insertMatchResult;
        private readonly GetMatch _getMatch;
        private readonly GetStanding _getStanding;
        private readonly UpdateStanding _updateStanding;

        public RegisterMatchResultUseCase(
            InsertMatchResult insertMatchResult,
            GetMatch getMatch,
            GetStanding getStanding,
            UpdateStanding updateStanding)
        {
            _insertMatchResult = insertMatchResult;
            _getMatch = getMatch;
            _getStanding = getStanding;
            _updateStanding = updateStanding;
        }

        public async Task ExecuteAsync(int standingId, int matchId, int localClubGoals, int visitingClubGoals)
        {
            var matchResult = new MatchResult()
            {
                Matchid = matchId,
                LocalClubGoals = localClubGoals,
                VisitingClubGoals = visitingClubGoals
            };
            await _insertMatchResult.ExecuteAsync(matchResult);

            Match match = await _getMatch.ExecuteAsync(matchResult.Matchid);

            Standing localStandingClub = await _getStanding.ExecuteAsync(standingId, match.LocalClubId);
            Standing visitingStandingClub = await _getStanding.ExecuteAsync(standingId, match.VisitingClubId);

            MatchResultEnum result = CalculateMatchResult(localClubGoals, visitingClubGoals);

            switch (result)
            {
                case MatchResultEnum.LocalClubWon:
                    localStandingClub.Win++;
                    visitingStandingClub.Loss++;
                    break;
                case MatchResultEnum.VisitingClubWon:
                    localStandingClub.Loss++;
                    visitingStandingClub.Win++;
                    break;
                case MatchResultEnum.Draw:
                    localStandingClub.Draw++;
                    visitingStandingClub.Draw++;
                    break;
            }

            await _updateStanding.ExecuteAsync(localStandingClub);
            await _updateStanding.ExecuteAsync(visitingStandingClub);
        }

        private MatchResultEnum CalculateMatchResult(int localGoals, int visitingGoals)
        {
            if (localGoals > visitingGoals)
                return MatchResultEnum.LocalClubWon;
            if (visitingGoals > localGoals)
                return MatchResultEnum.VisitingClubWon;
            return MatchResultEnum.Draw;
        }
    }
}
