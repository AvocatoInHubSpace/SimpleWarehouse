using SimpleWarehouse.Core.Domain;

namespace SimpleWarehouse.Application.Interfaces.Repositories;

public interface IDocumentRepository : IRepository<Document>, IUniqueNameRepository
{
}