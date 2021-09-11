using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


#nullable disable

namespace FrontPruebaNxs.WebFront.Models
{
    public class Libro 
    {
     
        public int id { get; set; }
        [Required]
        [MaxLength(40)]
        [Display(Name = "Título")]
        public string Titulo { get; set; }
        [Required]
        [Display(Name = "No paginas")]
        public int Nopaginas { get; set; }
        [Required]
        [Display(Name = "Año")]
        public int Ano { get; set; }
        [Required]
        [MaxLength(30)]
        public string Genero { get; set; }
        
        [Required]
        [Display(Name = "Autor Id")]
        public int Idautor { get; set; }

    }
}
