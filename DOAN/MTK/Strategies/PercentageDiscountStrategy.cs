using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Strategies
{
    public class PercentageDiscountStrategy : IDiscountStrategy
    {
        public int CalculateDiscount(List<GIOHANG> gioHang, KHUYENMAI khuyenMai, int tongTien)
        {
            if (khuyenMai == null || khuyenMai.LoaiKM != 1)
            {
                return 0;
            }
            return (tongTien * (khuyenMai.GiaTri ?? 0)) / 100;
        }
    }
}