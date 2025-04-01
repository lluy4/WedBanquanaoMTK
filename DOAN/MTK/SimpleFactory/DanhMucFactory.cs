using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.SimpleFactory
{
    public static class DanhMucFactory
    {
        public static List<DANHMUC> CreateDanhMucList()
        {
            return new List<DANHMUC>
        {
            new DANHMUC(1, "ÁO SƠ MI"),
            new DANHMUC(2, "QUẦN DÀI"),
            new DANHMUC(3, "ÁO ẤM"),
            new DANHMUC(4, "ÁO THUN"),
            new DANHMUC(5, "ÁO KHOÁC"),
            new DANHMUC(6, "QUẦN NGẮN")
        };
        }
    }
}