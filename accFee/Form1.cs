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
using System.Security.Cryptography;

namespace accFee
{
    public partial class Form1 : Form
    {
        public static string Login;
        public static string Admin;
        public static string Name1;
        public static string Name2;
        public static string pass;

        private addSlot addSlot;
        private remOtd remOtd;
        //Добавление примичания в ремонте
        //Пароль хешировать в md5 либо sha256 +++
        //Если есть плата очищать, и наоборот +++
        //логин пустое+++
        //пароль пустое

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
            button7.Text = "Отдел плат для ремонта";

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
            button7.Enabled = false;

            checkBox1.Enabled = false; 
        }

        //Вход
        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                if (textBox1.Text == "" && textBox5.Text == "")
                {
                    MessageBox.Show("Поля логина и пароля пусты");
                    return;
                }
                else if (textBox1.Text == "")
                {
                    MessageBox.Show("Поле логина пустое");
                    return;
                }
                else if (textBox5.Text == "")
                {
                    MessageBox.Show("Поле пароля пустое");
                    return;
                }
                else
                {
                    ///
                    ///
                    ///
                }

                Login = textBox1.Text;

                MD5 MD5Hash = MD5.Create(); //создаем объект для работы с MD5
                byte[] inputBytes = Encoding.ASCII.GetBytes(textBox5.Text); //преобразуем строку в массив байтов
                byte[] hash = MD5Hash.ComputeHash(inputBytes); //получаем хэш в виде массива байтов
                pass = Convert.ToBase64String(hash);

                int login = Convert.ToInt32(textBox1.Text);
                if (login > 0)
                {
                    DB db = new DB();
                    DataTable table = new DataTable();
                    MySqlDataAdapter adapter = new MySqlDataAdapter();
                    MySqlCommand command = new MySqlCommand("SELECT * FROM `users` WHERE `LoginName` = @uL", db.getConnection());

/*                    MySqlCommand commandPass = new MySqlCommand("SELECT * FROM `users` WHERE `password` = @aL", db.getConnection());

                    commandPass.Parameters.Add("@uL", MySqlDbType.VarChar).Value = passs;

                    ///Добавить сравнение и выход*/

                    command.Parameters.Add("@uL", MySqlDbType.VarChar).Value = login;
                    adapter.SelectCommand = command;

                    adapter.Fill(table);
                    if (table.Rows.Count > 0)
                    {
                        db.openConnection();

                        //Тут берёт фамилию
                        MySqlCommand commandName2 = new MySqlCommand("SELECT Name2 FROM users WHERE loginName LIKE @name;", db.getConnection());
                        commandName2.Parameters.Add("@name", MySqlDbType.VarChar).Value = login;
                        Name2 = Convert.ToString(commandName2.ExecuteScalar());

                        //тут пароль                                 SELECT `password` FROM `users` WHERE `Name2` = 12412412;
                        //                                           SELECT `password` FROM `users` WHERE `LoginName` = 100000;
                        MySqlCommand commandPass = new MySqlCommand("SELECT `password` FROM `users` WHERE `LoginName` = @uL", db.getConnection());
                        commandPass.Parameters.Add("@uL", MySqlDbType.VarChar).Value = Login;
                        string passs = Convert.ToString(commandPass.ExecuteScalar());

                        
                        if (pass != passs)
                        {
                            MessageBox.Show("Не верный пароль/логин");
                            return;
                        }
                        else if(pass == passs)
                        {
                                //Правильный пароль
                        }

                        //Тут степень админа
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
                        button7.Enabled = true;
                        textBox5.Text = null;
                        textBox5.Enabled = false;
                        textBox1.Text = null;

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
                        addLog();
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
                // + в таблицу
            }
            catch (Exception)
            {
                MessageBox.Show("Пользователя не существует");
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
            button7.Enabled = false;

            textBox2.Enabled = false;
            textBox3.Enabled = false;
            textBox4.Enabled = false;
            button6.Enabled = false;

            textBox2.Text = null;
            textBox3.Text = null;
            textBox4.Text = null;
            textBox5.Enabled = true;
            checkBox1.Enabled = false;

            if (addSlot != null)
            {
                addSlot.Close();
            }

            if (label5.Text != null)
            {
                label5.Text = null;
            }
        }

        //добавление сотрудника (новая форма)
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
                if (textBox2.Text.Length != 6)
                {
                    MessageBox.Show("Длинна платы меньше 6");
                    return;
                }
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
                        button1.BackColor = Color.Red;
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
                        label5.Text = "Плата " + textBox2.Text + " была УСПЕШНО добавлена.";
                        textBox2.Text = null;
                        button1.BackColor = Color.Green;
                        timer1.Start();
                        timer2.Start();
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
                MessageBox.Show("Не вводите буквы и точки ГДЕ ДОЛЖНЫ БЫТЬ ЦИФРЫ ну или пробел");
            }
        }

        //Штука для того что бы узнать есть ли плата(Добавлена ли плата)
        public Boolean checkPlat(int plata)
        {
            DB dB = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `plats` WHERE `NomerPlat` = @numPlat", dB.getConnection());
            command.Parameters.Add("@numPlat", MySqlDbType.VarChar).Value = textBox2.Text;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                int plat = Convert.ToInt32(plata);
                label5.Text = "Плата" + " " + plat + " уже есть";
                return true;
            }
            else
            {
                return false;
            }
        }

        //Таймер кнопки
        private void timer1_Tick(object sender, EventArgs e)
        {
            //ВЫПОЛНЯЕТСЯ В ИНТЕРВАЛЕ 5сек
            button1.BackColor = Color.White;
            timer1.Stop();
        }

        //Таймер label
        private void timer2_Tick(object sender, EventArgs e)
        {
            label5.Text = null;
            timer2.Stop();
        }

        //Отдел ремонта плат
        private void button7_Click(object sender, EventArgs e)
        {
            remOtd = new remOtd();
            remOtd.ShowDialog();
        }

        ///
        /// Добавить добавление плат с n по n+7
        ///


        /// Добавить для добавлнения времени в бд кто и когда вошел
        public void addLog()
        {
            try
            {
                DateTime dt = DateTime.Now;
                DB db = new DB();
                MySqlCommand command = new MySqlCommand("INSERT INTO `log`(`LoginName`, `Name2`, `NamePC`, `Date`) VALUES (@LoginName, @Name2, @NamePC, @Date);", db.getConnection());

                command.Parameters.Add("@LoginName", MySqlDbType.VarChar).Value = Login;
                command.Parameters.Add("@Name2", MySqlDbType.VarChar).Value = Name2;
                command.Parameters.Add("@NamePC", MySqlDbType.VarChar).Value = Environment.MachineName;
                command.Parameters.Add("@Date", MySqlDbType.VarChar).Value = dt.ToShortDateString();

                db.openConnection();


                if (command.ExecuteNonQuery() == 1)
                {
                    
                }
                else
                {
                    MessageBox.Show("Нету подключение к БД");
                }
                db.closeConnection();
            }
            catch (Exception)
            {

                
            }
        }
    }
}