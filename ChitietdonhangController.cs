using Quanlythuoc.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quanlythuoc.ModelFromDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace Quanlythuoc.Controllers
{
    [Route("api/Donhang/Chitiet")]
    [ApiController]
    public class ChitietdonhangController : ControllerBase
    {
        private readonly CSDLBanThuoc db;
        public ChitietdonhangController(CSDLBanThuoc _db)
        {
            db = _db;
        }
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<TblChitietdonhang>>> GetAllDetailOrder()
        {
            if (db.TblChitietdonhangs == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = await db.TblChitietdonhangs.ToListAsync();
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblChitietdonhang>>> GetDetailOrder(Guid id)
        {
            if (db.TblChitietdonhangs == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = await db.TblChitietdonhangs.Where(x => x.CtdhMa == id).ToListAsync();
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }
        [HttpPost("add")]
        public async Task<ActionResult> AddDetail([FromBody] TblChitietdonhang ctdh)
        {

            var _detail = await db.TblChitietdonhangs.Where(x => x.SpMa == ctdh.SpMa).Where(x => x.DhMa == ctdh.DhMa).FirstOrDefaultAsync();
            if (_detail == null)
            {
                await db.TblChitietdonhangs.AddAsync(ctdh);
            }
            else
            {
                _detail.CtdhGia += ctdh.CtdhGia;
                db.Entry(await db.TblChitietdonhangs.FirstOrDefaultAsync(x => x.CtdhMa == _detail.CtdhMa)).CurrentValues.SetValues(_detail);
            }
            await db.SaveChangesAsync(); 
            return Ok(new
            {
                message = "Tạo thành công!",
                status = 200,
                data = ctdh 
            });
        }
        [HttpPut("edit")]
        public async Task<ActionResult> Edit([FromBody] TblChitietdonhang ctdh)
        {
            var _detail = await db.TblChitietdonhangs.FindAsync(ctdh.CtdhMa);
            if (_detail == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }
            db.Entry(await db.TblChitietdonhangs.FirstOrDefaultAsync(x => x.DhMa== _detail.DhMa)).CurrentValues.SetValues(ctdh);
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
            if (db.TblChitietdonhangs == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _detail = await db.TblChitietdonhangs.FindAsync(id);
            if (_detail == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            try
            {
                db.TblChitietdonhangs.Remove(_detail);
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

        [HttpGet("getAllByOrder")]

        public async Task<ActionResult<IEnumerable<TblChitietdonhang>>> GetAllByOrder(Guid Dhma)
        {
            var _data = from dt in db.TblChitietdonhangs
                        join pr in db.TblSanphams on dt.SpMa equals pr.SpMa
                        where dt.DhMa == Dhma
                        select new
                        {
                            dt.SpMa,
                            dt.CtdhGia,
                            dt.CtdhSoluong,
                            dt.CtdhNgaytao,
                            pr.SpMota,
                            pr.SpDuongdananh,
                            pr.SpTen,
                        };
            //var _data = await db.Detailorders.Where(x => x.IdOrder == idOrder).ToListAsync();
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            });
        }

        [HttpPut("increase")]
        public async Task<ActionResult> Increase([FromBody] Guid Dhma)
        {
            var _detail = await db.TblChitietdonhangs.FindAsync(Dhma);
            if (_detail == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }
            _detail.CtdhSoluong = _detail.CtdhSoluong + 1;
            db.Entry(await db.TblChitietdonhangs.FirstOrDefaultAsync(x => x.CtdhMa == _detail.CtdhMa)).CurrentValues.SetValues(_detail);
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Sửa thành công!",
                status = 200
            });
        }
        [HttpPut("decrease")]

        public async Task<ActionResult> Decrease([FromBody] Guid Dhma)
        {
            var _detail = await db.TblChitietdonhangs.FindAsync(Dhma);
            if (_detail == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }
            if (_detail.CtdhSoluong == 1)
            {
                db.TblChitietdonhangs.Remove(_detail);
                await db.SaveChangesAsync();
                return Ok(new
                {
                    status = 200
                });
            }
            _detail.CtdhSoluong = _detail.CtdhSoluong - 1;
            db.Entry(await db.TblChitietdonhangs.FirstOrDefaultAsync(x => x.DhMa == _detail.DhMa)).CurrentValues.SetValues(_detail);
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Sửa thành công!",
                status = 200
            });
        }
    }
}
