using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using User.Application.Bases;
using User.Application.DTOs;
using User.Application.Interfaces.AutoMapper;
using User.Application.Interfaces.RedisCache;
using User.Application.Interfaces.Services;
using User.Application.Interfaces.UnitOfWorks;
using User.Domain.Entities;

namespace User.Application.Services.Querires
{
    public class UserQueryService : BaseHandler, IUserQueryService
    {
        public UserQueryService(IMapper mapper,IUnitOfWork unitOfWork, IRedisCacheService redisService)
            : base(mapper, unitOfWork, redisService)
        {
        }

        public async Task<IEnumerable<object>> GetAllAsync<T>() where T : class, new()
        {
            string cacheKey = $"all_{typeof(T).Name}";
            var cachedData = await redisService.GetAsync<IEnumerable<T>>(cacheKey);
            if (cachedData != null)
            {
                return cachedData;
            }
            var users = await unitOfWork.GetReadRepository<Domain.Entities.User>().GetAllAsync();

            var userDtos = users.Select(user => new GetUserDto
            {
                Id = user.Id.ToString(),
                EmailConfirmed = user.EmailConfirmed,
                FullName = user.FullName,
                UserName = user.UserName
            }).ToList();


            var response = new GetAllUsersResponse { Users = userDtos };

            var mapped = mapper.Map<GetAllUsersResponse, List<GetUserDto>>(userDtos);


            await redisService.SetAsync(cacheKey, mapped);
            return response.Users;
        }
    }


}
