using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Observe
{
    public interface IObserver
    {
        void Update(string message);
    }

    public class NguoiDung : IObserver
    {
        public string Ten { get; set; }

        public NguoiDung(string ten)
        {
            Ten = ten;
        }

        public void Update(string message)
        {
            Console.WriteLine($"{Ten} nhận được thông báo: {message}");
        }
    }

    public class DonHangSubject
    {
        private List<IObserver> _observers = new List<IObserver>();

        public void Attach(IObserver observer)
        {
            _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }
    }

}