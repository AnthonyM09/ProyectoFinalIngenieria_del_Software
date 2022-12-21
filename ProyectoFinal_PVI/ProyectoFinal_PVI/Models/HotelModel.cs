using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal_PVI.Models
{
    public class HotelModel : PersonaModel
    {
        [Required(ErrorMessage = "Campo requerido")]
        public int idHotel { set; get; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Nombre completo")]
        [StringLength(150, ErrorMessage = "La longitud de este campo debe de estar entre 1 y 150 caracteres", MinimumLength = 1)]
        public string nombre { set; get; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Dirección")]
        [StringLength(500, ErrorMessage = "La longitud de este campo debe de estar entre 1 y 500 caracteres", MinimumLength = 1)]
        public string direccion { set; get; }

        [Display(Name = "Teléfono")]
        [StringLength(20, ErrorMessage = "La longitud de este campo debe de estar entre 1 y 20 caracteres", MinimumLength = 1)]
        public string telefono { set; get; }

        [Required(ErrorMessage = "Campo requerido")]
        public decimal costoPorCadaAdulto { set; get; } //Este dato no se debe de solicitar al usuario, ya que se obtiene de la tabla llamada Hotel.

        [Required(ErrorMessage = "Campo requerido")]
        public decimal costoPorCadaNihno { set; get; } //Este dato no se debe de solicitar al usuario, ya que se obtiene de la tabla llamada Hotel.
    }
}