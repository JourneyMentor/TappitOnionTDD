using MassTransit;
using System.Text.Json;
using User.Application.Bases;
using User.Application.DTOs;
using User.Application.Interfaces.AutoMapper;
using User.Application.Interfaces.RedisCache;
using User.Application.Interfaces.UnitOfWorks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace User.Application.Consumers
{
    public class GetUsersConsumer : BaseHandler, IConsumer<GetUserDto>
    {

        public GetUsersConsumer(IMapper mapper, IUnitOfWork unitOfWork, IRedisCacheService redisService)
            : base(mapper, unitOfWork, redisService)
        {
        }

        public async Task Consume(ConsumeContext<GetUserDto> context)
        {
            var cacheKey = "";
            var message = context.Message;
            var users = await unitOfWork.GetReadRepository<Domain.Entities.User>().GetAllAsync();

            var cachedUser = await redisService.GetAsync<GetAllUsersResponse>(cacheKey);
            if (cachedUser is not null)
            {
                await context.RespondAsync(cachedUser);
                return;
            }
            await redisService.SetAsync(cacheKey, JsonSerializer.Serialize(users));


            var userDtos = users.Select(user => new GetUserDto
            {
                Id = user.Id.ToString(),
                EmailConfirmed = user.EmailConfirmed,
                FullName = user.FullName,
                UserName = user.UserName
            }).ToList();

            var response = new GetAllUsersResponse { Users = userDtos };

            var mapped = mapper.Map<GetAllUsersResponse, List<GetUserDto>>(userDtos);

            #region
            await context.RespondAsync(response);
            #endregion

        }
    }
}