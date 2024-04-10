using DataEntity;
using IServer;
using Microsoft.AspNetCore.Mvc;
using Model;
using qcloudsms_csharp;


namespace WebApplication1.Controllers
{
    [Route("[controller]/[action]")]
    public class SmsController : ControllerBase
    {
        #region 依赖注入
        private ISmsService ismsService;
        private IJwtService _jwtService;

        private ILoginService _loginIServser;

        public SmsController(ISmsService smsService, IJwtService jwtService, ILoginService loginService)
        {
            ismsService = smsService;
            _loginIServser = loginService;
            _jwtService = jwtService;
        }
        #endregion


        private int appid = 1400888698;
        // 短信应用 SDK AppKey
        private string appkey = "6651c49bc3b6523af7c1d29157f24131";
       
        // 短信模板 ID，需要在短信控制台中申请
        private int templateId = 2104449; // NOTE: 这里的模板 ID`7839`只是示例，真实的模板 ID 需要在短信控制台中申请

        private string smsSign = "童柏华测试毕设专用公众号"; // NOTE: 签名参数使用的是`签名内容`，而不是`签名ID`。这里的签名"腾讯云"只是示例，真实的签名需要在短信控制台申请

        [HttpPost]
        public IActionResult SendSms(string PhoneNum)
        {
            MsgResult msgResult = ismsService.CheckLock(PhoneNum);
            if (!msgResult.result) {
                return Ok(msgResult);
            }
            if (ismsService.VerifyExpiration(PhoneNum)) {
                return Ok(new MsgResult
                {
                    msg = "请勿重复获取",
                    result = false
                });
            }

            #region 发送验证码部分
           
            string Code = "";            
            Random random = new Random();
            for (int i = 0; i < 4; i++)
            {
                Code = Code + random.Next(0, 10);
            }
            try
            {
                SmsSingleSender ssender = new SmsSingleSender(appid, appkey);
                var result = ssender.sendWithParam("86", PhoneNum,
                    templateId, new[] { Code, "5" }, smsSign, "", "");
                //result={ "result":0,"errmsg":"OK","ext":"","sid":"9319:109792256117126553305979057","fee":1,"isocode":"CN"}

                if (result.errMsg == "OK")
                {
                    ismsService.Add(PhoneNum, Code);
                    if (msgResult.msg == "注册") {
                        _loginIServser.AddUserInfo(PhoneNum);
                    }
                    return Ok(new MsgResult
                    {
                        msg = "验证码已发送，有效期为5分钟",
                        result = true
                    });
                }
                return Ok(new MsgResult
                {
                    msg = result.errMsg,
                    result = false
                });

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Ok(new MsgResult
                {
                    msg = "发生错误",
                    result = false
                });
            }
            #endregion
        }


        [HttpPost]
        public IActionResult VerifyCode(string phoneNumber,string code)
        {
            if (ismsService.VerifyCode(phoneNumber, code)) {
                JwtMdel jwtMdel = _loginIServser.VerificationLogin(phoneNumber);
                string Token = _jwtService.GetToken(jwtMdel);
                return new JsonResult(new
                {
                    TokenString = Token,
                    result = true
                });
            }
            return Ok(new MsgResult
            {
                msg = "验证码错误",
                result = false
            });

        }

      
    }
}
