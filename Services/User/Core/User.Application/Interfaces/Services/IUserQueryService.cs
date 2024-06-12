using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Interfaces.Services
{
    public interface IUserQueryService
    {
        Task<IEnumerable<object>> GetAllAsync<T>() where T : class, new();
        Task<object> GetByIdAsync<T>(Guid id) where T : class, new();
    }
}
