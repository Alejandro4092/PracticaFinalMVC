using Microsoft.Data.SqlClient;
using SP_PLANTILLA_UPSERT.Models;
using System.Data;

namespace SP_PLANTILLA_UPSERT.Repositories
{
    public class RepositoryDeportes
    {
        private SqlConnection cn;
        private SqlCommand com;
        private DataTable tablaAlumnos;

        public RepositoryDeportes()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=COSASDEMARTES;Persist Security Info=True;User ID=SA;Password=Admin123;Encrypt=True;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;

            string sql = @"SELECT 
                            u.IDUSUARIO,
                            u.NOMBRE,
                            u.APELLIDOS,
                            u.EMAIL,
                            u.IMAGEN,
                            c.NOMBRE as NombreCurso,
                            a.nombre as NombreActividad,
                            e.fecha_evento,
                            i.quiere_ser_capitan,
                            i.fecha_inscripcion
                          FROM USUARIOSTAJAMAR u
                          INNER JOIN CURSOSTAJAMAR c ON u.IDCURSO = c.IDCURSO
                          INNER JOIN INSCRIPCIONES i ON u.IDUSUARIO = i.id_usuario
                          INNER JOIN EVENTO_ACTIVIDADES ea ON i.IdEventoActividad = ea.IdEventoActividad
                          INNER JOIN ACTIVIDADES a ON ea.IdActividad = a.id_actividad
                          INNER JOIN EVENTOS e ON ea.IdEvento = e.id_evento";

            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);
            this.tablaAlumnos = new DataTable();
            ad.Fill(this.tablaAlumnos);
        }

        public List<AlumnoDetalle> GetAlumnos()
        {
            var consulta = from datos in this.tablaAlumnos.AsEnumerable() select datos;

            List<AlumnoDetalle> alumnos = new List<AlumnoDetalle>();

            foreach (var row in consulta)
            {
                AlumnoDetalle alumno = new AlumnoDetalle();
                alumno.IdUsuario = row.Field<int>("IDUSUARIO");
                alumno.Nombre = row.Field<string>("NOMBRE");
                alumno.Apellidos = row.Field<string>("APELLIDOS");
                alumno.Email = row.Field<string>("EMAIL");
                alumno.Imagen = row.Field<string>("IMAGEN");
                alumno.NombreCurso = row.Field<string>("NombreCurso");
                alumno.NombreActividad = row.Field<string>("NombreActividad");
                alumno.FechaEvento = row.Field<DateTime>("fecha_evento");
                alumno.QuiereSerCapitan = row.Field<bool>("quiere_ser_capitan");
                alumno.FechaInscripcion = row.Field<DateTime>("fecha_inscripcion");
                alumnos.Add(alumno);
            }

            return alumnos;
        }

        public AlumnoDetalle FindAlumno(int id)
        {
            var consulta = from datos in this.tablaAlumnos.AsEnumerable()
                          where datos.Field<int>("IDUSUARIO") == id
                          select datos;

            var row = consulta.First();

            AlumnoDetalle alumno = new AlumnoDetalle();
            alumno.IdUsuario = row.Field<int>("IDUSUARIO");
            alumno.Nombre = row.Field<string>("NOMBRE");
            alumno.Apellidos = row.Field<string>("APELLIDOS");
            alumno.Email = row.Field<string>("EMAIL");
            alumno.Imagen = row.Field<string>("IMAGEN");
            alumno.NombreCurso = row.Field<string>("NombreCurso");
            alumno.NombreActividad = row.Field<string>("NombreActividad");
            alumno.FechaEvento = row.Field<DateTime>("fecha_evento");
            alumno.QuiereSerCapitan = row.Field<bool>("quiere_ser_capitan");
            alumno.FechaInscripcion = row.Field<DateTime>("fecha_inscripcion");

            return alumno;
        }       
    }
}
