using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Student_Registration_System
{
    public partial class Form1 : Form
    {
        string connectionString = "server=localhost;database=studentdb;uid=root;pwd=pat102805;";

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                MySqlDataAdapter da = new MySqlDataAdapter("SELECT * FROM students", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvStudents.DataSource = dt;
            }

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "INSERT INTO students (FullName, Email, Phone, Address) VALUES (@FullName, @Email, @Phone, @Address)";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@FullName", txtName.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("Student Added Successfully!");
            LoadData();
            ClearFields();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null)
            {
                MessageBox.Show("Please select a record to update.");
                return;
            }

            int id = Convert.ToInt32(dgvStudents.CurrentRow.Cells["Id"].Value);

            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "UPDATE students SET FullName=@FullName, Email=@Email, Phone=@Phone, Address=@Address WHERE Id=@Id";
                MySqlCommand cmd = new MySqlCommand(query, con);
                cmd.Parameters.AddWithValue("@FullName", txtName.Text);
                cmd.Parameters.AddWithValue("@Email", txtEmail.Text);
                cmd.Parameters.AddWithValue("@Phone", txtPhone.Text);
                cmd.Parameters.AddWithValue("@Address", txtAddress.Text);
                cmd.Parameters.AddWithValue("@Id", id);
                cmd.ExecuteNonQuery();
            }
            MessageBox.Show("Student Updated Successfully!");
            LoadData();
            ClearFields();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvStudents.CurrentRow == null)
            {
                MessageBox.Show("Please select a record to delete.");
                return;
            }

            int id = Convert.ToInt32(dgvStudents.CurrentRow.Cells["Id"].Value);

            DialogResult result = MessageBox.Show("Are you sure you want to delete this record?",
                                                  "Confirm Delete",
                                                  MessageBoxButtons.YesNo,
                                                  MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                using (MySqlConnection con = new MySqlConnection(connectionString))
                {
                    con.Open();
                    string query = "DELETE FROM students WHERE Id=@Id";
                    MySqlCommand cmd = new MySqlCommand(query, con);
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
                MessageBox.Show("Student Deleted Successfully!");
                LoadData();
                ClearFields();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            SearchData(txtSearch.Text.Trim());
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text))
                LoadData();
            else
                SearchData(txtSearch.Text.Trim());
        }

        private void SearchData(string keyword)
        {
            using (MySqlConnection con = new MySqlConnection(connectionString))
            {
                con.Open();
                string query = "SELECT * FROM students WHERE FullName LIKE @keyword OR Email LIKE @keyword OR Phone LIKE @keyword OR Address LIKE @keyword";
                MySqlDataAdapter da = new MySqlDataAdapter(query, con);
                da.SelectCommand.Parameters.AddWithValue("@keyword", "%" + keyword + "%");
                DataTable dt = new DataTable();
                da.Fill(dt);
                dgvStudents.DataSource = dt;
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void ClearFields()
        {
            txtName.Text = "";
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            txtSearch.Text = "";
        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                txtName.Text = dgvStudents.Rows[e.RowIndex].Cells["FullName"].Value.ToString();
                txtEmail.Text = dgvStudents.Rows[e.RowIndex].Cells["Email"].Value.ToString();
                txtPhone.Text = dgvStudents.Rows[e.RowIndex].Cells["Phone"].Value.ToString();
                txtAddress.Text = dgvStudents.Rows[e.RowIndex].Cells["Address"].Value.ToString();
            }
        }
    }
}
