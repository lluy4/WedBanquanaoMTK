using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Builder
{
    public interface INhapHangBuilder
    {
        INhapHangBuilder SetNgayNhap(DateTime ngayNhap);
        INhapHangBuilder SetIdSP(int idSP);
        INhapHangBuilder SetSoLuong(int soLuong);
        INhapHangBuilder SetGiaNhap(int giaNhap);
        NHAPHANG Build();
    }
}