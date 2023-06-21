using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Controls;
using MetroFramework.Forms;
using MySql.Data.MySqlClient;

namespace accFee
{
    public partial class remOtd : MetroForm
    {
        public string numplat;
        public remOtd()
        {
            InitializeComponent();
            NameInProgAddSlot();
        }

        //Надписи
        public void NameInProgAddSlot()
        {
            label1.Text = "Номер платы";
            label2.Text = null;

            button1.Text = "Найти плату";
            button2.Text = "Изменить статус платы";

            textBox1.MaxLength = 6;
        }

        //Таймер
        private void timer1_Tick(object sender, EventArgs e)
        {
            label2.Text = null;
            timer1.Stop();
        }

        //Дня нахождения платы
        private void button1_Click(object sender, EventArgs e)
        {
            numplat = textBox1.Text;
            if (textBox1.Text != null)
            {
                DB dB = new DB();
                DataTable table = new DataTable();
                MySqlDataAdapter adapter = new MySqlDataAdapter();

                MySqlCommand command = new MySqlCommand("SELECT * FROM `plats` WHERE `NomerPlat` = @qq", dB.getConnection());
                command.Parameters.Add("@qq", MySqlDbType.VarChar).Value = textBox1.Text;

                adapter.SelectCommand = command;
                adapter.Fill(table);
                MySqlCommand command1 = new MySqlCommand("SELECT `remont` FROM `plats` WHERE `NomerPlat` = @qq", dB.getConnection());
                command1.Parameters.Add("@qq", MySqlDbType.VarChar).Value = numplat;
                dB.openConnection();
                string remka = Convert.ToString(command1.ExecuteScalar());
                string qwe = null;
                if (remka == "Нет")
                {
                    qwe = "Плата не сломана";
                }
                if (remka == "На ремонте")
                {
                    qwe = "Плата находится на ремонте";
                }
                dB.closeConnection();
                if (table.Rows.Count > 0)
                {
                    label2.Text = "Плата в статусе: " + qwe;
                    timer1.Start();
                }
                else
                {
                    label2.Text = "Такой платы нету";
                    timer1.Start();
                }
            }
            else
            {
                label2.Text = "Пустое поле";
                timer1.Start();
            }
        }

        //Исправление
        private void button2_Click(object sender, EventArgs e)
        {
            DB dB = new DB();
            DataTable table = new DataTable();
            MySqlDataAdapter adapter = new MySqlDataAdapter();

            dB.openConnection();
            MySqlCommand command = new MySqlCommand("SELECT `remont` FROM `plats` WHERE `NomerPlat` = @qq", dB.getConnection());
            command.Parameters.Add("@qq", MySqlDbType.VarChar).Value = numplat;
            string remka = Convert.ToString(command.ExecuteScalar());
            dB.closeConnection();

            string no = "Нет";
            string yes = "На ремонте";

            if (remka == "Нет")
            {
                //Изменить на "На ремонте"
                MySqlCommand commandgorem = new MySqlCommand("UPDATE `plats` SET `remont` = @qq WHERE `NomerPlat` = @num", dB.getConnection());
                commandgorem.Parameters.Add("@qq", MySqlDbType.VarChar).Value = yes;
                commandgorem.Parameters.Add("@num", MySqlDbType.VarChar).Value = numplat;
                
            }
            if (remka == "На ремонте")
            {
                //Изменить на "Нет"
                MySqlCommand commandgorem = new MySqlCommand("UPDATE `plats` SET `remont` = @qq WHERE `NomerPlat` = @num", dB.getConnection());
                commandgorem.Parameters.Add("@qq", MySqlDbType.VarChar).Value = no;
                
                commandgorem.Parameters.Add("@num", MySqlDbType.VarChar).Value = numplat;
                
            }

            //UPDATE `plats` SET `remont` = @qq WHERE `NomerPlat` = @num;
        }
    }
}
