using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace accFee
{
    public partial class Form1 : Form
    {
        public string Admin;
        public string Name2;
        public Form1()
        {
            InitializeComponent();
            NameInProgLogin();
        }

        public void NameInProgLogin()
        {
            label1.Text = "Логин";
            button2.Text = "Войти";
            button3.Text = "Выйти";
            button4.Text = "Печать статистики";
            button5.Text = "Добавить сотрудника";

            label2.Text = null;

            button1.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
        }

        //Вход
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                int login = Convert.ToInt32(textBox1.Text);
                if (login > 0)
                {
                    DB db = new DB();
                    DataTable table = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `LoginName` = @uL", db.getConnection());

                    command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = login;
                    adapter.SelectCommand = command;

                    adapter.Fill(table);
                    if (table.Rows.Count > 0)
                    {
                        db.openConnection();
                        MySqlCommand commandName2 = new MySqlCommand("SELECT Name2 FROM users WHERE loginName LIKE @name;", db.getConnection());
                        commandName2.Parameters.Add("@name", MySqlDbType.VarChar).Value = login;
                        Name2 = Convert.ToString(commandName2.ExecuteScalar());

                        MySqlCommand commandAdmin = new MySqlCommand("SELECT Admin FROM users WHERE loginName LIKE @admin;", db.getConnection());
                        commandAdmin.Parameters.Add("@admin", MySqlDbType.VarChar).Value = login;
                        Admin = Convert.ToString(commandAdmin.ExecuteScalar());
                        db.closeConnection();

                        button2.Enabled = false;
                        button1.Enabled = true;
                        textBox1.Enabled = false;
                        button3.Enabled = true;

                        label2.Text = "Пользователь" + " " + Name2;
                        AdminPanel();
                    }
                    else
                    {
                        MessageBox.Show("Неправильный логин");
                    }
                }
            }
            catch (Exception)
            {
                MessageBox.Show("В логине должны присутствовоть только цифры");
            }

        }

        //Выход
        private void button3_Click(object sender, EventArgs e)
        {
            label2.Text = null;
            Admin = null;
            Name2 = null;
            
            button1.Enabled = false;
            button3.Enabled = false;
            button2.Enabled = true;
            textBox1.Text = null;
            textBox1.Enabled = true;
        }

        //Доступ в админ панель
        public void AdminPanel()
        {
            int a = Convert.ToInt32(Admin);
            if (a == 1)//Можно добавлять замов и пользователей и админов
            {
                button4.Enabled = true;
                button5.Enabled = true;
            }
            else if(a == 2)//можно добавлять пользователей
            {
                button4.Enabled = true;
                button5.Enabled = true;
            }
            else if(a == 3)//работяга
            {
                button4.Enabled = false;
                button5.Enabled = false;
            }
        }
    }
}