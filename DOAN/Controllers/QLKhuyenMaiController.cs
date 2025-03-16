using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;
using DOAN.Common;
using System.Net;
using System.Data.Entity;
using DOAN.MTK.Decorator;
//DECORATOR_HUY
namespace DOAN.Controllers
{
    [Authorize(Roles = "*")]
    public class QLKhuyenMaiController : Controller
    {
        private readonly TMDTDbContext _context;

        public QLKhuyenMaiController()
        {
            _context = new TMDTDbContext();
        }

        // GET: QLKhuyenMai
        public ActionResult Index(int error = 0)
        {
            var list = _context.KHUYENMAIs.Where(x => x.TinhTrang == true).OrderByDescending(y => y.NgayBD);
            List<LoaiKM> listLoai = new List<LoaiKM>
            {
                new LoaiKM { IdLoai = 1, TenLoai = "Giảm phần trăm" },
                new LoaiKM { IdLoai = 2, TenLoai = "Giảm trực tiếp" }
            };
            ViewBag.items = new SelectList(listLoai, "IdLoai", "TenLoai");
            ViewBag.GiaTri = 0;
            ViewBag.DanhSach = list;
            ViewBag.Error = error;
            return View(list);
        }

        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            var kq = f["ddlLoai"];
            List<LoaiKM> listLoai = new List<LoaiKM>
            {
                new LoaiKM { IdLoai = 1, TenLoai = "Giảm phần trăm" },
                new LoaiKM { IdLoai = 2, TenLoai = "Giảm trực tiếp" }
            };

