 using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace accFee
{
    public partial class Form1 : Form
    {
        public static string Login;
        public static string Admin;
        public static string Name1;
        public static string Name2;

        public static string numPlat;

        private addSlot addSlot;


        public Form1()
        {
            InitializeComponent();
            NameInProgLogin();
        }

        //Первоначальные надписи
        public void NameInProgLogin()
        {
            label1.Text = "Логин";
            button2.Text = "Войти";
            button3.Text = "Выйти";
            button4.Text = "Печать статистики";
            button5.Text = "Добавить сотрудника";
            checkBox1.Text = "Ремонт";

            label2.Text = null;

            label3.Text = "Добавление одной платны";
            label4.Text = "Добаление нескольких плат";

            label5.Text = null;

            button1.Text = "Добавить";
            button6.Text = "Добавить несколько";

            textBox1.MaxLength = 6;
            textBox2.MaxLength = 6;
            textBox3.MaxLength = 6;
            textBox4.MaxLength = 6;

            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;

            button1.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
            button5.Enabled = false;
            button6.Enabled = false;

            checkBox1.Enabled = false;
        }

        //Вход
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                Login = textBox1.Text;
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
                        textBox2.Enabled = true;
                        textBox3.Enabled = true;
                        textBox4.Enabled = true;
                        button6.Enabled = true;
                        checkBox1.Enabled = true;

                        label2.Text = "Пользователь" + " " + Name2;

                        ///
                        /// Доступ в админку
                        ///
                        int a = Convert.ToInt32(Admin);
                        if (a == 1)//Можно добавлять замов и пользователей и админов
                        {
                            button4.Enabled = true;
                            button5.Enabled = true;
                        }
                        else if (a == 2)//можно добавлять пользователей
                        {
                            button4.Enabled = true;
                            button5.Enabled = true;
                        }
                        else if (a == 3)//работяга
                        {
                            button4.Enabled = false;
                            button5.Enabled = false;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Неправильный логин");
                    }
                }
                else if (textBox1.TextLength < 6)
                {
                    MessageBox.Show("Меньше 6");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("В логине должны присутствовоть только цифры ну либо нету соединения");
                //Добавить цифры
                //на буквы
                //и связь
            }
        }

        //Выход
        private void button3_Click(object sender, EventArgs e)
        {
            label2.Text = null;
            Admin = null;
            Name1 = null;
            Name2 = null;
            Login = null;

            textBox1.Text = null;
            textBox1.Enabled = true;
            button2.Enabled = true;
            button3.Enabled = false;

            button4.Enabled = false;
            button5.Enabled = false;
            button1.Enabled = false;

            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            button6.Enabled = false;

            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;

            if (addSlot != null)
            {
                addSlot.Close();
            }

            if (label5.Text != null)
            {
                label5.Text = null;
            }
        }

        
        //добавление сотрудника
        private void button5_Click(object sender, EventArgs e)
        {
            addSlot = new addSlot();
            addSlot.ShowDialog();
        }

        //добавление платы
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int plata = Convert.ToInt32(textBox2.Text);
                string rem = null;
                numPlat = textBox2.Text;

                if (checkBox1.Checked == true)
                {
                    rem = "На ремонте";
                }
                else
                {
                    rem = "Нет";
                }

                if (plata >= 0)
                {
                    if (checkPlat(plata))
                    {
                        ///
                        ///Добавить красный прогресс бар
                        ///
                        return;
                    }
                    ///
                    /// Создание даты
                    ///
                    DateTime dt = DateTime.Now;         

                    ///
                    /// Работы с БД
                    ///
                    DB db = new DB();
                    MySqlCommand command = new MySqlCommand("INSERT INTO `plats`(`LoginName`, `Name1`, `NomerPlat`, `remont`, `Date`) VALUES (@LoginName, @Name1, @NomerPlat, @remont, @Date);", db.getConnection());

                    command.Parameters.Add("@LoginName", MySqlDbType.VarChar).Value = Login;
                    command.Parameters.Add("@Name1", MySqlDbType.VarChar).Value = Name2;
                    command.Parameters.Add("@NomerPlat", MySqlDbType.VarChar).Value = textBox2.Text;
                    command.Parameters.Add("@remont", MySqlDbType.VarChar).Value = rem;
                    command.Parameters.Add("@Date", MySqlDbType.VarChar).Value = dt.ToShortDateString();

                    db.openConnection();


                    if (command.ExecuteNonQuery() == 1)
                    {
                        //Успешно добавленно 
                        label5.Text = "Плата с номер " + plata + " была УСПЕШНО добавлена.";
                    }
                    else
                    {
                        MessageBox.Show("Нет и не знаю почему это тут просто есть!");
                        //Не добавленно
                    }
                    db.closeConnection();
                }
                else
                {
                    MessageBox.Show("Вводите только цифры");
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Не вводите буквы и точки ГДЕ ДОЛЖНЫ БЫТЬ ЦИФРЫ");
            }
        }

        //Штука для того что бы узнать есть ли плата(Добавлена ли плата)
        public Boolean checkPlat(int plata)
        {
            DB dB = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `plats` WHERE `NomerPlat` = @numPlat", dB.getConnection());
            command.Parameters.Add("@numPlat", MySqlDbType.VarChar).Value = numPlat;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                int plat = Convert.ToInt32(plata);
                label5.Text = "Плата с номер" + " " + plat + " не была добавленна, такая плата уже есть.";
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}