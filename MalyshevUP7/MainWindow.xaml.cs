using System;
using System.Collections.Generic;
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
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data;

namespace MalyshevUP7
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public static string CurTable;
        public MainWindow()
        {
            InitializeComponent();
        }


        private void JuryButton_Click(object sender, RoutedEventArgs e)
        {
            CurTable = "Jury";
            SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=MalyshevImport;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            con.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM Jury", con);
            SqlDataReader reader = com.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            DataView.ItemsSource = dt.DefaultView;
            con.Close();
            con.Dispose();
        }

        private void AllButton_Click(object sender, RoutedEventArgs e)
        {
            CurTable = "AllMembers";
            SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=MalyshevImport;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            con.Open();
            SqlCommand com = new SqlCommand("SELECT * FROM AllMembers", con);
            SqlDataReader reader = com.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(reader);
            DataView.ItemsSource = dt.DefaultView;
            con.Close();
            con.Dispose();
        }

        private void DataView_AddingNewItem(object sender, AddingNewItemEventArgs e)
        {
            SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=MalyshevImport;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            con.Open();
            SqlDataAdapter adapter = new SqlDataAdapter();
            SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
            DataTable dt = DataView.ItemsSource as DataTable;
            adapter.Update(dt);
            con.Close();
            con.Dispose();
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection con = new SqlConnection("Data Source=(LocalDB)\\MSSQLLocalDB;Initial Catalog=MalyshevImport;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
                con.Open();
                SqlCommand com = new SqlCommand($"DELETE FROM {CurTable}\r\nDBCC CHECKIDENT ({CurTable}, RESEED, -1)", con);
                com.ExecuteNonQuery();
                string SCcom = $"SELECT * FROM {CurTable}";
                SqlDataAdapter adapter = new SqlDataAdapter(SCcom, con);
                SqlCommandBuilder builder = new SqlCommandBuilder(adapter);
                DataTable dt = new DataTable();
                dt.TableName = CurTable;
                dt = ((DataView)DataView.ItemsSource).ToTable();
                adapter.Update(dt);
                dt.Clear();
                adapter.Fill(dt);
                DataView.ItemsSource = dt.DefaultView;
                con.Close();
                con.Dispose();
            }
            catch (System.NullReferenceException)
            {
                MessageBox.Show("Сначала загрузите таблицу", "Внимание!", MessageBoxButton.OK);
            }
        }
    }
}
