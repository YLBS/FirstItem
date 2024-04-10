
using IServer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Model;

namespace WebApplication1.Controllers
{
    [Route("[controller]/[action]")]
    public class CarController : ControllerBase
    {
        #region 依赖注入

        private ICarInfoSerivce iCarService;

        private IJwtService _jwtService;
        public CarController(ICarInfoSerivce carInfoService, IJwtService jwtService)
        {
            iCarService = carInfoService;
            _jwtService = jwtService;
        }
        #endregion
        [HttpPost]
        [Authorize]
        public bool Add([FromBody]InputCarInfo CarInfo) {
            //return true;
            var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");

            string[] strings = _jwtService.ValidateToken(tokenHeader);
            if (strings[0].Equals("token过期")) { 
                return false;
            }
            else
            {
                return iCarService.Add(CarInfo, int.Parse(strings[1]));
            }
           
        }

        
        /// <summary>
        /// 首页未登录状态获取的
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetCar() {

            int count = 0;
            var carlist = iCarService.GetCarList(1, 12, out count, "", "桂林市");
            return new JsonResult(new
            {
                tatleCount = count,
                data = carlist,
            });
        }
        [HttpPost]
        [Authorize]
        public IActionResult GetCar1(int page, int limit,string searchCriteria="",string  cName="桂林市")
        {
            if (page == 0 || limit == 0)
            {
                return Ok("page,limit不能为0");
            }
            int count = 0;
            var carlist = iCarService.GetCarList(page, limit, out count, searchCriteria,cName);
            return new JsonResult(new
            {
                tatleCount = count,
                data = carlist
            });
        }
        [HttpPost]
        public IActionResult GetSearchCar(int page, int limit, SearchKey searchKey)
        {

            int count = 0;
            var carlist = iCarService.GetCarListBySearch(page, limit, out count, searchKey);
            return new JsonResult(new
            {
                tatleCount = count,
                data = carlist
            });
            //return Ok(searchKay);
        }
        [HttpPost]
        public IActionResult GetConOrder(int page)
        {

            int count = 0;
            var ConsignmentOrder = iCarService.GetConsignmentOrder(page, 12, out count);
            return new JsonResult(new
            {
                tatleCount = count,
                data = ConsignmentOrder
            });
        }
        /// <summary>
        /// 后台获取
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult GetCarInfo(int Id,bool tf=false)
        {

            var carInfoDTO = iCarService.GetCarInfo(Id,tf);
            return new JsonResult(new
            {
                data = carInfoDTO
            });
        }

        [HttpPost]
        [Authorize]
        public IActionResult UpState(int Id,string Idea,string Option)
        {
            bool result = false;
            if (Idea == "同意" || Idea == "不同意")
            {
                result = iCarService.UpCarState(Id, Idea, Option);
            }
            else {
                return new JsonResult(new
                {
                    data = result,
                    msg = "参数错误，传同意或不同意",
                });
            }
            return new JsonResult(new
            {
                data = result
            }) ;
        }
        [HttpPost]
        [Authorize]
        public IActionResult GetCarListByUserId(int page) {
            var tokenHeader = HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            string[] strings = _jwtService.ValidateToken(tokenHeader);
            int count = 0;
            var carlist = iCarService.GetCarListByUserId(page, 10, out count,Convert.ToInt32(strings[1]));
            return new JsonResult(new
            {
                tatleCount = count,
                data = carlist,
            });
        }
        [HttpPost, Authorize]
        public IActionResult SoldOutById(int id) {
            return Ok(iCarService.SoldOutById(id));
        }

        [HttpPost, Authorize]
        public IActionResult UpSalesAndCarState(UpSales upSales,int button,bool secondLevel)
        {
            return Ok(iCarService.UpNewSalesTickets(upSales,button, secondLevel));
        }
    }
}
