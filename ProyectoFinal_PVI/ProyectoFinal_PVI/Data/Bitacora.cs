//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ProyectoFinal_PVI.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class Bitacora
    {
        public int idBitacora { get; set; }
        public int idReservacion { get; set; }
        public int idPersona { get; set; }
        public string accionRealizada { get; set; }
        public System.DateTime fechaDeLaAccion { get; set; }
    
        public virtual Persona Persona { get; set; }
        public virtual Reservacion Reservacion { get; set; }
    }
}
