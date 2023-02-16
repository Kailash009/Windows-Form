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
    public partial class Form1 : Form
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["DBCS"].ToString());
        public Form1()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string sql = "insert into tbl_register(sname,age,saddress) values(@name,@age,@address)";
            SqlCommand cmd = new SqlCommand(sql,con);
            con.Open();
            cmd.Parameters.AddWithValue("@name", txtName.Text);
            cmd.Parameters.AddWithValue("@age", txtAge.Text);
            cmd.Parameters.AddWithValue("@address", txtAddress.Text);
            int n=cmd.ExecuteNonQuery();
            if (n != 0)
            {
                MessageBox.Show("Register SuccessFully!");
            }
            else
            {
                MessageBox.Show("Register Failed!");
            }
            con.Close();
        }
    }
}
