using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Repository
{
    public class NhapHangRepository : INhapHangRepository
    {
        private readonly TMDTDbContext _context;

        public NhapHangRepository(TMDTDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public bool ExistsNhapHang(int idSP, DateTime ngayNhap)
        {
            return _context.NHAPHANGs.Any(x => x.IdSP == idSP && x.NgayNhap == ngayNhap);
        }

        public void AddNhapHang(NHAPHANG nhapHang)
        {
            _context.NHAPHANGs.Add(nhapHang);
            _context.SaveChanges();
        }
    }
}