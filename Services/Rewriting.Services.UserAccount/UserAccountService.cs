using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Rewriting.Common.Exceptions;
using Rewriting.Common.Validator;
using Rewriting.Context.Entities;
using System.Security.Claims;

namespace Rewriting.Services.UserAccount
{
    internal class UserAccountService : IUserAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IModelValidator<RegisterUserModel> _registerUserAccountModelValidator;
        private readonly IModelValidator<ChangePasswordModel> _changePasswordModelValidator;

        public UserAccountService(
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IModelValidator<RegisterUserModel> validator,
            IModelValidator<ChangePasswordModel> changePasswordModelValidator)
        {
            _userManager = userManager;
            _mapper = mapper;
            _registerUserAccountModelValidator = validator;
            _changePasswordModelValidator = changePasswordModelValidator;
        }

        public bool IsAnyUsers()
        {
            return _userManager.Users.Any();
        }

        public async Task ChangePasswordAsync(ChangePasswordModel model)
        {
            _changePasswordModelValidator.Check(model);

            var user = await _userManager.GetUserAsync(model.Issuer)
                ?? throw new ProcessException($"User not found");

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                string text = AggregateErrors(result);
                throw new ProcessException($"{text}");
            }
        }

        public async Task<UserModel> CreateAsync(RegisterUserModel model)
        {
            _registerUserAccountModelValidator.Check(model);

            var userIdentity = await _userManager.FindByEmailAsync(model.Email);
            if (userIdentity != null)
                throw new ProcessException($"User account with email {model.Email} already exists");

            userIdentity = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
            };
            var userData = new UserData
            {
                Uid = userIdentity.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                RegistrationDate = DateOnly.FromDateTime(DateTime.UtcNow)
            };
            userIdentity.UserData = userData;

            var result = await _userManager.CreateAsync(userIdentity, model.Password);
            if (!result.Succeeded)
            {
                string text = AggregateErrors(result);
                throw new ProcessException($"{text}");
            }

            return _mapper.Map<UserModel>(userIdentity);
        }

        public async Task AddToRoleAsync(AddToRoleModel model)
        {
            var userIdentity = await _userManager.FindByIdAsync(model.UserUid.ToString())
                ?? throw new ProcessException($"User {model.UserUid} not found");

            var claim = new Claim(ClaimTypes.Role, model.RoleName);

            await _userManager.AddClaimAsync(userIdentity, claim);
        }

        private static string AggregateErrors(IdentityResult result)
        {
            return result.Errors.ToList().Select(a => a.Description).Aggregate((a, b) => a + "\n" + b);
        }
    }
}
