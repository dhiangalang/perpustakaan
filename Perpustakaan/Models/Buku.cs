using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Perpustakaan.Models
{
    public class Buku
    {
        [Key]
        [Display(Name = "ID Book")]
        public int IDBook { get; set; }

        [Required]
        [Display(Name = "Judul Buku")]
        public string JudulBuku { get; set; }

        [Required]
        public string Pengarang { get; set; }

        [Required]
        [Display(Name = "Jenis Buku")]
        public string JenisBuku { get; set; }

        [Required]
        [Display(Name = "Harga Sewa per Hari (Rp)")]
        public int HargaSewa { get; set; }

        public virtual ICollection<Peminjaman> Peminjamans { get; set; }
    }
}