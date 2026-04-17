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
    public partial class DoctorList : Form
    {
        public DoctorList()
        {
            InitializeComponent();
            dataGridView1.CurrentCell = null;
            dataGridView1.ClearSelection();
        }
        MySqlConnection con = new MySqlConnection("server = localhost; database = nuclinic; username = root; password = ; Convert Zero Datetime=True");

        private void DoctorList_Load(object sender, EventArgs e)
        {
            Load1();
        }

        private new void Load1()
        {

            string query = "select id,firstname,lastname,middlename from user";

            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, con))
            {

                DataSet dset = new DataSet();

                adpt.Fill(dset);

                dataGridView1.DataSource = dset.Tables[0];


            }
            con.Close();
        }

        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void bunifuFlatButton10_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            add();
            Load1();
        }

        public void add()
        {
            try
            {


                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;


                cmd.CommandText = "insert into user(firstname,lastname,middlename,username,password)" +
                                                     "values(@firstname,@lastname,@middlename,@username,@password)  ";

                cmd.Parameters.AddWithValue("@firstname", firstname.Text);
                cmd.Parameters.AddWithValue("@lastname", lastname.Text);
                cmd.Parameters.AddWithValue("@middlename", mi.Text);
                cmd.Parameters.AddWithValue("@username", username.Text);
                cmd.Parameters.AddWithValue("@password", password.Text);




                con.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show("Record Successfully Added");
                con.Close();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();

            }
        }

        private void username_TextChanged(object sender, EventArgs e)
        {

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "Delete")
            {




                int id = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["id"].Value.ToString());
                MySqlDataAdapter ada = new MySqlDataAdapter("select * from user where id= '" + id + "'", con);

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
                        cmd.CommandText = "delete from user where id = '" + textBox1.Text + "'";


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
