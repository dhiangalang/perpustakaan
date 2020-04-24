using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web.Helpers;

namespace Perpustakaan.Models
{
    public class LoginViewModel
    {
        [DataType(DataType.Password)]
        [StringLength(100)]
        [Required]
        public string Password { get; set; }

        [Display(Name = "Nama User")]
        [StringLength(100)]
        [Required]
        public string NamaUser { get; set; }
    }
}