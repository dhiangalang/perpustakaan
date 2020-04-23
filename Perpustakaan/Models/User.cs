using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Helpers;

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
        public string Password
        {
            get { return ""; }
            set { Password = Crypto.HashPassword(value); }            
        }
        [Display(Name = "Nama User")]
        public string NamaUser { get; set; }
        public Role Role { get; set; }

        public virtual ICollection<Peminjaman> Peminjamans { get; set; }

        public bool VerifyHashedPassword(string password)
        {
            return Crypto.VerifyHashedPassword(Password, password);
        }
    }
}