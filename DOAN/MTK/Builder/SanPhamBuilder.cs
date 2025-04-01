using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Builder
{
    public class SanPhamBuilder : ISanPhamBuilder
    {
        private SANPHAM _sanPham;

        public SanPhamBuilder()
        {
            _sanPham = new SANPHAM();
        }

        public ISanPhamBuilder SetTenSP(string tenSP)
        {
            _sanPham.TenSP = tenSP;
            return this;
        }

        public ISanPhamBuilder SetAnhSP(string anhSP)
        {
            _sanPham.AnhSP = anhSP;
            return this;
        }

        public ISanPhamBuilder SetNgayTao(DateTime ngayTao)
        {
            _sanPham.NgayTao = ngayTao;
            return this;
        }

        public ISanPhamBuilder SetMoTa(string moTa)
        {
            _sanPham.MoTa = moTa;
            return this;
        }

        public ISanPhamBuilder SetTinhTrang(int? tinhTrang)
        {
            _sanPham.TinhTrang = tinhTrang;
            return this;
        }

        public ISanPhamBuilder SetGiaGoc(int? giaGoc)
        {
            _sanPham.GiaGoc = giaGoc;
            return this;
        }

        public ISanPhamBuilder SetLoiNhuan(float? loiNhuan)
        {
            _sanPham.LoiNhuan = loiNhuan;
            return this;
        }

        public ISanPhamBuilder SetDonVi(string donVi)
        {
            _sanPham.DonVi = donVi;
            return this;
        }

        public ISanPhamBuilder SetSoLuong(int? soLuong)
        {
            _sanPham.SoLuong = soLuong;
            return this;
        }

        public ISanPhamBuilder SetSoLanMua(int? soLanMua)
        {
            _sanPham.SoLanMua = soLanMua;
            return this;
        }

        public ISanPhamBuilder SetMaKM(int? maKM)
        {
            _sanPham.MaKM = maKM;
            return this;
        }

        public ISanPhamBuilder SetIdTH(int? idTH)
        {
            _sanPham.IdTH = idTH;
            return this;
        }

        public ISanPhamBuilder SetIdLoaiSP(int? idLoaiSP)
        {
            _sanPham.IdLoaiSP = idLoaiSP;
            return this;
        }

        public SANPHAM Build()
        {
            return _sanPham;
        }
    }
}