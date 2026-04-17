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
    public partial class frmAddPatient : Form
    {
        public frmAddPatient()
        {
            InitializeComponent();
            cmbCourse.BackColor = SystemColors.Control;
            combCourse();
        }

        public void combCourse()
        {
            string query2 = "select * from patient_by_course";
            MySqlDataAdapter data2 = new MySqlDataAdapter(query2, con);
            DataTable dt2 = new DataTable();
            data2.Fill(dt2);

            DataRow dr2 = dt2.NewRow();
            dr2["course"] = "Select Course";
            dt2.Rows.InsertAt(dr2, 0);
            cmbCourse.ValueMember = "id";
            cmbCourse.DisplayMember = "course";
            cmbCourse.DataSource = dt2;
        }
        public void DrawRectangleFloat(PaintEventArgs e)
        {

            // Create pen.
            Pen blackPen = new Pen(Color.Black, 3);

            // Create location and size of rectangle.
            float x = 100.0F;
            float y = 100.0F;
            float width = 200.0F;
            float height = 200.0F;

            // Draw rectangle to screen.
            e.Graphics.DrawRectangle(blackPen, x, y, width, height);
        }
        MySqlConnection con = new MySqlConnection("server= localhost; database = nuclinic; username=root; password=;Convert Zero Datetime=True");


        private void bunifuTileButton1_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("haha");
        }

        private void frmAddPatient_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                button1_Click(this, new EventArgs());
            }
        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {


            string tString = textBox8.Text;
            if (tString.Trim() == "") return;
            for (int i = 0; i < tString.Length; i++)
            {
                if (!char.IsNumber(tString[i]))
                {
                    MessageBox.Show("Student ID should only contain number");
                    textBox8.Text = "";
                    return;
                }

            }
            if (((TextBox)sender).TextLength == 4)
                textBox1.Focus();


        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string tString = textBox1.Text;
            if (tString.Trim() == "") return;
            for (int i = 0; i < tString.Length; i++)
            {
                if (!char.IsNumber(tString[i]))
                {
                    MessageBox.Show("Student ID should only contain number");
                    textBox1.Text = "";
                    return;
                }

            }
            if (((TextBox)sender).TextLength == 6)
                txtLastname.Focus();


        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            checkEmpty();
        }




        public void checkEmpty()
        {
            if (textBox1.Text == "" || textBox8.Text == "" || txtLastname.Text == "" || txtFirstname.Text == "" ||
                txtMi.Text == "" || txtAddress.Text == "" || txtTel.Text == "" || txtAge.Text == "" || cmbCourse.Text == ""
                || txtReligion.Text == "" || txtPerson.Text == "" || txtRelation.Text == "" || cmbNationality.Text == "" || cmbStatus.Text == "")
            {
                MessageBox.Show("Please Complete the form");
            }
            else
            try
            {
                string id1 = textBox1.Text;
                string id2 = textBox8.Text;

                string studentid = id2 + "-" + id1;
                label35.Text = studentid;
                string gender = null;
                if (rbMale.Checked)
                {
                    gender = "Male";
                }
                else if (rbFemale.Checked)
                {
                    gender = "Female";
                }

                MySqlCommand cmd = new MySqlCommand();
                MySqlCommand cmd2 = new MySqlCommand();
                cmd.Connection = con;
                cmd2.Connection = con;
                cmd2.CommandText = "UPDATE patient_by_course SET qty = qty + 1 WHERE course ='" + cmbCourse.Text + "'";
                cmd.CommandText = "insert into patient(studentid,firstname,lastname,middlename,address,telno,age,course,birthday,sex,religion,naionality,status,person_incase,relation,person_telno)" +
                                   "values(@studentid,@lastname,@firstname,@middlename,@address,@telno,@age,@course,@birthday,@sex,@religion,@naionality,@status,@person_incase,@relation,@person_telno)";

                cmd.Parameters.AddWithValue("@studentid", studentid);
                cmd.Parameters.AddWithValue("@lastname", txtLastname.Text);
                cmd.Parameters.AddWithValue("@firstname", txtFirstname.Text);
                cmd.Parameters.AddWithValue("@middlename", txtMi.Text);
                cmd.Parameters.AddWithValue("@address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@telno", txtTel.Text);
                cmd.Parameters.AddWithValue("@age", txtAge.Text);
                cmd.Parameters.AddWithValue("@course", cmbCourse.Text);
                cmd.Parameters.AddWithValue("@birthday", birthday.Value.Date);
                cmd.Parameters.AddWithValue("@sex", gender);
                cmd.Parameters.AddWithValue("@religion", txtReligion.Text);
                cmd.Parameters.AddWithValue("@naionality", cmbNationality.Text);
                cmd.Parameters.AddWithValue("@status", cmbStatus.Text);
                cmd.Parameters.AddWithValue("@person_incase", txtPerson.Text);
                cmd.Parameters.AddWithValue("@relation", txtRelation.Text);
                cmd.Parameters.AddWithValue("@person_telno", txtPersontel.Text);
              

                con.Open();
                cmd.ExecuteNonQuery();
                cmd2.ExecuteNonQuery();
                MessageBox.Show("Record Successfully Added");
                con.Close();
                clear();
                panelMH.Visible = true;
                panelBI.Visible = false;
                circle2.Visible = true;
                line2.Visible = true;
                bar1.Visible = false;
                bar2.Visible = true;
                panel1.Visible = true;

                try
                {
                    string query = "select * from patient where studentid = '" + label35.Text + "' ";
                    MySqlDataAdapter data = new MySqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    data.Fill(dt);

                    label36.Text = dt.Rows[0]["id"].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    con.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();
            }

        }

        private void txtSurname_TextChanged(object sender, EventArgs e)
        {
            string tString = txtLastname.Text;
            if (tString.Trim() == "") return;
            for (int i = 0; i < tString.Length; i++)
            {
                if (!char.IsLetter(tString[i]) && !char.IsWhiteSpace(tString[i]))
                {
                    MessageBox.Show("Surname should be letters only");
                    txtLastname.Text = "";
                    return;
                }
            }
        }

        private void txtFirstname_TextChanged(object sender, EventArgs e)
        {
            string tString = txtFirstname.Text;
            if (tString.Trim() == "") return;
            for (int i = 0; i < tString.Length; i++)
            {
                if (!char.IsLetter(tString[i]) && !char.IsWhiteSpace(tString[i]))
                {
                    MessageBox.Show("Firstname should be letters only");
                    txtFirstname.Text = "";
                    return;
                }
            }
        }

        private void txtMi_TextChanged(object sender, EventArgs e)
        {
            string tString = txtMi.Text;
            if (tString.Trim() == "") return;
            for (int i = 0; i < tString.Length; i++)
            {
                if (!char.IsLetter(tString[i]) && !char.IsWhiteSpace(tString[i]))
                {
                    MessageBox.Show("MI should be letters only");
                    txtMi.Text = "";
                    return;
                }
            }
        }

        private void txtTel_TextChanged(object sender, EventArgs e)
        {
            string tString = txtTel.Text;
            if (tString.Trim() == "") return;
            for (int i = 0; i < tString.Length; i++)
            {
                if (!char.IsNumber(tString[i]))
                {
                    MessageBox.Show("Contact number should only contain number");
                    txtTel.Text = "";
                    return;
                }

            }

        }

        private void txtAge_TextChanged(object sender, EventArgs e)
        {
            string tString = txtAge.Text;
            if (tString.Trim() == "") return;
            for (int i = 0; i < tString.Length; i++)
            {
                if (!char.IsNumber(tString[i]))
                {
                    MessageBox.Show("Age should only contain number");
                    txtAge.Text = "";
                    return;
                }

            }

        }
        public void clear()
        {
            textBox1.Clear();
            textBox8.Clear();
            txtLastname.Clear();
            txtFirstname.Clear();
            txtMi.Clear();
            txtAddress.Clear();
            txtTel.Clear();
            txtAge.Clear();
            rbMale.Checked = true;
            rbFemale.Checked = false;
            cmbCourse.SelectedIndex = -1;
            birthday.ResetText();
            txtPerson.Clear();
            txtPersontel.Clear();
            txtRelation.Clear();
            comboBox2.SelectedIndex = -1;
            cmbStatus.SelectedIndex = -1;
            cmbNationality.SelectedIndex = -1;
            txtReligion.Clear();
        }

        private void bunifuFlatButton3_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to clear all the fields?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                clear();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void bunifuFlatButton2_Click(object sender, EventArgs e)
        {

        }

        private void bunifuFlatButton1_Click(object sender, EventArgs e)
        {
            bar4.Visible = false;
            circle2.Visible = true;
            line2.Visible = true;
            panelBI.Visible = false;
            panelMH.Visible = true;
            bunifuFlatButton1.ForeColor = Color.FromArgb(36, 116, 166);
            bar1.Visible = false;
            bar2.Visible = true;
            panel1.Visible = true;
            panelPE.Visible = false;
            line3.Visible = false;
            circle3.Visible = false;
            bar3.Visible = false;
            line4.Visible = false;
            circle4.Visible = false;
            panelLast.Visible = false;

        }

        private void bunifuFlatButton2_Click_1(object sender, EventArgs e)
        {
            circle2.Visible = false;
            line2.Visible = false;
            panelBI.Visible = true;
            panelMH.Visible = false;
            panelPE.Visible = false;
            bar1.Visible = true;
            bar2.Visible = false;
            panel1.Visible = false;
            bar4.Visible = false;
            line3.Visible = false;
            circle3.Visible = false;
            bar3.Visible = false;
            line4.Visible = false;
            circle4.Visible = false;
            panelLast.Visible = false;
        }

        private void bunifuFlatButton4_Click(object sender, EventArgs e)
        {
            panelBI.Visible = false;
            panelPE.Visible = true;
            panelBI.Hide();
            bar4.Visible = false;
            line3.Visible = true;
            circle3.Visible = true;
            bar3.Visible = true;
            bar2.Visible = false;
            circle2.Visible = true;
            line2.Visible = true;
            bar1.Visible = false;
            line4.Visible = false;
            circle4.Visible = false;
            panelLast.Visible = false;





        }

        private void bunifuFlatButton5_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void bunifuFlatButton6_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to clear all the fields?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                clearPanelMH();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        public void clearPanelMH()
        {
            HOPI.Checked = false;
            Allergy.Checked = false;
            TB.Checked = false;
            DM.Checked = false;
            HA.Checked = false;
            HPN.Checked = false;
            KD.Checked = false;
            GO.Checked = false;
            Smoker.Checked = false;
            Alcoholic.Checked = false;
            txtHOPI.Text = "";
            txtHOPI.Text = "";
            txtAllergy.Text = "";
            txtTB.Text = "";
            txtDM.Text = "";
            txtHA.Text = "";
            txtHPN.Text = "";
            txtKD.Text = "";
            txtGO.Text = "";
            txtSmoker.Text = "";
            txtAlcohol.Text = "";

        }

        private void txtReligion_TextChanged(object sender, EventArgs e)
        {
            string tString = txtReligion.Text;
            if (tString.Trim() == "") return;
            for (int i = 0; i < tString.Length; i++)
            {
                if (!char.IsLetter(tString[i]) && !char.IsWhiteSpace(tString[i]))
                {
                    MessageBox.Show("Use letters only");
                    txtReligion.Text = "";
                    return;
                }
            }
        }

        private void txtPerson_TextChanged(object sender, EventArgs e)
        {
            string tString = txtPerson.Text;
            if (tString.Trim() == "") return;
            for (int i = 0; i < tString.Length; i++)
            {
                if (!char.IsLetter(tString[i]) && !char.IsWhiteSpace(tString[i]))
                {
                    MessageBox.Show("Use letters only");
                    txtPerson.Text = "";
                    return;
                }
            }
        }

        private void txtRelation_TextChanged(object sender, EventArgs e)
        {
            string tString = txtRelation.Text;
            if (tString.Trim() == "") return;
            for (int i = 0; i < tString.Length; i++)
            {
                if (!char.IsLetter(tString[i]) && !char.IsWhiteSpace(tString[i]))
                {
                    MessageBox.Show("Use letters only");
                    txtRelation.Text = "";
                    return;
                }
            }
        }

        private void txtPersontel_TextChanged(object sender, EventArgs e)
        {
            string tString = txtPersontel.Text;
            if (tString.Trim() == "") return;
            for (int i = 0; i < tString.Length; i++)
            {
                if (!char.IsNumber(tString[i]))
                {
                    MessageBox.Show("Contact number should only contain number");
                    txtPersontel.Text = "";
                    return;
                }

            }
        }
        string cHOPI, cAllergy, cTB, cDM, cHA, cHPN, cKD, cGO, cSmoker, cAlcoholic;


        private void GO_OnChange(object sender, EventArgs e)
        {
            if (GO.Checked == true)
            {
                cGO = "YES ";
            }
            else if (GO.Checked == false)
                cGO = "";
        }

        private void Smoker_OnChange(object sender, EventArgs e)
        {
            if (Smoker.Checked == true)
            {
                cSmoker = "YES ";
            }
            else if (Smoker.Checked == false)
                cSmoker = "";
        }

        private void Alcoholic_OnChange(object sender, EventArgs e)
        {
            if (Alcoholic.Checked == true)
            {
                cAlcoholic = "YES ";
            }
            else if (Alcoholic.Checked == false)
                cAlcoholic = "";
        }

        private void save_Click(object sender, EventArgs e)
        {
            if (BP.Text == "" || PR.Text == "" || Wt.Text == ""||Ht.Text==""||Skin.Text==""||Eyes.Text==""||OD.Text==""||OS.Text==""||Ears.Text==""||AD.Text==""||AD1.Text==""||Nose.Text==""||Throat.Text==""||Neck.Text==""||Thorax.Text==""||Heart.Text==""||
                Lungs.Text==""||Abdomen.Text==""||Extremities.Text==""||Deformities.Text=="")
            {
                DialogResult dialogResult = MessageBox.Show("The form is incomplete, do you want to proceed?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    executePE();
                }
                else if (dialogResult == DialogResult.No)
                {
                    //do something else
                }
            }
            else
                executePE();

        }
        public void executePE()
        {
            try
            {
                string query = "select * from patient where studentid = '" + label35.Text + "' ";
                MySqlDataAdapter data = new MySqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                data.Fill(dt);

                label36.Text = dt.Rows[0]["id"].ToString();
                string id = label36.Text;

                MySqlCommand cmd = new MySqlCommand();
                cmd.Connection = con;
                /* cmd.CommandText = "insert into physical_examination(id,BP,PR)" +
                                                      "values(@id,@BP,@PR)  ";*/

                cmd.CommandText = "insert into physical_examination(id,BP,PR,Wt,Ht,Skin,Eyes,OD,OS,Ears,AD,AD1,Nose,Throat,Neck,Thorax,Heart,Lungs,Abdomen,Extremities,Deformities,Other)" +
                                                     "values(@id,@BP,@PR,@Wt,@Ht,@Skin,@Eyes,@OD,@OS,@Ears,@AD,@AD1,@Nose,@Throat,@Neck,@Thorax,@Heart,@Lungs,@Abdomen,@Extremities,@Deformities,@Other)  ";
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@BP", BP.Text);
                cmd.Parameters.AddWithValue("@PR", PR.Text);
                cmd.Parameters.AddWithValue("@Wt", Wt.Text);
                cmd.Parameters.AddWithValue("@Ht", Ht.Text);
                cmd.Parameters.AddWithValue("@Skin", Skin.Text);
                cmd.Parameters.AddWithValue("@Eyes", Eyes.Text);
                cmd.Parameters.AddWithValue("@OD", OD.Text);
                cmd.Parameters.AddWithValue("@OS", OS.Text);
                cmd.Parameters.AddWithValue("@Ears", Ears.Text);
                cmd.Parameters.AddWithValue("@AD", AD.Text);
                cmd.Parameters.AddWithValue("@AD1", AD1.Text);
                cmd.Parameters.AddWithValue("@Nose", Nose.Text);
                cmd.Parameters.AddWithValue("@Throat", Throat.Text);
                cmd.Parameters.AddWithValue("@Neck", Neck.Text);
                cmd.Parameters.AddWithValue("@Thorax", Thorax.Text);
                cmd.Parameters.AddWithValue("@Heart", Heart.Text);
                cmd.Parameters.AddWithValue("@Lungs", Lungs.Text);
                cmd.Parameters.AddWithValue("@Abdomen", Abdomen.Text);
                cmd.Parameters.AddWithValue("@Extremities", Extremities.Text);
                cmd.Parameters.AddWithValue("@Deformities", Deformities.Text);
                cmd.Parameters.AddWithValue("@Other", Other.Text);


                con.Open();
                cmd.ExecuteNonQuery();
                MessageBox.Show("Record Successfully Added");
                con.Close();
                clearPE();
                line4.Visible = true;
                circle4.Visible = true;
                panelLast.Visible = true;
                bar4.Visible = true;
                bar3.Visible = false;
                bar2.Visible = false;
                bar1.Visible = false;


            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                con.Close();

            }

        }
        public void clearPE()
        {
            BP.Clear();
            PR.Clear();
            Wt.Clear();
            Ht.Clear();
            Skin.Clear();
            Eyes.Clear();
            OD.Clear();
            OS.Clear();
            Ears.Clear();
            AD.Clear();
            AD1.Clear();
            Throat.Clear();
            Neck.Clear();
            Thorax.Clear();
            Heart.Clear();
            Lungs.Clear();
            Abdomen.Clear();
            Extremities.Clear();
            Deformities.Clear();
            Other.Clear();
            Nose.Clear();


        }

        private void bunifuFlatButton9_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want to clear all the fields?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                clearPE();
            }
            else if (dialogResult == DialogResult.No)
            {
                //do something else
            }
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            panelBI.Visible = false;
            panelPE.Visible = false;
            pictureBox3.Visible = true;
            panelMH.Visible = false;
            line3.Visible = true;
            circle3.Visible = true;
            bar3.Visible = false;
            bar2.Visible = false;
            circle2.Visible = true;
            line2.Visible = false;
            bar1.Visible = false;
            bar4.Visible = true;
            line2.Visible = true;
            line4.Visible = true;
            circle4.Visible = true;
            panelLast.Visible = true;
        }

        private void KD_OnChange(object sender, EventArgs e)
        {
            if (KD.Checked == true)
            {
                cKD = "YES ";
            }
            else if (KD.Checked == false)
                cKD = "";
        }

        private void HPN_OnChange(object sender, EventArgs e)
        {
            if (HPN.Checked == true)
            {
                cHPN = "YES ";
            }
            else if (HPN.Checked == false)
                cHPN = "";
        }

        private void HA_OnChange(object sender, EventArgs e)
        {
            if (HA.Checked == true)
            {
                cHA = "YES ";
            }
            else if (HA.Checked == false)
                cHA = "";
        }

        private void DM_OnChange(object sender, EventArgs e)
        {
            if (DM.Checked == true)
            {
                cDM = "YES ";
            }
            else if (DM.Checked == false)
                cDM = "";
        }

        private void TB_OnChange(object sender, EventArgs e)
        {
            if (TB.Checked == true)
            {
                cTB = "YES ";
            }
            else if (TB.Checked == false)
                cTB = "";
        }

        private void Allergy_OnChange(object sender, EventArgs e)
        {
            if (Allergy.Checked == true)
            {
                cAllergy = "YES ";
            }
            else if (Allergy.Checked == false)
                cAllergy = "";
        }

        private void frmAddPatient_Load(object sender, EventArgs e)
        {
  
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
        public void cleardctd()
        {
            complaints.Clear();
            treatment.Clear();
            cmbComplaint.Text = "Select Category";
            comboBox1.Text = "";

        }

        public void runLast()
        {
            try
            {
                //string commmandLine = "UPDATE category SET number = number + 1 WHERE id = @id";
                string query = "select * from patient where studentid = '" + label35.Text + "' ";
                MySqlDataAdapter data = new MySqlDataAdapter(query, con);
                DataTable dt = new DataTable();
                data.Fill(dt);

                label36.Text = dt.Rows[0]["id"].ToString();
                string id = label36.Text;

                MySqlCommand cmd = new MySqlCommand();
                MySqlCommand cmd2 = new MySqlCommand();
                cmd.Connection = con;
                cmd2.Connection = con;

                cmd.CommandText = "insert into dctd(id,date,complaints,category,treatment,doctor)" +
                                                     "values(@id,@date,@complaints,@category,@treatment,@doctor)  ";
                cmd.Parameters.AddWithValue("@id", id);
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
                clearPE();
                cleardctd();
                cmbComplaint.Text = "Select Category";
                




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
        public void clearTreatment()
        {
            complaints.Text = "";
            treatment.Text = "";
            cmbComplaint.Text = "";
            comboBox1.Text = "";
            cmbComplaint.Text = "Select Category";

        }

        private void bunifuFlatButton10_Click(object sender, EventArgs e)
        {
            clearTreatment();
        }

        private void panelBI_Paint(object sender, PaintEventArgs e)
        {
          
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panelLast_Paint(object sender, PaintEventArgs e)
        {
            date.Value = DateTime.Now;
        }


        private void bunifuFlatButton7_Click(object sender, EventArgs e)
        {
            if (HOPI.Checked == false)
            {
                cHOPI = "";
            }
            if (Allergy.Checked == false)
            {
                cAllergy = "";
            }
            if (TB.Checked == false)
                cTB = "";
            if (DM.Checked == false)
                cDM = "";
            if (HA.Checked == false)
                cHA = "";
            if (HPN.Checked == false)
                cHPN = "";
            if (KD.Checked == false)
                cKD = "";
            if (GO.Checked == false)
                cGO = "";
            if (Smoker.Checked == false)
                cSmoker = "";
            if (Alcoholic.Checked == false)
                cAlcoholic = "";


            if (label35.Text == "0" || label36.Text == "0")
            {
                MessageBox.Show("Please Add Patient First");
            }
            else
            {

                try
                {
                    string query = "select * from patient where studentid = '" + label35.Text + "' ";
                    MySqlDataAdapter data = new MySqlDataAdapter(query, con);
                    DataTable dt = new DataTable();
                    data.Fill(dt);

                    label36.Text = dt.Rows[0]["id"].ToString();
                    string id = label36.Text;

                    MySqlCommand cmd = new MySqlCommand();
                    cmd.Connection = con;
                    cmd.CommandText = "insert into medical_history(id,HOPI,Allergy,TB,DM,HA,HPN,KD,GO,Smoker,Alcoholic)" +
                                                          "values(@id,@HOPI,@Allergy,@TB,@DM,@HA,@HPN,@KD,@GO,@Smoker,@Alcoholic)  ";
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.Parameters.AddWithValue("@HOPI", cHOPI  + txtHOPI.Text);
                    cmd.Parameters.AddWithValue("@Allergy", cAllergy  + txtAllergy.Text);
                    cmd.Parameters.AddWithValue("@TB", cTB  + txtTB.Text);
                    cmd.Parameters.AddWithValue("DM", cDM +  txtDM.Text);
                    cmd.Parameters.AddWithValue("@HA", cHA +  txtHA.Text);
                    cmd.Parameters.AddWithValue("@HPN", cHPN  + txtHPN.Text);
                    cmd.Parameters.AddWithValue("@KD", cKD  + txtKD.Text);
                    cmd.Parameters.AddWithValue("@GO", cGO  + txtGO.Text);
                    cmd.Parameters.AddWithValue("@Smoker", cSmoker + txtSmoker.Text);
                    cmd.Parameters.AddWithValue("@Alcoholic", cAlcoholic +  txtAlcohol.Text);

                    con.Open();
                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Record Successfully Added");
                 
                    con.Close();
                    clearPanelMH();
                    panelPE.Visible = true;
                    panelMH.Visible = false;
                    bar1.Visible = false;
                    line3.Visible = true;
                    circle3.Visible = true;
               
                    bar4.Visible = false;
                    bar3.Visible = true;
                    bar2.Visible = false;
                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    con.Close();
                    clearPanelMH();
                }
            }
        }
  
     
        private string firstname, lastname, middlename, username,fullname;

     

        public string Fullname
        {
            get { return fullname; }
            set { fullname = value; }
        }
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
      

        private void HOPI_OnChange(object sender, EventArgs e)
        {
            if (HOPI.Checked == true)
            {
                cHOPI = "YES ";
            }
            else if(HOPI.Checked == false)
                cHOPI = "";
        }
    }
}
