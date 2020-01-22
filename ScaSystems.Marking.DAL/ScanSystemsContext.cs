using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ScaSystems.Marking.DAL
{
    public class ScanSystemsContext : DbContext
    {
        private string databasePath;

        public ScanSystemsContext()
        {
            databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "ScanSystemsDemo.db"); ;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Filename={databasePath}");
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*User user = new User { UserId = Guid.NewGuid(), Login = "admin", Password = "admin" };

            CodeType item = new CodeType { CodeTypeId = 1, CodeTypeName = "Продукт" };
            CodeType box = new CodeType { CodeTypeId = 2, CodeTypeName = "Коробка" };
            CodeType pallet = new CodeType { CodeTypeId = 3, CodeTypeName = "Паллета" };

            CodeTypeDimention itemInBox = new CodeTypeDimention { CodeTypeDimentionId = Guid.NewGuid(), ParentCodeTypeId = 2, ChildrenCodeTypeId = 1, MaxCountChildrens = 4 };
            CodeTypeDimention boxInPallet = new CodeTypeDimention { CodeTypeDimentionId = Guid.NewGuid(), ParentCodeTypeId = 3, ChildrenCodeTypeId = 2, MaxCountChildrens = 2 };

            modelBuilder.Entity<User>().HasData(user);
            modelBuilder.Entity<CodeType>().HasData(item, box, pallet);
            modelBuilder.Entity<CodeTypeDimention>().HasData(itemInBox, boxInPallet);*/

            base.OnModelCreating(modelBuilder);

        }
    }
}
