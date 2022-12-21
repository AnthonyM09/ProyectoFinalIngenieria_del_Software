using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProyectoFinal_PVI.Models
{
    public class BitacoraModel
    {
        public int idBitacora { set; get; }

        //Nombre completo : Este dato siempre se debe de obtener de la sesión del sistema que se creó a partir de la autenticación del usuario.
        public string nombre { set; get; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Acción")]
        [StringLength(25, ErrorMessage = "La longitud de este campo debe de estar entre 1 y 25 caracteres", MinimumLength = 1)]
        public string accionRealizada { set; get; }

        [Required(ErrorMessage = "Campo requerido")]
        [Display(Name = "Fecha")]
        public DateTime fechaDeLaAccion { set; get; } //Este dato no debe ser solicitado al usuario; ya que su valor se debe obtener
                                                      //a partir de la fecha y hora del equipo de cómputo en el que se está ejecutando el proyecto.
        
    }
}