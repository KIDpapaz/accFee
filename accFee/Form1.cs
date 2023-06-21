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
using MaterialSkin;
using MaterialSkin.Controls;
using MetroFramework.Controls;
using MetroFramework.Forms;

namespace accFee
{
    public partial class Form1 : MetroForm
    {
        public static string Login;
        public static string Admin;
        public static string Name1;
        public static string Name2;
        public static string pass;

        private addSlot addSlot;
        private remOtd remOtd;
        // логин пароль визибл +++(Сделал нормальную фомру ввода)
        // изменнение пароля (первый вход)
        // вкладки(поломал проект 4 раз)
        // дабовление плат и исключением
        // печать статистики птал(отчет за день)

        public Form1()
        {
            InitializeComponent();
            NameInProgLogin();
           /* var materialSkinManager = MaterialSkinManager.Instance;
            materialSkinManager.AddFormToManage(this);
            materialSkinManager.Theme = MaterialSkinManager.Themes.LIGHT;
            materialSkinManager.ColorScheme = new ColorScheme(Primary.BlueGrey800, Primary.BlueGrey900, Primary.BlueGrey500, Accent.LightBlue200, TextShade.WHITE);*/
        }

        //Первоначальные надписи
        public void NameInProgLogin()
        {
            
            materialLabel2.Text = "Логин";
            materialLabel6.Text = "Пароль";
            metroButton2.Text = "Войти";
            metroButton3.Text = "Выйти";
            metroButton4.Text = "Печать статистики";
            metroButton5.Text = "Добавить сотрудника";
            materialCheckBox1.Text = "Ремонт";

            materialLabel1.Text = null;

            materialLabel3.Text = "Добавить плату";
            materialLabel4.Text = "Добавить несколько плат";

            metroLabel1.Text = null;
            materialLabel5.Text = null;

            metroButton1.Text = "Добавить";
            metroButton7.Text = "Добавить";
            metroButton6.Text = "Ремонт";
            
            metroTextBox1.MaxLength = 6;
            metroTextBox2.MaxLength = 6;
            metroTextBox3.MaxLength = 6;
            metroTextBox4.MaxLength = 6;

            metroTextBox2.Enabled = false;
            metroTextBox3.Enabled = false;
            metroTextBox4.Enabled = false;

            metroButton1.Enabled = false;
            metroButton3.Enabled = false;
            metroButton4.Enabled = false;
            metroButton5.Enabled = false;
            metroButton5.Enabled = false;
            metroButton7.Enabled = false;
            metroButton6.Enabled = false;

            materialCheckBox1.Enabled = false;
            
            //
            //v2
            //

            //Кнопка выход
            metroButton3.Visible = false;

            //Меню
            metroButton4.Visible = false;
            metroButton5.Visible = false; 
            metroButton6.Visible = false; 

            //Добавление 1 платы
            materialLabel3.Visible = false;
            metroTextBox2.Visible = false;
            metroButton1.Visible = false;
            materialCheckBox1.Visible = false;

            //Добавление нескольких плат
            materialLabel4.Visible = false;
            metroTextBox3.Visible = false;
            metroTextBox4.Visible = false;
            metroButton7.Visible = false;
        }

