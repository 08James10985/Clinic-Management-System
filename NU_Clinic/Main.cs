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
using System.Data.SqlClient;

namespace NU_Clinic
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            frmPatient frm = new frmPatient();
            frm.TopLevel = false;
            frm.AutoScroll = true;
            //frmMain.panelPatientList.Controls.Add(frm);
            frm.Show();
            chart1.Series["Data"]["PixelPointWidth"] = "30";
            chart2.Series["Data"]["PixelPointWidth"] = "30";
            label25.Text = dataGridView1.RowCount.ToString();
            bunifuDatepicker1.Value = DateTime.Now;
            bunifuDatepicker2.Value = DateTime.Now;
            cmbComplaints();



        }
        public string MyValue
        {
            get { return label.Text; }
        }

        MySqlConnection con = new MySqlConnection("server = localhost; database = nuclinic; username = root; password = ; Convert Zero Datetime=True");


        private string firstname,lastname,middlename,username;

        public string Lastname
        {
            get { return lastname; }
            set { lastname = value; }
        }

        public string Firstname
        {
            get { return firstname; }
            set { firstname = value; }
        }
        public string Middlename
        {
            get { return middlename; }
            set { middlename = value; }
        }
        public string Username
        {
            get { return username; }
            set { username = value; }
        }

        public void cmbComplaints()
        {

            string query = "select complaints_cat as Name from category";
            MySqlDataAdapter data = new MySqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            data.Fill(dt);

            DataRow dr = dt.NewRow();
         
            dt.Rows.InsertAt(dr, 0);
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "Name";
            comboBox1.DataSource = dt;
        }


        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            line5.Visible = false;
            line4.Visible = false;
            panelArhive.Visible = false;
            picPatientList.Visible = true;
            line3.Visible = false;
            pictureBox6.Visible = false;
            panel1.Visible = true;
            panel1.Controls.Clear();
            frmPatient frm = new frmPatient();
            frm.TopLevel = false;
            panel1.Controls.Add(frm);
            frm.Show();
            panel7.Visible = false;

            //frmPatient frm = new frmPatient();
            //frm.Show();

        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            panelArhive.Visible = false;
            panel1.Visible = false;
            line3.Visible = false;
            picPatientList.Visible = false;
            pictureBox6.Visible = true;
            load();
            line4.Visible = false;
            panel7.Visible = false;
            line5.Visible = false;


        }

        private void panelPatientList_Paint(object sender, PaintEventArgs e)
        {
            
        }

       

        private void btnLogout_Click_1(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to logout "+ Firstname + "?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                frmLogin frm = new frmLogin();
                this.Hide();
                frm.Show();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            load();
        }

        frmAddPatient frm = new frmAddPatient();

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
           
        }

        private void chart1_Click(object sender, EventArgs e)
        {
           

        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            load();
            loadchart1();
            loadchart2();
        }
        public void loadchart1()
        {
            String Conn1 = "server = localhost; database = nuclinic; username = root; password = ; Convert Zero Datetime=True";
            String query1 = "select * from category";
            MySqlConnection myConn1 = new MySqlConnection(Conn1);
            MySqlCommand cmdDatabase1 = new MySqlCommand(query1, myConn1);
            MySqlDataReader myReader;

            try
            {
                myConn1.Open();
                myReader = cmdDatabase1.ExecuteReader();
                while (myReader.Read())
                {
                    this.chart1.Series["Data"].Points.AddXY(myReader.GetString("complaints_cat"), myReader.GetUInt32("number"));

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            line5.Visible = false;
            line4.Visible = false;
            picPatientList.Visible = false;
            line3.Visible = true;
            pictureBox6.Visible = false;
            panelArhive.Visible = true;
            panel1.Visible = false;
            panel7.Visible = false;
        }
        public void loadArhive()
        {
            if (comboBox1.Text == "" && txtSearch.Text == "")
            {
                startArchive();
            }
            else
            {


                MySqlConnection con = new MySqlConnection("server = localhost; database = nuclinic; username = root; password = ; Convert Zero Datetime=True");
                MySqlCommand command = new MySqlCommand("SELECT dctd.date, patient.studentid,dctd.id,patient.firstname,patient.lastname,dctd.doctor,dctd.treatment,dctd.complaints,dctd.category from patient INNER JOIN dctd ON patient.id=dctd.id  Where (date BETWEEN @date AND @date2) && (category = @category || category = @category2) ", con);
                command.Parameters.AddWithValue("@date", dateTimePicker1.Value.Date);
                command.Parameters.AddWithValue("@date2", dateTimePicker2.Value.Date);
                command.Parameters.AddWithValue("@category", txtSearch.Text);
                command.Parameters.AddWithValue("@category2", comboBox1.Text);

                //MySqlCommand command = new MySqlCommand("SELECT patient.lastname,patient.studentid,dctd.id,dctd.complaints,dctd.date from patient INNER JOIN dctd ON patient.id=dctd.id", con);
                MySqlDataAdapter da = new MySqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
                label25.Text = dataGridView1.RowCount.ToString();
                label29.Visible = true;
                label29.Text = "Showing: "+ dataGridView1.RowCount.ToString()+ " results";
            }

        }
        public void startArchive()
        {
           
                MySqlConnection con = new MySqlConnection("server = localhost; database = nuclinic; username = root; password = ; Convert Zero Datetime=True");
                MySqlCommand command = new MySqlCommand("SELECT dctd.date, patient.studentid,dctd.id,patient.firstname,patient.lastname,dctd.doctor,dctd.treatment,dctd.complaints,dctd.category from patient INNER JOIN dctd ON patient.id=dctd.id  Where date BETWEEN @date AND @date2", con);
                command.Parameters.AddWithValue("@date", dateTimePicker1.Value.Date);
                command.Parameters.AddWithValue("@date2", dateTimePicker2.Value.Date);


                //MySqlCommand command = new MySqlCommand("SELECT patient.lastname,patient.studentid,dctd.id,dctd.complaints,dctd.date from patient INNER JOIN dctd ON patient.id=dctd.id", con);
                MySqlDataAdapter da = new MySqlDataAdapter(command);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dataGridView1.DataSource = dt;
            label25.Text = dataGridView1.RowCount.ToString();
            label29.Visible = true;
            label29.Text = "Showing: " + dataGridView1.RowCount.ToString() + " results";
        }

       

        private void button1_Click(object sender, EventArgs e)
        {
            loadArhive();
        }

        private void bunifuDatepicker1_onValueChanged(object sender, EventArgs e)
        {
            var date = bunifuDatepicker1.Value;
            dateTimePicker1.Value = date;
            
        }

        private void bunifuDatepicker2_onValueChanged(object sender, EventArgs e)
        {
            var date = bunifuDatepicker2.Value;
            dateTimePicker2.Value = date;
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {




                int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString());
                MySqlDataAdapter ada = new MySqlDataAdapter("select id as 'id',studentid as 'Student ID',firstname as'Firstname',lastname as 'Lastname',telno as 'Contact No',age as 'Age',sex as 'Gender',course as'Course' from patient where id= '" + id + "'", con);

                dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                dataGridView1.Rows[e.RowIndex].Selected = true;
                dataGridView1.Focus();
                DataTable dt = new DataTable();
                ada.Fill(dt);
                dataGridView1.DataSource = dt;

                foreach (DataRow dr in dt.Rows)
                {
                    textBox1.Text = dr["id"].ToString();

                }

                if (MessageBox.Show("Are you sure you want to delete this?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {


                    try
                    {

                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = "delete from patient where id = '" + textBox1.Text + "'";


                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Item Deleted");
                        loadArhive();
                        con.Close();
                   
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    loadArhive();
                }
            }
            else if (dataGridView1.Columns[e.ColumnIndex].Name == "View")
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "View")
                {

                    int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString());
                    MySqlDataAdapter ada = new MySqlDataAdapter("select id as 'id',studentid as 'Student ID',firstname as'Firstname',lastname as 'Lastname',telno as 'Contact No',age as 'Age',sex as 'Gender',course as'Course' from patient where id= '" + id + "'", con);

                    dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dataGridView1.Rows[e.RowIndex].Selected = true;




                    DataTable dt = new DataTable();
                    ada.Fill(dt);
                    dataGridView1.DataSource = dt;

                    foreach (DataRow dr in dt.Rows)
                    {
                        frmViewPatient frm = new frmViewPatient();

                        //frm.label2.Text = dr["id"].ToString();
                        frm.ID = dt.Rows[0]["id"].ToString();
                        frm.Firstname = dt.Rows[0]["Firstname"].ToString();
                        frm.Lastname = dt.Rows[0]["Lastname"].ToString();
                        // frm.Middlename = dt.Rows[0]["middlename"].ToString();
                        frm.Studentid = dt.Rows[0]["Student ID"].ToString();
                        //frm.Birthday = dt.Rows[0]["Birthday"].ToString();
                        frm.Course = dt.Rows[0]["Course"].ToString();
                        frm.ShowDialog();

                    }
                }
                else
                {
                    loadArhive();
                }



            }
        }


        private void bunifuFlatButton9_Click(object sender, EventArgs e)
        {
            loadArhive();
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            loadArhive();
        }

        public void load()
        {
            timer1.Start();
            time.Text = DateTime.Now.ToLongTimeString();
            label.Text = "  " + Firstname + " " + Middlename + " " + Lastname;
            label6.Text = Firstname + " " + Middlename + " " + Lastname;
            date.Text = DateTime.Now.ToLongDateString();
            time.Text = DateTime.Now.ToLongTimeString();
            bunifuFlatButton1.Text = "Dashboard";
            label3.Text = username;

            try
            {

                MySqlCommand cmd = con.CreateCommand();
                MySqlCommand cmd2 = con.CreateCommand();
                MySqlCommand cmd3 = con.CreateCommand();
                MySqlCommand cmd4 = con.CreateCommand();
                MySqlCommand cmd5 = con.CreateCommand();
                cmd.CommandText = "Select count(*) as myCount from patient";
                cmd2.CommandText = "Select count(*) as myCount from user";
                cmd3.CommandText = "Select count(*) as myCount from dctd";
                cmd4.CommandText = "SELECT count(*) FROM dctd WHERE date = CURDATE()";
                cmd5.CommandText = "SELECT count(*) FROM dctd WHERE MONTH (date) = MONTH(CURRENT_DATE())";

                con.Open();
                long count = (long)cmd.ExecuteScalar();
                long doc = (long)cmd2.ExecuteScalar();
                long dctd = (long)cmd3.ExecuteScalar();
                long today = (long)cmd4.ExecuteScalar();
                long monthly = (long)cmd5.ExecuteScalar();

                label9.Text = count.ToString();
                lblDoctors.Text = doc.ToString();
                lblDiagnosis.Text = dctd.ToString();
                lbltoday.Text = today.ToString();
                thismonth.Text = monthly.ToString();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }
        }

        private void linkLabel1_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            txtSearch.Text = "";
            bunifuDatepicker1.Value = DateTime.Now;
            bunifuDatepicker2.Value = DateTime.Now;
            dataGridView1.DataSource = null;
            dataGridView1.Refresh();
            label25.Text = "0";
            label29.Visible = false;
        }

        private void cmbComplaint_onItemSelected(object sender, EventArgs e)
        {
           
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            txtSearch.Enabled = false;
            comboBox1.Enabled = true;
            txtSearch.Text = "";
            label28.Enabled = false;
            label27.Enabled = true;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            comboBox1.Enabled = false;
            txtSearch.Enabled = true;
            comboBox1.Text = "";
            label28.Enabled = true;
            label27.Enabled = false;
        }

        private void bunifuFlatButton9_Click_1(object sender, EventArgs e)
        {
            line5.Visible = false;
            line4.Visible = true;
            picPatientList.Visible = false;
            pictureBox6.Visible = false;
            line3.Visible = false;
            panel7.Visible = true;
            panel8.Visible = false;
        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            DoctorList frm = new DoctorList();
            frm.ShowDialog();
        }

        private void bunifuTileButton2_Click(object sender, EventArgs e)
        {
            Category frm = new Category();
            frm.ShowDialog();
        }

        private void bunifuTileButton3_Click(object sender, EventArgs e)
        {
            Course frm = new Course();
            frm.ShowDialog();
        }

        private void bunifuFlatButton10_Click(object sender, EventArgs e)
        {
            Load3();
            panel8.Visible = true;
            line4.Visible = true;
            picPatientList.Visible = false;
            pictureBox6.Visible = false;
            line3.Visible = false;
            line5.Visible = true;
            line4.Visible = false;

        }

        private void panel8_Paint(object sender, PaintEventArgs e)
        {
            Load3();


        }
        public void Load3()
        {
            MySqlConnection con = new MySqlConnection("server = localhost; database = nuclinic; username = root; password = ; Convert Zero Datetime=True");
            MySqlCommand command = new MySqlCommand("SELECT dctd.date, patient.studentid,dctd.id,patient.firstname,patient.lastname,dctd.doctor,dctd.treatment,dctd.complaints,dctd.category from patient INNER JOIN dctd ON patient.id=dctd.id", con);
  

            //MySqlCommand command = new MySqlCommand("SELECT patient.lastname,patient.studentid,dctd.id,dctd.complaints,dctd.date from patient INNER JOIN dctd ON patient.id=dctd.id", con);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Diagnosis frm = new Diagnosis();
            frm.ShowDialog();
        }

        private void bunifuSeparator9_Load(object sender, EventArgs e)
        {

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Load3();
            searchbox.Text="";
            result.Visible = false;
        }

        private void dataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
         
            if (dataGridView2.Columns[e.ColumnIndex].Name == "VIEW")
            {
                try
                {

                    int id = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["id"].Value.ToString());
                    MySqlDataAdapter ada = new MySqlDataAdapter("select id,date,complaints,category, doctor from dctd where id= '" + id + "'", con);

                    dataGridView2.CurrentCell = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dataGridView2.Rows[e.RowIndex].Selected = true;




                    DataTable dt = new DataTable();
                    ada.Fill(dt);
                    dataGridView2.DataSource = dt;

                    foreach (DataRow dr in dt.Rows)
                    {
                        frmViewPatient frm = new frmViewPatient();

                        //frm.label2.Text = dr["id"].ToString();
                        frm.ID = dt.Rows[0]["id"].ToString();
                        frm.Firstname = dt.Rows[0]["Firstname"].ToString();
                        frm.Lastname = dt.Rows[0]["Lastname"].ToString();
                        // frm.Middlename = dt.Rows[0]["middlename"].ToString();
                        frm.Studentid = dt.Rows[0]["Student ID"].ToString();
                        //frm.Birthday = dt.Rows[0]["Birthday"].ToString();
                        frm.Course = dt.Rows[0]["Course"].ToString();
                        frm.ShowDialog();

                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }


            }
        }

        private void dataGridView2_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Columns[e.ColumnIndex].Name == "Delete")
            {




                int id = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["id"].Value.ToString());
                MySqlDataAdapter ada = new MySqlDataAdapter("select id as 'id',studentid as 'Student ID',firstname as'Firstname',lastname as 'Lastname',telno as 'Contact No',age as 'Age',sex as 'Gender',course as'Course' from patient where id= '" + id + "'", con);

                dataGridView2.CurrentCell = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex];
                dataGridView2.Rows[e.RowIndex].Selected = true;
                dataGridView2.Focus();
                DataTable dt = new DataTable();
                ada.Fill(dt);
                dataGridView2.DataSource = dt;

                foreach (DataRow dr in dt.Rows)
                {
                    textBox1.Text = dr["id"].ToString();

                }

                if (MessageBox.Show("Are you sure you want to delete this?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {


                    try
                    {

                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = "delete from patient where id = '" + textBox1.Text + "'";


                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Item Deleted");
                        Load3();
                        con.Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    Load3();
                }
            }
            else if (dataGridView2.Columns[e.ColumnIndex].Name == "View")
            {
                if (dataGridView2.Columns[e.ColumnIndex].Name == "View")
                {

                    int id = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["id"].Value.ToString());
                    MySqlDataAdapter ada = new MySqlDataAdapter("select id as 'id',studentid as 'Student ID',firstname as'Firstname',lastname as 'Lastname',telno as 'Contact No',age as 'Age',sex as 'Gender',course as'Course' from patient where id= '" + id + "'", con);

                    dataGridView2.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dataGridView2.Rows[e.RowIndex].Selected = true;




                    DataTable dt = new DataTable();
                    ada.Fill(dt);
                    dataGridView2.DataSource = dt;

                    foreach (DataRow dr in dt.Rows)
                    {
                        frmViewPatient frm = new frmViewPatient();

                        //frm.label2.Text = dr["id"].ToString();
                        frm.ID = dt.Rows[0]["id"].ToString();
                        frm.Firstname = dt.Rows[0]["Firstname"].ToString();
                        frm.Lastname = dt.Rows[0]["Lastname"].ToString();
                        // frm.Middlename = dt.Rows[0]["middlename"].ToString();
                        frm.Studentid = dt.Rows[0]["Student ID"].ToString();
                        //frm.Birthday = dt.Rows[0]["Birthday"].ToString();
                        frm.Course = dt.Rows[0]["Course"].ToString();
                        frm.ShowDialog();

                    }
                }
                else
                {
                    Load3();
                }
            }
        }

        private void dataGridView2_CellContentClick_2(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Columns[e.ColumnIndex].Name == "Delete")
            {




                int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString());
                MySqlDataAdapter ada = new MySqlDataAdapter("select id as 'id',studentid as 'Student ID',firstname as'Firstname',lastname as 'Lastname',telno as 'Contact No',age as 'Age',sex as 'Gender',course as'Course' from patient where id= '" + id + "'", con);

                dataGridView1.CurrentCell = dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex];
                dataGridView1.Rows[e.RowIndex].Selected = true;
                dataGridView1.Focus();
                DataTable dt = new DataTable();
                ada.Fill(dt);
                dataGridView1.DataSource = dt;

                foreach (DataRow dr in dt.Rows)
                {
                    textBox1.Text = dr["id"].ToString();

                }

                if (MessageBox.Show("Are you sure you want to delete this?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {


                    try
                    {

                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = "delete from patient where id = '" + textBox1.Text + "'";


                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Item Deleted");
                        loadArhive();
                        con.Close();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    loadArhive();
                }
            }
            else if (dataGridView2.Columns[e.ColumnIndex].Name == "VIEW")
            {
                if (dataGridView2.Columns[e.ColumnIndex].Name == "VIEW")
                {

                    int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString());
                    MySqlDataAdapter ada = new MySqlDataAdapter("select * from dctd where id= '" + id + "'", con);

                    dataGridView2.CurrentCell = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dataGridView2.Rows[e.RowIndex].Selected = true;




                    DataTable dt = new DataTable();
                    ada.Fill(dt);
                    dataGridView2.DataSource = dt;

                    foreach (DataRow dr in dt.Rows)
                    {
                        frmViewPatient frm = new frmViewPatient();

                        //frm.label2.Text = dr["id"].ToString();
                        frm.ID = dt.Rows[0]["id"].ToString();
                        frm.Firstname = dt.Rows[0]["Firstname"].ToString();
                        frm.Lastname = dt.Rows[0]["Lastname"].ToString();
                        // frm.Middlename = dt.Rows[0]["middlename"].ToString();
                        frm.Studentid = dt.Rows[0]["Student ID"].ToString();
                        //frm.Birthday = dt.Rows[0]["Birthday"].ToString();
                        frm.Course = dt.Rows[0]["Course"].ToString();
                        frm.ShowDialog();

                    }
                }
                else
                {
                    Load3();
                }



            }
        }

        private void dataGridView2_CellContentClick_3(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView2.Columns[e.ColumnIndex].Name == "Delete")
            {




                int id = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["id"].Value.ToString());
                MySqlDataAdapter ada = new MySqlDataAdapter("select id as 'id',studentid as 'Student ID',firstname as'Firstname',lastname as 'Lastname',telno as 'Contact No',age as 'Age',sex as 'Gender',course as'Course' from patient where id= '" + id + "'", con);

                dataGridView2.CurrentCell = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex];
                dataGridView2.Rows[e.RowIndex].Selected = true;
                dataGridView2.Focus();
                DataTable dt = new DataTable();
                ada.Fill(dt);
                dataGridView2.DataSource = dt;

                foreach (DataRow dr in dt.Rows)
                {
                    textBox1.Text = dr["id"].ToString();

                }

                if (MessageBox.Show("Are you sure you want to delete this?", "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {


                    try
                    {

                        MySqlCommand cmd = new MySqlCommand();
                        cmd.Connection = con;
                        cmd.CommandText = "delete from dctd where id = '" + textBox1.Text + "'";


                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Item Deleted");
                        con.Close();
                        Load3();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    Load3();
                }
            }
            else if (dataGridView2.Columns[e.ColumnIndex].Name == "PROFILE")
            {
                if (dataGridView2.Columns[e.ColumnIndex].Name == "PROFILE")
                {

                    int id = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["id"].Value.ToString());
                    MySqlDataAdapter ada = new MySqlDataAdapter("select id as 'id',studentid as 'Student ID',firstname as'Firstname',lastname as 'Lastname',telno as 'Contact No',age as 'Age',sex as 'Gender',course as'Course' from patient where id= '" + id + "'", con);

                    dataGridView2.CurrentCell = dataGridView2.Rows[e.RowIndex].Cells[e.ColumnIndex];
                    dataGridView2.Rows[e.RowIndex].Selected = true;




                    DataTable dt = new DataTable();
                    ada.Fill(dt);
                    dataGridView2.DataSource = dt;

                    foreach (DataRow dr in dt.Rows)
                    {
                        frmViewPatient frm = new frmViewPatient();

                        //frm.label2.Text = dr["id"].ToString();
                        frm.ID = dt.Rows[0]["id"].ToString();
                        frm.Firstname = dt.Rows[0]["Firstname"].ToString();
                        frm.Lastname = dt.Rows[0]["Lastname"].ToString();
                        // frm.Middlename = dt.Rows[0]["middlename"].ToString();
                        frm.Studentid = dt.Rows[0]["Student ID"].ToString();
                        //frm.Birthday = dt.Rows[0]["Birthday"].ToString();
                        frm.Course = dt.Rows[0]["Course"].ToString();
                        frm.ShowDialog();

                    }
                }
                else
                {
                    Load3();
                }



            }
        }

        private void bunifuFlatButton11_Click(object sender, EventArgs e)
        {
            search();
        }
        public void search()
        {
             /*try
             {
                 MySqlDataAdapter ada = new MySqlDataAdapter("select dctd.date,patient.studentid,patient.id,patient.firstname,patient.lastname,dctd.doctor,dctd.treatment, dctd.complaints, dctd.category from patient INNER JOIN dctd ON patient.id = dctd.id where  studentid = '" + searchbox.Text + "' OR dctd.complaints = '" + searchbox.Text + "' ", con);
                 //SELECT dctd.date, patient.studentid,dctd.doctor,dctd.treatment,dctd.complaints,dctd.id from patient INNER JOIN dctd ON patient.id = dctd.id
                 DataTable dt = new DataTable();
                 ada.Fill(dt);
                 dataGridView1.DataSource = dt;
                 label2.Text = dataGridView1.RowCount.ToString();
                 result.Visible = true;
                 result.Text = "Showing: " + dataGridView1.RowCount.ToString() + " results";
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message);

             }*/

            MySqlConnection con = new MySqlConnection("server = localhost; database = nuclinic; username = root; password = ; Convert Zero Datetime=True");
            MySqlCommand command = new MySqlCommand("SELECT dctd.date, patient.studentid,dctd.id,patient.firstname,patient.lastname,dctd.doctor,dctd.treatment,dctd.complaints,dctd.category from patient INNER JOIN dctd ON patient.id=dctd.id  Where category = @category || studentid= @studentid", con);
          
            command.Parameters.AddWithValue("@category", searchbox.Text);
            command.Parameters.AddWithValue("@studentid", searchbox.Text);

            //MySqlCommand command = new MySqlCommand("SELECT patient.lastname,patient.studentid,dctd.id,dctd.complaints,dctd.date from patient INNER JOIN dctd ON patient.id=dctd.id", con);
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView2.DataSource = dt;
            label35.Visible = true;
            label34.Visible = true;
            label34.Text = dataGridView2.RowCount.ToString();
            result.Visible = true;
            result.Text = "Showing: " + dataGridView2.RowCount.ToString() + " results";
        }

        private void bunifuFlatButton12_Click(object sender, EventArgs e)
        {
            panel8.Visible = false;
            line5.Visible = false;
            line4.Visible = false;
            panelArhive.Visible = false;
            picPatientList.Visible = false;
            pictureBox6.Visible = false;
            line3.Visible = false;
            panel7.Visible = false;
            panel1.Visible = true;
            panel1.Controls.Clear();
            WarehouseStockForm warehouseForm = new WarehouseStockForm();
            warehouseForm.TopLevel = false;
            warehouseForm.AutoScroll = true;
            panel1.Controls.Add(warehouseForm);
            warehouseForm.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            time.Text = DateTime.Now.ToLongTimeString();
            timer1.Start();
        }

        private new void Load1()
        {
        
            string query = "SELECT dctd.date, patient.studentid,dctd.doctor,dctd.treatment,dctd.complaints,dctd.id from patient INNER JOIN dctd ON patient.id=dctd.id  Where date BETWEEN @date AND @date2";

            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, con))
            {

                DataSet dset = new DataSet();

                adpt.Fill(dset);

                dataGridView1.DataSource = dset.Tables[0];


            }
            con.Close();
        }


        public void loadchart2()
        {
            String Conn1 = "server = localhost; database = nuclinic; username = root; password = ; Convert Zero Datetime=True";
            String query1 = "select * from patient_by_course";
            MySqlConnection myConn1 = new MySqlConnection(Conn1);
            MySqlCommand cmdDatabase1 = new MySqlCommand(query1, myConn1);
            MySqlDataReader myReader;

            try
            {
                myConn1.Open();
                myReader = cmdDatabase1.ExecuteReader();
                while (myReader.Read())
                {
                    this.chart2.Series["Data"].Points.AddXY(myReader.GetString("course"), myReader.GetUInt32("qty"));

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
      
    }
}
