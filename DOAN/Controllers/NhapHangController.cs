using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;
using DOAN.MTK.Facades; 

namespace DOAN.Controllers
{
    [Authorize(Roles = "*")]
    public class NhapHangController : Controller
    {
        private readonly TMDTDbContext _context;
        private readonly NhapHangFacade _nhapHangFacade;

        // Constructor sử dụng Dependency Injection (tạm thời khởi tạo thủ công)
        public NhapHangController()
        {
            _context = new TMDTDbContext();
            _nhapHangFacade = new NhapHangFacade(_context);
        }

        // GET: NhapHang
        public ActionResult Index()
        {
            var model = _context.NHAPHANGs.OrderByDescending(x => x.NgayNhap);
            return View(model);
        }

        public ActionResult PhieuNhap(int idTH = 0, int error = 0, string noidung = null)
        {
            ViewBag.Error = error;
            ViewBag.NoiDung = noidung;
            if (idTH == 0)
            {
                ViewBag.SanPham = _context.SANPHAMs.Where(x => x.TinhTrang == 1 || x.TinhTrang == 2);
                ViewBag.ThuongHieu = null;
            }
            else
            {
                THUONGHIEU th = _context.THUONGHIEUx.Find(idTH);
                if (th == null)
                    return HttpNotFound();
                ViewBag.ThuongHieu = th;
                ViewBag.SanPham = _context.SANPHAMs.Where(x => x.IdTH == th.IdTH && (x.TinhTrang == 1 || x.TinhTrang == 2));
            }
            return View();
        }

        [HttpPost]
        public ActionResult PhieuNhap(IEnumerable<NHAPHANG> Model, FormCollection f)
        {
            try
            {
                DateTime dt = DateTime.Parse(f["NgayNhap"].ToString());
                var result = _nhapHangFacade.ProcessNhapHang(Model, dt); // Gọi NhapHangFacade
                if (!result.Success)
                {
                    return RedirectToAction("PhieuNhap", "NhapHang", new { error = 1, noidung = result.ErrorMessage });
                }
                return RedirectToAction("Index");
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Please check the information you entered.");
                ViewBag.SanPham = _context.SANPHAMs;
                return View();
            }
        }
    }
}