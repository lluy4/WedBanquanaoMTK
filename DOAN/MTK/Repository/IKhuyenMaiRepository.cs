using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Repository
{
    public interface IKhuyenMaiRepository
    {
        IEnumerable<KHUYENMAI> GetAllKhuyenMais();
    }
}