using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using User.Application.Interfaces.AutoMapper;
using User.Application.Interfaces.RedisCache;
using User.Application.Interfaces.UnitOfWorks;

namespace User.Application.Bases
{
    public class BaseHandler
    {
        public readonly IMapper mapper;
        public readonly IUnitOfWork unitOfWork;
    //    public readonly IHttpContextAccessor httpContextAccessor;
        public readonly IRedisCacheService redisService;
      //  public readonly string userId;

        public BaseHandler(IMapper mapper,IUnitOfWork unitOfWork/*, IHttpContextAccessor httpContextAccessor*/,IRedisCacheService redisService)
        {
            this.mapper = mapper;
            this.unitOfWork = unitOfWork;
        //    this.httpContextAccessor = httpContextAccessor;
            this.redisService = redisService;
           // userId = httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
        }
    }
}
