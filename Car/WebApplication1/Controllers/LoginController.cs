using DataEntity;
using IServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApplication1.Controllers
{

    [Route("[controller]/[action]")]
    public class UserLoginController : ControllerBase
    {
        private ILoginService _loginIServser;
        private IJwtService _jwtService;
        public UserLoginController(ILoginService loginIServser, IJwtService jwtService)
        {
            _loginIServser = loginIServser;
            _jwtService = jwtService;
        }
        [HttpPost]
        public IActionResult CheckPwd(LoginDTO loginDTO)
        {
            bool tf;
            MsgResult msgResult = _loginIServser.CheckLock(loginDTO.Phone);
            if (msgResult.result)
            {
                return new JsonResult(new
                {
                    data=!msgResult.result,
                    msgResult.msg,
                });
            }
            JwtMdel jwtMdel = _loginIServser.VerificationLogin(loginDTO.Phone, loginDTO.PassWord,out tf);
            if (tf)
            {
                string Token = _jwtService.GetToken(jwtMdel);
                return new JsonResult(new
                {
                    TokenString = Token,
                    data = true
                }) ;
            }
            return Ok(new
            {
                data = false,
                msg = "密码错误"
            });
        }
        [HttpPost]
        [Authorize]
        public IActionResult ChangePS(string pass)
        {
            var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            string[] strings = _jwtService.ValidateToken(tokenHeader);
            bool tf = _loginIServser.UpdatePassword(pass, Convert.ToInt32(strings[1]));
            
            return Ok(tf);
        }
        [HttpPost]
        [Authorize]
        public IActionResult Test(int id)
        {
            var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            string[] strings = _jwtService.ValidateToken(tokenHeader);
            // bool tf = _loginIServser.UpdatePassword(PassWord,Convert.ToInt32(strings[1]));

            return Ok(true);
        }

        [HttpPost]
        [Authorize]
        public IActionResult AnalysisToken()
        {
            var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            string[] strings = _jwtService.ValidateToken(tokenHeader);
            return Ok(strings);
        }
        [HttpPost]
        [Authorize]
        public IActionResult GetSalesTicket(int CarId)
        {
            var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            string[] strings = _jwtService.ValidateToken(tokenHeader);
            var sale = _loginIServser.GetSalesContract(CarId,Convert.ToInt32(strings[1]));
            return new JsonResult(new { 
                data=sale
            });
        }
        [HttpPost]
        public IActionResult GetUserInfoList(int page,string searchName="")
        {
             int count;
            var userinfo = _loginIServser.GetUserInfo(page, 10, out count, searchName);
            return new JsonResult(new
            {
                data=userinfo,
                total= count
            });
        }
        [HttpPost]
        [Authorize]
        public IActionResult GetUserInfo()
        {
            var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            string[] strings = _jwtService.ValidateToken(tokenHeader);
            var userinfo = _loginIServser.GetUserInfo(Convert.ToInt32(strings[1]));
            return new JsonResult(new
            {
                data = userinfo,
            });
        }
        [HttpPost]
        [Authorize]
        public IActionResult UpdateUserInfo([FromBody] OutUserInfo outUser)
        {
            bool tf = _loginIServser.UpdateUserInfo(outUser);
            return new JsonResult(new
            {
                data = tf,
            });
        }
        [HttpPost]
        [Authorize]
        public IActionResult ResetPasswords(int userId)
        {
            return new JsonResult(new
            {
                data = _loginIServser.ResetPasswords(userId)
            });
        }
        [HttpPost]
        [Authorize]
        public IActionResult NegationLock(int userId)
        {
            return new JsonResult(new
            {
                data = _loginIServser.NegationLock(userId)
            });
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddUser(string phone, string name, int roleId)
        {
            MsgResult msgResult = _loginIServser.CheckRepeatLock(phone);
            if (msgResult.result)
            {
                return new JsonResult(new
                {
                    msgResult.msg,
                    data = !msgResult.result
                });
            }
            return new JsonResult(new
            {
                data = _loginIServser.AddUserInfo(phone,name,roleId)
            });
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddUserByPhone(string phone)
        {
            return new JsonResult(new
            {
                data = _loginIServser.AddUserInfo(phone)
            });
        }
       

        [HttpPost]
        [Authorize]
        public IActionResult GetNamePhoneByCarId(int CarId)
        {
            OutNamePhone outNamePhone = _loginIServser.GetOutNamePhone(CarId);
            return new JsonResult(new
            {
                outNamePhone.Name,
                outNamePhone.PhoneNumber,
            });
        }
        [HttpPost]
        [Authorize]
        public IActionResult AddSalesTicket(int CarId) {
            var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            string[] strings = _jwtService.ValidateToken(tokenHeader);
            if (_loginIServser.AddSalesTicket(CarId, Convert.ToInt32(strings[1])))
            {
                return Ok(new{ data=true });
            }
            return Ok(new { data=false });
        }
        [HttpPost]
        [Authorize]
        public IActionResult GetSalesList(int page) {
            var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            string[] strings = _jwtService.ValidateToken(tokenHeader);
            int count;
            var outSaleList = _loginIServser.GetSalesList(page, 10, out count, Convert.ToInt32(strings[1]));
            return Ok(new {
                data = outSaleList,
                total=count
            });
        }
        [HttpPost]
        [Authorize]
        public IActionResult GetSellOrdersList(int page,bool tf)
        {
            var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            string[] strings = _jwtService.ValidateToken(tokenHeader);
            int count;
            var outSaleList = _loginIServser.GetSellOrdersListByUserId(page, 10, out count, Convert.ToInt32(strings[1]),tf);
            return Ok(new
            {
                data = outSaleList,
                total = count
            });
        }
        [HttpPost]
        [Authorize]
        public IActionResult GetSalesTicketBySaleId(int SaleId)
        {
            var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            string[] strings = _jwtService.ValidateToken(tokenHeader);
            var sale = _loginIServser.GetSalesContract1(SaleId,Convert.ToInt32(strings[1]));
            return new JsonResult(new
            {
                data = sale
            });
        }

        [HttpPost]
        public IActionResult GetMenu() {
            var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            string[] strings = _jwtService.ValidateToken(tokenHeader);
            
            var menu = _loginIServser.outMenus(Convert.ToInt32(strings[1]));
            return new JsonResult(new
            {
                data = menu,
            });
        }
        
    }
}
