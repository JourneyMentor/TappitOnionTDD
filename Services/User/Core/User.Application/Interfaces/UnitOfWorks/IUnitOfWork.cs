using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Interfaces.Repositories;
using User.Domain.Common;

namespace User.Application.Interfaces.UnitOfWorks
{
    public interface IUnitOfWork //: IAsyncDisposable
    {
        IReadRepository<T> GetReadRepository<T>() where T : class, new();
        IWriteRepository<T> GetWriteRepository<T>() where T : class, new();
        Task<int> SaveAsync();
        int Save();
    }
}
