using SimpleWarehouse.Core.Domain;

namespace SimpleWarehouse.Infrastructure.Extanstions;

public static class NotArchivedQueryExtension
{
    public static IQueryable<T> NotArchived<T>(this IQueryable<T> query) where T : ArchivedEntity
    {
        return query.Where(x => !x.IsArchived);
    }
}