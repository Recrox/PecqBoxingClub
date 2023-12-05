using RamDam.BackEnd.Core;
using RamDam.BackEnd.Core.Models.Api;
using RamDam.BackEnd.Core.Repositories;
using RamDam.BackEnd.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using RamDam.BackEnd.Core.Utilities;
using RamDam.BackEnd.Core.Enums;
using RamDam.BackEnd.Core.Models.Enums;
using RamDam.BackEnd.Configuration;

namespace RamDam.BackEnd.Api.Controllers
{
    [ApiController]
    [Authorize(Policy = "RamDam")]
    [Route("users")]
    public class UsersController : BaseController
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserService _userService;
        private readonly IStoredProcedureRepository _storedProcedureRepository;

        public UsersController(
            IUserRepository userRepository,
            IUserService userService,
            IStoredProcedureRepository storedProcedureRepository,
            GlobalSettings globalSettings, 
            CurrentContext currentContext) 
            : base(globalSettings, currentContext)
        {
            _userRepository = userRepository;
            _userService = userService;
            _storedProcedureRepository = storedProcedureRepository;
        }

        [Authorize(Policy = "Admin")]
        [HttpGet]
        [AuthorizeRoles(RoleCode.SuperAdmin)]
        public async Task<IActionResult> Get()
        {
            var users = await _userRepository.GetManyAsync(orderBy: u => u.Email);
            return Json(users);
        }

        [HttpGet("current")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCurrent()
        {
            var user = await _userRepository.GetByIdAsync(_user.Id);
            return Json(user);
        }
        
        [HttpPost]
        [HttpPut]
        [AllowAnonymous]
        public async Task<IActionResult> PutOrPost([FromBody] User userRequest)
        {
            if (userRequest?.SocialNetwork?.Id != null && !userRequest.SocialNetwork.Id.Equals(SocialNetworkEnum.Email))
            {
                userRequest.IsActive = true;
                userRequest.UserName = userRequest.IdSocial;
            }
            var user = await _userService.SaveAsync(userRequest);
            if (user == null)
                return BadRequest();
            return Json(user);
        }

       

        [HttpPost("password")]
        public async Task<IActionResult> ChangePassword(
            [FromForm(Name = "current_password")] string currentPassword, 
            [FromForm(Name = "new_password")] string newPassword)
        {
            var result = await _userService.ChangePasswordAsync(_user, currentPassword, newPassword);

            if (result.Succeeded)
                return Ok();

            return BadRequest(new
            {
                Error = "invalid_grant",
                Error_description = "invalid_username_or_password"
            });
        }

        [AllowAnonymous]
        [HttpPost("password-welcome-request")]
        public async Task<IActionResult> PasswordInitRequest([FromForm(Name = "Email")] string Email)
        {
            var success = await _userService.SendPasswordInitAsync(Email);
            if (success)
                return Ok();
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("{Email}/password-reset-request")]
        public async Task<IActionResult> PasswordResetRequest([FromRoute] string Email)
        {
            var success = await _userService.SendPasswordResetAsync(Email);
            if (success)
                return Ok();
            return BadRequest();
        }

        [AllowAnonymous]
        [HttpPost("{id}/password-reset")]
        public async Task<IActionResult> PasswordReset([FromRoute] Guid id, [FromBody] JObject value)
        {
            var success = await _userService.ResetPasswordAsync(
                id, 
                value["token"].Value<string>(), 
                value["new_password"].Value<string>());
            if (success)
                return Ok();
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser()
        {
            if (await _storedProcedureRepository.DeleteUser(_currentContext.User.Id))
                return Ok();
            return BadRequest();
        }

        //[HttpPut("{id}/desactivate")]
        //[HttpDelete("{id}")]
        //[AllowAnonymous]
        //public async Task<IActionResult> Delete(Guid id)
        //{
        //    var success = await _userService.DeleteAsync(id);
        //    if (success)
        //        return Ok();
        //    return NotFound();
        //}
        [HttpGet("{id}/activate")]
        [HttpPut("{id}/activate")]
        [AllowAnonymous]
        public async Task<IActionResult> PutActive(Guid id)
        {
            var success = await _userService.UndeleteAsync(id);
            if (success)
                return Ok();
            return NotFound();
        }

        [Authorize(Policy = "Admin")]
        [AuthorizeRoles(RoleCode.SuperAdmin)]
        [HttpGet("answer")]
        public async Task<IActionResult> GetUsersAnswers()
        {
            return new JsonResult(await _storedProcedureRepository.GetUserAnswers());
        }
    }
}
