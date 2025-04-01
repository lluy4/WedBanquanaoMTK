using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Repository
{
    public interface INguoiDungRepository
    {
        IEnumerable<NGUOIDUNG> LayTatCaNguoiDung();
        IEnumerable<NGUOIDUNG> LayNguoiDungTheoLoaiUser(int loaiUserId);
        NGUOIDUNG LayNguoiDungTheoId(int id);
        void ThemNguoiDung(NGUOIDUNG nguoiDung);
        void CapNhatNguoiDung(NGUOIDUNG nguoiDung);
        void Luu();
        IEnumerable<LOAIUSER> LayTatCaLoaiUser();
    }
}