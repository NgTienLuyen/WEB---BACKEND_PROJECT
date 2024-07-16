using Quanlythuoc.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quanlythuoc.ModelFromDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Quanlythuoc.Controllers
{
    [Route("api/Vaitro")]
    [ApiController]
    public class VaitroController : ControllerBase
    {
        private readonly CSDLBanThuoc db;
        public VaitroController(CSDLBanThuoc _db)
        {
            db = _db;
        }
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<TblVaitro>>> GetAllRole()
        {
            if (db.TblVaitros == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = await db.TblVaitros.ToListAsync();
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblVaitro>>> GetRole(Guid id)
        {
            if (db.TblVaitros == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = await db.TblVaitros.Where(x => x.VtMa == id).ToListAsync();
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }
        [HttpPost("add")]

        public async Task<ActionResult> AddRole([FromBody] TblVaitro role)
        {
            var _role = await db.TblVaitros.FirstOrDefaultAsync(x => String.Compare(x.VtTen, role.VtTen, StringComparison.OrdinalIgnoreCase) == 0);
            if (_role != null)
            {
                return Ok(new
                {
                    message = "Role đã tồn tại!",
                    status = 400
                });
            }
            await db.TblVaitros.AddAsync(role);
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Tạo thành công!",
                status = 200,
                data = role
            });
        }
        [HttpPut("edit")]
        public async Task<ActionResult> Edit([FromBody] TblVaitro role)
        {
            var _role = await db.TblVaitros.FindAsync(role.VtMa);
            if (_role == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }
            db.Entry(await db.TblVaitros.FirstOrDefaultAsync(x => x.VtMa == role.VtMa)).CurrentValues.SetValues(role);
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Sửa thành công!",
                status = 200
            });
        }
        [HttpDelete("delete")]

        public async Task<ActionResult> Delete([FromBody] Guid id)
        {
            if (db.TblVaitros == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _role = await db.TblVaitros.FindAsync(id);
            if (_role == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            try
            {
                db.TblVaitros.Remove(_role);
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
