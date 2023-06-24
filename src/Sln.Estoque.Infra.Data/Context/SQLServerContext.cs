using Microsoft.EntityFrameworkCore;
using Sln.Estoque.Domain.Entities;
using System;

namespace Sln.Estoque.Infra.Data.Context
{
    public class SQLServerContext : DbContext
    {
        public SQLServerContext() :base() { }

        public SQLServerContext(DbContextOptions<SQLServerContext> options) : base(options) { }

		/*protected override void OnModelCreating(ModelBuilder modelBuilder)
		{*/
			/*modelBuilder.Entity<FinishedOrder>()
					 .HasData(
					 new { Id = 1, OrderId = 46693, LayoutCode = 0100, Quantity = 350, User = 1 }
					 );

			modelBuilder.Entity<Category>()
					.HasData(
					new { Id = 1, Name = "Material" },
					new { Id = 2, Name = "Ribbon" }
					);

			modelBuilder.Entity<Unit>()
					.HasData(
					new { Id = 1, Name = "Milhar" },
					new { Id = 2, Name = "Unidade" },
					new { Id = 3, Name = "M²" }
					);

			modelBuilder.Entity<Product>()
					.HasData(
					new { Id = 1, CodeProduct = "P1098", Name = "Tag 30x56x3", Alias = "Tag 30x56", Quantity = 54, UnitId = 1, Price = 48.65m, CategoryId = 1, UpdateTime = DateTime.Now },
					new { Id = 2, CodeProduct = "P6540", Name = "Nylon 35mm", Alias = "Nylon 35", Quantity = 20, UnitId = 2, Price = 23.50m, CategoryId = 1, UpdateTime = DateTime.Now },
					new { Id = 3, CodeProduct = "P3517", Name = "Nylon 45mm", Alias = "Nylon 45", Quantity = 12, UnitId = 2, Price = 29.48m, CategoryId = 1, UpdateTime = DateTime.Now },
					new { Id = 4, CodeProduct = "P8590", Name = "Ribbon Cera", Alias = "Ribbon Cera", Quantity = 8, UnitId = 2, Price = 37.42m, CategoryId = 2, UpdateTime = DateTime.Now }
					);

			modelBuilder.Entity<Product>()
					.HasData(
					new { Id = 1, CodeProduct = "P1098", Name = "Tag 30x56x3", Quantity = 54, UnitId = 1, Price = 48.65m, CategoryId = 1, UpdateTime = DateTime.Now },
					new { Id = 2, CodeProduct = "P6540", Name = "Nylon 35mm", Quantity = 20, UnitId = 2, Price = 23.50m, CategoryId = 1, UpdateTime = DateTime.Now },
					new { Id = 3, CodeProduct = "P3517", Name = "Nylon 45mm", Quantity = 12, UnitId = 2, Price = 29.48m, CategoryId = 1, UpdateTime = DateTime.Now },
					new { Id = 4, CodeProduct = "P8590", Name = "Ribbon Cera", Quantity = 8, UnitId = 2, Price = 37.42m, CategoryId = 2, UpdateTime = DateTime.Now }
					);

			modelBuilder.Entity<Role>()
					 .HasData(
					 new { Id = 1, Level = "Alta", LevelId = 1 },
					 new { Id = 2, Level = "Media", LevelId = 2 },
					 new { Id = 3, Level = "Baixa", LevelId = 3 }
					 );

			modelBuilder.Entity<User>()
					 .HasData(
					 new { Id = 1, Name = "Gérson Bonetti", Username = "@gb", Password = "lotr", RoleId = 1 },
					 new { Id = 2, Name = "Gabriel Mundt", Username = "@md", Password = "rainbowsix", RoleId = 1 },
					 new { Id = 3, Name = "Yuri Laurindo", Username = "@yl", Password = "impressora", RoleId = 2 },
					 new { Id = 4, Name = "João Lopes", Username = "@jv", Password = "mequi", RoleId = 3 }
					 );

			modelBuilder.Entity<Layout>()
					.HasData(
					new { Id = 1, ViewName = "Aramis", FileName = "_Aramis", Multiplier = 1, Method = 1, QuantityPosition = 0 },
					new { Id = 2, ViewName = "CK Jeans Composição", FileName = "CK-JEANS-COMPOSICAO-", Multiplier = 1, Method = 2, QuantityPosition = 1 },
					new { Id = 3, ViewName = "Dudalina (preço)", FileName = "PREÇO__", Multiplier = 1, Method = 2, QuantityPosition = 1 },
					new { Id = 4, ViewName = "Tag Renner", FileName = "TAG ", Multiplier = 2, Method = 1, QuantityPosition = 0 },
					new { Id = 5, ViewName = "Dafiti", FileName = "Dafiti_", Multiplier = 1, Method = 1, QuantityPosition = 0 }
				);*/
		//}

		public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Unit> Units { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Layout> Layouts { get; set; }
        public DbSet<FinishedOrder> FinishedOrders { get; set; }
    }
}