using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Strategies
{
    public class FixedDiscountStrategy : IDiscountStrategy
    {
        public int CalculateDiscount(List<GIOHANG> gioHang, KHUYENMAI khuyenMai, int tongTien)
        {
            if (khuyenMai == null || khuyenMai.LoaiKM != 2)
            {
                return 0;
            }
            return khuyenMai.GiaTri ?? 0;
        }
    }
}