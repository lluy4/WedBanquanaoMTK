using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Builder
{
    public interface ISanPhamBuilder
    {
        ISanPhamBuilder SetTenSP(string tenSP);
        ISanPhamBuilder SetAnhSP(string anhSP);
        ISanPhamBuilder SetNgayTao(DateTime ngayTao);
        ISanPhamBuilder SetMoTa(string moTa);
        ISanPhamBuilder SetTinhTrang(int? tinhTrang);
        ISanPhamBuilder SetGiaGoc(int? giaGoc);
        ISanPhamBuilder SetLoiNhuan(float? loiNhuan);
        ISanPhamBuilder SetDonVi(string donVi);
        ISanPhamBuilder SetSoLuong(int? soLuong);
        ISanPhamBuilder SetSoLanMua(int? soLanMua);
        ISanPhamBuilder SetMaKM(int? maKM);
        ISanPhamBuilder SetIdTH(int? idTH);
        ISanPhamBuilder SetIdLoaiSP(int? idLoaiSP);
        SANPHAM Build();
    }
}