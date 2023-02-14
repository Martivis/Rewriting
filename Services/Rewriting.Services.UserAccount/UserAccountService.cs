using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Rewriting.Common.Validator;
using Rewriting.Context.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rewriting.Services.UserAccount
{
    internal class UserAccountService : IUserAccountService
    {
        private readonly UserManager<UserIdentity> _userManager;
        private readonly IMapper _mapper;
        private readonly IModelValidator<RegisterUserModel> _registerUserAccountModelValidator;
        private readonly IModelValidator<ChangePasswordModel> _changePasswordModelValidator;

        public UserAccountService(
            UserManager<UserIdentity> userManager,
            IMapper mapper,
            IModelValidator<RegisterUserModel> validator,
            IModelValidator<ChangePasswordModel> changePasswordModelValidator)
        {
            _userManager = userManager;
            _mapper = mapper;
            _registerUserAccountModelValidator = validator;
            _changePasswordModelValidator = changePasswordModelValidator;
        }

        public async Task ChangePassword(ChangePasswordModel model)
        {
            _changePasswordModelValidator.Check(model);

            var user = await _userManager.GetUserAsync(model.Issuer)
                ?? throw new Exception($"User  not found");

            var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                var text = result.Errors.ToList().Select(a => a.Description).Aggregate((a, b) => a + "\n" + b);
                throw new Exception($"{text}");
            }
        }

        public async Task<UserModel> Create(RegisterUserModel model)
        {
            _registerUserAccountModelValidator.Check(model);

            var userIdentity = await _userManager.FindByEmailAsync(model.Email);
            if (userIdentity != null)
                throw new Exception($"User account with email {model.Email} already exists");

            userIdentity = new UserIdentity
            {
                UserName = model.Email,
                Email = model.Email,
            };
            var userData = new UserData
            {
                Uid = userIdentity.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
            };
            userIdentity.UserData = userData;

            var result = await _userManager.CreateAsync(userIdentity, model.Password);
            if (!result.Succeeded)
                throw new Exception("Operation failed");

            return _mapper.Map<UserModel>(userIdentity);
        }
    }
}
