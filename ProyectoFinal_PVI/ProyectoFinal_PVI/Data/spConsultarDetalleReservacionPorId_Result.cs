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
    
    public partial class spConsultarDetalleReservacionPorId_Result
    {
        public int idReservacion { get; set; }
        public int idHotel { get; set; }
        public string nombre { get; set; }
        public int idHabitacion { get; set; }
        public string numeroHabitacion { get; set; }
        public string nombreCompleto { get; set; }
        public System.DateTime fechaEntrada { get; set; }
        public System.DateTime fechaSalida { get; set; }
        public int numeroAdultos { get; set; }
        public int numeroNinhos { get; set; }
        public decimal costoPorCadaAdulto { get; set; }
        public decimal costoPorCadaNinho { get; set; }
        public string estado { get; set; }
    }
}
