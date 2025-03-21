using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Repository
{
    public interface ISanPhamRepository
    {
        IEnumerable<SANPHAM> GetActiveSanPhams();
        IEnumerable<SANPHAM> GetSanPhamsByThuongHieu(int idTH);
        SANPHAM GetById(int id);
        void AddSanPham(SANPHAM sanPham);
        void UpdateSanPham(SANPHAM sanPham);
        void DeleteSanPham(int id);
    }
}