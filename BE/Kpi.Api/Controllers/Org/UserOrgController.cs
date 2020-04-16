using Kpi.Api.Controllers.Base;
using Kpi.Api.Models;
using Kpi.Api.Models.Resources;
using Kpi.Api.Models.Utilities;
using Kpi.Core.DTO;
using Kpi.Core.Helper;
using Kpi.DataAccess.DataContext;
using Kpi.Service.DataServices.Org;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Threading.Tasks;
using Kpi.Api.Models.OwinManager;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Net.Http;
using System;

namespace Kpi.Api.Controllers.Org
{
    public class UserOrgFilterRs
    {
        public int OrgId { get; set; }
        public int EventDiaryId { get; set; }
        public List<int> EventDiaryIds { get; set; }
    }

    [Authorize]
    public class UserOrgController : BaseGridApiController<IUserOrgService, UserOrgRS, UserInfoModel, UserOrgFilterModel, OrgInfoModelFilterModelDTO>
    {
        public UserOrgController(IUserOrgService service) : base(service)
        { }

        [HttpPost]
        public RespondData GetEDUsersByOrg(UserOrgFilterRs filter)
        {
            return Success(this._service.GetEDUsersByOrg(filter.OrgId));
        }

        [HttpPost]
        public RespondData GetEDUsersByEvent(UserOrgFilterRs filter)
        {
            return Success(this._service.GetEDUsersByEvent(filter.EventDiaryId));
        }

        [HttpPost]
        public RespondData GetEDUsersByEvents(UserOrgFilterRs filter)
        {
            return Success(this._service.GetEDUsersByEvents(filter.EventDiaryIds));
        }

        [HttpPost]
        public RespondData GetAllOrgs()
        {
            return Success(this._service.GetAllOrgs());
        }

        [HttpPost]
        public RespondData GetAllUsers()
        {
            return Success(this._service.GetAllUsers());
        }

        [HttpPost]
        public RespondData GetAllJobTitles()
        {
            return Success(this._service.GetAllJobTitles());
        }

        [HttpPost]
        public RespondData GetOrgs()
        {
            return Success(this._service.GetOrgs());
        }

        [HttpPost]
        public RespondData GetUsers()
        {
            return Success(this._service.GetUsers());
        }

        [HttpPost]
        public RespondData GetOrgByUser()
        {
            return Success(this._service.GetOrgByUser(this.UserName));
        }

        [HttpPost]
        public RespondData GetJobTitles()
        {
            return Success(this._service.GetJobTitles());
        }

        protected async Task AddAsync(UserOrgRS model)
        {
            var appDbContext = Request.GetOwinContext().Get<ApplicationDbContext>();
            using (var transaction = appDbContext.Database.BeginTransaction())
            {
                if (string.IsNullOrWhiteSpace(model.UserFullName))
                {
                    transaction.Rollback();
                    throw new System.Exception("Chưa nhập tên nhân viên");
                }
                if (string.IsNullOrWhiteSpace(model.PhoneNumber))
                {
                    transaction.Rollback();
                    throw new System.Exception("Chưa nhập số điện thoại nhân viên");
                }
                var id = "";
                if (model.IsCreateUser)
                {
                    // tim user
                    var user = this.AppUserManager.Users.FirstOrDefault(u => u.UserName.ToLower() == model.UserName.ToLower());
                    // chua ton tai
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = model.UserName,
                            Email = model.UserEmail,
                            Name = model.UserFullName,
                            PhoneNumber = model.PhoneNumber,
                            IsActived = false,
                        };
                        var pwd = model.DOB.GetValueOrDefault().ToString("ddMMyyyy");
                        var result = await this.AppUserManager.CreateAsync(user, pwd);
                        if (result.Succeeded)
                        {
                            id = user.Id;
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new System.Exception(string.Join(";", result.Errors));
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new System.Exception("Tên đăng nhập đã tồn tại");
                    }
                }

                // tao roles neu chua co
                await CreateRoles();

                //IEnumerable<AspNetRole> roles = this._service.GetRoles();
                if (!string.IsNullOrWhiteSpace(id))
                {
                    await AddRoleToUser(model, id);
                }
                var entity = AutoMapperHelper.Map<UserOrgRS, UserOrgDTO>(model);
                // tao user org thanh cong
                if (this._service.AddUserNoTrans(entity))
                {
                    transaction.Commit();
                }
                else
                {
                    transaction.Rollback();
                }
            }
        }

