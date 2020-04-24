using System.Data.Entity;
using System.Collections.Generic;
using Perpustakaan.Models;
using System.Web.Helpers;

namespace Perpustakaan.DAL
{
    public class PerpustakaanInitializer : DropCreateDatabaseAlways<PerpustakaanContext>
    {
        protected override void Seed(PerpustakaanContext context)
        {
            List<User> users = new List<User>
            {
                new User{Password=Crypto.HashPassword("admin123"),NamaUser="admin",Role=Role.Admin},
                new User{Password=Crypto.HashPassword("peminjam123"),NamaUser="peminjam",Role=Role.Peminjam}
            };

            users.ForEach(u => context.Users.Add(u));
            context.SaveChanges();

            List<Buku> bukus = new List<Buku>
            {
                new Buku{JudulBuku="Tutorial Memasak",JenisBuku="Tutorial",Pengarang="Marinka",HargaSewa=3500},
                new Buku{JudulBuku="Tutorial Balapan",JenisBuku="Tutorial",Pengarang="Rosi",HargaSewa=2500},
                new Buku{JudulBuku="Tutorial Gaming",JenisBuku="Tutorial",Pengarang="Tara",HargaSewa=3100}
            };

            bukus.ForEach(b => context.Bukus.Add(b));
            context.SaveChanges();
        }
    }
}