using AutoMapper;
using Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Dto;
using Model.Form;
using Form;
using Modle.Model;
using Model.Dto;
using RepresentativesTracking;

namespace Controllers
{
    [Route("api/[action]")]
    [ApiController]
    public class UserController : BaseController
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController(IUserService userService,IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginForm form)
        {
            var user = await _userService.Authintication(form);
            if (user == null)
                return BadRequest(new Authintication
                {
                    Token = null,
                    Error = "UserName or Password Incorrect"
                });
            bool validPassword = BCrypt.Net.BCrypt.Verify(form.Password, user.Password);

            if (!validPassword)
            {
                return BadRequest(new Authintication
                {
                    Token = null,
                    Error = "UserName or Password Incorrect"
                });
            }
            var claims = new[]
                {
                   new Claim("ID", user.ID.ToString()),
                   new Claim("Username", user.UserName),
                   new Claim("Email", user.Email),
                   new Claim("Activated", user.Activated.ToString()),
                   new Claim("CompanyID", user.CompanyID.ToString()),
                   new Claim(ClaimTypes.Role, user.Role),
                   new Claim("Role", user.Role),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddDays(30),
                  notBefore: DateTime.UtcNow, audience: "Audience", issuer: "Issuer",
                  signingCredentials: new SigningCredentials(
                      new SymmetricSecurityKey(
                          Encoding.UTF8.GetBytes("Hlkjds0-324mf34pojf-14r34fwlknef0943")),
                      SecurityAlgorithms.HmacSha256));
                var Token = new JwtSecurityTokenHandler().WriteToken(token);
                var expire = DateTime.UtcNow.AddDays(30);
                return Ok(new { Token = Token, Expire = expire });

            
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword([FromBody] EmailForm EmailForm)
        {
            var Code = SendEmail.SendMessage(EmailForm.Email);
            var User = await _userService.GetUserByEmail(EmailForm.Email);
            if (User == null)
                return BadRequest(new { ERROR = "لم يتم العثور على حسابك" });
            await _userService.ChangeStatus(User.ID, false);
            if (Code != null)
            {
                var claims = new[]
                {
                   new Claim("Email", EmailForm.Email),
                   new Claim("Code",BCrypt.Net.BCrypt.HashPassword(Code.ToString())),
                   new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                var token = new JwtSecurityToken(claims: claims, expires: DateTime.UtcNow.AddMinutes(5),
                  notBefore: DateTime.UtcNow, audience: "Audience", issuer: "Issuer",
                  signingCredentials: new SigningCredentials(
                      new SymmetricSecurityKey(
                          Encoding.UTF8.GetBytes("Hlkjds0-324mf34pojf-14r34fwlknef0943")),
                      SecurityAlgorithms.HmacSha256));
                var Token = new JwtSecurityTokenHandler().WriteToken(token);
                var expire = DateTime.UtcNow.AddMinutes(1);
                return Ok(new { Token = Token, Expire = expire });

            }
            else return BadRequest();
        }
        [HttpGet("{Id}", Name = "GetUserById")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin + "," + UserRole.Representative)]
        public async Task<ActionResult<UserReadDto>> GetUserById(int Id)
        {
            var User = await _userService.FindById(Id);
            if (GetClaim("Role") != "Admin" || (GetClaim("Role")!= "DeliveryAdmin" && GetClaim("CompanyID") != User.CompanyID.ToString())|| GetClaim("ID")!=User.ID.ToString())
            {
                return BadRequest(new { Error = "لا يمكن تعديل بيانات تخص هذا المستخدم من دون صلاحية المدير" });
            }
            if (User == null)
            {
                return NotFound();
            }
            var UserModel = _mapper.Map<UserReadDto>(User);
            return Ok(UserModel);
        }
        [HttpGet("{PageNumber}/{Count}")]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<ActionResult<UserReadDto>> GetAllUsers(int PageNumber, int Count)
        {
            var Users = _userService.FindAll(PageNumber, Count).Result.ToList();
            var UserModel = _mapper.Map<List<UserReadDto>>(Users);
            return Ok(UserModel);
        }
        [HttpGet("{PageNumber}/{Count}")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<ActionResult<UserReadDto>> GetUsersByCompany(int PageNumber, int Count)
        {
            var Users = _userService.GetUsersByCompany(Convert.ToInt32(GetClaim("CompanyID")),PageNumber, Count).Result.ToList();
            var UserModel = _mapper.Map<List<UserReadDto>>(Users);
            return Ok(UserModel);
        }
        [HttpPost]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<IActionResult> AddUser([FromBody] UserWriteDto UserWriteDto)
        {
            if (GetClaim("Role") != "Admin" && UserWriteDto.CompanyId == null)
                return BadRequest(new { Error = "معرف الشركة مطلوب" });
            if (GetClaim("Role") != "DeliveryAdmin")
            {
                UserWriteDto.CompanyId = Convert.ToInt32(GetClaim("CompanyId"));
                UserWriteDto.Role = "Representative";
            }
            UserWriteDto.Password = BCrypt.Net.BCrypt.HashPassword(UserWriteDto.Password);
            var UserModel = _mapper.Map<User>(UserWriteDto);
            await _userService.Create(UserModel);
            var UserReadDto = _mapper.Map<UserReadDto>(UserModel);
            return CreatedAtRoute("GetUserById", new { Id = UserReadDto.ID }, UserReadDto);
        }
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordForm ChangePasswordForm)
        {
            var Email = GetClaim("Email");
            var User = await _userService.GetUserByEmail(Email);
            if (User.Activated == true)
            {
                if (User == null)
                {
                    return NotFound();
                }
                ChangePasswordForm.Password = BCrypt.Net.BCrypt.HashPassword(ChangePasswordForm.Password);
                await _userService.ChangePassword(User.ID, ChangePasswordForm.Password);
                return NoContent();
            }
            return BadRequest(new { Message = "الرمز غير صحيح" });
        }
        [HttpPut]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin + "," + UserRole.Representative)]
        public async Task<IActionResult> ChangePasswordByOld([FromBody] ChangePasswordFromOldForm ChangePasswordFromOldForm)
        {
            var UserModelFromRepo = await _userService.FindById(Convert.ToInt32(GetClaim("ID")));
            if (GetClaim("Role") != "Admin" && GetClaim("ID") != UserModelFromRepo.ID.ToString())
            {
                return BadRequest(new { Error = "لا يمكن تعديل بيانات تخص مستخدم آخر من دون صلاحية المدير" });
            }
            if (UserModelFromRepo == null)
            {
                return NotFound();
            }
            ChangePasswordFromOldForm.NewPassword = BCrypt.Net.BCrypt.HashPassword(ChangePasswordFromOldForm.NewPassword);
            await _userService.ChangePassword(UserModelFromRepo.ID, ChangePasswordFromOldForm.NewPassword);
            return NoContent();
            }
        [HttpPut("{id}")]
         [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin)]
        public async Task<IActionResult> UpdateUserByAdmin(int Id, [FromBody] UserUpdateByAdminDto UserUpdateByAdminDto)
        {
            if (GetClaim("Role") != "Admin" && GetClaim("ID") != Id.ToString())
            {
                return BadRequest(new { Error = "لا يمكن تعديل بيانات تخص مستخدم آخر من دون صلاحية المدير" });
            }
            var UserModelFromRepo = await _userService.FindById(Id);
            if (UserModelFromRepo == null)
            {
                return NotFound();
            }
            var UserModel = _mapper.Map<User>(UserUpdateByAdminDto);
            await _userService.ModifyByAdmin(Id, UserModel);
            return NoContent();
        }
        [HttpPut("{id}")]
        [Authorize(Roles = UserRole.Admin + "," + UserRole.DeliveryAdmin + "," + UserRole.Representative)]
        public async Task<IActionResult> UpdateUser(int Id, [FromBody] UserUpdateDto UserUpdateDto)
        {
            if (GetClaim("Role") != "Admin" && GetClaim("ID") != Id.ToString())
            {
                return BadRequest(new { Error = "لا يمكن تعديل بيانات تخص مستخدم آخر من دون صلاحية المدير" });
            }
            var UserModelFromRepo = await _userService.FindById(Id);
            if (UserModelFromRepo == null)
            {
                return NotFound();
            }
            var UserModel = _mapper.Map<User>(UserUpdateDto);
            await _userService.Modify(Id, UserModel);
            return NoContent();
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> SendCode([FromBody] CodeForm CodeForm)
        {
            var Code = GetClaim("Code");
            var Email = GetClaim("Email");
            var User = await _userService.GetUserByEmail(Email);
            if (BCrypt.Net.BCrypt.HashPassword(CodeForm.Code.ToString()) == Code)
            {
                await _userService.ChangeStatus(User.ID, true);
                return Ok(new { Message = "تم تفعيل حساب المستخدم" });
            }
            return BadRequest(new { Message = "الرمز غير صحيح" });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRole.Admin)]
        public async Task<IActionResult> DeleteUser(int Id)
        {
            var User = await _userService.Delete(Id);
            if (User == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}