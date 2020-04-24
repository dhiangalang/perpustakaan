using System;
using System.ComponentModel.DataAnnotations;

namespace Perpustakaan.Models
{
    public class Peminjaman
    {
        [Key]
        public int IDPeminjaman { get; set; }

        public int IDUser { get; set; }

        public int IDBook { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tanggal Mulai")]
        public DateTime TanggalMulai { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Tanggal Selesai")]
        public DateTime TanggalSelesai { get; set; }

        [Display(Name = "Jumlah Hari Sewa")]
        public int JumlahHari { get; set; }

        [Display(Name = "Total Sewa (Rp)")]
        public int TotalHarga { get; set; }

        public virtual User User { get; set; }
        public virtual Buku Buku { get; set; }
    }
}