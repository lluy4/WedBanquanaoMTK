using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Repository
{
    public class KhuyenMaiRepository : IKhuyenMaiRepository
    {
        private readonly TMDTDbContext _context;

        public KhuyenMaiRepository(TMDTDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<KHUYENMAI> GetAllKhuyenMais()
        {
            return _context.KHUYENMAIs.ToList();
        }
    }
}