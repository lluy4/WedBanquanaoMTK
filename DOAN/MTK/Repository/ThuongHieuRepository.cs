using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Repository
{
    public class ThuongHieuRepository : IThuongHieuRepository
    {
        private readonly TMDTDbContext _context;

        public ThuongHieuRepository(TMDTDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<THUONGHIEU> GetActiveThuongHieus()
        {
            return _context.THUONGHIEUx.Where(x => x.TinhTrang == true).ToList();
        }

        public THUONGHIEU GetById(int id)
        {
            return _context.THUONGHIEUx.Find(id);
        }
    }
}