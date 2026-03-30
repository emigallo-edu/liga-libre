using Model.Entities;

namespace Model.Repositories
{
    public interface IResponseAuditRepository
    {
        Task<int> InsertAsync(ResponseAudit item);
        Task<List<ResponseAudit>> GetAllAsync();
    }
}
