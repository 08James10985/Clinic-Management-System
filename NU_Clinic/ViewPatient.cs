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
    public partial class frmViewPatient : Form
    {
        public frmViewPatient()
        {
            InitializeComponent();
            panel1.VerticalScroll.Value = 0;
        }
  
        private string id,firstname,lastname,middlename,studentid,sex,telno,address,course,age,status,nationality,birthday;

        MySqlConnection con = new MySqlConnection("server = localhost; database = nuclinic; username = root; password = ; convert zero datetime=True");
        public string ID
        {
            get { return id; }
            set { id = value; }
        }

        public string Firstname
        {
            get { return firstname; }
            set { firstname = value; }
        }

        public string Birthday
        {
            get { return birthday; }
            set { birthday = value; }
        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            button2();
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {
            button1();
        }

        public void button1()
        {
            bar1.Visible = true;
            bar2.Visible = false;
            bar3.Hide();
            panelBI.Visible = true;
            panelMH.Visible = false;
            panelPE.Visible = false;
            bar4.Visible = false;
            panelLast.Visible = false;

        }
        public void button2()
        {
            bar1.Visible = false;
            bar2.Visible = true;
            bar3.Hide();
            panelBI.Visible = false;
            panelMH.Visible = true;
            panelPE.Visible = false;
            bar4.Visible = false;
            panelLast.Visible = false;
        }
        public void button3()
        {
            bar1.Visible = false;
            bar2.Visible = false;
            bar3.Show();
            panelBI.Visible = false;
            panelMH.Visible = false;
            bar4.Visible = false;
            panelLast.Visible = false;

        }

        private void panelMH_Paint(object sender, PaintEventArgs e)
        {
            try
            {

                string query2 = "select * from medical_history where id = '" + id + "' ";
                MySqlDataAdapter data2 = new MySqlDataAdapter(query2, con);
                DataTable dt2 = new DataTable();
                data2.Fill(dt2);
                HOPI.Text = dt2.Rows[0]["HOPI"].ToString();
                Allergy.Text = dt2.Rows[0]["Allergy"].ToString();
                TB.Text = dt2.Rows[0]["TB"].ToString();
                DM.Text = dt2.Rows[0]["DM"].ToString();
                HA.Text = dt2.Rows[0]["HA"].ToString();
                HPN.Text = dt2.Rows[0]["HPN"].ToString();
                KD.Text = dt2.Rows[0]["KD"].ToString();
                GO.Text = dt2.Rows[0]["GO"].ToString();
                TB.Text = dt2.Rows[0]["TB"].ToString();
                Smoker.Text = dt2.Rows[0]["Smoker"].ToString();
                Alcohol.Text = dt2.Rows[0]["Alcoholic"].ToString();



            }
            catch (Exception ex)
            {
              //  MessageBox.Show(ex.Message);
                //con.Close();
            }

        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            panelPE.Visible = true;
            button3();
        }

        private void panelPE_Paint(object sender, PaintEventArgs e)
        {
            try
            {

                string query2 = "select * from physical_examination where id = '" + id + "' ";
                MySqlDataAdapter data2 = new MySqlDataAdapter(query2, con);
                DataTable dt2 = new DataTable();
                data2.Fill(dt2);
                label62.Text = dt2.Rows[0]["BP"].ToString();
                PR.Text = dt2.Rows[0]["PR"].ToString();
                Wt.Text = dt2.Rows[0]["Wt"].ToString();
                Ht.Text = dt2.Rows[0]["Ht"].ToString();
                Skin.Text = dt2.Rows[0]["Skin"].ToString();
                Eyes.Text = dt2.Rows[0]["Eyes"].ToString();
                OD.Text = dt2.Rows[0]["OS"].ToString();
                OS.Text = dt2.Rows[0]["OS"].ToString();
                Ears.Text = dt2.Rows[0]["Ears"].ToString();
                AD.Text = dt2.Rows[0]["AD"].ToString();
                AD1.Text = dt2.Rows[0]["AD1"].ToString();
                Nose.Text = dt2.Rows[0]["Nose"].ToString();
                Throat.Text = dt2.Rows[0]["Throat"].ToString();
                Neck.Text = dt2.Rows[0]["Neck"].ToString();
                Thorax.Text = dt2.Rows[0]["Thorax"].ToString();
                Heart.Text = dt2.Rows[0]["Heart"].ToString();
                Lungs.Text = dt2.Rows[0]["Lungs"].ToString();
                Abdomen.Text = dt2.Rows[0]["Abdomen"].ToString();
                Extremities.Text = dt2.Rows[0]["Extremities"].ToString();
                Deformities.Text = dt2.Rows[0]["Deformities"].ToString();
                Other.Text = dt2.Rows[0]["Other"].ToString();

            }
            catch (Exception ex)
            {
                //  MessageBox.Show(ex.Message);
                //con.Close();
            }
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            button4();
            DTDC();
        }
        public void DTDC()
        {
            try
            {
               // MySqlDataAdapter ada = new MySqlDataAdapter("select id as 'id',studentid as 'Student ID',firstname as'Firstname',lastname as 'Lastname',telno as 'Contact No',age as 'Age',sex as 'Gender',course as'Course' from patient where id= '" + id + "'", con);
                MySqlDataAdapter ada = new MySqlDataAdapter("select date as 'DATE',complaints as 'COMPLAINTS P.E. FINDINGS AND DIAGNOSIS',treatment as 'TREATMENT/REMARKS',doctor as 'DOCTOR/NURSE' from dctd where id= '" + no.Text + "'", con);
                DataTable dt = new DataTable();
                ada.Fill(dt);
                dataGridView1.DataSource = dt;
                label2.Text = dataGridView1.RowCount.ToString();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }
        public void button4()
        {
            bar4.Visible = true;
            bar1.Visible = false;
            bar2.Visible = false;
            bar3.Visible = false ;
            panelBI.Visible = false;
            panelMH.Visible = false;
            panelPE.Visible = false;
            panelLast.Visible = true;
        }

        private void panelLast_Paint(object sender, PaintEventArgs e)
        {
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.Columns[0].Width = 108;
            dataGridView1.Columns[1].Width = 300;
            dataGridView1.RowTemplate.MinimumHeight = 105;
            dataGridView1.CurrentCell = null;
       
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        public string Nationality
        {
            get { return nationality; }
            set { nationality = value; }
        }

        public string Status
        {
            get { return status; }
            set { status = value; }
        }



        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            frmPatient ma = new frmPatient();
            this.Hide();
        }

   



        public string Lastname
        {
            get { return lastname; }
            set { lastname = value; }
        }

        public string Middlename
        {
            get { return middlename; }
            set { middlename = value; }
        }

        public string Studentid
        {
            get { return studentid; }
            set { studentid = value; }
        }

        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        public string Telno
        {
            get { return telno; }
            set { telno = value; }
        }

        public string Address
        {
            get { return address; }
            set { address = value; }
        }

        public string Course
        {
            get { return course; }
            set { course = value; }
        }

        public string Age
        {
            get { return age; }
            set { age = value; }
        }




        private void frmViewPatient_Load_1(object sender, EventArgs e)
        {
            try
            {
                string query = "select * from patient where id = '" + id + "'&& lastname = '" + lastname + "' ";
                string query2 = "select * from medical_history where id = '" + id + "' ";
                MySqlDataAdapter data = new MySqlDataAdapter(query, con);
                MySqlDataAdapter data2 = new MySqlDataAdapter(query2, con);
                DataTable dt = new DataTable();
                DataTable dt2 = new DataTable();
                data.Fill(dt);
                data2.Fill(dt2);
                label2.Text = id;
                no.Text = dt.Rows[0]["id"].ToString();
                label3.Text = lastname + ", " + firstname + " " + middlename;
                label8.Text = dt.Rows[0]["studentid"].ToString();
                label9.Text = dt.Rows[0]["sex"].ToString();
                label10.Text = dt.Rows[0]["telno"].ToString();
                label11.Text = dt.Rows[0]["address"].ToString();
                //label12.Text = course;
                label14.Text = dt.Rows[0]["age"].ToString();
                label1.Text = dt.Rows[0]["status"].ToString();
                label21.Text = dt.Rows[0]["religion"].ToString();
                label16.Text = dt.Rows[0]["naionality"].ToString();
                Birthday = dt.Rows[0]["Birthday"].ToString();
                label18.Text = DateTime.Parse(birthday).ToString("d");
                label12.Text = dt.Rows[0]["course"].ToString();
                label26.Text = dt.Rows[0]["person_incase"].ToString();
                label27.Text = dt.Rows[0]["relation"].ToString();
                label28.Text = dt.Rows[0]["person_telno"].ToString();
         

            }
            catch (Exception ex)
            {
               MessageBox.Show(ex.Message);
               con.Close();
            }

































        }



    }
}
