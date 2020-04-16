using Kpi.Api.Controllers.Base;
using Kpi.Api.Models;
using Kpi.Api.Models.Resources;
using Kpi.Core.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace Kpi.Api.Controllers.Sys
{
    [Authorize]
    public class UserController : BaseApiController
    {
        [HttpPost]
        public RespondData GetUsers()
        {
            var result = AppUserManager.Users.Select(x => new { x.Id, x.UserName, x.Email, x.Name }).ToList();
            return Success(result);
        }

        [HttpPost]
        public async Task<RespondData> ChangePassword(PasswordResourceModel model)
        {
            if (!ModelState.IsValid)
            {
                return Fail("Dữ liệu không hợp lệ");
            }

            if (model.Password != model.ConfirmPassword)
            {
                return Fail("Mật khẩu mới không khớp");
            }

            // find user
            ApplicationUser user = this.AppUser;

            // check old password
            var isValidPassword = await this.UserManager.CheckPasswordAsync(user, model.OldPassword);
            if (!isValidPassword)
            {
                return Fail("Mật khẩu cũ không hợp lệ");
            }

            // generate token to reset password
            var token = await UserManager.GeneratePasswordResetTokenAsync(user.Id);
            var result = await UserManager.ResetPasswordAsync(user.Id, token, model.Password);
            if (result.Succeeded)
            {
                if (!user.IsActived)
                {
                    var u = await this.UserManager.FindByNameAsync(user.UserName);
                    u.IsActived = true;
                    await this.UserManager.UpdateAsync(u);
                }

                return Success("Thay đổi thành công");
            }
            var err = string.Join("; ", result.Errors);
            return Fail(err);
        }
    }
}