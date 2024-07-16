using Quanlythuoc.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quanlythuoc.ModelFromDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace Quanlythuoc.Controllers
{
    [Route("api/Donhang")]
    [ApiController]
    public class DonhangController : ControllerBase
    {
        private readonly CSDLBanThuoc db;
        public DonhangController(CSDLBanThuoc _db)
        {
            db = _db;
        }
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<TblDonhang>>> GetAllOrder()
        {
            if (db.TblNguoidungs == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = from order in db.TblDonhangs
                        join khachhang in db.TblKhachhangs on order.KhMa equals khachhang.KhMa
                        join nguoidung in db.TblNguoidungs on order.NdMa equals nguoidung.NdMa
                        orderby order.DhNgaylap descending
                        select new
                        {
                            order.DhMa,
                            order.KhMa,
                            order.NdMa,
                            khachhang.KhTen,
                            order.DhTongtien,
                            order.DhNgaylap,
                        };
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }
        [HttpGet]

        public async Task<ActionResult<IEnumerable<TblDonhang>>> GetOrder(Guid id)
        {
            if (db.TblDonhangs == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = await db.TblDonhangs.Where(x => x.DhMa == id).ToListAsync();
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }
        [HttpPut("edit")]
        public async Task<ActionResult> Edit([FromBody] TblDonhang order)
        {
            var _order = await db.TblDonhangs.FindAsync(order.DhMa);
            if (_order == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }
            db.Entry(await db.TblNguoidungs.FirstOrDefaultAsync(x => x.NdMa == _order.DhMa)).CurrentValues.SetValues(order);
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Sửa thành công!",
                status = 200
            });
        }
        [HttpPost("add")]
        public async Task<ActionResult> AddOrder([FromBody] TblDonhang order)
        {
            await db.TblDonhangs.AddAsync(order);
            await db.SaveChangesAsync();
            var _data = await db.TblDonhangs.Where(x => x.KhMa == order.KhMa).FirstOrDefaultAsync();
            return Ok(new
            {
                message = "Tạo thành công!",
                status = 200,
                data = _data
            });
        }

        [HttpDelete("delete")]
        public async Task<ActionResult> Delete([FromBody] Guid id)
        {
            if (db.TblDonhangs == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _order = await db.TblDonhangs.FindAsync(id);
            if (_order == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            try
            {
                db.TblDonhangs .Remove(_order);
                db.SaveChanges();
                return Ok(new
                {
                    message = "Xóa thành công!",
                    status = 200
                });
            }
            catch (Exception e)
            {
                return Ok(new
                {
                    message = "Lỗi rồi!",
                    status = 400,
                    data = e.Message
                });
            }
        }
        
    }
}
