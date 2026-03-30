using Model.Entities;
using Model.EnterpriseBusinessRules;

namespace ApplicationBusinessRules
{
    public class GetAllResponseAuditsUseCase
    {
        private readonly GetAllResponseAudits _getAllResponseAudits;

        public GetAllResponseAuditsUseCase(GetAllResponseAudits getAllResponseAudits)
        {
            _getAllResponseAudits = getAllResponseAudits;
        }

        public async Task<List<ResponseAudit>> ExecuteAsync()
        {
            return await _getAllResponseAudits.ExecuteAsync();
        }
    }
}
