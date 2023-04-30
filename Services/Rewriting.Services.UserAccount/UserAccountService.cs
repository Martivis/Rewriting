using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Rewriting.Common.Exceptions;
using Rewriting.Common.Validator;
using Rewriting.Context.Entities;
using Rewriting.Services.EmailService;
using System.Security.Claims;

namespace Rewriting.Services.UserAccount
{
    internal class UserAccountService : IUserAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;
        private readonly IModelValidator<RegisterUserModel> _registerUserAccountModelValidator;
        private readonly IModelValidator<ChangePasswordModel> _changePasswordModelValidator;

        public UserAccountService(
            UserManager<ApplicationUser> userManager,
            IEmailService emailService,
            IMapper mapper,
            IModelValidator<RegisterUserModel> validator,
            IModelValidator<ChangePasswordModel> changePasswordModelValidator)
        {
            _userManager = userManager;
            _emailService = emailService;
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

        private static string AggregateErrors(IdentityResult result)
        {
            return result.Errors.ToList().Select(a => a.Description).Aggregate((a, b) => a + "\n" + b);
        }

        public async Task AddToRoleAsync(AddToRoleModel model)
        {
            var userIdentity = await _userManager.FindByIdAsync(model.UserUid.ToString())
                ?? throw new ProcessException($"User {model.UserUid} not found");

            var claim = new Claim(ClaimTypes.Role, model.RoleName);

            await _userManager.AddClaimAsync(userIdentity, claim);
        }

        public async Task InitializePasswordReset(InitialResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email)
                ?? throw new ProcessException($"User registered with email {model.Email} not found");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var mailModel = ResetPasswordMailFactory.GetMailModel(model.Email, token);
            await _emailService.SendMail(mailModel);
        }

        public async Task ResetPassword(ResetPasswordModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email)
                ?? throw new ProcessException($"User registered with email {model.Email} not found");

            await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
        }
    }
}
