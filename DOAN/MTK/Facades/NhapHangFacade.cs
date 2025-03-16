using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DOAN.Models;
using System.Data.Entity;
//FACADE_BYHUY
namespace DOAN.MTK.Facades
{
    public class NhapHangFacade
    {
        private readonly TMDTDbContext _context;

        public NhapHangFacade(TMDTDbContext context)
        {
            _context = context;
        }

        public (bool Success, string ErrorMessage) ProcessNhapHang(IEnumerable<NHAPHANG> model, DateTime ngayNhap)
        {
            string loi = "";
            int error = 0;

            foreach (var item in model)
            {
                var nh = new NHAPHANG
                {
                    IdSP = item.IdSP,
                    NgayNhap = ngayNhap,
                    SoLuong = item.SoLuong,
                    GiaNhap = item.GiaNhap
                };

                if (_context.NHAPHANGs.Any(x => x.NgayNhap == nh.NgayNhap && x.IdSP == nh.IdSP))
                {
                    error = 1;
                    loi += _context.SANPHAMs.Find(nh.IdSP)?.TenSP + ", ";
                }
                else
                {
                    _context.NHAPHANGs.Add(nh);
                }
            }

            if (error == 1)
            {
                string noidung = "Sản phẩm " + loi.Substring(0, loi.Length - 2) + " đã được nhập hàng vào ngày hôm nay. Không thể cập nhật lại";
                return (false, noidung);
            }

            _context.SaveChanges();
            return (true, null);
        }
    }
}