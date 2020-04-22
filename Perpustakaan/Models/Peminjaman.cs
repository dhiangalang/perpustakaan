using System;

namespace Perpustakaan.Models
{
    public class Peminjaman
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int IDPeminjaman { get; set; }
        public int IDUser { get; set; }
        public int IDBuku { get; set; }
        public DateTime TanggalMulai { get; set; }
        public DateTime TanggalSelesai { get; set; }

        public virtual User User { get; set; }
        public virtual Buku Buku { get; set; }
    }
}