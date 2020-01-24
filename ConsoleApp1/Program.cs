using ScanSystems.Marking.DAL;
using ScanSystems.Marking.DAL.Models;
using System;
using System.Collections.Generic;
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

                int? id = 3;
                List<CodeType> codeTypes = new List<CodeType>();
                db.CodeTypes.ToList().ForEach((x) =>
                {
                    if (id.HasValue && x.Id == id.Value)
                    {
                        codeTypes.Add(x);
                        id = x.ChildrenCodeTypeId;
                    }
                });
            }
            Console.ReadKey(true);
        }
    }
}
