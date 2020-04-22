using System.Collections.Generic;

namespace Perpustakaan.Models
{
    public enum Role
    {
        Admin, Peminjam
    }

    public class User
    {
        [System.ComponentModel.DataAnnotations.Key]
        public int IDUser { get; set; }
        public string Password { get; set; }
        public string NamaUser { get; set; }
        public Role Role { get; set; }

        public virtual ICollection<Peminjaman> Peminjamans { get; set; }
    }
}