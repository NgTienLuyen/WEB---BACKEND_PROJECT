using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quanlythuoc.ModelFromDB;

namespace Quanlythuoc.Controllers
{
    [Route("api/Nguoidung")]
    [ApiController]
    public class NguoidungController : ControllerBase
    {
        private readonly CSDLBanThuoc db;
        private readonly IConfiguration _config;

        public NguoidungController(CSDLBanThuoc _db, IConfiguration cf)
        {
            db = _db;
            _config = cf;
        }
        [HttpGet("all")]

        public async Task<ActionResult<IEnumerable<TblNguoidung>>> GetAllUser()
        {
            if (db.TblNguoidungs == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = from x in db.TblNguoidungs
                        join role in db.TblVaitros on x.VtMa equals role.VtMa
                        select new
                        {
                            x.NdMa,
                            x.NdTen,
                            x.NdEmail,
                            x.NdMatkhau,
                            x.NdSodienthoai,
                            x.NdDiachi,
                            x.NdNgaytao,
                            x.VtMa,
                            x.NdDuongdananh,
                            nameRole = role.VtTen,
                        };
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }
    }
}
