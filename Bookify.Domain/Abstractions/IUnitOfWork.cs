namespace Bookify.Domain.Abstractions
{
    public interface IUnitOfWork
    {
       // Task<int> SaveChangesAsAsync(CancellationToken cancellationToken = default);
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    }
}
