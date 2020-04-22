using Perpustakaan.Models;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace Perpustakaan.DAL
{
    public class PerpustakaanContext : DbContext
    {
        public PerpustakaanContext() : base("PerpustakaanContext")
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Buku> Bukus { get; set; }
        public DbSet<Peminjaman> Peminjamans { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}