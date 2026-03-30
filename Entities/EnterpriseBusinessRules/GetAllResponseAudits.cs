using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class GetAllResponseAudits
    {
        private readonly IResponseAuditRepository _responseAuditRepository;

        public GetAllResponseAudits(IResponseAuditRepository responseAuditRepository)
        {
            _responseAuditRepository = responseAuditRepository;
        }

        public async Task<List<ResponseAudit>> ExecuteAsync()
        {
            return await _responseAuditRepository.GetAllAsync();
        }
    }
}
