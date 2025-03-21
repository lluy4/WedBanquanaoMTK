using DOAN.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Repository
{
    public class SanPhamRepository : ISanPhamRepository
    {
        private readonly TMDTDbContext _context;

        public SanPhamRepository(TMDTDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public IEnumerable<SANPHAM> GetActiveSanPhams()
        {
            return _context.SANPHAMs.Where(x => x.TinhTrang == 1 || x.TinhTrang == 2).ToList();
        }

        public IEnumerable<SANPHAM> GetSanPhamsByThuongHieu(int idTH)
        {
            return _context.SANPHAMs.Where(x => (x.TinhTrang == 1 || x.TinhTrang == 2) && x.IdTH == idTH).ToList();
        }

        public SANPHAM GetById(int id)
        {
            return _context.SANPHAMs.FirstOrDefault(x => x.IdSP == id);
        }

        public void AddSanPham(SANPHAM sanPham)
        {
            _context.SANPHAMs.Add(sanPham);
            _context.SaveChanges();
        }

        public void UpdateSanPham(SANPHAM sanPham)
        {
            _context.Entry(sanPham).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public void DeleteSanPham(int id)
        {
            var sanPham = _context.SANPHAMs.Find(id);
            if (sanPham != null)
            {
                sanPham.TinhTrang = 10;
                _context.Entry(sanPham).State = EntityState.Modified;
                _context.SaveChanges();
            }
        }
    }
}