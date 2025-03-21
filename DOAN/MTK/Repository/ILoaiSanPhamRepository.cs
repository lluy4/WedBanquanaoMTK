using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Repository
{
    public interface ILoaiSanPhamRepository
    {
        IEnumerable<LOAISANPHAM> GetAllLoaiSanPhams();
    }
}