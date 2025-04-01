using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using DOAN.Models;
using DOAN.MTK.Repository;

namespace DOAN.Repositories
{
    public class NguoiDungRepository : INguoiDungRepository
    {
        private readonly TMDTDbContext _context;

        public NguoiDungRepository(TMDTDbContext context)
        {
            _context = context;
        }

        public IEnumerable<NGUOIDUNG> LayTatCaNguoiDung()
        {
            return _context.NGUOIDUNGs.Where(x => x.TT_User == true).ToList();
        }

        public IEnumerable<NGUOIDUNG> LayNguoiDungTheoLoaiUser(int loaiUserId)
        {
            return _context.NGUOIDUNGs.Where(x => x.TT_User == true && x.IdLoaiUser == loaiUserId).ToList();
        }

        public NGUOIDUNG LayNguoiDungTheoId(int id)
        {
            return _context.NGUOIDUNGs.Find(id);
        }

        public void ThemNguoiDung(NGUOIDUNG nguoiDung)
        {
            _context.NGUOIDUNGs.Add(nguoiDung);
        }

        public void CapNhatNguoiDung(NGUOIDUNG nguoiDung)
        {
            _context.Entry(nguoiDung).State = EntityState.Modified;
        }

        public void Luu()
        {
            _context.SaveChanges();
        }

        public IEnumerable<LOAIUSER> LayTatCaLoaiUser()
        {
            return _context.LOAIUSERs.ToList();
        }
    }
}