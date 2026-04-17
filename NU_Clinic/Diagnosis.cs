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

namespace NU_Clinic
{
    public partial class Diagnosis : Form
    {
        public Diagnosis()
        {
            InitializeComponent();
        }
        MySqlConnection con = new MySqlConnection("server= localhost; database = nuclinic; username=root; password=;Convert Zero Datetime=True");

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }
        public void runLast()
        {
            try
            {
               
                MySqlCommand cmd = new MySqlCommand();
                MySqlCommand cmd2 = new MySqlCommand();
                cmd.Connection = con;
                cmd2.Connection = con;

                cmd.CommandText = "insert into dctd(id,date,complaints,category,treatment,doctor)" +
                                                     "values(@id,@date,@complaints,@category,@treatment,@doctor)  ";
                cmd.Parameters.AddWithValue("@id", id.Text);
                cmd.Parameters.AddWithValue("@date", date.Value.Date);
                cmd.Parameters.AddWithValue("@complaints", complaints.Text);
                cmd.Parameters.AddWithValue("@category", cmbComplaint.Text);
                cmd.Parameters.AddWithValue("@treatment", treatment.Text);
                cmd.Parameters.AddWithValue("@doctor", comboBox1.Text);
                cmd2.CommandText = "UPDATE category SET number = number + 1 WHERE complaints_cat ='" + cmbComplaint.Text + "'";




                con.Open();
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                MessageBox.Show("Record Successfully Added");
                con.Close();
                
                cmbComplaint.Text = "Select Category";
                clearTreatment();




            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();

            }
        }

        private void bunifuFlatButton8_Click(object sender, EventArgs e)
        {
            if (cmbComplaint.Text == "Select Category")
            {
                MessageBox.Show("Please Select a Category");
            }
            else if (comboBox1.Text == "")
            {
                MessageBox.Show("Please Select Doctor");
            }
            else if (complaints.Text == "" || treatment.Text == "")
            {
                DialogResult dialogResult = MessageBox.Show("The form is incomplete, do you want to proceed?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    runLast();
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
            else
                runLast();


        }

        private void Diagnosis_Load(object sender, EventArgs e)
        {
        
                date.Value = DateTime.Now;
            string query = "select username,id,CONCAT(lastname,', ', firstname) as Name from user";
            MySqlDataAdapter data = new MySqlDataAdapter(query, con);
            DataTable dt = new DataTable();
            data.Fill(dt);

            DataRow dr = dt.NewRow();
            dr["username"] = "Select Doctor";
            dt.Rows.InsertAt(dr, 0);
            comboBox1.ValueMember = "id";
            comboBox1.DisplayMember = "Name";
            comboBox1.DataSource = dt;

            string query2 = "select id,complaints_cat,number from category";
            MySqlDataAdapter data2 = new MySqlDataAdapter(query2, con);
            DataTable dt2 = new DataTable();
            data2.Fill(dt2);

            DataRow dr2 = dt2.NewRow();
            dr2["complaints_cat"] = "Select Category";
            dt2.Rows.InsertAt(dr2, 0);
            cmbComplaint.ValueMember = "id";
            cmbComplaint.DisplayMember = "complaints_cat";
            cmbComplaint.DataSource = dt2;
        }

        private void bunifuFlatButton10_Click(object sender, EventArgs e)
        {
            clearTreatment();
        }
        public void clearTreatment()
        {
            complaints.Text = "";
            treatment.Text = "";
            cmbComplaint.Text = "Select Category";
            comboBox1.Text = "";
            id.Text = "";

        }
    }
}
