using Microsoft.EntityFrameworkCore;

namespace Contracts.Common.Interfaces
{
    // Quản lí phần commit, khi có tình huống lưu quá nhiều Table thì chỉ Commit 1 lần.
    public interface IUnitOfWork<TContext> : IDisposable where TContext : DbContext
    {
        Task<int> CommitAsync();
    }
}
