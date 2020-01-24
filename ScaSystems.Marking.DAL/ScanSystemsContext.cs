using Microsoft.EntityFrameworkCore;
using ScanSystems.Marking.DAL.Models;
using System;
using System.IO;

namespace ScanSystems.Marking.DAL
{
    public class ScanSystemsContext : DbContext
    {
        private string databasePath;

        public DbSet<DMCode> DMCodes {get;set;}
        public DbSet<DMCodeState> DMCodeStates {get;set;}
        public DbSet<Product> Products {get;set;}
        public DbSet<RegisterCode> RegisterCodes {get;set;}
        public DbSet<User> Users { get;set;}
        public DbSet<CodeType> CodeTypes { get; set; }

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
            DMCodeState dmStateFree = new DMCodeState { Id = 1, Name = "Свободный" };
            DMCodeState dmStateProduct = new DMCodeState { Id = 2, Name = "Связан с товаром" };
            DMCodeState dmStateBox = new DMCodeState { Id = 3, Name = "Связан с коробкой" };
            DMCodeState dmStatePallet = new DMCodeState { Id = 4, Name = "Связан с паллетой" };
            DMCodeState dmStateDefected = new DMCodeState { Id = 5, Name = "Бракованный" };

            DMCode[] dmCodes = new DMCode[100];
            for (int i = 0; i < dmCodes.Length; ++i)
            {
                dmCodes[i] = new DMCode { Id = Guid.NewGuid(), DataMatrix = (i + 1).ToString().PadLeft(10, '0'), DMCodeStateId = 1, ProductId = null };
            }

            Product product1 = new Product { Id = Guid.NewGuid(), Barcode = "978020137962", Name = "Кока-кола" };
            Product product2 = new Product { Id = Guid.NewGuid(), Barcode = "460123456789", Name = "Сникерс" };

            User user = new User { Id = Guid.NewGuid(), Password = "admin", Login = "admin", Name = "Администратор", LastSignIn = DateTime.Now, DeviceSerialNumber = "" };

            CodeType product = new CodeType { Id = 1, Name = "Продукт", ChildrenCodeTypeId = null, MaxCountChildrens = 0, DMCodeStateId = 2 };
            CodeType productInBox = new CodeType { Id = 2, Name = "Продукт в коробке", ChildrenCodeTypeId = 1, MaxCountChildrens = 3, DMCodeStateId = 3 };
            CodeType boxOnPallet = new CodeType { Id = 3, Name = "Коробка на паллете", ChildrenCodeTypeId = 2, MaxCountChildrens = 2, DMCodeStateId = 4 };
            CodeType productOnPallet = new CodeType { Id = 4, Name = "Продукт на паллете", ChildrenCodeTypeId = 1, MaxCountChildrens = 4, DMCodeStateId = 4 };

            modelBuilder.Entity<DMCodeState>().HasData(dmStateFree, dmStateProduct, dmStateBox, dmStatePallet, dmStateDefected);
            modelBuilder.Entity<DMCode>().HasData(dmCodes);
            modelBuilder.Entity<Product>().HasData(product1, product2);
            modelBuilder.Entity<User>().HasData(user);
            modelBuilder.Entity<CodeType>().HasData(product, productInBox, productOnPallet, boxOnPallet);

            modelBuilder.Entity<DMCode>().Property(x => x.Id).IsRequired().HasColumnType("blob");
            modelBuilder.Entity<DMCodeState>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();
            modelBuilder.Entity<Product>().Property(x => x.Id).IsRequired().HasColumnType("blob");
            modelBuilder.Entity<RegisterCode>().Property(x => x.Id).IsRequired().HasColumnType("blob");
            modelBuilder.Entity<User>().Property(x => x.Id).IsRequired().HasColumnType("blob");
            modelBuilder.Entity<CodeType>().Property(x => x.Id).IsRequired().ValueGeneratedOnAdd();

            base.OnModelCreating(modelBuilder);

        }
    }
}
