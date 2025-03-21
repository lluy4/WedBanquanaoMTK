using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.FactoryMethod
{
    public abstract class QuanAo
    {
        public string Ten { get; set; }
        public abstract void HienThiThongTin();
    }

    public class AoThun : QuanAo
    {
        public override void HienThiThongTin()
        {
            Console.WriteLine($"ÁO THUN: {Ten}");
        }
    }

    public class QuanJean : QuanAo
    {
        public override void HienThiThongTin()
        {
            Console.WriteLine($"Quần jean: {Ten}");
        }
    }

    public class Vay : QuanAo
    {
        public override void HienThiThongTin()
        {
            Console.WriteLine($"Váy: {Ten}");
        }
    }
    public abstract class QuanAoFactory
    {
        public abstract QuanAo TaoQuanAo(string ten);
    }

    public class AoThunFactory : QuanAoFactory
    {
        public override QuanAo TaoQuanAo(string ten)
        {
            return new AoThun { Ten = ten };
        }
    }

    public class QuanJeanFactory : QuanAoFactory
    {
        public override QuanAo TaoQuanAo(string ten)
        {
            return new QuanJean { Ten = ten };
        }
    }

    public class VayFactory : QuanAoFactory
    {
        public override QuanAo TaoQuanAo(string ten)
        {
            return new Vay { Ten = ten };
        }
    }


}