        [HttpPost]
        public async Task<RespondData> AddOrEditAsync(UserOrgRS model)
        {
            //// add
            if (model.Id <= 0)
            {
                await this.AddAsync(model);
            }
            // delete
            else
            {
                await this.Update(model);
            }
            return Success(null);
        }

        [HttpPost]
        public RespondData GetLevel1ManagerUserOrg()
        {
            return Success(this._service.GetLevel1ManagerUserOrg());
        }

        [HttpPost]
        public RespondData GetLevel2ManagerUserOrg()
        {
            return Success(this._service.GetLevel2ManagerUserOrg());
        }

        [HttpPost]
        public RespondData getLevel1ManagerByUserName([FromBody]UserOrgRS rs)
        {
            return Success(this._service.GetLevel1ManagerByUserName(rs.UserName, rs.YearMonth.Value));
        }

        [HttpPost]
        public RespondData GetOrgsByCurrentLevel2Manager()
        {
            return Success(this._service.GetOrgsByLevel2Manager(this.UserName));
        }

        [HttpPost]
        public RespondData GetOrgsByCurrentLevel1Manager()
        {
            return Success(this._service.GetOrgsByLevel1Manager(this.UserName));
        }

        protected async Task Update(UserOrgRS model)
        {
            var appDbContext = Request.GetOwinContext().Get<ApplicationDbContext>();
            using (var transaction = appDbContext.Database.BeginTransaction())
            {
                if (string.IsNullOrWhiteSpace(model.UserFullName))
                {
                    transaction.Rollback();
                    throw new System.Exception("Chưa nhập tên nhân viên");
                }
                var entity = AutoMapperHelper.Map<UserOrgRS, UserOrgDTO>(model);
                // tao user org thanh cong va tick chon tao user dang nhap

                var id = "";

                //  lay thong tin user hien tai
                var oldInfo = Get(model.Id).Data as UserInfoModel;
                if (oldInfo == null)
                {
                    throw new System.Exception("User không tồn tại");
                }
                // tim user hien tai neu co thi khong cho tao dang nhap
                var user = this.AppUserManager.Users.FirstOrDefault(u => u.UserName.ToLower() == oldInfo.UserName.ToLower());
                if (model.IsCreateUser)
                {
                    // chua ton tai
                    if (user == null)
                    {
                        user = new ApplicationUser
                        {
                            UserName = model.UserName,
                            Email = model.UserEmail,
                            Name = model.UserFullName,
                            PhoneNumber = model.PhoneNumber,
                            IsActived = model.IsActived,
                        };
                        var result = await this.AppUserManager.CreateAsync(user, "Brg@123456");
                        if (result.Succeeded)
                        {
                            id = user.Id;
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new System.Exception(string.Join(";", result.Errors));
                        }
                    }
                    else
                    {
                        transaction.Rollback();
                        throw new System.Exception("Tên đăng nhập đã tồn tại");
                    }
                }
                else
                {
                    // user da ton tai
                    if (user != null)
                    {
                        id = user.Id;
                        user.PhoneNumber = model.PhoneNumber;
                        user.Email = model.UserEmail;
                        user.IsActived = model.IsActived;
                        await this.AppUserManager.UpdateAsync(user);
                    }
                }

                // tao roles neu chua co
                await CreateRoles();

                if (!string.IsNullOrWhiteSpace(id))
                {
                    await AddRoleToUser(model, id);
                }

                if (this._service.UpdateUserNoTrans(entity))
                {

                    //
                    //user.IsDeleted = entity.Status == "NV";
                    //await this.AppUserManager.UpdateAsync(user);
                    transaction.Commit();
                }
            }
        }