            if (kq != "")
            {
                int giatri = int.Parse(kq);
                var list = _context.KHUYENMAIs.Where(x => x.TinhTrang == true && x.LoaiKM == giatri).OrderByDescending(y => y.NgayBD);
                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listLoai, "IdLoai", "TenLoai", giatri);
                ViewBag.GiaTri = giatri;
                return View(list);
            }
            else
            {
                var list = _context.KHUYENMAIs.Where(x => x.TinhTrang == true).OrderByDescending(y => y.NgayBD);
                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listLoai, "IdLoai", "TenLoai");
                ViewBag.GiaTri = 0;
                return View(list);
            }
        }

        public ActionResult Create()
        {
            List<LoaiKM> listLoai = new List<LoaiKM>
            {
                new LoaiKM { IdLoai = 1, TenLoai = "Giảm phần trăm" },
                new LoaiKM { IdLoai = 2, TenLoai = "Giảm trực tiếp" }
            };
            ViewBag.LoaiKM = new SelectList(listLoai, "IdLoai", "TenLoai");
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DOAN.Models.KHUYENMAI khuyenmai)
        {
            khuyenmai.TinhTrang = true; // Gán true cho bit, cần kiểm tra kiểu
            if (ModelState.IsValid)
            {
                try
                {
                    // Áp dụng Decorator để tính toán chiết khấu
                    IKhuyenMai decoratedKhuyenMai = new DecoratedKhuyenMai(khuyenmai);
                    decimal extraDiscount = 0;
                    if (khuyenmai.LoaiKM.HasValue && khuyenmai.LoaiKM.Value == 1 && khuyenmai.GiaTri.HasValue && khuyenmai.GiaTri.Value > 20)
                    {
                        extraDiscount = 5m;
                        decoratedKhuyenMai = new ExtraDiscountDecorator(decoratedKhuyenMai, extraDiscount);
                    }

                    // Tạo instance mới của KHUYENMAI để lưu vào database
                    var newKhuyenMai = new DOAN.Models.KHUYENMAI
                    {
                        IdMa = decoratedKhuyenMai.IdMa,
                        MaKM = decoratedKhuyenMai.MaKM,
                        LoaiKM = decoratedKhuyenMai.LoaiKM,
                        NgayBD = decoratedKhuyenMai.NgayBD,
                        NgayKT = decoratedKhuyenMai.NgayKT,
                        GiaTri = decoratedKhuyenMai.GiaTri,
                        ChiTiet = decoratedKhuyenMai.ChiTiet,
                        TinhTrang = decoratedKhuyenMai.TinhTrang
                    };
                    _context.KHUYENMAIs.Add(newKhuyenMai);
                    _context.SaveChanges();
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
            List<LoaiKM> listLoai = new List<LoaiKM>
            {
                new LoaiKM { IdLoai = 1, TenLoai = "Giảm phần trăm" },
                new LoaiKM { IdLoai = 2, TenLoai = "Giảm trực tiếp" }
            };
            ViewBag.LoaiKM = new SelectList(listLoai, "IdLoai", "TenLoai", khuyenmai.LoaiKM);
            return View(khuyenmai);
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DOAN.Models.KHUYENMAI khuyenmai = _context.KHUYENMAIs.Find(id);
            if (khuyenmai == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            if (DateTime.Compare(DateTime.Now, khuyenmai.NgayBD ?? DateTime.Now) < 0)
            {
                try
                {
                    khuyenmai.TinhTrang = false;
                    _context.Entry(khuyenmai).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
                    return Content("<script> alert(\"Quá trình thực hiện thất bại.\")</script>");
                }
            }
            else
            {
                return RedirectToAction("Index", "QLKhuyenMai", new { error = 2 });
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DOAN.Models.KHUYENMAI khuyenmai = _context.KHUYENMAIs.SingleOrDefault(x => x.IdMa == id);
            if (khuyenmai == null)
            {
                return HttpNotFound();
            }
            if (DateTime.Compare(DateTime.Now, khuyenmai.NgayBD ?? DateTime.Now) < 0)
            {
                List<LoaiKM> listLoai = new List<LoaiKM>
                {
                    new LoaiKM { IdLoai = 1, TenLoai = "Giảm phần trăm" },
                    new LoaiKM { IdLoai = 2, TenLoai = "Giảm trực tiếp" }
                };
                ViewBag.LoaiKM = new SelectList(listLoai, "IdLoai", "TenLoai", khuyenmai.LoaiKM);
                return View(khuyenmai);
            }
            else
            {
                return RedirectToAction("Index", "QLKhuyenMai", new { error = 2 });
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(DOAN.Models.KHUYENMAI khuyenmai)
        {
            khuyenmai.TinhTrang = true;
            if (ModelState.IsValid)
            {
                try
                {
                    // Áp dụng Decorator để tính toán chiết khấu
                    IKhuyenMai decoratedKhuyenMai = new DecoratedKhuyenMai(khuyenmai);
                    decimal extraDiscount = 0;
                    if (khuyenmai.LoaiKM.HasValue && khuyenmai.LoaiKM.Value == 1 && khuyenmai.GiaTri.HasValue && khuyenmai.GiaTri.Value > 20)
                    {
                        extraDiscount = 5m;
                        decoratedKhuyenMai = new ExtraDiscountDecorator(decoratedKhuyenMai, extraDiscount);
                    }

                    // Cập nhật instance hiện tại thay vì ép kiểu
                    khuyenmai.LoaiKM = decoratedKhuyenMai.LoaiKM;
                    khuyenmai.GiaTri = decoratedKhuyenMai.GiaTri;
                    khuyenmai.ChiTiet = decoratedKhuyenMai.ChiTiet;
                    _context.Entry(khuyenmai).State = EntityState.Modified;
                    _context.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại.");
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
            }
            List<LoaiKM> listLoai = new List<LoaiKM>
            {
                new LoaiKM { IdLoai = 1, TenLoai = "Giảm phần trăm" },
                new LoaiKM { IdLoai = 2, TenLoai = "Giảm trực tiếp" }
            };
            ViewBag.LoaiKM = new SelectList(listLoai, "IdLoai", "TenLoai", khuyenmai.LoaiKM);
            return View(khuyenmai);
        }
    }
}