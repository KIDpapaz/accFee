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
    public partial class addSlot : Form
    {
        public string Admin;
        public string Name1;
        public string Name2;
        public addSlot()
        {
            InitializeComponent();
            Info();
            NameInProgAddSlot();
        }

        //Первоначальные надписи
        public void NameInProgAddSlot()
        {
            label1.Text = "Имя";
            label2.Text = "Фамилия";
            label3.Text = "Отчество";
            label4.Text = "Степень рабочего в программе";
            label5.Text = "Логин(Табельный)";
            label6.Text = "Должность";

            button1.Text = "Добавить";

            label7.Text = null;


            /// Выесняется какой пользователь для добавления на combobox1
            if (Convert.ToInt32(Admin) == 1)
            {
                comboBox1.Items.Add("Главный администратор");
                comboBox1.Items.Add("Администратор");
                comboBox1.Items.Add("Рабочий");
            }
            else if (Convert.ToInt32(Admin) == 2)
            {
                comboBox1.Items.Add("Рабочий");
            }
            else if (Convert.ToInt32(Admin) == 3)
            {
                MessageBox.Show("Ты кто воин?!");
            }
        }
        //Данные о пользователе
        public void Info()
        {
            Admin = Form1.Admin;
            Name1 = Form1.Name1;
            Name2 = Form1.Name2;
        }
        
        //Добавление пользователя
        private void button1_Click(object sender, EventArgs e)
        {

            string Name1New = textBox1.Text;
            string Name2New = textBox2.Text;
            string Name3New = textBox3.Text;
            string AdminNew = null;
            if (comboBox1.SelectedItem != null)
            {
                AdminNew = comboBox1.SelectedItem.ToString();
            }
            string LoginNameNew = textBox5.Text;
            string DolshnostNew = textBox6.Text;

            if (Name1New == null || Name1New == "" && Name2New == null || Name2New == "" && Name3New == null || Name3New == "" && AdminNew == null || AdminNew == "" && LoginNameNew == null || LoginNameNew == "" && DolshnostNew == null || DolshnostNew == "")
            {
                //Пустой ввод строки
                Check_new_user(Name1New, Name2New, Name3New, AdminNew, LoginNameNew, DolshnostNew);
            }
            else
            {
                //Если всё введено верно и тд

                if (AdminNew == "Главный администратор")
                {
                    AdminNew = "1";
                }
                else if (AdminNew == "Администратор")
                {
                    AdminNew = "2";
                }
                else if (AdminNew == "Рабочий")
                {
                    AdminNew = "3";
                }

                DB db = new DB();
                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();
                MySqlCommand command = new MySqlCommand("INSERT INTO `users` (`Name1`, `Name2`, `Name3`, `Admin`, `LoginName`, `Dolshnost`) VALUES(@N1, @N2, @N3, @Admin, @LN, @Dol)", db.getConnection());

                command.Parameters.Add("@N1", MySqlDbType.VarChar).Value = Name1New;
                command.Parameters.Add("@N2", MySqlDbType.VarChar).Value = Name2New;
                command.Parameters.Add("@N3", MySqlDbType.VarChar).Value = Name3New;
                command.Parameters.Add("@Admin", MySqlDbType.VarChar).Value = AdminNew;
                command.Parameters.Add("@LN", MySqlDbType.VarChar).Value = LoginNameNew;
                command.Parameters.Add("@DOL", MySqlDbType.VarChar).Value = DolshnostNew;
                adapter.SelectCommand = command;


                //возможно нужная штука
                //adapter.Fill(table); 
                db.openConnection();

                if (command.ExecuteNonQuery() == 1)
                {
                    textBox1.Text = null;
                    textBox2.Text = null;
                    textBox3.Text = null;
                    textBox5.Text = null;
                    textBox6.Text = null;

                    MessageBox.Show("Новый пользователь добавлен");
                }
                else
                {
                    MessageBox.Show("Пользователь не добавлен");
                }

                db.closeConnection();
            }
        }

        //Что не ввёл пользователь
        public void Check_new_user(string Name1New, string Name2New, string Name3New, string AdminNew, string LoginNameNew, string DolshnostNew)
        {
            if (Name1New == null || Name1New == "")
            {
                MessageBox.Show("Введите имя пользователя");
            }
            else if (Name2New == null || Name2New == "")
            {
                MessageBox.Show("Введите фамилию пользователя");
            }
            else if (Name3New == null || Name3New == "")
            {
                MessageBox.Show("Введите отчество пользователя");
            }
            else if (AdminNew == null || AdminNew == "")
            {
                MessageBox.Show("Вы не выбрали степень доступа к программе");
            }
            else if (LoginNameNew == null || LoginNameNew == "")
            {
                MessageBox.Show("Вы не ввели табельный");
            }
            else if (DolshnostNew == null || DolshnostNew == "")
            {
                MessageBox.Show("Введите должность рабочего");
            }
        }
    }
}
