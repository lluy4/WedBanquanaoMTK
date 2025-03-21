using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Repository
{
    public class LoaiSanPhamRepository : ILoaiSanPhamRepository
    {
        private readonly TMDTDbContext _context;

        public LoaiSanPhamRepository(TMDTDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<LOAISANPHAM> GetAllLoaiSanPhams()
        {
            return _context.LOAISANPHAMs.ToList();
        }
    }
}