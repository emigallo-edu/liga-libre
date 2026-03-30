using Model.Entities;
using Model.Repositories;

namespace Model.EnterpriseBusinessRules
{
    public class InsertResponseAudit
    {
        private readonly IResponseAuditRepository _responseAuditRepository;

        public InsertResponseAudit(IResponseAuditRepository responseAuditRepository)
        {
            _responseAuditRepository = responseAuditRepository;
        }

        public async Task<int> ExecuteAsync(ResponseAudit item)
        {
            return await _responseAuditRepository.InsertAsync(item);
        }
    }
}
