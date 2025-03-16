using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;
using CaptchaMvc.HtmlHelpers;
using CaptchaMvc;
using DOAN.Common;
using System.Web.Security;
using System.Data.Entity;
using System.Text.RegularExpressions;
using System.Net.Mail;
using System.Net;

namespace DOAN.Controllers
{
    public class HomeController : Controller
    {
        TMDTDbContext db = new TMDTDbContext();
        // GET: Home
        public ActionResult Index()
        {
            var listSP = db.SANPHAMs.Where(x => x.TinhTrang == 1);
            ViewBag.listTH = db.THUONGHIEUx;
              

            return View();
        }

        [ChildActionOnly]
        public ActionResult FeaturedBrandsPartial()
        {
            var listSP = db.SANPHAMs.Where(x => x.TinhTrang == 1);
            ViewBag.listTH = db.THUONGHIEUx;
            ViewBag.HienThi = db.THUONGHIEUx.Count() > 5 ? 5 : db.THUONGHIEUx.Count();

            
            return PartialView(listSP);
        }


        [ChildActionOnly]
        public ActionResult HotItemPartial()
        {
            var listSP = db.SANPHAMs.Where(x => x.TinhTrang == 1);
            
            return PartialView(listSP);
        }


        [ChildActionOnly]
        public ActionResult MenuPartial()
        {
            var listSP = db.SANPHAMs;
            ViewBag.listSP =listSP ;
            var listLoai = db.LOAISANPHAMs;
            return PartialView(listLoai);
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            ViewBag.ThongBao =0;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(NGUOIDUNG user)
        {
            ViewBag.ThongBao = 0;
            if (this.IsCaptchaValid("Captcha is not valid"))
            {
                
                user.NgayTao = DateTime.Now;
                user.TT_User = true;
                user.IdLoaiUser = 1;
                if(user.Password!=user.Password1)
                {
                    ModelState.AddModelError("", "Mật khẩu xác nhận không khớp");
                    ViewBag.ThongBao = 4;
                    return View();
                }    
                if (ModelState.IsValid)
                {
                        user.Password = Encryptor.MD5Hash(user.Password);
                        user.Password1 = Encryptor.MD5Hash(user.Password1);
                        if (db.NGUOIDUNGs.Where(x => x.Username == user.Mail.Trim()).Count()==0)
                    {
                        user.Username = user.Mail.Trim();
                        db.NGUOIDUNGs.Add(user);
                        db.SaveChanges();
                        ViewBag.ThongBao = 1;
                    }
                    else
                        ViewBag.ThongBao = 2;
                }
                else
                {
                    ViewBag.ThongBao = 4;
                }
            }
            else
                ViewBag.ThongBao = 3;
            return View();
        }

        [HttpGet]
        public ActionResult DangNhap(string strURL)
        {
            ViewBag.ThongBao = 0;
            ViewBag.strURL = strURL;
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(FormCollection f, string strURL)
        {
            ViewBag.ThongBao = 0;
            string username = f["username"].ToString();
            string password = Encryptor.MD5Hash(f["password"].ToString());
            var user = db.NGUOIDUNGs.SingleOrDefault(x => x.Username == username && (x.Password == password|| x.Password1==password) && x.TT_User==true);
            if(user!=null)
            {
                IEnumerable<PHANQUYEN> lstQuyen = db.PHANQUYENs.Where(x => x.IdLoaiUser == user.IdLoaiUser);
                string Quyen = "";
                foreach (var item in lstQuyen)
                {
                    Quyen += item.TinhNang + ","; 
                }
                Quyen = Quyen.Substring(0, Quyen.Length - 1);
                PhanQuyen(username, Quyen);


                Session["TaiKhoan"] = user;
                List<GIOHANG> lstGioHang = Session["GioHang"] as List<GIOHANG>;
                if (lstGioHang != null)
                {
                    var ds = db.GIOHANGs.Where(x => x.IdKH == user.IdUser);
                    db.GIOHANGs.RemoveRange(ds);
                    foreach (var i in lstGioHang)
                    {
                        GIOHANG gh = new GIOHANG()
                        {
                            IdKH = user.IdUser,
                            IdSP = i.IdSP,
                            SoLuong = i.SoLuong,
                            TinhTrang = i.TinhTrang
                        };
                        db.GIOHANGs.Add(gh);
                    }
                    db.SaveChanges();
                }
                else
                {
                    var listGH = db.GIOHANGs.Where(x => x.IdKH == user.IdUser).ToList();
                    Session["GioHang"] = listGH;
                }
                if (user.Password != user.Password1)
                    return RedirectToAction("ThayDoiMatKhau","Home");
                if (strURL != null && strURL != "")
                    return Redirect(strURL);
                else
                    return RedirectToAction("Index", "Home");
            }
            ViewBag.ThongBao = 1;
            return View();
        }

        public ActionResult QuenMatKhau()
        {
            ViewBag.ThongBao = "";
            return View();
        }

        [HttpPost]
        public ActionResult QuenMatKhau(FormCollection f)
        {
            string email = f["email"];
            var nguoidung = db.NGUOIDUNGs.SingleOrDefault(x => x.Mail.ToLower().Trim() == email.ToLower().Trim());
            if (nguoidung == null)
            {
                ViewBag.ThongBao = "Email không tồn tại. Vui lòng nhập lại";
                return View();
            }
            string password = Membership.GeneratePassword(6, 0);
            password = Regex.Replace(password, @"[^a-zA-Z0-9]", m => "9");
            Gmail gmail = new Gmail();
            gmail.To = email.Trim();
            gmail.From = "testgoog96@gmail.com";
            gmail.Subject = "Cấp lại mật khẩu đăng nhập";
            gmail.Body = "<p>Mật khẩu đăng nhập tạm thời của bạn l&agrave; <span style=\"color: #3598db;\">" + password + "</span>. Vui l&ograve;ng thay đổi lại mật khẩu khi đăng nhập th&agrave;nh c&ocirc;ng.</p>";


            try
            {
                MailMessage mail = new MailMessage(gmail.From, gmail.To);
                mail.Subject = gmail.Subject;
                mail.Body = gmail.Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.EnableSsl = true;
                NetworkCredential nc = new NetworkCredential("testgoog96@gmail.com", "thuytien1234567890");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = nc;
                smtp.Send(mail);

                nguoidung.SDT= nguoidung.SDT.Trim();
                nguoidung.Password1 = Encryptor.MD5Hash(password);
                db.Entry(nguoidung).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                ViewBag.ThongBao = "Email đã được gửi, vui lòng kiểm tra hộp thư để cập nhật thông tin.";
                return View();
            }
            catch (Exception)
            {
                ViewBag.ThongBao = "Quá trình thực hiện thất bại";
                return View();
            }
        }
        public void PhanQuyen(string username, string quyen)
        {
            FormsAuthentication.Initialize();
            var ticket = new FormsAuthenticationTicket(1, username, DateTime.Now,
                                                        DateTime.Now.AddHours(3), //timeout
                                                        false,//remember me
                                                        quyen,
                                                        FormsAuthentication.FormsCookiePath);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
            if (ticket.IsPersistent)
                cookie.Expires = ticket.Expiration;
            Response.Cookies.Add(cookie);

        }

        public ActionResult DangXuat(string strURL)
        {
            Session["TaiKhoan"] = null;
            Session["GioHang"] = null;
            FormsAuthentication.SignOut();
            if (strURL != null && strURL != "")
            {
                return Redirect(strURL);
            }
            else
                return RedirectToAction("Index", "Home");
           
        }

        public ActionResult ThongTinCaNhan(int error=0)
        {
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return RedirectToAction("DangNhap", "Home", new { strURL = Request.Url.ToString() });
            ViewBag.Error = error;
            ViewBag.GT = user.GioiTinh;
            return View(user);
        }

        [HttpPost]
        public ActionResult ThongTinCaNhan(NGUOIDUNG nguoidung, FormCollection f)
        {
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return RedirectToAction("DangNhap", "Home", new { strURL = Request.Url.ToString() });
            var GioiTinh = f["GioiTinh"];
            nguoidung.GioiTinh = bool.Parse(GioiTinh);
            nguoidung.SDT = nguoidung.SDT.Trim();
            
            if(ModelState.IsValid)
            {
                try
                {
                    db.Entry(nguoidung).State = EntityState.Modified;
                    db.SaveChanges();
                    NGUOIDUNG nd = db.NGUOIDUNGs.Find(user.IdUser);
                    if (nd != null)
                        Session["TaiKhoan"] = nd;
                    return RedirectToAction("ThongTinCaNhan", "Home", new { error = -1 });
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
            ViewBag.GT = nguoidung.GioiTinh;
            return View(user);
        }

        public ActionResult ThongTinDonHang()
        {
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return RedirectToAction("DangNhap", "Home", new { strURL = Request.Url.ToString() });

            var list = db.HOADONs.Where(x => x.IdKH==user.IdUser).OrderByDescending(y => y.NgayDH);
            ViewBag.SL = list.Count();
            return View(list);
        }

        public ActionResult ChiTietDonHang(int id, int error=0)
        {
            var hoadon = db.HOADONs.Find(id);
            if (hoadon == null)
                return HttpNotFound();
            var listCT = db.CHITIETHDs.Where(x => x.IdHD == id);
            ViewBag.Error = error;
            ViewBag.DoiTacGH = db.DTGIAOHANGs.SingleOrDefault(x => x.IdDTGH == hoadon.IdDTGH);
            ViewBag.HoaDon = hoadon;
            ViewBag.ChiTiet = listCT;
            return View();
        }

        public ActionResult ThayDoiMatKhau(int error=0)
        {
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return RedirectToAction("DangNhap", "Home",new {strURL=Request.Url.ToString()});
            ViewBag.Error = error;
            return View();
        }

        [HttpPost]
        public ActionResult ThayDoiMatKhau(FormCollection f)
        {
            int error = 0;
            var user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return HttpNotFound();
            var nd = db.NGUOIDUNGs.Find(user.IdUser);
            string matkhaucu = f["matkhaucu"];
            string matkhaumoi = f["matkhaumoi"];
            string xacnhan = f["xacnhan"];
            if (matkhaumoi != xacnhan)
            {
                error = 2;
                return RedirectToAction("ThayDoiMatKhau", new { error = error });
            }
            if (Encryptor.MD5Hash(matkhaucu) != nd.Password && Encryptor.MD5Hash(matkhaucu) != nd.Password1)
            {
                error = 3;
                return RedirectToAction("ThayDoiMatKhau", new { error = error });
            }
            else
            {
                nd.SDT = nd.SDT.Trim();
                nd.Password = Encryptor.MD5Hash(matkhaumoi);
                nd.Password1 = Encryptor.MD5Hash(matkhaumoi);
                db.Entry(nd).State = EntityState.Modified;
                db.SaveChanges();
                Session["TaiKhoan"] = null;
                FormsAuthentication.SignOut();
                return RedirectToAction("DangNhap", "Home");
            }
        }

        public ActionResult HuyBo(int id)
        {
            var hoadon = db.HOADONs.Find(id);
            if (hoadon == null)
                return HttpNotFound();
            try
            {
                hoadon.TinhTrang = 6;
                db.Entry(hoadon).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("ChiTietDonHang", "Home", new { id = hoadon.IdHD, error=-1 });
            }
            catch (Exception)
            {
                return RedirectToAction("ChiTietDonHang", "Home", new { id = hoadon.IdHD, error = 1 });
            }
        }
    }
}