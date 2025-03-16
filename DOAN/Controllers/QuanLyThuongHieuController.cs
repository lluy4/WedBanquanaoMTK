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
    [Authorize(Roles = "*")]
    public class QuanLyThuongHieuController : Controller
    {
        TMDTDbContext db = new TMDTDbContext();
        // GET: QuanLyThuongHieu
        public ActionResult Index(int error=0)
        {
            ViewBag.Error = error;
            var model = db.THUONGHIEUx.Where(x => x.TinhTrang == true);
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateInput(false)]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        public ActionResult Create(THUONGHIEU th, HttpPostedFileBase AnhTH)
        {
            string tenth = th.TenTH.ToLower();
            th.TenTH = th.TenTH.ToUpper();
            var folder = Server.MapPath("~/assets/client/hinhsp/" + tenth);
            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            if (AnhTH != null  && AnhTH.ContentLength > 0)
            {
                
                var fileName = Path.GetFileName(AnhTH.FileName);
                var path = Path.Combine(Server.MapPath("~/assets/client/hinhsp/" + tenth), fileName);
                th.AnhTH = fileName;
                if (!System.IO.File.Exists(path))
                {
                    AnhTH.SaveAs(path);
                }
            }

            th.TinhTrang = true;

            if (ModelState.IsValid)
            {
                try
                {
                    db.THUONGHIEUx.Add(th);
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
            return View(th);
        }

        [HttpPost]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THUONGHIEU th = db.THUONGHIEUx.Find(id);
            if (th == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            try
            {
                th.TinhTrang = false;
                db.Entry(th).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index","QuanLyThuongHieu",new {error=-1});

            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return RedirectToAction("Index", "QuanLyThuongHieu", new { error = 1 });
            }
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            THUONGHIEU th = db.THUONGHIEUx.FirstOrDefault(x => x.IdTH == id);
            if (th == null)
            {
                return HttpNotFound();
            }
            ViewBag.AnhCu = th.AnhTH;
            return View(th);
        }

        [HttpPost]
        [Route("Edit")]
        [ValidateInput(false)]
        public ActionResult Edit(THUONGHIEU th, HttpPostedFileBase AnhTH, string AnhCu)
        {
            if (AnhTH != null  && AnhTH.ContentLength > 0)
            {
                string tenth = th.TenTH.ToLower();
                var fileName = Path.GetFileName(AnhTH.FileName);
                var path = Path.Combine(Server.MapPath("~/assets/client/hinhsp/" + tenth), fileName);
                th.AnhTH = fileName;
                if (!System.IO.File.Exists(path))
                {
                    AnhTH.SaveAs(path);
                }
            }
            else
            {
                th.AnhTH = AnhCu;
            }
            th.TinhTrang = true;
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(th).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Index", "QuanLyThuongHieu", new { error=-1});
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại.");
                    ViewBag.AnhCu = th.AnhTH;
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập."); 
                ViewBag.AnhCu = th.AnhTH;
            }
            return View(th);
        }
    }
}