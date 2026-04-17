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
    public partial class Course : Form
    {
        public Course()
        {
            InitializeComponent();
        }

        private void bunifuFlatButton10_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void Course_Load(object sender, EventArgs e)
        {
            Load1();
        }
        MySqlConnection con = new MySqlConnection("server = localhost; database = nuclinic; username = root; password = ; Convert Zero Datetime=True");
        private new void Load1()
        {

            string query = "select id,course,course_name from patient_by_course";

            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, con))
            {

                DataSet dset = new DataSet();

                adpt.Fill(dset);

                dataGridView1.DataSource = dset.Tables[0];


            }
            con.Close();
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            try
            {


                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;


                cmd.CommandText = "insert into patient_by_course(course,course_name)" +
                                                     "values(@course,@course_name)  ";

                cmd.Parameters.AddWithValue("@course", code.Text);
                cmd.Parameters.AddWithValue("@course_name", name.Text);
       




                con.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show("Record Successfully Added");
                con.Close();
                code.Text = "";
                name.Text = "";
;                Load1();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {




                int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString());
                MySqlDataAdapter ada = new MySqlDataAdapter("select id,course from patient_by_course where id= '" + id + "'", con);

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
                        cmd.CommandText = "delete from patient_by_course where id = '" + textBox1.Text + "'";


                        con.Open();
                        cmd.ExecuteNonQuery();
                        MessageBox.Show("Item Deleted");
                        con.Close();
                        Load1();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                else
                {
                    Load1();
                }
            }
        }
    }
}
