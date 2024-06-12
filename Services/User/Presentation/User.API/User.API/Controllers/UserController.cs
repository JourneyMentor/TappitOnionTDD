using MassTransit;
using Microsoft.AspNetCore.Mvc;
using User.Application.DTOs;
using User.Application.Interfaces.Services;
using User.Domain.Entities;

namespace User.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        // Use MassTransit
        #region
        //private readonly IRequestClient<GetUserDto> _requestClient;

        //public UserController(IRequestClient<GetUserDto> requestClient)
        //{
        //    _requestClient = requestClient ?? throw new ArgumentNullException(nameof(requestClient));
        //}
        #endregion


        //Use User Service
        #region
        private readonly IUserQueryService _userQueryService;

        public UserController(IUserQueryService userQueryService) : base()
        {
            _userQueryService = userQueryService;
        }
        #endregion


        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Use MassTransit
            #region
            // var response = await _requestClient.GetResponse<GetAllUsersResponse>(new { });
            #endregion


            //Other options
            #region


            // Use User Service
            #region
            var response = await _userQueryService.GetAllAsync<GetAllUsersResponse>();
            #endregion

            //Use BaseHandler
            #region
            //  var users = await _baseHandler.unitOfWork.GetReadRepository<Domain.Entities.User>().GetAllAsync();
            #endregion

            #endregion

            return Ok(response);
        }

        // GET api/<UserController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var user = await _userQueryService.GetByIdAsync<GetUserDto>(id);
            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
