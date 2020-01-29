using ScanSystems.Logic.Controllers;
using ScanSystems.Marking.DAL;
using ScanSystems.Marking.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleApp1
{
    class Program
    {
        private abstract class F1
        {
            protected int? val1 { get; private set; }

            public F1()
            {
                Init();
            }

            protected virtual void Init()
            {
                val1 = 1;
            }
        }

        private class F2 : F1
        {
            protected int? val2 { get; private set; }

            protected override void Init()
            {
                val2 = 2;

                base.Init();
            }
        }

        static void Main(string[] args)
        {
            string[] codes = GetCodes();

            Product product = null;

            using (ScanSystemsContext db = new ScanSystemsContext())
            {
                db.Database.EnsureCreated();
                product = db.Products.FirstOrDefault();
            }

            Registrator reg = new Registrator();
            reg.Initialize(new CodeType() { Id = 3 }, product);
            

            foreach (var dm in codes)
            {
                reg.Registration(dm);
            }

            Console.ReadKey(true);
        }

        static string[] GetCodes()
        {
            string[] codes = new string[60];
            for (int i = 0; i < codes.Length; ++i)
            {
                codes[i] = (i + 1).ToString().PadLeft(10, '0');
            }

            for (int counter = codes.Length; counter > -1; --counter)
            {
                int idx1 = (new Random()).Next(0, codes.Length);
                int idx2 = (new Random()).Next(0, codes.Length);

                string temp = codes[idx1];
                codes[idx1] = codes[idx2];
                codes[idx2] = temp;
            }

            return codes;
        }
    }
}
