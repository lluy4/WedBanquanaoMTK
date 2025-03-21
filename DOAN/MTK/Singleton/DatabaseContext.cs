using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DOAN.MTK.Singleton
{
    public class DatabaseContext
    {
        private static DatabaseContext _instance;
        private static readonly object _lock = new object();

        private DatabaseContext()
        {
            
            Console.WriteLine("Đã khởi tạo kết nối cơ sở dữ liệu.");
        }

        public static DatabaseContext GetInstance()
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DatabaseContext();
                    }
                }
            }
            return _instance;
        }

        public void ExecuteQuery(string query)
        {
            Console.WriteLine($"Thực hiện truy vấn: {query}");
        }
    }
}
