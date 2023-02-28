using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sat.Recruitment.Api.Models;
using Sat.Recruitment.Core;
using Sat.Recruitment.Core.Domain;
using Sat.Recruitment.Core.Services;

namespace Sat.Recruitment.Api.Controllers
{
    public partial class UsersController : Controller
    {
        #region Fields

        private readonly ILogger<UsersController> _logger;
        private readonly IUserService _userService;

        #endregion

        #region Utilities

        /// <summary>
        /// Returns a common error request result
        /// </summary>
        /// <param name="e">The exception</param>
        /// <returns>
        /// The action result to be retorned to the client
        /// </returns>
        protected IActionResult RequestErrorResult(Exception e)
        {
            _logger.LogError(e.Message);
            return BadRequest(new RequestResultModel<string>
            {
                ErrorMessage = e.Message,
                IsSuccess = false
            });
        }

        #endregion

        #region Ctor

        public UsersController(ILogger<UsersController> logger,
            IUserService userService)
        {
            _logger = logger;
            _userService = userService;
        }

        #endregion

        #region Methods

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            try
            {
                return Ok(new RequestResultModel<IList<User>>
                {
                    Result = await _userService.GetAllUsersAsync(),
                    IsSuccess = true
                });
            }
            catch (Exception e)
            {
                return RequestErrorResult(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string email)
        {
            try
            {
                if (!CommonHelper.IsValidEmail(email))
                    return Ok(new RequestResultModel<string>
                    {
                        IsSuccess = false,
                        ErrorMessage = "Please provide a valid email"
                    });

                var user = await _userService.GetUserByEmailAsync(email);
                if (user == null)
                    return Ok(new RequestResultModel<string>
                    {
                        IsSuccess = false,
                        ErrorMessage = $"User with email {email} cannot be found"
                    });

                await _userService.DeleteUserAsync(user);
                return Ok(new RequestResultModel<string>
                {
                    IsSuccess = true,
                    Result = $"User {email} has been deleted successfully"
                });
            }
            catch (Exception e)
            {
                return RequestErrorResult(e);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody]UserModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                    return Ok(new RequestResultModel<object>
                    {
                        IsSuccess = false,
                        ErrorMessage = string.Join(". ", ModelState.SelectMany(x => x.Value.Errors).Select(x => x.ErrorMessage))
                    });

                if (!CommonHelper.IsValidEmail(model.Email))
                    return Ok(new RequestResultModel<string>
                    {
                        IsSuccess = false,
                        ErrorMessage = "Please provide a valid email"
                    });

                if (model.UserTypeId < 1 || model.UserTypeId > 3)
                    return Ok(new RequestResultModel<string>
                    {
                        IsSuccess = false,
                        ErrorMessage = "Please provide a valid user type ID (1.-Normal, 2.-SuperUser, 3.-Premium)"
                    });

                var gif = (UserType)model.UserTypeId switch
                {
                    UserType.SuperUser => model.Money > 100 ? model.Money * (model.Money > 100 ? 0.20M : 0) : 0,
                    UserType.Premium => model.Money > 100 ? model.Money * 2 : 0,
                    _ => model.Money * (model.Money > 100 ? 0.12M : model.Money < 100 && model.Money > 10 ? 0.8M : 0),
                };

                var users = await _userService.GetAllUsersAsync();
                if (users.Any(x => x.Email == model.Email || x.Phone == model.Phone || (x.Name == model.Name && x.Address == model.Address)))
                    return Ok(new RequestResultModel<string>
                    {
                        IsSuccess = false,
                        ErrorMessage = "Cannot insert duplicated users"
                    });

                var user = new User
                {
                    Money = model.Money + gif,
                    Address = model.Address,
                    Email = model.Email,
                    Name = model.Name,
                    Phone = model.Phone,
                    UserTypeId = model.UserTypeId.Value,
                };

                await _userService.InsertUserAsync(user);

                return Ok(new RequestResultModel<string>
                {
                    IsSuccess = true,
                    Result = $"User {user.Email} has been created successfully"
                });
            }
            catch (Exception e)
            {
                return RequestErrorResult(e);
            }
        }

        #endregion
    }
}