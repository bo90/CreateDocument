using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;
using Microsoft.Reporting;
using Microsoft.ReportingServices;
//using Microsoft.Office.Interop.Excel;

//using Application = Microsoft.Office.Interop.Excel.Application;
    


namespace Create_documt_sluzhb_infrom
{
    public partial class Form1 : Form
    {
        private string connstring = String.Format("Server={0};Port={1};" +
            "User Id={2}; Password={3}; Database={4};",
            "localhost", 5432, "postgres",
            "150590", "Test_DB");

        private NpgsqlConnection conn;
        private string sql;
        private NpgsqlCommand cmd;
        private DataTable dt;
        private int rowIndex = -1;

       
        public Form1()
        {
            InitializeComponent();
        }

        private string SqlQuery = null;

        private void Form1_Load(object sender, EventArgs e)
        {
            conn = new NpgsqlConnection(connstring);
            Select();
            NameTable();
        }
        //private new void Select()
        private void Select()
        {
            try
            {
                conn.Open();
                sql = @"select * from st_select();";
                cmd = new NpgsqlCommand(sql, conn);
                dt = new DataTable();
                dt.Load(cmd.ExecuteReader());
                conn.Close();
                dgvData.DataSource = null; //reset Datagribview
                dgvData.DataSource = dt;
                SqlQuery = sql;

            }
            catch (Exception ex)
            {
                conn.Close();
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private string query;

        private void NameTable()
        {
            try
            {
                string  Squery = "SELECT * FROM tableInfo";
                query = Squery;
                conn.Open();
                DataTable dt = new DataTable();
                string result = null;
                string descript = null;
                using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
                {
                    NpgsqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    //dt.Load(dr);
                    result = dr["nameTable"].ToString();
                    descript = dr["descripttable"].ToString();
                    dr.Close();
                }
                conn.Close();
                textBox5.Text = result;
                textBox6.Text = descript;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Data + " " + ex.Message);
            }
        }

        private void ReadFile()
        {
            using (StreamWriter sw = new StreamWriter("query.txt"))
            {
                sw.WriteLine(query);
                sw.WriteLine(SqlQuery);
                sw.Close();
                Process.Start("query.txt");
            }
        }

        private string st_id;
        private string FirstName;
        private string MidName;
        private string Lastname;
        private string age;
        private string dob;

        private NpgsqlConnection connect;

        private string connstring2 = String.Format("Server={0};Port={1};" +
            "User Id={2}; Password={3}; Database={4};",
            "localhost", 5432, "postgres",
            "150590", "DemoDB");

        private void ReadDATA()
        {
            string Squery = "SELECT * FROM student";
            query = Squery;
            conn.Open();
            DataTable dt = new DataTable();
            string result = null;
            string descript = null;
            using (NpgsqlCommand cmd = new NpgsqlCommand(query, conn))
            {
                NpgsqlDataReader dr = cmd.ExecuteReader();
                dr.Read();
                st_id = dr["st_id"].ToString();
                FirstName = dr["st_firstname"].ToString();
                MidName = dr["st_midname"].ToString();
                Lastname = dr["st_lastname"].ToString();
                age = dr["st_age"].ToString();
                dob = dr["st_dob"].ToString();
                dr.Close();
            }
            conn.Close();
        }

        // перевод данных из одной в другую БД
        private void TranslateData()
        {
            try
            {
                connect = new NpgsqlConnection(connstring2);
                connect.Open();
                int id = default(int);
                id = Convert.ToInt32(st_id);

                int newAge = Convert.ToInt32(age);
                int newDob = Convert.ToInt32(dob);

                string quer = "Insert into student (st_id,st_firstname, st_midname,st_lastname," +
                    "st_age,st_dob) values (@st_id, @st_firstname, @st_midname, @st_lastname," +
                    "@st_age, @st_dob)";
                using (NpgsqlCommand cmd = new NpgsqlCommand(quer, connect))
                {
                    NpgsqlParameter parameter = new NpgsqlParameter();
                    parameter.ParameterName = "@st_id";
                    parameter.Value = id;
                    parameter.DbType = DbType.Int32;
                    cmd.Parameters.Add(parameter);
                    //
                    parameter = new NpgsqlParameter();
                    parameter.ParameterName = "@st_firstname";
                    parameter.Value = FirstName;
                    parameter.DbType = DbType.String;
                    cmd.Parameters.Add(parameter);
                    //
                    parameter = new NpgsqlParameter();
                    parameter.ParameterName = "@st_midname";
                    parameter.Value = MidName;
                    parameter.DbType = DbType.String;
                    cmd.Parameters.Add(parameter);
                    //
                    parameter = new NpgsqlParameter();
                    parameter.ParameterName = "@st_lastname";
                    parameter.Value = Lastname;
                    parameter.DbType = DbType.String;
                    cmd.Parameters.Add(parameter);
                    //
                    parameter = new NpgsqlParameter();
                    parameter.ParameterName = "@st_age";
                    parameter.Value = newAge;
                    parameter.DbType = DbType.Int32;
                    cmd.Parameters.Add(parameter);
                    //
                    parameter = new NpgsqlParameter();
                    parameter.ParameterName = "@st_dob";
                    parameter.Value = newDob;
                    parameter.DbType = DbType.Int32;
                    cmd.Parameters.Add(parameter);

                    cmd.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {


        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void dgvData_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            //if (e.RowIndex >= 0)
            //{
            //    txtFirstname1.Text = dgvData.Rows[e.RowIndex].Cells["firstname"].Value.ToString();
            //    txtMidname1.Text = dgvData.Rows[e.RowIndex].Cells["midname"].Value.ToString();
            //    txtLastname.Text = dgvData.Rows[e.RowIndex].Cells["lastname"].Value.ToString();
            //}
            Int32 selectedRowcount = dataGridView1.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if(selectedRowcount > 0)
            {

            }
        }

        private string GetID;

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                rowIndex = e.RowIndex;
                txtFirstname.Text = dgvData.Rows[e.RowIndex].Cells["firstname"].Value.ToString();
                txtMidname.Text = dgvData.Rows[e.RowIndex].Cells["midname"].Value.ToString();
                txtLastname.Text = dgvData.Rows[e.RowIndex].Cells["lastname"].Value.ToString();
                GetID = dgvData.Rows[e.RowIndex].Cells["id"].Value.ToString();
            }
        }

        //
        //private void button1_Click(object sender, EventArgs e)
        private void btnSave_Click(object sender, EventArgs e)
        {
            int result = 0;
            if (rowIndex < 0)//insert
            {
                try
                {
                    conn.Open();
                    sql = @"select *from st_insert(_firstname,_midname,_lastname)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_firstname", txtFirstname.Text);
                    cmd.Parameters.AddWithValue("_midname", txtMidname.Text);
                    cmd.Parameters.AddWithValue("_lastname", txtLastname.Text);
                    result = (int)cmd.ExecuteScalar();
                    conn.Close();
                    if (result ==1)
                    {
                        MessageBox.Show("Inserted new info successfully");
                        Select();

                    }
                    else
                    {
                        MessageBox.Show("Inserted fail");
                    }
                    
                }
                catch (Exception ex)
                {
                    conn.Close();
                    MessageBox.Show("Updated fail. Error:" + ex.Message);
                }
            }
            else//update
            {
                try
                {
                    conn.Open();
                    sql = @"select * from st_update(:_id,:_firstname,:_midname,:_lastname)";
                    cmd = new NpgsqlCommand(sql, conn);
                    cmd.Parameters.AddWithValue("_id", int.Parse(dgvData.Rows[rowIndex].Cells["id"].Value.ToString()));
                    cmd.Parameters.AddWithValue("_firstname", txtFirstname.Text);
                    cmd.Parameters.AddWithValue("_midname", txtMidname.Text);
                    cmd.Parameters.AddWithValue("_lastname", txtLastname.Text);
                    result = (int)cmd.ExecuteScalar();
                    conn.Close();
                    if (result ==1)
                    {
                        MessageBox.Show("Updated successfully");
                            Select();
                    }
                    else
                    {
                        MessageBox.Show("Updated fail");
                    }
                }
                catch (Exception ex)
                {
                    conn.Close();
                    MessageBox.Show("Updated fail. Error:" + ex.Message);
                }
                result = 0;
                txtFirstname.Text = txtMidname.Text = txtLastname.Text = null; 
                txtFirstname.Enabled = txtMidname.Enabled = txtLastname.Enabled = false;
            }
        }
        private void btnInsert_Click(object sender, EventArgs e)
        {
            rowIndex = -1;
            txtFirstname.Enabled = txtMidname.Enabled = txtLastname.Enabled = true;
            txtFirstname.Text = txtMidname.Text = txtLastname.Text = null;
            txtFirstname.Select();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if(rowIndex <0)
            {
                MessageBox.Show("Please choose the line to Update");
                return;
            }
            txtFirstname.Enabled = txtMidname.Enabled = txtLastname.Enabled = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (rowIndex < 0)
            {
                MessageBox.Show("Please choose the line to Delete");
                return;
            }
            try
            {
                conn.Open();
                sql = @"select * from st_delete (:_id)";
                cmd = new NpgsqlCommand(sql, conn);
                cmd.Parameters.AddWithValue("_id",int.Parse(dgvData.Rows[rowIndex].Cells["id"].Value.ToString()));
                if((int)cmd.ExecuteScalar() == 1)
                {
                    MessageBox.Show("Delete student successfully");
                    rowIndex = -1;
                    Select();
                }
                conn.Close();
            }
            catch (Exception ex)
            {

                conn.Close();
                MessageBox.Show("Delete bad" + ex.Message);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //PostGreSQL Create_documnt_sluzhb = new PostGreSQL();
            //dataItems = Create_documnt_sluzhb.PostgreSQLtest1();
            //tbDataItems.Clear();
            //for (int i = 0; i< dataItems.Count; i+)
            //{
            //    tbDataItems.Text += dataItems[i];
            //    tbDataItems.ScrollToCaret();

            //}
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //PostGreSQL Create_documnt_sluzhb = new PostGreSQL();
            //dataItems = Create_documnt_sluzhb.PostgreSQLtest1();
            //tbDataItems.Clear();
            //for (int = 0; i < dataItems.Count; i++)
            //{
            //    tbDataItems.Text += dataItems[i];
            //    tbDataItems.ScrollToCaret();
            //}
        }

        private void button3_Click(object sender, EventArgs e)
        {
        //string constring = "datasource= localhost;port =3232; username=root;password=root";
        //string uery= "insert into database.edata (Eid,name,surname,age,Gender, DOB)"+
        //    "values('"+this.Eid_txt.Text+"','"+this.Name_txt.Text+"','"+this.Surname_txt.Text"','" + this.dateTimePicker1.Text+ "');";
        //    MySqlConnection conDataBase = new MySqlConnection(constring);
        //MySqlCommand cmdDatabase = new MySqlCommand(Query, conDataBase);
        //MySqlDataReader myReader;
        //try
        //{
        //    conDataBase.Open();
        //    myReader = cmdDatabase.ExecuteReader();
        //    MessageBox.Show("Saved");
        //    while (myReader.Read())
        //    {
       // https://youtu.be/Yb7R7Dr2DBk
            //    }
            //    catch(Exception ex)
            //{
            //    MessageBox.Show(ex.Message);
            //}
            //load_table();
            //}
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtFirstname_TextChanged(object sender, EventArgs e)
        {
            //txtFirstname.ForeColor = Color.FromArgb(221, 0, 0);
        }

        private void txtMidname_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void txtLastname_TextChanged(object sender, EventArgs e)
        {
            
        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void txtFirstname_TextChanged_1(object sender, EventArgs e)
        {
            
        }

        private void txtFirstname_Click(object sender, EventArgs e)
        {
            txtFirstname.Clear();
            
            pictureBox1.BackgroundImage = Properties.Resources.user21;
            //pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;       хз что но фотку должно по центру ставить
            panel1.BackColor = Color.FromArgb(255, 0, 0);
            panel1.ForeColor = Color.WhiteSmoke; // вроде как используется но у меня не получилось
            txtFirstname.ForeColor = Color.FromArgb(221, 160, 221);

            //panel2.BackColor = Color.FromArgb(221, 160, 260);
            //panel3.BackColor = Color.FromArgb(221, 160, 260);

            pictureBox1.BackgroundImage = Properties.Resources.table1;



            //else{
            //    panel1.BackColor= Color.FromArgb(221, 160, 221);
            //}

            //picuser.BackgroundImage = Properties.Resources.user1;
            //txtFirstname.ForeColor = Color.WhiteSmoke;
            //panel1.BackColor = Color.WhiteSmoke;
            //picemail.BackfroundImage = Properties.Resources.email;
            //panel3.Backcolor = Color.WhiteSmoke;
            //textbox3.Forecolor = Color.WhiteSmoke;
        }

        private void txtMidname_Click(object sender, EventArgs e)
        {
            txtMidname.Clear();
            pictureBox2.BackgroundImage = Properties.Resources.dog1;
            panel2.BackColor = Color.FromArgb(255, 0, 0);
            txtMidname.ForeColor = Color.FromArgb(221, 160, 221);



            //panel1.BackColor = Color.FromArgb(221, 160, 260);
            //panel3.BackColor = Color.FromArgb(221, 160, 260);


  
            



            pictureBox3.BackgroundImage = Properties.Resources.dog2;
            panel2.BackColor = Color.FromArgb(255, 0, 0);
            txtLastname.ForeColor = Color.WhiteSmoke;
        }

        private void txtLastname_Click(object sender, EventArgs e)
        {
            txtLastname.Clear();
            pictureBox1.BackgroundImage = Properties.Resources.user1;
            panel3.BackColor = Color.FromArgb(255, 0, 0);
            txtLastname.ForeColor = Color.FromArgb(221, 160, 221);

            //panel1.BackColor = Color.FromArgb(221, 160, 263); вылетает
            //panel2.BackColor = Color.FromArgb(221, 160, 260);

            pictureBox2.BackgroundImage = Properties.Resources.table1;
            pictureBox3.BackgroundImage = Properties.Resources.dog1;
            panel2.ForeColor = Color.White;
     
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button8_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            var newForm = new Form3();
            newForm.Show();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                ReadFile();
                ReadDATA();
                TranslateData();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void CheckedListBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button11_Click(object sender, EventArgs e)
        {
            try
            {
                int getId = Convert.ToInt32(textBox3.Text);
                SearchFunction(getId);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void SearchFunction(int id)
        {
            try
            {
                string quer = "SELECT * FROM student where st_id = '" + id + "'";
                conn.Open();
                string seek = null;
                using (NpgsqlCommand cmd = new NpgsqlCommand(quer, conn))
                {
                    NpgsqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    seek = dr["st_id"].ToString();
                    dr.Close();
                }
                conn.Close();
                if (seek != null)
                {
                    MessageBox.Show("Уникальный номер студента: " + seek);
                }
                else
                {
                    MessageBox.Show("Номера не существует!");
                }
            }
            catch(Exception)
            {
                MessageBox.Show("Студент с таким номер не найден!");
                return;
            }
        }

        protected int click = 0;

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (click == 0)
            {
                InvisibaleElements(false);
                click++;
                return;
            }
            if (click == 1)
            {
                InvisibaleElements(true);
                click = 0;
                return;
            }
        }

        protected void InvisibaleElements(bool state)
        {
            pictureBox1.Visible = state;
            txtFirstname1.Visible = state;
            txtFirstname.Visible = state;
            pictureBox2.Visible = state;
            txtMidname1.Visible = state;
            txtMidname.Visible = state;

            pictureBox3.Visible = state;
            txtLastname1.Visible = state;
            txtLastname.Visible = state;
            pictureBox4.Visible = state;
            label3.Visible = state;
            textBox1.Visible = state;
            pictureBox5.Visible = state;
            label4.Visible = state;
            textBox2.Visible = state;

            gnrtSQL_code.Visible = state;
            chooseDtSet.Visible = state;
            label2.Visible = state;
            checkedListBox1.Visible = state;
            textBox5.Visible = state;

            button1.Visible = state;
            dgvData.Visible = state;
            dataGridView1.Visible = state;
            button6.Visible = state;
            button7.Visible = state;
            button8.Visible = state;
            btnSave.Visible = state;
            button4.Visible = state;
            button11.Visible = state;
            textBox6.Visible = state;
            textBox3.Visible = state;

            button9.Visible = state;
            button10.Visible = state;
            btnUpdate.Visible = state;
            btnInsert.Visible = state;
            btnDelete.Visible = state;

            label1.Visible = state;
            textBox4.Visible = state;
            dateTimePicker1.Visible = state;
            SQLDatabase_button.Visible = state;
            panel1.Visible = state;
            panel2.Visible = state;
            panel3.Visible = state;
            panel4.Visible = state;
            panel5.Visible = state;
            panel6.Visible = state;
            panel7.Visible = state;
            panel8.Visible = state;
            this.WindowState = FormWindowState.Minimized;
        }

        private void Report()
        {
            
        }

        private Application application;
        

        private void button8_Click_1(object sender, EventArgs e)
        {
            //WriteToExel();
            ReadPriznak();
        }

        private void WriteToExel()
        {
            cl_exel exel = new cl_exel();
            string s1 = "Table1";
            string s2 = "OOne";
            int n = checkedListBox1.CheckedItems.Count;
            exel.ExelAction(n, s1, s2);
        }

        private void ReadPriznak()
        {
            int n = checkedListBox1.CheckedItems.Count;
            for (int i = 0; i < n; i++)
            {
                string t = checkedListBox1.CheckedItems[i].ToString();
                SeekRecord(t);
            }
        }

        //поиск записи по атрибуту
        private void SeekRecord(string priznak)
        {
            
            try
            {
                string quer = "SELECT * FROM student where priznak = '" + priznak + "'";
                conn.Open();
                DataTable dt = new DataTable();
                string seek = null;
                using (NpgsqlCommand cmd = new NpgsqlCommand(quer, conn))
                {
                    NpgsqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();                    
                    dt.Load(dr);
                    dr.Close();
                }
                conn.Close();
                
                //подключение к другой базе
                connect = new NpgsqlConnection(connstring2);
                connect.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(quer, connect))
                {
                    NpgsqlDataReader dr = cmd.ExecuteReader();
                    dr.Read();
                    seek = dr["priznak"].ToString();                    
                    dr.Close();
                }
                MessageBox.Show("В DemoDT имееться запись с признаком: " + seek);
            }
            catch (Exception)
            {
                MessageBox.Show("Запись с таким показателем не найдена!");
                return;
            }
        }

        private void btnUpdate_Click_1(object sender, EventArgs e)
        {
            upd();
        }

        //обновление 
        private void upd()
        {
            string firstname = txtFirstname.Text;
            string mid = txtMidname.Text;
            int id = Convert.ToInt32(GetID);
            string las = txtLastname.Text;
            conn.Open();
            string quer = "Update student set st_firstname = '" + firstname + "'," +
               "st_midname= '" + mid + "', st_lastname= '" + las + "' where st_id='" + id + "'";
            NpgsqlCommand cmd = new NpgsqlCommand(quer, conn);
            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}

