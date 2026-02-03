namespace SP_PLANTILLA_UPSERT.Models
{
    public class AlumnoDetalle
    {
        public int IdUsuario { get; set; }
        public string Nombre { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
        public string Imagen { get; set; }
        public string NombreCurso { get; set; }
        public string NombreActividad { get; set; }
        public DateTime FechaEvento { get; set; }
        public bool QuiereSerCapitan { get; set; }
        public DateTime FechaInscripcion { get; set; }
    }
}