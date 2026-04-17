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
    public partial class frmPatient : Form
    {
        public frmPatient()
        {
            InitializeComponent();
            Load(); dataGridView1.FirstDisplayedScrollingRowIndex = 2;
            label2.Text = dataGridView1.RowCount.ToString();
        }


        MySqlConnection con = new MySqlConnection("server = localhost; database = nuclinic; username = root; password = ; Convert Zero Datetime=True");
    
        

        private new void Load()
        {
            txtSearch.Text = "";
            result.Visible = false;
            string query = "select id as 'id',studentid as 'Student ID',firstname as'Firstname',lastname as 'Lastname',telno as 'Contact No',age as 'Age',sex as 'Gender',course as'Course' from patient";

            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, con))
            {

                DataSet dset = new DataSet();

                adpt.Fill(dset);

                dataGridView1.DataSource = dset.Tables[0];


            }
            con.Close();
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
                        con.Close();
                        Load();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    Load();
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
                    Load();
                }
                 
               

            }
        }
        
        private void txtSearch_Enter(object sender, EventArgs e)
        {
            if (txtSearch.Text == "Search...")
            {
                txtSearch.Text = "";
                txtSearch.ForeColor = Color.Black;
            }
        }

        private string firstname;
        public string Firstname
        {
            get { return firstname; }
            set { firstname = value; }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            result.Visible = false;
            txtSearch.Text = "";
            Load();
            label2.Text = dataGridView1.RowCount.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddPatient frm = new frmAddPatient();
            frm.ShowDialog();
            
  
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {

            search();

        }
        public void search()
        {
            try
            {
                MySqlDataAdapter ada = new MySqlDataAdapter("select lastname as 'Lastname',firstname as 'Firstname', id ,studentid as 'Student ID' , telno as 'Contact No' ,course as 'Course'  from patient where firstname = '" + txtSearch.Text + "' OR course = '" + txtSearch.Text + "' OR studentid = '" + txtSearch.Text + "' OR id = '" + txtSearch.Text + "' OR sex = '" + txtSearch.Text + "' OR lastname = '" + txtSearch.Text + "' ", con);

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
                
            }
        }
      

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            result.Visible = false;
            txtSearch.Text = "";
            this.dataGridView1.Sort(this.dataGridView1.Columns["Lastname"], ListSortDirection.Ascending);
            //string query = "select id as 'id',studentid as 'Student ID',CONCAT(lastname,', ', firstname) as Name,telno as 'Contact No', birthday as 'Birthday',age as 'Age',sex as 'Gender',course as'Course' from patient ORDER BY lastname";

            /*using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, con))
            {

                DataSet dset = new DataSet();

                adpt.Fill(dset);

                dataGridView1.DataSource = dset.Tables[0];
                label2.Text = dataGridView1.RowCount.ToString();

            }
            con.Close();*/
        }

       
    }
}
