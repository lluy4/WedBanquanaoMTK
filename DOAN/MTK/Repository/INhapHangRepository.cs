using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Repository
{
    public interface INhapHangRepository
    {
        bool ExistsNhapHang(int idSP, System.DateTime ngayNhap);
        void AddNhapHang(NHAPHANG nhapHang);
    }
}