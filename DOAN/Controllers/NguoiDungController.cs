using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DOAN.Common;
using DOAN.Models;
using DOAN.MTK.Repository;
using DOAN.Repositories;

namespace DOAN.Controllers
{
    [Authorize(Roles = "*,xemthongtin")]
    public class NguoiDungController : Controller
    {
        private readonly INguoiDungRepository _nguoiDungRepository;

        // Constructor không tham số
        public NguoiDungController() : this(new NguoiDungRepository(new TMDTDbContext()))
        {
        }

        // Constructor có tham số
        public NguoiDungController(INguoiDungRepository nguoiDungRepository)
        {
            _nguoiDungRepository = nguoiDungRepository;
        }

        [Authorize(Roles = "*")]
        public ActionResult Index()
        {
            var list = _nguoiDungRepository.LayTatCaNguoiDung();
            var listND = _nguoiDungRepository.LayTatCaLoaiUser();
            ViewBag.items = new SelectList(listND, "IdLoaiUser", "TenLoai");
            ViewBag.GiaTri = 0;
            ViewBag.DanhSach = list;

            return View(list);
        }

        [HttpPost]
        [Authorize(Roles = "*")]
        public ActionResult Index(FormCollection f)
        {
            var kq = f["ddlNguoiDung"];
            var listND = _nguoiDungRepository.LayTatCaLoaiUser();

            if (!string.IsNullOrEmpty(kq))
            {
                int giatri = int.Parse(kq);
                var list = _nguoiDungRepository.LayNguoiDungTheoLoaiUser(giatri);
                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listND, "IdLoaiUser", "TenLoai", giatri);
                ViewBag.GiaTri = giatri;
                return View(list);
            }
            else
            {
                var list = _nguoiDungRepository.LayTatCaNguoiDung();
                ViewBag.DanhSach = list;
                ViewBag.items = new SelectList(listND, "IdLoaiUser", "TenLoai");
                ViewBag.GiaTri = 0;
                return View(list);
            }
        }

