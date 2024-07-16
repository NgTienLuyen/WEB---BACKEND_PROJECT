using Quanlythuoc.ModelFromDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Quanlythuoc.Controllers
{
    [Route("api/DanhMuc")]
    [ApiController]
    public class DanhMucController : ControllerBase
    {
        private readonly CSDLBanThuoc db;
        public DanhMucController(CSDLBanThuoc _db)
        {
            db = _db;
        }
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<TblDanhmuc>>> GetAllDanhMuc()
        {
            if (db.TblDanhmucs == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = await db.TblDanhmucs.ToListAsync();
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblDanhmuc>>> GetDanhMuc(Guid id)
        {
            if (db.TblDanhmucs == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = await db.TblDanhmucs.Where(x => x.DmMa == id).ToListAsync();
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }
        [HttpPost("add")]
        public async Task<ActionResult> AddCategory([FromBody] TblDanhmuc danhmuc)
        {
            var _danhmuc = await db.TblDanhmucs.Where(x => x.DmDinhdang.Equals(danhmuc.DmDinhdang)).ToListAsync();
            if (_danhmuc.Count != 0)
            {
                return Ok(new
                {
                    message = "Tạo thất bại!",
                    status = 400,
                }); 
            }
            await db.TblDanhmucs.AddAsync(danhmuc);
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Tạo thành công!",
                status = 200,
                data = danhmuc
            });
        }
        [HttpPut("edit")]
        public async Task<ActionResult> Edit([FromBody] TblDanhmuc danhmuc)
        {
            var _danhmuc = await db.TblDanhmucs.FindAsync(danhmuc.DmMa);
            if (_danhmuc == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }
            db.Entry(await db.TblDanhmucs.FirstOrDefaultAsync(x => x.DmMa == danhmuc.DmMa)).CurrentValues.SetValues(danhmuc);
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Sửa thành công!",
                status = 200
            });
        }
        [HttpPut("delete")]
        public async Task<ActionResult> Delete([FromBody] Guid DmMa)
        {
            if (db.TblDanhmucs == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _danhmuc = await db.TblDanhmucs.FindAsync(DmMa);
            if (_danhmuc == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            try
            {
                db.TblDanhmucs.Remove(_danhmuc);
                await db.SaveChangesAsync();
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
