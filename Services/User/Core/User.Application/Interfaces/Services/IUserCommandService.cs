using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace User.Application.Interfaces.Services
{
    public interface IUserCommandService
    {
        Task<IEnumerable<T>> GetAllAsync<T>() where T : class, new();

    }
}
