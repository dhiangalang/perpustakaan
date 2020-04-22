using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Perpustakaan.Models
{
    public class Buku
    {
        [Key]
        [Display(Name = "ID Book")]
        public int IDBook { get; set; }
        [Display(Name = "Judul Buku")]
        public string JudulBuku { get; set; }
        public string Pengarang { get; set; }
        [Display(Name = "Jenis Buku")]
        public string JenisBuku { get; set; }
        [Display(Name = "Harga Sewa per Hari (Rp)")]
        public int HargaSewa { get; set; }

        public virtual ICollection<Peminjaman> Peminjamans { get; set; }
    }
}