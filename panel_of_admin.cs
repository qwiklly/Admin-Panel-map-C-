using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp1
{
    public partial class panel_of_admin : Form
    {
        readonly DataBase database = new DataBase();
        public panel_of_admin()
        {
            InitializeComponent();
            

        }

        private void panel_of_admin_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            CreateColums();
            RefreshDataGrid();

        }
        //создание полей с айти логином паролем и тд
        private void CreateColums()
        {
            dataGridView1.Columns.Add("id_user", "ID");
            dataGridView1.Columns.Add("Login", "login");
            dataGridView1.Columns.Add("Password", "password");
            var checkColumn = new DataGridViewCheckBoxColumn();
            checkColumn.HeaderText = "IsAdmin";
            dataGridView1.Columns.Add(checkColumn);
        }
        //Считывание данных строки
        private void ReadSingleRow(IDataRecord record)
        {
            dataGridView1.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), record.GetBoolean(3));
        }
        //Обновление даныных
        private void RefreshDataGrid()
        {
            dataGridView1.Rows.Clear();

            string queryString = $"SELECT * FROM register";

            SqlCommand command = new SqlCommand(queryString, database.GetConnection());

            database.openConnection();

            SqlDataReader reader = command.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(reader);
            }
            reader.Close();
            database.closeConnection();

        }
        



        
        //button save
        private void button7_Click(object sender, EventArgs e)
        {
            database.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                var isadmin = dataGridView1.Rows[index].Cells[3].Value.ToString();

                var changeQuery = $"UPDATE register SET is_admin = '{isadmin}' WHERE id_user = '{id}'";

                var command = new SqlCommand(changeQuery, database.GetConnection());
                command.ExecuteNonQuery();
            }
            database.closeConnection();

            RefreshDataGrid();
        }
        //button delete
        private void button8_Click(object sender, EventArgs e)
        {

            database.openConnection();

            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var id = Convert.ToInt32(dataGridView1.Rows[selectedRowIndex].Cells[0].Value);
            var isAdmin = Convert.ToBoolean(dataGridView1.Rows[selectedRowIndex].Cells[3].Value);

            // Проверяем, является ли пользователь администратором
            if (isAdmin)
            {
                MessageBox.Show("Нельзя удалить администратора!");
            }
            else
            {
                var deleteQuery = $"DELETE FROM register WHERE id_user = '{id}'";

                var command = new SqlCommand(deleteQuery, database.GetConnection());
                command.ExecuteNonQuery();

                database.closeConnection();
                RefreshDataGrid();
            }
        }
        //Переход на вкладку с картой
        private void button1_Click_1(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            this.Hide();
            form1.ShowDialog();
            this.Show();
            Close();
        }

        

        //add user
        private void button6_Click(object sender, EventArgs e)
        {
            
            Sign_up form1 = new Sign_up();
            this.Hide();
            form1.ShowDialog();
            this.Show();
            Close();
        }


    }
}
