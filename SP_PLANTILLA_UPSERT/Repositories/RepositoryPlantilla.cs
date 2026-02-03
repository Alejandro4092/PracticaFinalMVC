using Microsoft.Data.SqlClient;
using SP_PLANTILLA_UPSERT.Models;
using System.Data;

namespace SP_PLANTILLA_UPSERT.Repositories
{
    public class RepositoryPlantilla
    {
        private SqlConnection cn;
        private SqlCommand com;
        private DataTable tablaPlantilla;
        public RepositoryPlantilla()
        {
            string connectionString = @"Data Source=LOCALHOST\DEVELOPER;Initial Catalog=HOSPITAL;Persist Security Info=True;User ID=SA;Password=Admin123;Encrypt=True;Trust Server Certificate=True";
            this.cn = new SqlConnection(connectionString);
            this.com = new SqlCommand();
            this.com.Connection = this.cn;
            string sql = "select * from PLANTILLA";

            SqlDataAdapter ad = new SqlDataAdapter(sql, connectionString);
            this.tablaPlantilla = new DataTable();
            ad.Fill(this.tablaPlantilla);
        }
        public List<Plantilla> GetPlantilla()
        {

            var consulata = from datos in this.tablaPlantilla.AsEnumerable() select datos;

            List<Plantilla> plantilla = new List<Plantilla>();

            foreach (var row in consulata)
            {
                //para extraer datos de un data row 
                //datarow.field<tipodato>("COLUMNA")
                Plantilla plant = new Plantilla();
                plant.IdHospital = row.Field<int>("HOSPITAL_COD");
                plant.SalaCod = row.Field<int>("SALA_COD");
                plant.empleadoNo = row.Field<int>("EMPLEADO_NO");
                plant.Apellido = row.Field<string>("APELLIDO");
                plant.Funcion = row.Field<string>("FUNCION");
                plant.Turno = row.Field<string>("T");
                plant.Salario = row.Field<int>("SALARIO");
                plantilla.Add(plant);
            }
            return plantilla;
        }
        public Plantilla FindPlantilla(int id)
        {
            var consulta = from datos in this.tablaPlantilla.AsEnumerable() where datos.Field<int>("HOSPITAL_COD") == id select datos;
            var row = consulta.First();
            Plantilla enfermo = new Plantilla();
            enfermo.IdHospital = row.Field<int>("HOSPITAL_COD");
            enfermo.SalaCod = row.Field<int>("SALA_COD");
            enfermo.empleadoNo = row.Field<int>("EMPLEADO_NO");
            enfermo.Apellido = row.Field<string>("APELLIDO");
            enfermo.Funcion = row.Field<string>("FUNCION");
            enfermo.Turno = row.Field<string>("T");
            enfermo.Salario = row.Field<int>("SALARIO");
            return enfermo;
        }
        public async Task DeletePlantilla(int id)
        {
            string sql = "delete from PLANTILLA where HOSPITAL_COD=@hospitalcod";
            this.com.Parameters.AddWithValue("@hospitalcod", id);
            this.com.CommandType = CommandType.Text;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
        }
        public async Task<int> CreatePlantillaAsync( int idhospital ,int salaCod ,int empleadoNo ,string apellido, string funcion, string turno, 
int salario) 
        {
            string sql = "SP_PLANTILLA_UPSERT";
            this.com.Parameters.AddWithValue("@hospitalcod", idhospital);
            this.com.Parameters.AddWithValue("@salacod", salaCod);
            this.com.Parameters.AddWithValue("@empleadono", empleadoNo);
            this.com.Parameters.AddWithValue("@apellido", apellido);
            this.com.Parameters.AddWithValue("@funcion", funcion);
            this.com.Parameters.AddWithValue("@t", turno);
            this.com.Parameters.AddWithValue("@salario", salario);
            this.com.CommandType = CommandType.StoredProcedure;
            this.com.CommandText = sql;
            await this.cn.OpenAsync();
            int registros = await this.com.ExecuteNonQueryAsync();
            await this.cn.CloseAsync();
            this.com.Parameters.Clear();
            return registros;

        }
        public ResumenPlantilla GetEmpleadosFuncion(string funcion)
        {
            var consulta = from datos in this.tablaPlantilla.AsEnumerable()
                           where datos.Field<string>("FUNCION") == funcion
                           orderby datos.Field<string>("FUNCION")
                           select datos;


            consulta = consulta.OrderBy(z => z.Field<int>("SALARIO"));

            if (consulta.Count() == 0)
            {
                return new ResumenPlantilla
                {
                    Personas = 0,
                    MaximoSalarial = 0,
                    MediaSalarial = 0,
                    Plantilla = null
                };
            }

            int personas = consulta.Count();
            int maximo = consulta.Max(x => x.Field<int>("SALARIO"));
            double media = consulta.Average(x => x.Field<int>("SALARIO"));
            List<Plantilla> empleados = new List<Plantilla>();
            foreach (var row in consulta)
            {
                Plantilla emp = new Plantilla
                {
                    IdHospital = row.Field<int>("HOSPITAL_COD"),
                    SalaCod = row.Field<int>("SALA_COD"),
                    empleadoNo = row.Field<int>("EMPLEADO_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Funcion = row.Field<string>("FUNCION"),
                    Turno = row.Field<string>("T"),
                    Salario = row.Field<int>("SALARIO"),

                };
                empleados.Add(emp);
            }
            ResumenPlantilla model = new ResumenPlantilla();
            model.Personas = personas;
            model.MaximoSalarial = maximo;
            model.MediaSalarial = media;
            model.Plantilla = empleados;
            return model;
        }
        public List<string> GetFuncion()
        {
            var consulta = (from datos in this.tablaPlantilla.AsEnumerable() select datos.Field<string>("FUNCION")).Distinct();
     
            return consulta.ToList();
        }
        public List<Plantilla> GetPlantillaFuncionSalario(string funcion, int salario)
        {
            var consulta = from datos in this.tablaPlantilla.AsEnumerable()
                           where datos.Field<string>("FUNCION") == funcion
                           && datos.Field<int>("SALARIO") >= salario
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                List<Plantilla> plantilla = new List<Plantilla>();
                foreach (var row in consulta)
                {
                    Plantilla plant = new Plantilla
                    {
                        IdHospital = row.Field<int>("HOSPITAL_COD"),
                        SalaCod = row.Field<int>("SALA_COD"),
                        empleadoNo = row.Field<int>("EMPLEADO_NO"),
                        Apellido = row.Field<string>("APELLIDO"),
                        Funcion = row.Field<string>("FUNCION"),
                        Turno = row.Field<string>("T"),
                        Salario = row.Field<int>("SALARIO"),


                    };
                    plantilla.Add(plant);
                }
                return plantilla;
            }

        }
    }
}
