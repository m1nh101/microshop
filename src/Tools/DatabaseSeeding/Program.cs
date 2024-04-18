// See https://aka.ms/new-console-template for more information

using DatabaseSeeding;
using Microsoft.EntityFrameworkCore;
using Product.API.Infrastructure.Database;

var contextBuilder = new DbContextOptionsBuilder<ProductDbContext>();
contextBuilder.UseSqlServer("Server=localhost;Database=ProductDb;UID=sa;PWD=M1ng@2002;Encrypt=False;");

var context = new ProductDbContext(contextBuilder.Options, new MigratorSession());

var migrator = new  ProductMigrator(context);

await Task.Run(migrator.Seeding).ContinueWith(_ => Console.WriteLine("Done"));