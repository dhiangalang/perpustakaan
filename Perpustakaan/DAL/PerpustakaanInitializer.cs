using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using Perpustakaan.Models;

namespace Perpustakaan.DAL
{
    public class PerpustakaanInitializer : DropCreateDatabaseIfModelChanges<PerpustakaanContext>
    {
        protected override void Seed(PerpustakaanContext context)
        {
            List<User> users = new List<User>
            {
                new User{Password="admin123",NamaUser="admin",Role=Role.Admin},
                new User{Password="peminjam123",NamaUser="peminjam",Role=Role.Peminjam}
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