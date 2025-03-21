using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Strategies
{
    public class QuantityDiscountStrategy : IDiscountStrategy
    {
        public int CalculateDiscount(List<GIOHANG> gioHang, KHUYENMAI khuyenMai, int tongTien)
        {
            if (khuyenMai == null || khuyenMai.LoaiKM != 3)
            {
                return 0;
            }
            int totalQuantity = gioHang.Sum(x => x.SoLuong);
            // Giả sử chiết khấu 10% nếu số lượng >= 5 sản phẩm
            if (totalQuantity >= 5)
            {
                return (int)(tongTien * 0.1);
            }
            return 0;
        }
    }
}