        private async Task AddRoleToUser(UserOrgRS model, string id)
        {
            if (model.IsEmployee)
            {
                var roleResult = await AppUserManager.AddToRolesAsync(id, KpiUserRole.Employee);
            }
            else
            {
                if (await AppUserManager.IsInRoleAsync(id, KpiUserRole.Employee))
                {
                    await AppUserManager.RemoveFromRoleAsync(id, KpiUserRole.Employee);
                }
            }

            if (model.IsEmpManager)
            {
                var roleResult = await AppUserManager.AddToRolesAsync(id, KpiUserRole.EmpManager);
            }
            else
            {
                if (await AppUserManager.IsInRoleAsync(id, KpiUserRole.EmpManager))
                {
                    await AppUserManager.RemoveFromRoleAsync(id, KpiUserRole.EmpManager);
                }
            }

            if (model.IsEVoucherDistributor)
            {
                var roleResult = await AppUserManager.AddToRolesAsync(id, KpiUserRole.EVoucherDistributor);
            }
            else
            {
                if (await AppUserManager.IsInRoleAsync(id, KpiUserRole.EVoucherDistributor))
                {
                    await AppUserManager.RemoveFromRoleAsync(id, KpiUserRole.EVoucherDistributor);
                }
            }

            if (model.IsBudgetDistributor)
            {
                var roleResult = await AppUserManager.AddToRolesAsync(id, KpiUserRole.BudgetDistributor);
            }
            else
            {
                if (await AppUserManager.IsInRoleAsync(id, KpiUserRole.BudgetDistributor))
                {
                    await AppUserManager.RemoveFromRoleAsync(id, KpiUserRole.BudgetDistributor);
                }
            }

            if (model.IsEVoucherManager)
            {
                var roleResult = await AppUserManager.AddToRolesAsync(id, KpiUserRole.EVoucherManager);
            }
            else
            {
                if (await AppUserManager.IsInRoleAsync(id, KpiUserRole.EVoucherManager))
                {
                    await AppUserManager.RemoveFromRoleAsync(id, KpiUserRole.EVoucherManager);
                }
            }

            if (model.IsLevel2Manager)
            {
                var roleResult = await AppUserManager.AddToRolesAsync(id, KpiUserRole.Level2Manager);
            }
            else
            {
                if (await AppUserManager.IsInRoleAsync(id, KpiUserRole.Level2Manager))
                {
                    await AppUserManager.RemoveFromRoleAsync(id, KpiUserRole.Level2Manager);
                }
            }

            if (model.IsDistributorApprover)
            {
                var roleResult = await AppUserManager.AddToRolesAsync(id, KpiUserRole.DistributorApprover);
            }
            else
            {
                if (await AppUserManager.IsInRoleAsync(id, KpiUserRole.DistributorApprover))
                {
                    await AppUserManager.RemoveFromRoleAsync(id, KpiUserRole.DistributorApprover);
                }
            }

            if (model.DivisionManager)
            {
                var roleResult = await AppUserManager.AddToRolesAsync(id, KpiUserRole.DivisionManager);
            }
            else
            {
                if (await AppUserManager.IsInRoleAsync(id, KpiUserRole.DivisionManager))
                {
                    await AppUserManager.RemoveFromRoleAsync(id, KpiUserRole.DivisionManager);
                }
            }

        }

        private async Task CreateRoles()
        {
            await CreateRole(KpiUserRole.Employee);
            await CreateRole(KpiUserRole.EmpManager);
            await CreateRole(KpiUserRole.EVoucherDistributor);
            await CreateRole(KpiUserRole.BudgetDistributor);
            await CreateRole(KpiUserRole.EVoucherManager);
            await CreateRole(KpiUserRole.Level2Manager);
            await CreateRole(KpiUserRole.DistributorApprover);
            await CreateRole(KpiUserRole.DivisionManager);
        }

        private async Task CreateRole(string str)
        {
            var role = new ApplicationRole();
            bool rExists = await AppRoleManager.RoleExistsAsync(str);
            if (!rExists)
            {
                role.Name = str;
                await AppRoleManager.CreateAsync(role);
            }
        }

        public override RespondData Get(int id)
        {
            var res = this._service.Get(id);           
            // tim thay user va username khac rong
            if (res != null && !string.IsNullOrWhiteSpace(res.UserName))
            {
                var usr = this.AppUserManager.Users.FirstOrDefault(u => u.UserName.ToLower() == res.UserName);
                res.IsActived = usr.IsActived;
                if (usr != null)
                {
                    res.IsHasLogin = true;
                    if (usr.Roles != null && usr.Roles.Count > 0)
                    {
                        // lay danh sach role tu entity framework
                        var dbRoles = this._service.GetRoles();
                        var lstRoleName = new List<string>();

                        foreach (var ur in usr.Roles)
                        {
                            var dbRole = dbRoles.Where(r => r.Id == ur.RoleId).FirstOrDefault();
                            if (dbRole != null)
                            {
                                lstRoleName.Add(dbRole.Name);
                            }
                        }

                        if (lstRoleName.Count > 0)
                        {
                            res.IsEmpManager = lstRoleName.Contains(KpiUserRole.EmpManager);
                            res.IsEmployee = lstRoleName.Contains(KpiUserRole.Employee);
                            res.IsEVoucherDistributor = lstRoleName.Contains(KpiUserRole.EVoucherDistributor);
                            res.IsBudgetDistributor = lstRoleName.Contains(KpiUserRole.BudgetDistributor);
                            res.IsEVoucherManager = lstRoleName.Contains(KpiUserRole.EVoucherManager);
                            res.IsLevel2Manager = lstRoleName.Contains(KpiUserRole.Level2Manager);
                            res.IsDistributorApprover = lstRoleName.Contains(KpiUserRole.DistributorApprover);
                            res.DivisionManager = lstRoleName.Contains(KpiUserRole.DivisionManager);
                        }
                    }
                }
            }
            return Success(res);
        }

