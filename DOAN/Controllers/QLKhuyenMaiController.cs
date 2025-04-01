using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using DOAN.Models;
using System.Net;
using System.Data.Entity;
using DOAN.Common;

namespace DOAN.Strategies
{
    // Giao diện chiến lược khuyến mãi
    public interface IPromotionStrategy
    {
        List<LoaiKM> GetPromotionTypes();
        IQueryable<KHUYENMAI> FilterPromotions(TMDTDbContext db, bool activeOnly = true);
    }

    // Chiến lược khuyến mãi giảm phần trăm
    public class PercentageDiscountStrategy : IPromotionStrategy
    {
        public List<LoaiKM> GetPromotionTypes()
        {
            return new List<LoaiKM>
            {
                new LoaiKM { IdLoai = 1, TenLoai = "Giảm phần trăm" }
            };
        }

        public IQueryable<KHUYENMAI> FilterPromotions(TMDTDbContext db, bool activeOnly = true)
        {
            var query = db.KHUYENMAIs.Where(x => x.LoaiKM == 1);
            return activeOnly
                ? query.Where(x => x.TinhTrang == true).OrderByDescending(y => y.NgayBD)
                : query;
        }
    }

    // Chiến lược khuyến mãi giảm trực tiếp
    public class DirectDiscountStrategy : IPromotionStrategy
    {
        public List<LoaiKM> GetPromotionTypes()
        {
            return new List<LoaiKM>
            {
                new LoaiKM { IdLoai = 2, TenLoai = "Giảm trực tiếp" }
            };
        }

        public IQueryable<KHUYENMAI> FilterPromotions(TMDTDbContext db, bool activeOnly = true)
        {
            var query = db.KHUYENMAIs.Where(x => x.LoaiKM == 2);
            return activeOnly
                ? query.Where(x => x.TinhTrang == true).OrderByDescending(y => y.NgayBD)
                : query;
        }
    }

    // Chiến lược khuyến mãi tổng hợp
    public class AllPromotionStrategy : IPromotionStrategy
    {
        public List<LoaiKM> GetPromotionTypes()
        {
            return new List<LoaiKM>
            {
                new LoaiKM { IdLoai = 1, TenLoai = "Giảm phần trăm" },
                new LoaiKM { IdLoai = 2, TenLoai = "Giảm trực tiếp" }
            };
        }

        public IQueryable<KHUYENMAI> FilterPromotions(TMDTDbContext db, bool activeOnly = true)
        {
            var query = db.KHUYENMAIs.AsQueryable();
            return activeOnly
                ? query.Where(x => x.TinhTrang == true).OrderByDescending(y => y.NgayBD)
                : query;
        }
    }

    // Nhà máy chiến lược để quản lý các chiến lược
    public class PromotionStrategyFactory
    {
        private static readonly Dictionary<int, IPromotionStrategy> _strategies = new Dictionary<int, IPromotionStrategy>
        {
            { 0, new AllPromotionStrategy() },
            { 1, new PercentageDiscountStrategy() },
            { 2, new DirectDiscountStrategy() }
        };

        public static IPromotionStrategy GetStrategy(int strategyType)
        {
            if (_strategies.TryGetValue(strategyType, out var strategy))
            {
                return strategy;
            }

            return _strategies[0]; // Mặc định trả về chiến lược tổng
        }
    }

    // Controller sử dụng Strategy Pattern
    [Authorize(Roles = "*")]
    public class QLKhuyenMaiController : Controller
    {
        private TMDTDbContext db = new TMDTDbContext();

        public ActionResult Index(int error = 0)
        {
            // Sử dụng chiến lược mặc định (tất cả khuyến mãi)
            var strategy = PromotionStrategyFactory.GetStrategy(0);
            var list = strategy.FilterPromotions(db);

            ViewBag.items = new SelectList(strategy.GetPromotionTypes(), "IdLoai", "TenLoai");
            ViewBag.GiaTri = 0;
            ViewBag.DanhSach = list;
            ViewBag.Error = error;

            return View(list);
        }

        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            var kq = f["ddlLoai"];

            if (!string.IsNullOrEmpty(kq))
            {
                int giatri = int.Parse(kq);
                // Chọn chiến lược phù hợp
                var strategy = PromotionStrategyFactory.GetStrategy(giatri);
                var list = strategy.FilterPromotions(db);

                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(strategy.GetPromotionTypes(), "IdLoai", "TenLoai", giatri);
                ViewBag.GiaTri = giatri;
                return View(list);
            }
            else
            {
                // Trở về chiến lược mặc định
                var strategy = PromotionStrategyFactory.GetStrategy(0);
                var list = strategy.FilterPromotions(db);

                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(strategy.GetPromotionTypes(), "IdLoai", "TenLoai");
                ViewBag.GiaTri = 0;
                return View(list);
            }
        }

        // Các phương thức khác vẫn giữ nguyên như ban đầu
        public ActionResult Create()
        {
            var strategy = PromotionStrategyFactory.GetStrategy(0);
            ViewBag.LoaiKM = new SelectList(strategy.GetPromotionTypes(), "IdLoai", "TenLoai");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(KHUYENMAI khuyenmai)
        {
            khuyenmai.TinhTrang = true;
            if (ModelState.IsValid)
            {
                try
                {
                    db.KHUYENMAIs.Add(khuyenmai);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
            }

            var strategy = PromotionStrategyFactory.GetStrategy(0);
            ViewBag.LoaiKM = new SelectList(strategy.GetPromotionTypes(), "IdLoai", "TenLoai", khuyenmai.LoaiKM);
            return View(khuyenmai);
        }

        
    }
}