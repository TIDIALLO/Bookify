﻿namespace Bookify.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsAsync(CancellationToken cancellationToken = default);
    }
}
