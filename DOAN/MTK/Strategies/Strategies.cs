using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Strategies
{
    public interface IDiscountStrategy
    {
        int CalculateDiscount(List<GIOHANG> gioHang, KHUYENMAI khuyenMai, int tongTien);
    }
}