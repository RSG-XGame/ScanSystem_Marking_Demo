using ScanSystems.Marking.DAL;
using ScanSystems.Marking.DAL.Models;
using System;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (ScanSystemsContext db = new ScanSystemsContext())
            {
                db.Database.EnsureCreated();
                var t1 = db.CodeTypes.ToList();
            }
            Console.ReadKey(true);
        }
    }
}
