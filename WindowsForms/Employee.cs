using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Configuration;
namespace WindowsForms
{
    public partial class Employee : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCS"].ToString());
        public Employee()
        {
            InitializeComponent();
            showEmployee();
            cbDesignation.Text = "Select Designation";
        }
        private void showEmployee()
        {
            string sql = "select eid,ename,age,eaddress,dname,salary from tbl_emp inner join designation on tbl_emp.designation=designation.did";
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                dgShowEmployee.DataSource = dt;
            }
        }
        private void clear()
        {
            txtName.Text = "";
            txtAge.Text = "";
            txtAddress.Text = "";
            txtSalary.Text = "";
        }
        private void btnAddEmp_Click(object sender, EventArgs e)
        {
            if(btnAddEmp.Text == "Update")
            {
                updateEmployee();
            }
            else 
            {
                AddEmployee();
            }
            
        }
        private void AddEmployee()
        {
            string sql = "insert into tbl_emp(ename,age,eaddress,designation,salary)values(@name,@age,@address,@designation,@salary)";
            SqlCommand cmd = new SqlCommand(sql, con);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@age", txtAge.Text.Trim());
            cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
            cmd.Parameters.AddWithValue("@designation", cbDesignation.SelectedValue);
            cmd.Parameters.AddWithValue("@salary", txtSalary.Text.Trim());
            int n = cmd.ExecuteNonQuery();
            if (n != 0)
            {
                MessageBox.Show("Register SuccessFully!");
                cbDesignation.Text = "Select Designation";
                showEmployee();
                clear();
            }
            else
            {
                MessageBox.Show("Failed!!");
            }
            con.Close();
        }
        private void updateEmployee()
        {
            string sql = "update tbl_emp set ename=@name,age=@age,eaddress=@address,designation=@designation,salary=@salary where eid='"+id+"'";
            SqlCommand cmd = new SqlCommand(sql, con);
            if (con.State == ConnectionState.Closed)
            {
                con.Open();
            }
            cmd.Parameters.AddWithValue("@name", txtName.Text.Trim());
            cmd.Parameters.AddWithValue("@age", txtAge.Text.Trim());
            cmd.Parameters.AddWithValue("@address", txtAddress.Text.Trim());
            cmd.Parameters.AddWithValue("@designation", cbDesignation.SelectedValue);
            cmd.Parameters.AddWithValue("@salary", txtSalary.Text.Trim());
            int n = cmd.ExecuteNonQuery();
            if (n != 0)
            {
                MessageBox.Show("Update SuccessFully!");
                cbDesignation.Text = "Select Designation";
                showEmployee();
                clear();
                btnAddEmp.Text = "Register";
            }
            else
            {
                MessageBox.Show("Failed!!");
            }
            con.Close();
        }

        private void cbDesignation_Click(object sender, EventArgs e)
        {
            string sql = "select * from designation";
            SqlCommand cmd = new SqlCommand(sql, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                cbDesignation.DisplayMember = "dname";
                cbDesignation.ValueMember = "did";
                cbDesignation.DataSource = dt;
            }
        }

        int id;
        private void dgShowEmployee_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgShowEmployee.Columns[e.ColumnIndex].HeaderText == "Edit")
            {
                int eid = Convert.ToInt32(dgShowEmployee.Rows[e.RowIndex].Cells["EID"].Value);
                string sql = "Select * from tbl_emp where eid='" + eid + "'";
                SqlCommand cmd = new SqlCommand(sql, con);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    id = Convert.ToInt32(dt.Rows[0][0].ToString());
                    txtName.Text = dt.Rows[0][1].ToString();
                    txtAge.Text = dt.Rows[0][2].ToString();
                    txtAddress.Text = dt.Rows[0][3].ToString();
                    cbDesignation.Text = dt.Rows[0][4].ToString();
                    txtSalary.Text = dt.Rows[0][5].ToString();
                    btnAddEmp.Text = "Update";
                }
            }
            else if (dgShowEmployee.Columns[e.ColumnIndex].HeaderText == "Delete")
            {
                if (con.State == ConnectionState.Closed)
                {
                    con.Open();
                }
                DialogResult confirm = MessageBox.Show("Are you Sure you want to Delete?(Y/N)","Message",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    int eid = Convert.ToInt32(dgShowEmployee.Rows[e.RowIndex].Cells["EID"].Value);
                    string sql = "delete from tbl_emp where eid='" + eid + "'";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    int n = cmd.ExecuteNonQuery();
                    if (n != 0)
                    {
                        MessageBox.Show("Delete SuccessFully!");
                        cbDesignation.Text = "Select Designation";
                        showEmployee();
                        clear();
                    }
                    else
                    {
                        MessageBox.Show("Failed!!");
                    }
                }
                con.Close();
            }
        }
    }
}