        [Authorize(Roles = "*")]
        public ActionResult Create()
        {
            ViewBag.IdLoaiUser = new SelectList(_nguoiDungRepository.LayTatCaLoaiUser(), "IdLoaiUser", "TenLoai");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Create")]
        [Authorize(Roles = "*")]
        public ActionResult Create(NGUOIDUNG nd, HttpPostedFileBase Avatar)
        {
            if (Avatar != null && Avatar.ContentLength > 0)
            {
                var fileName = Path.GetFileName(Avatar.FileName);
                var path = Path.Combine(Server.MapPath("~/assets/admin/hinhnd"), fileName);
                nd.Avatar = fileName;
                if (!System.IO.File.Exists(path))
                {
                    Avatar.SaveAs(path);
                }
            }

            string email = nd.Mail;
            string password = Membership.GeneratePassword(6, 0);
            password = Regex.Replace(password, @"[^a-zA-Z0-9]", m => "9");
            Gmail gmail = new Gmail
            {
                To = email.Trim(),
                From = "testgoog96@gmail.com",
                Subject = "Cấp mật khẩu đăng nhập",
                Body = "<p>Mật khẩu đăng nhập tạm thời của bạn l&agrave; <span style=\"color: #3598db;\">" + password + "</span>. Vui l&ograve;ng thay đổi lại mật khẩu khi đăng nhập th&agrave;nh c&ocirc;ng.</p>"
            };

            try
            {
                MailMessage mail = new MailMessage(gmail.From, gmail.To)
                {
                    Subject = gmail.Subject,
                    Body = gmail.Body,
                    IsBodyHtml = true
                };
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com",
                    Port = 587,
                    EnableSsl = true,
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential("testgoog96@gmail.com", "thuytien1234567890")
                };
                smtp.Send(mail);

                nd.Password = Encryptor.MD5Hash(password);
                nd.Password1 = Encryptor.MD5Hash(password);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Quá trình thực hiện thất bại.");
                return View(nd);
            }

            nd.SDT = nd.SDT.Trim();
            nd.NgayTao = DateTime.Now;
            nd.TT_User = true;

            if (ModelState.IsValid)
            {
                if (!_nguoiDungRepository.LayTatCaNguoiDung().Any(x => x.Username == nd.Mail.Trim()))
                {
                    try
                    {
                        nd.Username = nd.Mail.Trim();
                        _nguoiDungRepository.ThemNguoiDung(nd);
                        _nguoiDungRepository.Luu();
                        return RedirectToAction("Index");
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("", "Quá trình thực hiện thất bại.");
                        ViewBag.IdLoaiUser = new SelectList(_nguoiDungRepository.LayTatCaLoaiUser(), "IdLoaiUser", "TenLoai", nd.IdLoaiUser);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Email bạn nhập đã được sử dụng. Vui lòng sử dụng email khác.");
                    ViewBag.IdLoaiUser = new SelectList(_nguoiDungRepository.LayTatCaLoaiUser(), "IdLoaiUser", "TenLoai", nd.IdLoaiUser);
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
                ViewBag.IdLoaiUser = new SelectList(_nguoiDungRepository.LayTatCaLoaiUser(), "IdLoaiUser", "TenLoai", nd.IdLoaiUser);
            }
            return View(nd);
        }

        [HttpPost]
        [Authorize(Roles = "*")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NGUOIDUNG nd = _nguoiDungRepository.LayNguoiDungTheoId(id.Value);
            if (nd == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            try
            {
                nd.TT_User = false;
                nd.Mail = nd.Mail.Trim();
                nd.SDT = nd.SDT.Trim();
                _nguoiDungRepository.CapNhatNguoiDung(nd);
                _nguoiDungRepository.Luu();
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return Content("<script> alert(\"Quá trình thực hiện thất bại\")</script>");
            }
        }

        [Authorize(Roles = "*")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            NGUOIDUNG nd = _nguoiDungRepository.LayNguoiDungTheoId(id.Value);
            if (nd == null)
            {
                return HttpNotFound();
            }
            nd.Mail = nd.Mail.Trim();
            nd.SDT = nd.SDT.Trim();
            ViewBag.AnhCu = nd.Avatar;
            ViewBag.IdLoaiUser = new SelectList(_nguoiDungRepository.LayTatCaLoaiUser(), "IdLoaiUser", "TenLoai", nd.IdLoaiUser);
            return View(nd);
        }

        [HttpPost]
        [Route("Edit")]
        [Authorize(Roles = "*")]
        public ActionResult Edit(NGUOIDUNG nd, HttpPostedFileBase Avatar, string AnhCu)
        {
            if (Avatar != null && Avatar.ContentLength > 0)
            {
                var fileName = Path.GetFileName(Avatar.FileName);
                var path = Path.Combine(Server.MapPath("~/assets/admin/hinhnd"), fileName);
                nd.Avatar = fileName;
                if (!System.IO.File.Exists(path))
                {
                    Avatar.SaveAs(path);
                }
            }
            else
            {
                nd.Avatar = AnhCu;
            }

            if (ModelState.IsValid)
            {
                try
                {
                    nd.Mail = nd.Mail.Trim();
                    nd.SDT = nd.SDT.Trim();
                    _nguoiDungRepository.CapNhatNguoiDung(nd);
                    _nguoiDungRepository.Luu();
                    NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
                    if (user != null && user.IdUser == nd.IdUser)
                    {
                        Session["TaiKhoan"] = _nguoiDungRepository.LayNguoiDungTheoId(nd.IdUser);
                    }
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "Quá trình thực hiện thất bại.");
                    ViewBag.IdLoaiUser = new SelectList(_nguoiDungRepository.LayTatCaLoaiUser(), "IdLoaiUser", "TenLoai", nd.IdLoaiUser);
                }
            }
            else
            {
                ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
                ViewBag.IdLoaiUser = new SelectList(_nguoiDungRepository.LayTatCaLoaiUser(), "IdLoaiUser", "TenLoai", nd.IdLoaiUser);
            }
            return View(nd);
        }

        [HttpPost]
        public ActionResult CapNhatThongTin(NGUOIDUNG nd, HttpPostedFileBase Avatar, string AnhCu)
        {
            var user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
            {
                RedirectToAction("DangNhap", "Home");
            }
            if (user.IdLoaiUser == 5 || (user.IdUser == nd.IdUser))
            {
                if (Avatar != null && Avatar.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(Avatar.FileName);
                    var path = Path.Combine(Server.MapPath("~/assets/admin/hinhnd"), fileName);
                    nd.Avatar = fileName;
                    if (!System.IO.File.Exists(path))
                    {
                        Avatar.SaveAs(path);
                    }
                }
                else
                {
                    nd.Avatar = AnhCu;
                }

                int error = 0;
                nd.SDT = nd.SDT.Trim();
                if (ModelState.IsValid)
                {
                    try
                    {
                        _nguoiDungRepository.CapNhatNguoiDung(nd);
                        _nguoiDungRepository.Luu();
                        error = -1;
                        return RedirectToAction("ChiTietNguoiDung", new { error = error });
                    }
                    catch (Exception)
                    {
                        error = 1;
                        return RedirectToAction("ChiTietNguoiDung", new { error = error });
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Vui lòng kiểm tra lại thông tin đã nhập.");
                }
                return RedirectToAction("ChiTietNguoiDung", "NguoiDung", new { id = nd.IdUser, error = 4 });
            }
            else
            {
                return View("LoiPhanQuyen", "Dashboard");
            }
        }

        public ActionResult ChiTietNguoiDung(int? id, int error = 0)
        {
            if (id == null)
            {
                var user = Session["TaiKhoan"] as NGUOIDUNG;
                if (user == null)
                    return HttpNotFound();
                var nd = _nguoiDungRepository.LayNguoiDungTheoId(user.IdUser);
                Session["TaiKhoan"] = nd;
                ViewBag.AnhCu = nd.Avatar;
                ViewBag.Error = error;
                return View(nd);
            }
            else
            {
                var nd = _nguoiDungRepository.LayNguoiDungTheoId(id.Value);
                ViewBag.Error = error;
                return View(nd);
            }
        }

        public ActionResult ThayDoiMatKhau(FormCollection f)
        {
            int error = 0;
            var user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return HttpNotFound();
            var nd = _nguoiDungRepository.LayNguoiDungTheoId(user.IdUser);
            string matkhaucu = f["matkhaucu"];
            string matkhaumoi = f["matkhaumoi"];
            string xacnhan = f["xacnhan"];
            if (matkhaumoi != xacnhan)
            {
                error = 2;
                return RedirectToAction("ChiTietNguoiDung", new { error = error });
            }
            if (Encryptor.MD5Hash(matkhaucu) != nd.Password)
            {
                error = 3;
                return RedirectToAction("ChiTietNguoiDung", new { error = error });
            }
            else
            {
                nd.SDT = nd.SDT.Trim();
                nd.Password = Encryptor.MD5Hash(matkhaumoi);
                nd.Password1 = Encryptor.MD5Hash(matkhaumoi);
                _nguoiDungRepository.CapNhatNguoiDung(nd);
                _nguoiDungRepository.Luu();
                Session["TaiKhoan"] = null;
                FormsAuthentication.SignOut();
                return RedirectToAction("Index", "Dashboard");
            }
        }
    }
}