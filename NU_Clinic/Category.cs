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
    public partial class Category : Form
    {
        public Category()
        {
            InitializeComponent();
            dataGridView1.CurrentCell = null;
            dataGridView1.ClearSelection();
        }
        MySqlConnection con = new MySqlConnection("server = localhost; database = nuclinic; username = root; password = ; Convert Zero Datetime=True");

        private void bunifuFlatButton10_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private new void Load1()
        {

            string query = "select id,complaints_cat as 'Complaints' from category";

            using (MySqlDataAdapter adpt = new MySqlDataAdapter(query, con))
            {

                DataSet dset = new DataSet();

                adpt.Fill(dset);

                dataGridView1.DataSource = dset.Tables[0];


            }
            con.Close();
        }

        private void Category_Load(object sender, EventArgs e)
        {
            Load1();
        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            try
            {


                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;


                cmd.CommandText = "insert into category(complaints_cat)" +
                                                     "values(@complaints_cat)  ";

                cmd.Parameters.AddWithValue("@complaints_cat", cat.Text);
            




                con.Open();
                cmd.ExecuteNonQuery();

                MessageBox.Show("Record Successfully Added");
                con.Close();
                cat.Text = "";
                Load1();

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
                MySqlDataAdapter ada = new MySqlDataAdapter("select id,complaints_cat as 'Category' from category where id= '" + id + "'", con);

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
                        cmd.CommandText = "delete from category where id = '" + textBox1.Text + "'";


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
