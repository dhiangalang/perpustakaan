﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [StringLength(100)]
        [Required]
        public string Password { get; set; }

        [Display(Name = "Nama User")]
        [Index(IsUnique = true)]
        [StringLength(100)]
        [Required]
        public string NamaUser { get; set; }

        [Required]
        public Role Role { get; set; }

        public virtual ICollection<Peminjaman> Peminjamans { get; set; }
    }
}