        [HttpPost]
        public async Task<RespondData> DeleteUser(UserOrgRS model)
        {
            //if (model.UserName.ToLower().StartsWith("test"))
            //{
            //    var appDbContext = Request.GetOwinContext().Get<ApplicationDbContext>();
            //    using (var transaction = appDbContext.Database.BeginTransaction())
            //    {
            //        var user = AppUserManager.Users.FirstOrDefault(x => x.UserName == model.UserName);
            //        if (user != null)
            //        {
            //            var result = await UserManager.DeleteAsync(user); //here result has two properties Errors and Succeeded.
            //            if (result.Succeeded)
            //            {
            //                var entity = AutoMapperHelper.Map<UserOrgRS, UserOrgDTO>(model);

            //                if (this._service.DeleteUser(entity))
            //                {
            //                    transaction.Commit();
            //                }
            //            }
            //        }
            //    }
            //}
            return Success(null);
        }

        [HttpPost]
        public RespondData CountDivisionManager(UserOrgFilterModel filter)
        {
            List<UserInfoModel> ret = SearchPagingDivision(filter);
            return Success(ret.Count());
        }

        [HttpPost]
        public RespondData SearchPagingDivisionManager(UserOrgFilterModel filter)
        {
            List<UserInfoModel> ret = SearchPagingDivision(filter);
            return Success(ret.Skip(filter.Start - 1).Take(filter.Length));
        }

        private List<UserInfoModel> SearchPagingDivision(UserOrgFilterModel filter)
        {
            // tim kiem
            var filterDTO = AutoMapperHelper.Map<UserOrgFilterModel, UserOrgFilterModelDTO>(filter);

            // lay danh sach user co quyen ql khoi/don vi
            var divManagerId = this.AppRoleManager.Roles.FirstOrDefault(x => x.Name == KpiUserRole.DivisionManager).Id;
            var users = this.AppUserManager.Users.Where(x => x.Roles.Any(r => r.RoleId == divManagerId)).Select(y => y.UserName).ToList();

            var userRes = _service.SearchPagingDivisionManager(filterDTO, users);

            return userRes.ToList();
        }

        [HttpPost]
        public RespondData GetDivisionManagerPermission(int id)
        {
            var res = this._service.GetDivisionManagerPermission(id);
            return Success(res);
        }

        [HttpPost]
        public RespondData saveDivisionManagerPermission(DivMngPerUser model)
        {
            this._service.SaveDivisionManagerPermission(model);
            return Success(null);
        }

        [HttpPost]
        public RespondData IsVIP()
        {
            return Success(this._service.IsVIP(this.UserName));
        }

        [HttpPost]
        public RespondData CheckExistedPhoneNumber(List<string> phones)
        {
            phones = phones.Where(x => !string.IsNullOrEmpty(x)).ToList();
            var rs = this.AppUserManager.Users.Where(x => phones.Contains(x.PhoneNumber)).Select(x => x.PhoneNumber).Distinct().ToList();
            return Success(rs);
        }

        [HttpPost]
        public async Task<RespondData> ImportUser(UserImportModel model)
        {
            var userDataList = model.ImportData.Select(x => new UserOrgRS {
                OrgId = model.OrgId,
                OrgName = model.OrgName,
                Code = x.Code,
                DOB = x.Dob,
                IdCardNumber = x.Identity,
                UserFullName = x.UserFullName,
                UserName = x.PhoneNumber,
                UserEmail = x.UserEmail,
                PhoneNumber = x.PhoneNumber
            });
            var invalidData = new List<string>();
            foreach (var user in userDataList)
            {
                try
                {
                    user.IsCreateUser = true;
                    user.UserName = user.PhoneNumber;
                    await this.AddAsync(user);
                } catch (Exception ex)
                {
                    invalidData.Add(user.PhoneNumber);
                }
            }
            return Success(invalidData);
        }
    }
}