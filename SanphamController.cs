using Quanlythuoc.ModelFromDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Quanlythuoc.ModelFromDB;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Quanlythuoc.Controllers
{
    [Route("api/Sanpham")]
    [ApiController]
    public class SanphamController : ControllerBase
    {
        private readonly CSDLBanThuoc db;
        public SanphamController(CSDLBanThuoc _db)
        {
            db = _db;
        }
        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<TblSanpham>>> GetAllProduct()
        {
            if (db.TblSanphams == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = from product in db.TblSanphams
                        join category in db.TblDanhmucs on product.DmMa equals category.DmMa
                        orderby product.SpNgaytao descending
                        select new
                        {
                            product.SpMa,
                            product.SpTen,
                            product.SpGia,
                            product.SpNgaytao,
                            product.SpMota,
                            product.DmMa,
                            product.SpDuongdananh,
                            category.DmDinhdang,
                            product.SpDonvi,
                            categoryName = category.DmTen
                        };
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data
            }); ;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TblSanpham>>> GetProduct(Guid id)
        {
            if (db.TblSanphams == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _data = await db.TblSanphams.Where(x => x.SpMa == id).FirstOrDefaultAsync();
            if (_data == null)
            {
                return Ok(new
                {
                    message = "Lấy dữ liệu thất bại!",
                    status = 400
                });
            }
            var category = db.TblSanphams.Find(_data.DmMa);
            return Ok(new
            {
                message = "Lấy dữ liệu thành công!",
                status = 200,
                data = _data,
                category
            });
        }
        [HttpPost("add")]
        public async Task<ActionResult> AddProduct([FromBody] TblSanpham product)
        {
            await db.TblSanphams.AddAsync(product);
            await db.SaveChangesAsync();
            return Ok(new
            {
                message = "Tạo thành công!",
                status = 200,
                data = product
            });
        }
        [HttpPut("edit")]

        public async Task<ActionResult> Edit([FromBody] TblSanpham product)
        {
            var _product = await db.TblSanphams.FindAsync(product.SpMa);
            if (_product == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu không tồn tại!",
                    status = 400
                });
            }
            db.Entry(await db.TblSanphams.FirstOrDefaultAsync(x => x.SpMa == _product.SpMa)).CurrentValues.SetValues(product);
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
            if (db.TblSanphams == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            var _product = await db.TblSanphams.FindAsync(id);
            if (_product == null)
            {
                return Ok(new
                {
                    message = "Dữ liệu trống!",
                    status = 404
                });
            }
            try
            {
                db.TblSanphams.Remove(_product);
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