        //Вход
        private void metroButton2_Click(object sender, EventArgs e)
        {
            try
            {
                if (metroTextBox1.Text == "" && metroTextBox5.Text == "")
                {
                    MessageBox.Show("Поля логина и пароля пусты");
                    return;
                }
                else if (metroTextBox1.Text == "")
                {
                    MessageBox.Show("Поле логина пустое");
                    return;
                }
                else if (metroTextBox5.Text == "")
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

                Login = metroTextBox1.Text;

                MD5 MD5Hash = MD5.Create(); //создаем объект для работы с MD5
                byte[] inputBytes = Encoding.ASCII.GetBytes(metroTextBox5.Text); //преобразуем строку в массив байтов
                byte[] hash = MD5Hash.ComputeHash(inputBytes); //получаем хэш в виде массива байтов
                pass = Convert.ToBase64String(hash);

                int login = Convert.ToInt32(metroTextBox1.Text);
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

                        metroButton2.Enabled = false;
                        metroButton1.Enabled = true;
                        metroTextBox1.Enabled = false;
                        metroButton3.Enabled = true;
                        metroTextBox2.Enabled = true;
                        metroTextBox3.Enabled = true;
                        metroTextBox4.Enabled = true;
                        metroButton7.Enabled = true;
                        materialCheckBox1.Enabled = true;
                        metroButton6.Enabled = true;
                        metroButton6.Visible = true;
                        metroTextBox5.Text = null;
                        metroTextBox5.Enabled = false;
                        metroTextBox1.Text = null;

                        //
                        //v2
                        //

                        //Вход
                        materialLabel2.Visible = false;
                        materialLabel6.Visible = false;
                        metroButton2.Visible = false;
                        metroTextBox1.Visible = false;
                        metroTextBox5.Visible = false;

                        //Добавление 1 платы
                        materialLabel3.Visible = true;
                        metroTextBox2.Visible = true;
                        metroButton1.Visible = true;
                        materialCheckBox1.Visible = true;

                        //Добавление нескольких плат
                        materialLabel4.Visible = true;
                        metroTextBox3.Visible = true;
                        metroTextBox4.Visible = true;
                        metroButton7.Visible = true;

                        //Кнопка выхода
                        metroButton3.Visible = true;

                        materialLabel1.Text = "Пользователь" + " " + Name2;

                        ///
                        /// Доступ в админку
                        ///
                        int a = Convert.ToInt32(Admin);
                        if (a == 1)//Можно добавлять замов и пользователей и админов
                        {
                            metroButton4.Enabled = true;
                            metroButton5.Enabled = true;
                            metroButton4.Visible = true;
                            metroButton5.Visible = true;
                        }
                        else if (a == 2)//можно добавлять пользователей
                        {
                            metroButton4.Enabled = true;
                            metroButton5.Enabled = true;
                            metroButton4.Visible = true;
                            metroButton5.Visible = true;
                        }
                        else if (a == 3)//работяга
                        {
                            metroButton4.Enabled = false;
                            metroButton5.Enabled = false;
                        }
                        addLog();
                    }
                    else
                    {
                        MessageBox.Show("Неправильный логин");
                    }
                }
                else
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
            materialLabel1.Text = null;

            Admin = null;
            Name1 = null;
            Name2 = null;
            Login = null;

            metroTextBox1.Text = null;
            metroTextBox1.Enabled = true;
            metroButton2.Enabled = true;
            metroButton3.Enabled = false;


            metroButton4.Enabled = false;
            metroButton5.Enabled = false;
            metroButton1.Enabled = false;
            metroButton6.Enabled = false;

            metroTextBox2.Enabled = false;
            metroTextBox3.Enabled = false;
            metroTextBox4.Enabled = false;
            metroButton7.Enabled = false;

            metroTextBox2.Text = null;
            metroTextBox3.Text = null;
            metroTextBox4.Text = null;
            metroTextBox5.Enabled = true;
            materialCheckBox1.Enabled = false;

            if (addSlot != null)
            {
                addSlot.Close();
            }

            if (materialLabel5.Text != null)
            {
                materialLabel5.Text = null;
            }
            
            //Кнопка выход
            metroButton3.Visible = false;

            //Меню
            metroButton4.Visible = false;
            metroButton5.Visible = false; 
            metroButton6.Visible = false; 

            //Добавление 1 платы
            materialLabel3.Visible = false;
            metroTextBox2.Visible = false;
            metroButton1.Visible = false;
            materialCheckBox1.Visible = false;

            //Добавление нескольких плат
            materialLabel4.Visible = false;
            metroTextBox3.Visible = false;
            metroTextBox4.Visible = false;
            metroButton7.Visible = false;

            //Вход
            materialLabel2.Visible = true;
            metroTextBox1.Visible = true;
            materialLabel6.Visible = true;
            metroTextBox5.Visible = true;
            metroButton2.Visible = true;
        }

        //добавление сотрудника (новая форма)
        private void button5_Click(object sender, EventArgs e)
        {
            addSlot = new addSlot();
            addSlot.ShowDialog();
        }

