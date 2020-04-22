using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Perpustakaan.Models
{
    public enum Role
    {
        Admin, Peminjam
    }

    public class User
    {
        [Key]
        [Display(Name = "ID User")]
        public int IDUser { get; set; }
        [DataType(DataType.Password)]
        public string Password { get; set; }
        [Display(Name = "Nama User")]
        public string NamaUser { get; set; }
        public Role Role { get; set; }

        public virtual ICollection<Peminjaman> Peminjamans { get; set; }
    }
}