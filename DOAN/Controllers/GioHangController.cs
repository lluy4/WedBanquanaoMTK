using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DOAN.Models;

namespace DOAN.Controllers
{
    public class GioHangController : Controller
    {
        TMDTDbContext db = new TMDTDbContext();
        public List<GIOHANG> LayGioHang()
        {
            List<GIOHANG> lstGioHang = Session["GioHang"] as List<GIOHANG>;
            if(lstGioHang==null)
            {
                NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
                if(user!=null)
                {
                    lstGioHang = db.GIOHANGs.Where(x => x.IdKH == user.IdUser).ToList();
                    if(lstGioHang==null)
                    {
                        lstGioHang = new List<GIOHANG>();
                    }    
                }
                else
                    lstGioHang = new List<GIOHANG>();
                Session["GioHang"] = lstGioHang;
            }    
            return lstGioHang;
        }

        public ActionResult GiamGioHang(int? MaSP, string strURL)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(x => x.IdSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GIOHANG> lstGioHang = LayGioHang();
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            GIOHANG spCheck = lstGioHang.SingleOrDefault(x => x.IdSP == MaSP);
            if (spCheck != null)
            {
                if (sp.SoLuong < spCheck.SoLuong - 1)
                {
                    spCheck.TinhTrang = false;
                    return Content("<script> alert(\"Sản phẩm không còn đủ số lượng\")</script>");
                }
                else
                {
                    spCheck.TinhTrang = true;
                    if (spCheck.SoLuong > 1)
                        spCheck.SoLuong--;
                }


                if (user != null)
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
            }

            return Redirect(strURL);
        }
        public ActionResult ThemGioHangAjax(int ?MaSP, string strURL)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(x => x.IdSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GIOHANG> lstGioHang = LayGioHang();
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            GIOHANG spCheck = lstGioHang.SingleOrDefault(x => x.IdSP == MaSP);
            if (spCheck != null)
            {
                if (sp.SoLuong < spCheck.SoLuong)
                {
                    spCheck.TinhTrang = false;
                    return Content("<script> alert(\"Sản phẩm không còn đủ số lượng\")</script>");
                }
                else
                {
                    spCheck.TinhTrang = true;
                    spCheck.SoLuong++;
                }

                
                if (user != null)
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
                return RedirectToAction("GioHangPartial");
            }

            GIOHANG item = new GIOHANG() { IdSP = MaSP, SoLuong = 1, TinhTrang = true, SANPHAM=sp };
            if (sp.SoLuong < item.SoLuong)
            {
                return Content("<script> alert(\"Sản phẩm không còn đủ số lượng\")</script>");
            }
            lstGioHang.Add(item);
            if(user!=null)
            {
                var ds = db.GIOHANGs.Where(x => x.IdKH == user.IdUser);
                db.GIOHANGs.RemoveRange(ds);
                foreach(var i in lstGioHang)
                {
                    GIOHANG gh = new GIOHANG()
                    {
                        IdKH=user.IdUser,
                        IdSP=i.IdSP,
                        SoLuong=i.SoLuong,
                        TinhTrang=i.TinhTrang
                    };
                    db.GIOHANGs.Add(gh);
                }
                db.SaveChanges();
            }
            return RedirectToAction("GioHangPartial");
        }

        public ActionResult ThemGioHang(int? MaSP, string strURL)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(x => x.IdSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GIOHANG> lstGioHang = LayGioHang();
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            GIOHANG spCheck = lstGioHang.SingleOrDefault(x => x.IdSP == MaSP);
            if (spCheck != null)
            {
                if (sp.SoLuong < spCheck.SoLuong)
                {
                    spCheck.TinhTrang = false;
                    return Content("<script> alert(\"Sản phẩm không còn đủ số lượng\")</script>");
                }
                else
                {
                    spCheck.TinhTrang = true;
                    spCheck.SoLuong++;
                }


                if (user != null)
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
                return Redirect(strURL);
            }

            GIOHANG item = new GIOHANG() { IdSP = MaSP, SoLuong = 1, TinhTrang = true, SANPHAM = sp };
            if (sp.SoLuong < item.SoLuong)
            {
                return Content("<script> alert(\"Sản phẩm không còn đủ số lượng\")</script>");
            }
            lstGioHang.Add(item);
            if (user != null)
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
            return Redirect(strURL);
        }

        public ActionResult XoaGioHang(int? MaSP, string strURL)
        {
            SANPHAM sp = db.SANPHAMs.SingleOrDefault(x => x.IdSP == MaSP);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            List<GIOHANG> lstGioHang = LayGioHang();
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            GIOHANG spCheck = lstGioHang.SingleOrDefault(x => x.IdSP == MaSP);
            if (spCheck != null)
            {
                lstGioHang.Remove(spCheck);


                if (user != null)
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
                return Redirect(strURL);
            }


            return Redirect(strURL);
        }

