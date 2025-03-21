using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Command
{
    public interface ICommand
    {
        void Execute();
    }

    public class ThemSanPhamCommand : ICommand
    {
        private GioHang _gioHang;
        private string _sanPham;

        public ThemSanPhamCommand(GioHang gioHang, string sanPham)
        {
            _gioHang = gioHang;
            _sanPham = sanPham;
        }

        public void Execute()
        {
            _gioHang.ThemSanPham(_sanPham);
        }
    }

    public class GioHang
    {
        private List<string> _sanPham = new List<string>();

        public void ThemSanPham(string sanPham)
        {
            _sanPham.Add(sanPham);
            Console.WriteLine($"Đã thêm sản phẩm: {sanPham}");
        }

        public void XemGioHang()
        {
            Console.WriteLine("Giỏ hàng hiện tại:");
            foreach (var sp in _sanPham)
            {
                Console.WriteLine(sp);
            }
        }
    }

}