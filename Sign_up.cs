using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Net.Http;
using System.Security.Principal;

namespace WindowsFormsApp1
{
    
    public partial class Sign_up : Form
    {


        readonly DataBase database = new DataBase();
        public Sign_up()
        {
            InitializeComponent();
            StartPosition = FormStartPosition.CenterScreen;
        }
        //Пароль - длина+звездочки
        private void Register1_Load(object sender, EventArgs e)
        {
            
                textBox_password2.PasswordChar = '*';
                textBox_login2.MaxLength = 50;
                textBox_password2.MaxLength = 50;

        }
        //Кнопка регистрация + логика
        private void button1_Click(object sender, EventArgs e)
        {
            
            if (checkUser())

            {
                return;
            }
            

            var login = textBox_login2.Text;
            var password = textBox_password2.Text;
           
            string querystring = $"insert into register(login_user, password_user,is_admin) values('{login}', '{password}', 0)";

            SqlCommand command = new SqlCommand(querystring, database.GetConnection());

            database.openConnection();

            if (command.ExecuteNonQuery() == 1 && login.Length >= 4 && password.Length >= 6 )
            {
                MessageBox.Show("Аккаунт успешно создан!", "Успешно!");
                panel_of_admin frm_login = new panel_of_admin();
                this.Hide();
                frm_login.ShowDialog();
                
                Close();
            }


            else
            {
                MessageBox.Show("Аккаунт не создан! - Проверьте длину пароля или логина, а также убедитесь, что аккаунт ещё не существует");

            }
            database.closeConnection();
        }
        
        //Проверка зарегистрирован ли человек
        private Boolean checkUser()
        {
            var loginUser = textBox_login2.Text;
            var passUser = textBox_password2.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();
            string querystring = $"select id_user, login_user, password_user, is_admin from register where login_user = '{loginUser}' and password_user = '{passUser}'";

            SqlCommand command = new SqlCommand(querystring, database.GetConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)

            {
                MessageBox.Show("Аккаунт существует!");
                return true;


            }
            else
            {
                return false;

            }
        }
        /*

        private void button2_Click(object sender, EventArgs e)
        {
            textBox_login2.Text = "";
            textBox_password2.Text = "";
        }*/
    }

}
