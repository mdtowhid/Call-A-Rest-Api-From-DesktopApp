using Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HomemFra
{
    public partial class Form1 : Form
    {
        List<Employee> employees;
        HttpClient client;
        public Form1()
        {
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            ReloadGridView(await GetEmployeesAsync());
        }

        private async Task<List<Employee>> GetEmployeesAsync()
        {
            client = new HttpClient();
            List<Employee> employees = new List<Employee>();
            client.BaseAddress = new Uri("http://localhost/DotnetTricksApi/api/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await client.GetStringAsync("employee/getall");
            employees = JsonConvert.DeserializeObject<List<Employee>>(response);
            client = null;
            return employees;
        }

        private void dataGridView1_CellMouseDown_1(object sender, DataGridViewCellMouseEventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                string value1 = row.Cells[0].Value.ToString();
                string value2 = row.Cells[1].Value.ToString();
                //...
            }
        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in dataGridView1.SelectedRows)
            {
                string value1 = row.Cells[0].Value.ToString();
                string value2 = row.Cells[1].Value.ToString();
                //...
            }
        }

        private async void dataGridView1_DoubleClick(object sender, EventArgs e)
        {
            var j = dataGridView1.CurrentRow.Index;
            employees = await GetEmployeesAsync();
            if (employees != null)
            {
                client = new HttpClient();
                Employee emp = employees[j];
                
                
            }


        }

        private async void SaveEmployeeInfo_Click(object sender, EventArgs e)
        {
            client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost/DotnetTricksApi/api/");
            //client.DefaultRequestHeaders.Accept.Clear();
            Employee employee = new Employee
            {
                EmpName = empNameTextBox.Text,
                Department = deptNameTextBox.Text,
                Designation = designationTextBox.Text
            };
            var myContent = JsonConvert.SerializeObject(employee);
            var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            var byteContent = new ByteArrayContent(buffer);
            byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var result = await client.PostAsync("employee/post", byteContent);
            client = null;
            ReloadGridView(await GetEmployeesAsync());
        }

        void ReloadGridView(List<Employee> employees)
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("EmpId");
            dt.Columns.Add("EmpName");
            dt.Columns.Add("Department");
            dt.Columns.Add("Designation");
            if (employees.Count > 0)
            {
                foreach (var item in employees)
                {
                    var row = dt.NewRow();
                    row["EmpId"] = item.EmpId;
                    row["EmpName"] = item.EmpName;
                    row["Department"] = item.Department;
                    row["Designation"] = item.Designation;
                    dataGridView1.Tag = item;
                    dt.Rows.Add(row);
                }
            }
            dataGridView1.DataSource = dt;
        }
    }
}
