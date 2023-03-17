using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rewriting.Common.Validator;
using Rewriting.Services.UserAccount;

namespace Rewriting.API.Controllers.Accounts
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserAccountService _userAccountService;
        private readonly IModelValidator<RegisterUserRequest> _registerUserRequestValidator;
        private readonly IModelValidator<ChangePasswordRequest> _changePasswordRequestValidator;

        public UserAccountController(
            IMapper mapper, 
            IUserAccountService userAccountService, 
            IModelValidator<RegisterUserRequest> registerUserRequestValidator,
            IModelValidator<ChangePasswordRequest> changePasswordRequestValidator)
        {
            _mapper = mapper;
            _userAccountService = userAccountService;
            _registerUserRequestValidator = registerUserRequestValidator;
            _changePasswordRequestValidator = changePasswordRequestValidator;
        }

        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="request">RegisterUserAccountRequest</param>
        /// <response code="200">Registered user model</response>
        [HttpPost("")]
        public async Task<UserAccountResponse> Register([FromBody] RegisterUserRequest request)
        {
            _registerUserRequestValidator.Check(request);
            var user = await _userAccountService.CreateAsync(_mapper.Map<RegisterUserModel>(request));

            return _mapper.Map<UserAccountResponse>(user);
        }

        /// <summary>
        /// Change password
        /// </summary>
        /// <param name="request">ChangePasswordRequest</param>
        /// <response code="200">Success report</response>
        [HttpPatch("")]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {
            _changePasswordRequestValidator.Check(request);
            var model = _mapper.Map<ChangePasswordModel>(request);
            model.Issuer = User;
            await _userAccountService.ChangePasswordAsync(model);
            return Ok();
        }
    }
}
