using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace FrontPruebaNxs.WebFront.Models
{
    public class Autor
    {
        public int id { get; set; }
        [Required]
        [MaxLength(35)]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }
        [Required]
        [Display(Name = "Fecha de nacimiento")]
        public DateTime Fechanacimiento { get; set; }
        [Required]
        [MaxLength(25)]
        public string Ciudad { get; set; }
        [Required]
        [MaxLength(40)]
        public string Email { get; set; }
        public virtual ICollection<Libro> Libros { get; set; }

    }
}
