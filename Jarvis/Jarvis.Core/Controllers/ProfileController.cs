﻿using Infrastructure.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Jarvis.Core.Database;
using Jarvis.Core.Services;
using Jarvis.Core.Database.Repositories;
using System.Threading.Tasks;
using Jarvis.Models.Identity.Models.Identity;
using System;

namespace Jarvis.Core.Controllers
{
    [Authorize]
    [Route("profile")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly IWorkContext _workContext;
        private readonly IIdentityService _identityService;
        private readonly ICoreUnitOfWork _uow;

        public ProfileController(
            IWorkContext workContext,
            IIdentityService identityService,
            ICoreUnitOfWork uow)
        {
            _workContext = workContext;
            _identityService = identityService;
            _uow = uow;
        }

        [HttpGet("user")]
        public async Task<IActionResult> GetAsync()
        {
            var user = await _workContext.GetUserAsync();
            var repoUser = _uow.GetRepository<IUserRepository>();
            var info = await repoUser.FindUserInfoByIdAsync(user.Id);

            var model = new ProfileModel
            {
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                FullName = info.FullName,
                AvatarPath = info.AvatarPath
            };
            return Ok(model);
        }

        [HttpPost("user")]
        public async Task<IActionResult> PostAsync([FromBody]ProfileModel model)
        {
            var user = await _workContext.GetUserAsync();
            var repoUser = _uow.GetRepository<IUserRepository>();

            user.Email = model.Email;
            user.NormalizedEmail = string.IsNullOrEmpty(model.Email) ? null : model.Email.ToUpper();
            user.PhoneNumber = model.PhoneNumber;
            user.UpdatedAt = DateTime.Now;
            user.UpdatedAtUtc = DateTime.UtcNow;
            user.UpdatedBy = user.Id;

            repoUser.Update(user);

            var repoInfo = _uow.GetRepository<IUserRepository>();
            var info = await repoInfo.FindUserInfoByIdAsync(user.Id);
            if (info != null)
            {
                repoInfo.UpdateUserInfoFields(info,
                    info.Set(x => x.FullName, model.FullName),
                    info.Set(x => x.AvatarPath, model.AvatarPath));
            }

            await _uow.CommitAsync();
            return Ok();
        }

        [HttpDelete("user")]
        public async Task<IActionResult> DeleteAsync()
        {
            var user = await _workContext.GetUserAsync();
            await _identityService.DeleteAsync(user.Id);
            return Ok();
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePasswordAsync([FromBody]ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _workContext.GetUserAsync();
            await _identityService.ChangePasswordAsync(user.Id, model);
            return Ok();
        }
    }
}
