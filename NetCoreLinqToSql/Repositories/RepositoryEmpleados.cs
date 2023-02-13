using NetCoreLinqToSql.Models;
using System.Data;
using System.Data.SqlClient;

namespace NetCoreLinqToSql.Repositories
{
    public class RepositoryEmpleados
    {
        private DataTable tablaEmpleados;

        public RepositoryEmpleados()
        {
            string connectionString = @"Data Source=LOCALHOST\DESARROLLO;Initial Catalog=HOSPITAL;User ID=sa; Password=";
            string sql = "SELECT * FROM EMP";
            SqlDataAdapter adapter = new SqlDataAdapter(sql, connectionString);
            this.tablaEmpleados = new DataTable();
            adapter.Fill(this.tablaEmpleados);
        }

        public ResumenEmpleados GetEmpleadosOficio(string oficio)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
                           select datos;
            consulta = consulta.OrderBy(x => x.Field<int>("SALARIO"));
            int personas = consulta.Count();
            int maximo = consulta.Max(z => z.Field<int>("SALARIO"));
            double media = consulta.Average(z => z.Field<int>("SALARIO"));
            List<Empleado> empleados = new List<Empleado>();
            foreach (var row in consulta)
            {
                Empleado empleado = new Empleado
                {
                    IdEmpleado = row.Field<int>("EMP_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Oficio = row.Field<string>("OFICIO"),
                    Salario = row.Field<int>("SALARIO"),
                    IdDepartamento = row.Field<int>("DEPT_NO"),
                };
                empleados.Add(empleado);
            }
            ResumenEmpleados model = new ResumenEmpleados
            {
                Empleados = empleados,
                MaximoSalarial = maximo,
                Personas = personas,
                MediaSalarial = media
            };
            return model;
        }

        public List<string> GetOficios()
        {
            var consulta = (from datos in this.tablaEmpleados.AsEnumerable()
                           select datos.Field<string>("OFICIO")).Distinct();
            List<string> oficios = new List<string>();
            foreach (var oficio in consulta)
            {
                oficios.Add(oficio);
            }
            return oficios;
        }

        public List<Empleado> GetEmpleados()
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           select datos;
            List<Empleado> empleados = new List<Empleado>();
            foreach (var row in consulta)
            {
                Empleado empleado = new Empleado
                {
                    IdEmpleado = row.Field<int>("EMP_NO"),
                    Apellido = row.Field<string>("APELLIDO"),
                    Oficio = row.Field<string>("OFICIO"),
                    Salario = row.Field<int>("SALARIO"),
                    IdDepartamento = row.Field<int>("DEPT_NO")
                };
                empleados.Add(empleado);
            }
            return empleados;
        }

        public Empleado FindEmpleado(int idempleado)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<int>("EMP_NO") == idempleado
                           select datos;
            var row = consulta.First();
            Empleado empleado = new Empleado
            {
                IdEmpleado = row.Field<int>("EMP_NO"),
                Apellido = row.Field<string>("APELLIDO"),
                Oficio = row.Field<string>("OFICIO"),
                Salario = row.Field<int>("SALARIO"),
                IdDepartamento = row.Field<int>("DEPT_NO")
            };
            return empleado;
        }

        public List<Empleado> GetEmpleados(string oficio, int salario)
        {
            var consulta = from datos in this.tablaEmpleados.AsEnumerable()
                           where datos.Field<string>("OFICIO") == oficio
                           && datos.Field<int>("SALARIO") >= salario
                           select datos;
            if (consulta.Count() == 0)
            {
                return null;
            }
            else
            {
                List<Empleado> empleados = new List<Empleado>();
                foreach (var row in consulta)
                {
                    Empleado empleado = new Empleado
                    {
                        IdEmpleado = row.Field<int>("EMP_NO"),
                        Apellido = row.Field<string>("APELLIDO"),
                        Oficio = row.Field<string>("OFICIO"),
                        Salario = row.Field<int>("SALARIO"),
                        IdDepartamento = row.Field<int>("DEPT_NO")
                    };
                    empleados.Add(empleado);
                }
                return empleados;
            }
        }
    }
}