        public ActionResult DatHang(string strURL, string MaKM, FormCollection f)
        {
            var vanchuyen = f["vanchuyen"];
            if (Session["GioHang"] == null)
                return RedirectToAction("Index", "Home");
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return RedirectToAction("DangNhap", "Home", new { strURL = strURL });
            

            int TongTienSP = TinhTongThanhTien();
            int TienGiam = 0;
            int TienVanChuyen = 20000;
            HOADON hd = new HOADON();
            hd.DaThanhToan = false;
            var dtgh = db.DTGIAOHANGs.SingleOrDefault(x => x.TinhTrang == true);
            if (dtgh != null)
            {
                hd.IdDTGH = dtgh.IdDTGH;
                TienVanChuyen = dtgh.TienVanChuyen ?? 20000;
            }
            
            hd.NgayDH = DateTime.Now;
            hd.IdKH = user.IdUser;
            hd.TinhTrang = 4;
            hd.SDT = user.SDT.Trim();
            hd.DiaChi = user.DiaChi;
            
            
            KHUYENMAI km = db.KHUYENMAIs.FirstOrDefault(x => x.MaKM == MaKM && x.TinhTrang == true && DateTime.Compare(DateTime.Now, x.NgayBD ?? DateTime.Now) >= 0 && DateTime.Compare(DateTime.Now, x.NgayKT ?? DateTime.Now) <= 0);
            if (km != null)
            {
                if (km.LoaiKM == 1)
                {
                    TienGiam = (TongTienSP * (km.GiaTri ?? 0)) / 100;
                }
                else if(km.LoaiKM==2)
                {
                    TienGiam = km.GiaTri ?? 0;
                }
                hd.IdKM = km.IdMa;
            }
            hd.TienVanChuyen = TienVanChuyen;
            hd.TongTien = TongTienSP + TienVanChuyen - TienGiam;
            try
            {
                db.HOADONs.Add(hd);
                db.SaveChanges();
                List<GIOHANG> lstGH = LayGioHang();
                foreach (var item in lstGH)
                {
                    CHITIETHD ct = new CHITIETHD();
                    ct.IdHD = hd.IdHD;
                    ct.IdSP = item.SANPHAM.IdSP;
                    ct.SoLuong = item.SoLuong;
                    ct.GiaGoc = item.SANPHAM.GiaGoc;
                    ct.LoiNhuan = item.SANPHAM.LoiNhuan;
                    db.CHITIETHDs.Add(ct);
                }
                db.SaveChanges();
                var dsGH = db.GIOHANGs.Where(x => x.IdKH == user.IdUser);
                db.GIOHANGs.RemoveRange(dsGH);
                db.SaveChanges();
                Session["GioHang"] = null;
                return RedirectToAction("XemGioHang");
            }
            catch (Exception)
            {
                return RedirectToAction("Checkout", "GioHang", new { strURL = strURL, MaKM = MaKM, error = 1 });
            }
        }

        public int TinhTongSoLuong()
        {
            List<GIOHANG> listGioHang = Session["GioHang"] as List<GIOHANG>;
            if (listGioHang == null)
                return 0;
            return listGioHang.Sum(x => x.SoLuong);
        }

        public int TinhTongThanhTien()
        {
            List<GIOHANG> listGioHang = Session["GioHang"] as List<GIOHANG>;
            if (listGioHang == null)
                return 0;
            return listGioHang.Sum(x => (Int32)(x.SoLuong*x.SANPHAM.GiaGoc*x.SANPHAM.LoiNhuan));
        }

        // GET: GioHang
        public ActionResult XemGioHang()
        {
            int TienVanChuyen= db.DTGIAOHANGs.SingleOrDefault(x=>x.TinhTrang==true).TienVanChuyen ??20000;
            int TongTienSP = TinhTongThanhTien();
            int SoLuongSP= TinhTongSoLuong();
            ViewBag.TienVanChuyen = TienVanChuyen;
            ViewBag.TongSoLuong = SoLuongSP;
            ViewBag.TongTien = TongTienSP;
            ViewBag.IsKM = 0;
            ViewBag.KhuyenMai = "";
            ViewBag.GiamGia = 0;
            ViewBag.ThanhToan = TongTienSP + TienVanChuyen;
            List<GIOHANG> listGioHang = LayGioHang();
            return View(listGioHang);
        }

