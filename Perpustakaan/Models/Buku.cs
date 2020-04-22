using System.Collections.Generic;

namespace Perpustakaan.Models
{
    public class Buku
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int IDBook { get; set; }
        public string JudulBuku { get; set; }
        public string Pengarang { get; set; }
        public string JenisBuku { get; set; }
        public int HargaSewa { get; set; }

        public virtual ICollection<Peminjaman> Peminjamans { get; set; }
    }
}