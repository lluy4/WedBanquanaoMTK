using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Builder
{
    public class NhapHangBuilder : INhapHangBuilder
    {
        private NHAPHANG _nhapHang;

        public NhapHangBuilder()
        {
            _nhapHang = new NHAPHANG();
        }

        public INhapHangBuilder SetNgayNhap(DateTime ngayNhap)
        {
            _nhapHang.NgayNhap = ngayNhap;
            return this;
        }

        public INhapHangBuilder SetIdSP(int idSP)
        {
            _nhapHang.IdSP = idSP;
            return this;
        }

        public INhapHangBuilder SetSoLuong(int soLuong)
        {
            _nhapHang.SoLuong = soLuong;
            return this;
        }

        public INhapHangBuilder SetGiaNhap(int giaNhap)
        {
            _nhapHang.GiaNhap = giaNhap;
            return this;
        }

        public NHAPHANG Build()
        {
            return _nhapHang;
        }
    }
}