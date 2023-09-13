using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private string connectionString = "Data Source=LAB1504-05\\SQLEXPRESS;Initial Catalog=TecsupDB;User ID=alumnoTecsup;Password=123456";
        private List<Student> studentsList;
        public MainWindow()
        {
            InitializeComponent();
            InitializeDatabaseConnection();
        }

        private void InitializeDatabaseConnection()
        {
            studentsList = new List<Student>();

            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("SELECT * FROM Students", connection))
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            studentsList.Add(new Student
                            {
                                StudentId = reader.GetInt32(0),
                                FirstName = reader.GetString(1),
                                LastName = reader.GetString(2)
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al conectar a la base de datos: " + ex.Message);
            }

            studentsDataGrid.ItemsSource = studentsList;
        }

        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            string searchTerm = searchTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(searchTerm))
            {
                List<Student> filteredList = studentsList.FindAll(student =>
                    student.FirstName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    student.LastName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)
                );
                studentsDataGrid.ItemsSource = filteredList;
            }
            else
            {
                studentsDataGrid.ItemsSource = studentsList;
            }
        }
    }

    public class Student
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

}