        [HttpPost]
        public ActionResult XemGioHang(FormCollection f)
        {
            string MaKM = f["MaKM"];
            int TienVanChuyen = db.DTGIAOHANGs.First().TienVanChuyen ?? 20000;
            int TongTienSP = TinhTongThanhTien();
            int SoLuongSP = TinhTongSoLuong();
            ViewBag.TienVanChuyen = TienVanChuyen;
            ViewBag.TongSoLuong = SoLuongSP;
            ViewBag.TongTien = TongTienSP;
            ViewBag.IsKM = 0;
            ViewBag.GiamGia = 0;
            List<GIOHANG> listGioHang = LayGioHang();
            KHUYENMAI km= db.KHUYENMAIs.FirstOrDefault(x => x.MaKM == MaKM && x.TinhTrang==true);
            if(km==null)
            {
                ViewBag.KhuyenMai = "";
                ViewBag.IsKM = 1;
                ViewBag.ThanhToan = TongTienSP + TienVanChuyen;
            }
            else if(DateTime.Compare(DateTime.Now, km.NgayBD ?? DateTime.Now) >= 0 && DateTime.Compare(DateTime.Now, km.NgayKT ?? DateTime.Now) <= 0)
            {
                int TienGiam = 0;
                ViewBag.KhuyenMai = MaKM;
                ViewBag.IsKM = 3;
                if (km.LoaiKM == 1)
                {
                    TienGiam = (TongTienSP * (km.GiaTri ?? 0)) / 100;
                    ViewBag.ThanhToan = TongTienSP + TienVanChuyen - TienGiam;
                    ViewBag.GiamGia = TienGiam;
                }
            }
            else
            {
                ViewBag.KhuyenMai = "";
                ViewBag.IsKM = 2;
                ViewBag.ThanhToan = TongTienSP + TienVanChuyen;
            }    
            
            return View(listGioHang);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSoLuong = TinhTongSoLuong();
            ViewBag.TongTien = TinhTongThanhTien();
            return PartialView();
        }

        public ActionResult Checkout(string strURL, string MaKM, int error=0)
        {
            if (Session["GioHang"] == null)
                return RedirectToAction("Index", "Home");
            NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
            if (user == null)
                return RedirectToAction("DangNhap", "Home", new { strURL = strURL });

            int TienGiam = 0;
            int TienVanChuyen = db.DTGIAOHANGs.SingleOrDefault(x => x.TinhTrang == true).TienVanChuyen ?? 20000;
            int TongTienSP = TinhTongThanhTien();
            int SoLuongSP = TinhTongSoLuong();

            ViewBag.DiaChi = user.DiaChi;
            ViewBag.SDT = user.SDT.Trim();

            if (MaKM!=null&& MaKM!=null)
            {
                KHUYENMAI km = db.KHUYENMAIs.FirstOrDefault(x => x.MaKM == MaKM && x.TinhTrang == true && DateTime.Compare(DateTime.Now, x.NgayBD ?? DateTime.Now) >= 0 && DateTime.Compare(DateTime.Now, x.NgayKT ?? DateTime.Now) <= 0);
                if (km != null)
                {
                    if (km.LoaiKM == 1)
                    {
                        TienGiam = (TongTienSP * (km.GiaTri ?? 0)) / 100;
                    }
                    else if (km.LoaiKM == 2)
                    {
                        TienGiam = km.GiaTri ?? 0;
                    }
                }
            }

            ViewBag.TienVanChuyen = TienVanChuyen;
            ViewBag.TongSoLuong = SoLuongSP;
            ViewBag.TongTien = TongTienSP;
            ViewBag.MaKM = MaKM;
            ViewBag.GiamGia = TienGiam;
            ViewBag.ThanhToan = TongTienSP + TienVanChuyen-TienGiam;
            ViewBag.Error = error;

            return View(user);
        }

        public ActionResult ChinhSuaTTVanChuyen(string MaKM, string strURL, NGUOIDUNG nguoidung)
        {
            nguoidung.SDT=nguoidung.SDT.Trim();
            if(ModelState.IsValid)
            {
                try
                {
                    db.Entry(nguoidung).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();
                    NGUOIDUNG user = Session["TaiKhoan"] as NGUOIDUNG;
                    NGUOIDUNG nd = db.NGUOIDUNGs.Find(user.IdUser);
                    if (nd != null)
                        Session["TaiKhoan"] = nd;
                    return RedirectToAction("Checkout", "GioHang", new { strURL = strURL, MaKM = MaKM, error = -1 });
                }
                catch (Exception e)
                {

                    return RedirectToAction("Checkout", "GioHang", new { strURL = strURL, MaKM = MaKM, error = 1 });
                }
            }
            return RedirectToAction("Checkout", "GioHang", new { strURL = strURL, MaKM = MaKM, error = 2 });
        }
    }
}