        //добавление платы
        private void metroButton1_Click(object sender, EventArgs e)
        {
            try
            {
                int plata = Convert.ToInt32(metroTextBox2.Text);
                string rem = null;
                if (metroTextBox2.Text.Length != 6)
                {
                    MessageBox.Show("Длинна платы меньше 6");
                    return;
                }
                if (materialCheckBox1.Checked == true)
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
                        metroButton1.BackColor = Color.Red;
                        timer1.Start();
                        timer2.Start();
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
                    command.Parameters.Add("@NomerPlat", MySqlDbType.VarChar).Value = metroTextBox2.Text;
                    command.Parameters.Add("@remont", MySqlDbType.VarChar).Value = rem;
                    command.Parameters.Add("@Date", MySqlDbType.VarChar).Value = dt.ToShortDateString();

                    db.openConnection();


                    if (command.ExecuteNonQuery() == 1)
                    {
                        //Успешно добавленно 
                        materialLabel5.Text = "Успешно.";
                        metroTextBox2.Text = null;
                        metroButton1.BackColor = Color.Green;
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
            command.Parameters.Add("@numPlat", MySqlDbType.VarChar).Value = metroTextBox2.Text;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                int plat = Convert.ToInt32(plata);
                materialLabel5.Text = "Плата уже есть";
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
            metroButton1.BackColor = Color.White;
            timer1.Stop();
        }

        //Таймер label
        private void timer2_Tick(object sender, EventArgs e)
        {
            materialLabel5.Text = null;
            metroLabel1.Text = null;
            timer2.Stop();
        }

        //Отдел ремонта плат
        private void button7_Click(object sender, EventArgs e)
        {
            remOtd = new remOtd();
            remOtd.ShowDialog();
        }

        //Добавить для добавлнения времени в бд кто и когда вошел
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

        //Добавление нескольких плат
        private void metroButton7_Click(object sender, EventArgs e)
        {
            //Число от пользователя
            Console.Write("Введите первую плату: ");//000001
            string num1 = metroTextBox3.Text;
            Console.Write("Введите вторую плату: ");//000005
            string num2 = metroTextBox4.Text;




            int inum1 = Convert.ToInt32(num1);//1
            int inum2 = Convert.ToInt32(num2);//5

            //**********************************
            // Больше ли второе число чем первое
            //**********************************
            if (inum1 < inum2)
            {

                int colplat = (inum2 - inum1) + 1;//Количество плат
                Console.Write("Кол-во плат: ");
                Console.WriteLine(colplat);// вывод плат
                DB db = new DB();
                if (colplat <= 8)//проверка на меньше 8 плат
                {
                    //тут перевод с 000001(int) в 1(string)
                    string strnum1 = Convert.ToString(inum1);

                    string ife = strnum1;
                    int b = inum1 - 1;
                    for (int i = 1; i < colplat + 1;)
                    {
                        string a;
                        string final;
                        if (ife.Length == 1)
                        {
                            string s5 = "00000";
                            a = Convert.ToString(b + i);
                            Console.Write("Числа с 1 по 5: ");
                            final = s5 + a;
                            DateTime dt = DateTime.Now;
                            if (checkPlat(final))
                            {
                                metroLabel1.Text = metroLabel1.Text + "-";
                                timer2.Start();
                            }
                            else
                            {
                                MySqlCommand command1 = new MySqlCommand("INSERT INTO `plats`(`LoginName`, `Name1`, `NomerPlat`, `remont`, `Date`) VALUES (@LoginName, @Name1, @NomerPlat, @remont, @Date);", db.getConnection());
                                command1.Parameters.Add("@LoginName", MySqlDbType.VarChar).Value = Login;
                                command1.Parameters.Add("@Name1", MySqlDbType.VarChar).Value = Name2;
                                command1.Parameters.Add("@NomerPlat", MySqlDbType.VarChar).Value = final;
                                command1.Parameters.Add("@remont", MySqlDbType.VarChar).Value = "Нет";
                                command1.Parameters.Add("@Date", MySqlDbType.VarChar).Value = dt.ToShortDateString();

                                db.openConnection();
                                if (command1.ExecuteNonQuery() == 1)
                                {
                                    //Успешно добавленно 
                                    Console.WriteLine("Нормик");
                                    metroLabel1.Text = metroLabel1.Text + "+";
                                    timer2.Start();
                                }
                                else
                                {
                                    Console.WriteLine("Неа");
                                    //Не добавленно
                                }
                                db.closeConnection();
                                Console.WriteLine(final);
                                
                            }
                            ++i;
                        }
                        else if (ife.Length == 2)
                        {
                            string s4 = "0000";
                            a = Convert.ToString(b + i);
                            Console.Write("Числа с 1 по 5: ");
                            final = s4 + a;
                            if (final.Length == 7)
                            {
                                final = final.Substring(1);
                            }
                            else if (final.Length == 8)
                            {
                                final = final.Substring(2);
                            }
                            else if (final.Length == 9)
                            {
                                final = final.Substring(3);
                            }
                            else if (final.Length == 10)
                            {
                                final = final.Substring(4);
                            }
                            else if (final.Length == 11)
                            {
                                final = final.Substring(5);
                            }
                            else if (final.Length == 12)
                            {
                                final = final.Substring(6);
                            }
                            else if (final.Length == 13)
                            {
                                final = final.Substring(7);
                            }
                            DateTime dt = DateTime.Now;
                            if (checkPlat(final))
                            {
                                metroLabel1.Text = metroLabel1.Text + "-";
                                timer2.Start();
                            }
                            else
                            {
                                MySqlCommand command1 = new MySqlCommand("INSERT INTO `plats`(`LoginName`, `Name1`, `NomerPlat`, `remont`, `Date`) VALUES (@LoginName, @Name1, @NomerPlat, @remont, @Date);", db.getConnection());
                                command1.Parameters.Add("@LoginName", MySqlDbType.VarChar).Value = Login;
                                command1.Parameters.Add("@Name1", MySqlDbType.VarChar).Value = Name2;
                                command1.Parameters.Add("@NomerPlat", MySqlDbType.VarChar).Value = final;
                                command1.Parameters.Add("@remont", MySqlDbType.VarChar).Value = "Нет";
                                command1.Parameters.Add("@Date", MySqlDbType.VarChar).Value = dt.ToShortDateString();

                                db.openConnection();
                                if (command1.ExecuteNonQuery() == 1)
                                {
                                    //Успешно добавленно 
                                    Console.WriteLine("Нормик");
                                    metroLabel1.Text = metroLabel1.Text + "+";
                                    timer2.Start();
                                }
                                else
                                {
                                    Console.WriteLine("Неа");
                                    //Не добавленно
                                }
                                db.closeConnection();
                                Console.WriteLine(final);
                                
                            }
                            ++i;
                        }
                        else if (ife.Length == 3)
                        {
                            string s3 = "000";
                            a = Convert.ToString(b + i);
                            Console.Write("Числа с 1 по 5: ");
                            final = s3 + a;
                            if (final.Length == 7)
                            {
                                final = final.Substring(1);
                            }
                            else if (final.Length == 8)
                            {
                                final = final.Substring(2);
                            }
                            else if (final.Length == 9)
                            {
                                final = final.Substring(3);
                            }
                            else if (final.Length == 10)
                            {
                                final = final.Substring(4);
                            }
                            else if (final.Length == 11)
                            {
                                final = final.Substring(5);
                            }
                            else if (final.Length == 12)
                            {
                                final = final.Substring(6);
                            }
                            else if (final.Length == 13)
                            {
                                final = final.Substring(7);
                            }
                            DateTime dt = DateTime.Now;
                            if (checkPlat(final))
                            {
                                metroLabel1.Text = metroLabel1.Text + "-";
                                timer2.Start();
                            }
                            else
                            {
                                MySqlCommand command1 = new MySqlCommand("INSERT INTO `plats`(`LoginName`, `Name1`, `NomerPlat`, `remont`, `Date`) VALUES (@LoginName, @Name1, @NomerPlat, @remont, @Date);", db.getConnection());
                                command1.Parameters.Add("@LoginName", MySqlDbType.VarChar).Value = Login;
                                command1.Parameters.Add("@Name1", MySqlDbType.VarChar).Value = Name2;
                                command1.Parameters.Add("@NomerPlat", MySqlDbType.VarChar).Value = final;
                                command1.Parameters.Add("@remont", MySqlDbType.VarChar).Value = "Нет";
                                command1.Parameters.Add("@Date", MySqlDbType.VarChar).Value = dt.ToShortDateString();

                                db.openConnection();
                                if (command1.ExecuteNonQuery() == 1)
                                {
                                    //Успешно добавленно 
                                    Console.WriteLine("Нормик");
                                    metroLabel1.Text = metroLabel1.Text + "+";
                                    timer2.Start();
                                }
                                else
                                {
                                    Console.WriteLine("Неа");
                                    //Не добавленно
                                }
                                db.closeConnection();
                                Console.WriteLine(final);
                                
                            }
                            ++i;
                        }
                        else if (ife.Length == 4)
                        {
                            string s2 = "00";
                            a = Convert.ToString(b + i);
                            Console.Write("Числа с 1 по 5: ");
                            final = s2 + a;
                            if (final.Length == 7)
                            {
                                final = final.Substring(1);
                            }
                            else if (final.Length == 8)
                            {
                                final = final.Substring(2);
                            }
                            else if (final.Length == 9)
                            {
                                final = final.Substring(3);
                            }
                            else if (final.Length == 10)
                            {
                                final = final.Substring(4);
                            }
                            else if (final.Length == 11)
                            {
                                final = final.Substring(5);
                            }
                            else if (final.Length == 12)
                            {
                                final = final.Substring(6);
                            }
                            else if (final.Length == 13)
                            {
                                final = final.Substring(7);
                            }
                            DateTime dt = DateTime.Now;
                            if (checkPlat(final))
                            {
                                metroLabel1.Text = metroLabel1.Text + "-";
                                timer2.Start();
                            }
                            else
                            {
                                MySqlCommand command1 = new MySqlCommand("INSERT INTO `plats`(`LoginName`, `Name1`, `NomerPlat`, `remont`, `Date`) VALUES (@LoginName, @Name1, @NomerPlat, @remont, @Date);", db.getConnection());
                                command1.Parameters.Add("@LoginName", MySqlDbType.VarChar).Value = Login;
                                command1.Parameters.Add("@Name1", MySqlDbType.VarChar).Value = Name2;
                                command1.Parameters.Add("@NomerPlat", MySqlDbType.VarChar).Value = final;
                                command1.Parameters.Add("@remont", MySqlDbType.VarChar).Value = "Нет";
                                command1.Parameters.Add("@Date", MySqlDbType.VarChar).Value = dt.ToShortDateString();
                                db.openConnection();


                                if (command1.ExecuteNonQuery() == 1)
                                {
                                    //Успешно добавленно 
                                    Console.WriteLine("Нормик");
                                    metroLabel1.Text = metroLabel1.Text + "+";
                                    timer2.Start();
                                }
                                else
                                {
                                    Console.WriteLine("Неа");
                                    //Не добавленно
                                }
                                db.closeConnection();
                                Console.WriteLine(final);
                                
                            }
                            ++i;
                        }
                        else if (ife.Length == 5)
                        {
                            string s1 = "0";
                            a = Convert.ToString(b + i);
                            Console.Write("Числа с 1 по 5: ");
                            final = s1 + a;
                            if (final.Length == 7)
                            {
                                final = final.Substring(1);
                            }
                            else if (final.Length == 8)
                            {
                                final = final.Substring(2);
                            }
                            else if (final.Length == 9)
                            {
                                final = final.Substring(3);
                            }
                            else if (final.Length == 10)
                            {
                                final = final.Substring(4);
                            }
                            else if (final.Length == 11)
                            {
                                final = final.Substring(5);
                            }
                            else if (final.Length == 12)
                            {
                                final = final.Substring(6);
                            }
                            else if (final.Length == 13)
                            {
                                final = final.Substring(7);
                            }
                            DateTime dt = DateTime.Now;
                            if (checkPlat(final))
                            {
                                metroLabel1.Text = metroLabel1.Text + "-";
                                timer2.Start();
                            }
                            else
                            {
                                MySqlCommand command1 = new MySqlCommand("INSERT INTO `plats`(`LoginName`, `Name1`, `NomerPlat`, `remont`, `Date`) VALUES (@LoginName, @Name1, @NomerPlat, @remont, @Date);", db.getConnection());
                                command1.Parameters.Add("@LoginName", MySqlDbType.VarChar).Value = Login;
                                command1.Parameters.Add("@Name1", MySqlDbType.VarChar).Value = Name2;
                                command1.Parameters.Add("@NomerPlat", MySqlDbType.VarChar).Value = final;
                                command1.Parameters.Add("@remont", MySqlDbType.VarChar).Value = "Нет";
                                command1.Parameters.Add("@Date", MySqlDbType.VarChar).Value = dt.ToShortDateString();
                                db.openConnection();


                                if (command1.ExecuteNonQuery() == 1)
                                {
                                    //Успешно добавленно 
                                    Console.WriteLine("Нормик");
                                    metroLabel1.Text = metroLabel1.Text + "+";
                                    timer2.Start();
                                }
                                else
                                {
                                    Console.WriteLine("Неа");
                                    //Не добавленно
                                }
                                db.closeConnection();
                                Console.WriteLine(final);
                                
                            }
                            ++i;
                        }
                        else if (ife.Length == 6)
                        {
                            a = Convert.ToString(b + i);
                            Console.Write("Числа с 1 по 5: ");
                            final = a;
                            if (final.Length == 7)
                            {
                                final = final.Substring(1);
                            }
                            else if (final.Length == 8)
                            {
                                final = final.Substring(2);
                            }
                            else if (final.Length == 9)
                            {
                                final = final.Substring(3);
                            }
                            else if (final.Length == 10)
                            {
                                final = final.Substring(4);
                            }
                            else if (final.Length == 11)
                            {
                                final = final.Substring(5);
                            }
                            else if (final.Length == 12)
                            {
                                final = final.Substring(6);
                            }
                            else if (final.Length == 13)
                            {
                                final = final.Substring(7);
                            }

                            DateTime dt = DateTime.Now;
                            if (checkPlat(final))
                            {
                                metroLabel1.Text = metroLabel1.Text + "-";
                                timer2.Start();
                            }
                            else
                            {
                                MySqlCommand command1 = new MySqlCommand("INSERT INTO `plats`(`LoginName`, `Name1`, `NomerPlat`, `remont`, `Date`) VALUES (@LoginName, @Name1, @NomerPlat, @remont, @Date);", db.getConnection());
                                command1.Parameters.Add("@LoginName", MySqlDbType.VarChar).Value = Login;
                                command1.Parameters.Add("@Name1", MySqlDbType.VarChar).Value = Name2;
                                command1.Parameters.Add("@NomerPlat", MySqlDbType.VarChar).Value = final;
                                command1.Parameters.Add("@remont", MySqlDbType.VarChar).Value = "Нет";
                                command1.Parameters.Add("@Date", MySqlDbType.VarChar).Value = dt.ToShortDateString();
                                db.openConnection();


                                if (command1.ExecuteNonQuery() == 1)
                                {
                                    //Успешно добавленно 
                                    Console.WriteLine("Нормик");
                                    metroLabel1.Text = metroLabel1.Text + "+";
                                    timer2.Start();
                                }
                                else
                                {
                                    Console.WriteLine("Неа");
                                    //Не добавленно
                                }
                                db.closeConnection();
                                Console.WriteLine(final);
                                
                            }
                            ++i;
                        }
                        else
                        {
                            Console.WriteLine("Число больше 6 чисел");
                            break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Нельзя внести полее 8 плат, Я думаю вы ошиблись");
                }
                metroTextBox3.Text = null;
                metroTextBox4.Text = null;
            }
            else
            {
                Console.WriteLine("Первое больше второго");
            }
        }

        //Чекает плату
        public Boolean checkPlat(string Num)
        {
            DB dB = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            MySqlCommand command = new MySqlCommand("SELECT * FROM `plats` WHERE `NomerPlat` = @qq", dB.getConnection());
            command.Parameters.Add("@qq", MySqlDbType.VarChar).Value = Num;

            adapter.SelectCommand = command;
            adapter.Fill(table);

            if (table.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}