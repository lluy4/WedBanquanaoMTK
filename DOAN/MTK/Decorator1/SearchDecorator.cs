using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DOAN.Models;

namespace DOAN.MTK.Decorator1
{
    public interface Timkiem
    {
        IQueryable<SANPHAM> Search(IQueryable<SANPHAM> sanpham, string tukhoa);
    }

    public class BaseSearch : Timkiem
    {
        public IQueryable<SANPHAM> Search(IQueryable<SANPHAM> sanpham, string tukhoa)
        {
            return sanpham.Where(p => p.TenSP.Contains(tukhoa) && p.TinhTrang == 1);
        }
    }

    public class Timkiemtheogia : Timkiem
    {
        private readonly Timkiem _timkiem;
        private readonly int? _giamin;
        private readonly int? giamax;

        public Timkiemtheogia(Timkiem search, int? minPrice, int? maxPrice)
        {
            _timkiem = search;
            _giamin = minPrice;
            giamax = maxPrice;
        }

        public IQueryable<SANPHAM> Search(IQueryable<SANPHAM> sanpham, string tukhoa)
        {
            var result = _timkiem.Search(sanpham, tukhoa);
            if (_giamin.HasValue)
            {
                result = result.Where(p => p.GiaGoc >= _giamin.Value);
            }
            if (giamax.HasValue)
            {
                result = result.Where(p => p.GiaGoc <= giamax.Value);
            }
            return result;
        }
    }

    public class Timkiemtheoloai : Timkiem
    {
        private readonly Timkiem timkiem;
        private readonly int? loai;

        public Timkiemtheoloai(Timkiem search, int? categoryId)
        {
            timkiem = search;
            loai = categoryId;
        }

        public IQueryable<SANPHAM> Search(IQueryable<SANPHAM> sanpham, string tukhoa)
        {
            var result = timkiem.Search(sanpham, tukhoa);
            if (loai.HasValue)
            {
                result = result.Where(p => p.IdLoaiSP == loai.Value);
            }
            return result;
        }
    }
}