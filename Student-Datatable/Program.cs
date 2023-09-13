//Librerias del ADO .NET
using System.Data.SqlClient;
using System.Data;
using System.Reflection.PortableExecutable;
using WpfApp1;

class Program
{
    // Cadena de conexión a la base de datos
    public static string connectionString = "Data Source=LAB1504-05\\SQLEXPRESS;Initial Catalog=TecsupDB;User ID=alumnoTecsup;Password=123456";


    static void Main()
    {

        #region FormaDesconectada
        //Datatable
        DataTable dataTable = ListarStudentsDataTable();
       
       
       Console.WriteLine("Lista de Estudiantes:");
       foreach (DataRow row in dataTable.Rows)
       {
           Console.WriteLine($"ID: {row["StudentId"]}, Nombre: {row["FirstName"]}, Apellido: {row["LastName"]}");
       }
        #endregion


        #region FormaConectada
        //Datareader
        List<Student> students = ListarStudents();
        foreach (var item in students)
        {
            Console.WriteLine($"ID: {item.StudentId}, Nombre: {item.FirstName}, Apellido: {item.LastName}");
        }
        #endregion
    }

    //De forma desconectada
    private static DataTable ListarStudentsDataTable()
    {
        // Crear un DataTable para almacenar los resultados
        DataTable dataTable = new DataTable();
        // Crear una conexión a la base de datos
        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // Abrir la conexión
            connection.Open();

            // Consulta SQL para seleccionar datos
            string query = "SELECT * FROM Students";

            // Crear un adaptador de datos
            SqlDataAdapter adapter = new SqlDataAdapter(query, connection);



            // Llenar el DataTable con los datos de la consulta
            adapter.Fill(dataTable);

            // Cerrar la conexión
            connection.Close();

        }
        return dataTable;
    }
    //De forma conectada
    private static List<Student> ListarStudents()
    {
        List<Student> students = new List<Student>();

        using (SqlConnection connection = new SqlConnection(connectionString))
        {
            // Abrir la conexión
            connection.Open();

            // Consulta SQL para seleccionar datos
            string query = "SELECT StudentId,FirstName,LastName FROM Students";

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    // Verificar si hay filas
                    if (reader.HasRows)
                    {
                        Console.WriteLine("Lista de Estudiantes:");
                        while (reader.Read())
                        {
                            // Leer los datos de cada fila

                            students.Add(new Student
                            {
                                StudentId = (int)reader["StudentId"],
                                FirstName = reader["FirstName"].ToString(),
                                LastName = reader["LastName"].ToString()
                            });

                        }
                    }
                }
            }
            // Cerrar la conexión
            connection.Close();
        }
        return students;
    }


}