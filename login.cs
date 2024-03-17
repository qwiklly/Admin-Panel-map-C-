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
using WindowsFormsApp2;

namespace WindowsFormsApp1
{
    public partial class login : Form
    {
        DataBase database = new DataBase();

        public login()
        {
            InitializeComponent();
            //in the center of the screen
            StartPosition = FormStartPosition.CenterScreen;
        }
        //пароль длина + вместо символов звездочки
        private void login3_Load(object sender, EventArgs e)
        {
            textBox_password.PasswordChar = '*';
            textBox_login.MaxLength = 50;
            textBox_password.MaxLength = 50;

        }
        private Label label12;
        private readonly CheckUser _user;
        //Логика при нажатии на кнопку вход
        private void button1_Click(object sender, EventArgs e)
        {
            var loginUser = textBox_login.Text;
            var passUser = textBox_password.Text;

            SqlDataAdapter adapter = new SqlDataAdapter();
            DataTable table = new DataTable();

            string querystring = $"select id_user, login_user, password_user,is_admin from register where login_user = '{loginUser}' and password_user = '{passUser}'";

            SqlCommand command = new SqlCommand(querystring, database.GetConnection());

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count == 1)

            {
                var user = new CheckUser(table.Rows[0].ItemArray[1].ToString(), Convert.ToBoolean(table.Rows[0].ItemArray[3]));
                if (user.Status == "admin")
                {
                    Form1 form = new Form1();
                    this.Hide();
                    form.ShowDialog();
                    this.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Вы успешно Вошли,но вы не админ попробуйте еше раз!", "Успешно!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                }
            }


            else
            {
                MessageBox.Show("Такого аккаунта не существует!", "Аккаунта не существует!!", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }

        private void textBox_password_TextChanged(object sender, EventArgs e)
        {

        }
        /*private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
{
sign_up frm_sign = new sign_up();
frm_sign.Show();
this.Hide();
}*/

    }
}
