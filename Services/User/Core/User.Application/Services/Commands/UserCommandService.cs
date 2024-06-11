using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Bases;
using User.Application.Interfaces.RedisCache;
using User.Application.Interfaces.Services;
using User.Application.Interfaces.UnitOfWorks;

namespace User.Application.Services.Commands
{
    public class UserCommandService :IUserCommandService
    {
        //public UserCommandService(IUnitOfWork unitOfWork, IRedisCacheService redisService) : base(unitOfWork, redisService)
        //{
        //}

        public async Task<IEnumerable<T>> GetAllAsync<T>() where T : class, new()
        {
            return default;// await unitOfWork.GetReadRepository<T>().GetAllAsync();
        }
    }


}
