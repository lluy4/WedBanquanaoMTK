using DOAN.Models;
using System;
//DECORATOR_HUY
namespace DOAN.MTK.Decorator
{
    public interface IKhuyenMai
    {
        int IdMa { get; set; }
        string MaKM { get; set; } // Sửa từ TenKM thành MaKM
        DateTime? NgayBD { get; set; }
        DateTime? NgayKT { get; set; }
        int? LoaiKM { get; set; }
        int? GiaTri { get; set; } // Sửa từ decimal? thành int? để khớp với database
        string ChiTiet { get; set; } // Thêm ChiTiet
        bool? TinhTrang { get; set; }
        decimal CalculateDiscount(decimal originalPrice); // Giữ nguyên, tính toán dựa trên GiaTri (int?)
    }

    public class DecoratedKhuyenMai : IKhuyenMai
    {
        private readonly DOAN.Models.KHUYENMAI _khuyenMai;

        public DecoratedKhuyenMai(DOAN.Models.KHUYENMAI khuyenMai)
        {
            _khuyenMai = khuyenMai ?? throw new ArgumentNullException(nameof(khuyenMai));
        }

        public int IdMa { get => _khuyenMai.IdMa; set => _khuyenMai.IdMa = value; }
        public string MaKM { get => _khuyenMai.MaKM; set => _khuyenMai.MaKM = value; }
        public DateTime? NgayBD { get => _khuyenMai.NgayBD; set => _khuyenMai.NgayBD = value; }
        public DateTime? NgayKT { get => _khuyenMai.NgayKT; set => _khuyenMai.NgayKT = value; }
        public int? LoaiKM { get => _khuyenMai.LoaiKM; set => _khuyenMai.LoaiKM = value; }
        public int? GiaTri { get => _khuyenMai.GiaTri; set => _khuyenMai.GiaTri = value; } // Sửa thành int?
        public string ChiTiet { get => _khuyenMai.ChiTiet; set => _khuyenMai.ChiTiet = value; }
        public bool? TinhTrang { get => _khuyenMai.TinhTrang; set => _khuyenMai.TinhTrang = value; }

        public virtual decimal CalculateDiscount(decimal originalPrice)
        {
            if (LoaiKM == null || GiaTri == null) return 0; // Kiểm tra null
            if (LoaiKM == 1) // Giảm phần trăm
                return originalPrice * ((decimal)GiaTri / 100); // Ép kiểu int? sang decimal
            else if (LoaiKM == 2) // Giảm trực tiếp
                return (decimal)GiaTri; // Ép kiểu int? sang decimal
            return 0;
        }
    }

    public abstract class KhuyenMaiDecorator : IKhuyenMai
    {
        protected IKhuyenMai _khuyenMai;

        public KhuyenMaiDecorator(IKhuyenMai khuyenMai)
        {
            _khuyenMai = khuyenMai ?? throw new ArgumentNullException(nameof(khuyenMai));
        }

        public int IdMa { get => _khuyenMai.IdMa; set => _khuyenMai.IdMa = value; }
        public string MaKM { get => _khuyenMai.MaKM; set => _khuyenMai.MaKM = value; }
        public DateTime? NgayBD { get => _khuyenMai.NgayBD; set => _khuyenMai.NgayBD = value; }
        public DateTime? NgayKT { get => _khuyenMai.NgayKT; set => _khuyenMai.NgayKT = value; }
        public int? LoaiKM { get => _khuyenMai.LoaiKM; set => _khuyenMai.LoaiKM = value; }
        public int? GiaTri { get => _khuyenMai.GiaTri; set => _khuyenMai.GiaTri = value; }
        public string ChiTiet { get => _khuyenMai.ChiTiet; set => _khuyenMai.ChiTiet = value; }
        public bool? TinhTrang { get => _khuyenMai.TinhTrang; set => _khuyenMai.TinhTrang = value; }

        public virtual decimal CalculateDiscount(decimal originalPrice) => _khuyenMai.CalculateDiscount(originalPrice);
    }

    public class ExtraDiscountDecorator : KhuyenMaiDecorator
    {
        private readonly decimal _extraDiscount;

        public ExtraDiscountDecorator(IKhuyenMai khuyenMai, decimal extraDiscount) : base(khuyenMai)
        {
            _extraDiscount = extraDiscount;
        }

        public override decimal CalculateDiscount(decimal originalPrice)
        {
            decimal baseDiscount = _khuyenMai.CalculateDiscount(originalPrice);
            return baseDiscount + _extraDiscount; // Thêm chiết khấu bổ sung
        }
    }
}