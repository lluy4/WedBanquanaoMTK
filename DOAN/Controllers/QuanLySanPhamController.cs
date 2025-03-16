using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;

namespace DOAN.Controllers
{
    [Authorize(Roles = "*,nhaphang")]
    public class QuanLySanPhamController : Controller
    {
        TMDTDbContext db = new TMDTDbContext();
        // GET: QuanLySanPham
        public ActionResult Index(int error=0)
        {
            ViewBag.ThuongHieu = null;
            ViewBag.Error = error;

            var list = db.SANPHAMs.Where(x => (x.TinhTrang == 1 || x.TinhTrang == 2));
            var listTH = db.THUONGHIEUx.Where(x => x.TinhTrang == true);
            ViewBag.items = new SelectList(listTH, "IdTH", "TenTH");
            ViewBag.GiaTri = 0;
            ViewBag.DanhSach = list;

            return View(list);
        }

        [HttpPost]
        public ActionResult Index(FormCollection f)
        {
            var kq = f["ddlThuongHieu"];
            var listTH = db.THUONGHIEUx.Where(x => x.TinhTrang == true);

            if (kq != "")
            {
                int giatri = int.Parse(kq);
                ViewBag.ThuongHieu = db.THUONGHIEUx.Find(giatri);
                var list = db.SANPHAMs.Where(x => (x.TinhTrang == 1 || x.TinhTrang == 2)&&x.IdTH==giatri);

                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listTH, "IdTH", "TenTH",giatri);
                ViewBag.GiaTri = giatri;
                return View(list);
            }
            else
            {
                ViewBag.ThuongHieu = null;
                var list = db.SANPHAMs.Where(x => (x.TinhTrang == 1 || x.TinhTrang == 2));
                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listTH, "IdTH", "TenTH");
                ViewBag.GiaTri = 0;
                return View(list);
            }
        }

        [Authorize(Roles = "*")]
        public ActionResult Create(int ?idTH)
        {
            if(idTH==null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THUONGHIEU th = db.THUONGHIEUx.FirstOrDefault(x => x.IdTH == idTH);
            if(th==null)
            {
                Response.StatusCode = 404;
                return null;
            }    
            
            ViewBag.MaKM = new SelectList(db.KHUYENMAIs, "IdMa", "MaKM");
            ViewBag.IdTH = new SelectList(db.THUONGHIEUx, "IdTH", "TenTH",th.IdTH);
            ViewBag.IdLoaiSP = new SelectList(db.LOAISANPHAMs, "IdLoaiSP", "TenLoai");
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        [Authorize(Roles = "*")]
        public ActionResult Create(SANPHAM sp, HttpPostedFileBase [] AnhSP)
        {
            for(int i=0; i<AnhSP.Length; i++)
            {
                if (AnhSP[i]!=null&& i==0 &&AnhSP[i].ContentLength > 0)
                {
                    string tenth = db.THUONGHIEUx.FirstOrDefault(x => x.IdTH == sp.IdTH).TenTH.ToLower();
                    var fileName = Path.GetFileName(AnhSP[i].FileName);
                    var path = Path.Combine(Server.MapPath("~/assets/client/hinhsp/"+tenth), fileName);
                    sp.AnhSP = fileName;
                    if (!System.IO.File.Exists(path))
                    {
                        AnhSP[i].SaveAs(path);
                    }
                }
            }    
            
            sp.NgayTao = DateTime.Now;
            sp.SoLanMua = 0;
            sp.SoLuong = 0;
            if (sp.SoLuong > 0)
                sp.TinhTrang = 1;
            else
                sp.TinhTrang = 2;

            if (ModelState.IsValid)
            {
                try
                {
                    db.SANPHAMs.Add(sp);
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception)
                {
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại");
                    ViewBag.MaKM = new SelectList(db.KHUYENMAIs, "IdMa", "MaKM",sp.MaKM);
                    ViewBag.IdTH = new SelectList(db.THUONGHIEUx, "IdTH", "TenTH",sp.IdTH);
                    ViewBag.IdLoaiSP = new SelectList(db.LOAISANPHAMs, "IdLoaiSP", "TenLoai",sp.IdLoaiSP);
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập");
                ViewBag.MaKM = new SelectList(db.KHUYENMAIs, "IdMa", "MaKM", sp.MaKM);
                ViewBag.IdTH = new SelectList(db.THUONGHIEUx, "IdTH", "TenTH", sp.IdTH);
                ViewBag.IdLoaiSP = new SelectList(db.LOAISANPHAMs, "IdLoaiSP", "TenLoai", sp.IdLoaiSP);
            }
            return View(sp);
        }

        [HttpPost]
        [Authorize(Roles = "*")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sp = db.SANPHAMs.Find(id);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            try
            {
                sp.TinhTrang = 10;
                db.Entry(sp).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");

            }
            catch (Exception ex)
            {
                string message = ex.Message;

                return RedirectToAction("Index","QuanLySanPham",new {error=2});
            }
        }

        [Authorize(Roles = "*")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sp = db.SANPHAMs.FirstOrDefault(x => x.IdSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaKM = new SelectList(db.KHUYENMAIs, "IdMa", "MaKM", sp.MaKM);
            ViewBag.IdTH = new SelectList(db.THUONGHIEUx, "IdTH", "TenTH", sp.IdTH);
            ViewBag.IdLoaiSP = new SelectList(db.LOAISANPHAMs, "IdLoaiSP", "TenLoai", sp.IdLoaiSP);
            ViewBag.AnhCu = sp.AnhSP;
            return View(sp);
        }

        [HttpPost]
        [Route("Edit")]
        [ValidateInput(false)]
        [Authorize(Roles = "*")]
        public ActionResult Edit(SANPHAM sp, HttpPostedFileBase[] AnhSP, string AnhCu)
        {
            for (int i = 0; i < AnhSP.Length; i++)
            {
                if (AnhSP[i] != null && i == 0 && AnhSP[i].ContentLength > 0)
                {
                    string tenth = db.THUONGHIEUx.FirstOrDefault(x => x.IdTH == sp.IdTH).TenTH.ToLower();
                    var fileName = Path.GetFileName(AnhSP[i].FileName);
                    var path = Path.Combine(Server.MapPath("~/assets/client/hinhsp/" + tenth), fileName);
                    sp.AnhSP = fileName;
                    if (!System.IO.File.Exists(path))
                    {
                        AnhSP[i].SaveAs(path);
                    }
                }
            }
            if(sp.AnhSP==null||sp.AnhSP=="")
            {
                sp.AnhSP = AnhCu;
            }    

            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(sp).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại.");
                    ViewBag.MaKM = new SelectList(db.KHUYENMAIs, "IdMa", "MaKM", sp.MaKM);
                    ViewBag.IdTH = new SelectList(db.THUONGHIEUx, "IdTH", "TenTH", sp.IdTH);
                    ViewBag.IdLoaiSP = new SelectList(db.LOAISANPHAMs, "IdLoaiSP", "TenLoai", sp.IdLoaiSP);
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
                ViewBag.MaKM = new SelectList(db.KHUYENMAIs, "IdMa", "MaKM", sp.MaKM);
                ViewBag.IdTH = new SelectList(db.THUONGHIEUx, "IdTH", "TenTH", sp.IdTH);
                ViewBag.IdLoaiSP = new SelectList(db.LOAISANPHAMs, "IdLoaiSP", "TenLoai", sp.IdLoaiSP);
            }
            return View(sp);
        }

        
        public ActionResult NhapHang(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SANPHAM sp = db.SANPHAMs.FirstOrDefault(x => x.IdSP == id);
            if (sp == null)
            {
                return HttpNotFound();
            }
            NHAPHANG nh = new NHAPHANG();
            nh.IdSP = sp.IdSP;
            ViewBag.TenSP = sp.TenSP;
            nh.NgayNhap = DateTime.Now;
            nh.GiaNhap = sp.GiaGoc;
            nh.SoLuong = 0;
            return View(nh);
        }

        [HttpPost]
        public ActionResult NhapHang(NHAPHANG nh)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    if(db.NHAPHANGs.Where(x=>x.NgayNhap==nh.NgayNhap&& x.IdSP==nh.IdSP).Count()==0)
                    {
                        db.NHAPHANGs.Add(nh);
                        db.SaveChanges();
                    }
                    else
                    {
                        return RedirectToAction("Index", "QuanLySanPham", new { error = 1 });
                    }    
                    return RedirectToAction("Index","QuanLySanPham",new { error=-1});
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
            return View(nh);
        }
    }
}