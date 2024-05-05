using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CashierManagement.Models;

namespace CashierManagement
{
    public partial class Transaction : Form
    {
        public Transaction()
        {
            InitializeComponent();
        }

        private string _username = MainClass.LoggedInUsername;
        private int _currentRowNumber = 0;

        private void Transaction_Load(object sender, EventArgs e)
        {
            foreach (var code in MainClass.GetDrugCodes())
            {
                comboBox1.Items.Add(code);
            }

            textBox4.Text = _username;
            textBox3.Text = DateTime.Now.Date.ToString("yyyy MMMM dd");
        }
            
        private void label2_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void label3_Click(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void textBox9_TextChanged(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var code = comboBox1.SelectedItem?.ToString();
            var name = textBox5.Text;
            var price = int.Parse(textBox6.Text);
            _currentRowNumber++;
            var total = 0;
            
            if (string.IsNullOrEmpty(code))
            {
                guna2MessageDialog2.Show("Please select the code first");
            }
            
            int quantity;
            if (!int.TryParse(textBox7.Text, out quantity))
            {
                guna2MessageDialog2.Show("Please enter a valid quantity");
            }
            var subTotal = price * quantity;

            if (dataGridView1.Columns.Count == 0)
            {
                var noColumn = new DataGridViewTextBoxColumn();
                noColumn.Name = "No";
                noColumn.HeaderText = "No";

                var barangIdColumn = new DataGridViewTextBoxColumn();
                barangIdColumn.Name = "BarangId";
                barangIdColumn.HeaderText = "BarangId";

                var namaColumn = new DataGridViewTextBoxColumn();
                namaColumn.Name = "Nama";
                namaColumn.HeaderText = "Nama";

                var jmlBeliColumn = new DataGridViewTextBoxColumn();
                jmlBeliColumn.Name = "JmlBeli";
                jmlBeliColumn.HeaderText = "JmlBeli";

                var subTotalColumn = new DataGridViewTextBoxColumn();
                subTotalColumn.Name = "SubTotal";
                subTotalColumn.HeaderText = "SubTotal";

                dataGridView1.Columns.AddRange(new DataGridViewColumn[] { noColumn, barangIdColumn, namaColumn, jmlBeliColumn, subTotalColumn });
            }
            
            var rowData = new object[] { _currentRowNumber, code, name, quantity, subTotal.ToString() };
            total += subTotal;
            textBox10.Text = total.ToString();
            dataGridView1.Rows.Add(rowData);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var transactionModel = new TransactionModel();
            transactionModel.Date = DateTime.Parse(textBox3.Text);
            transactionModel.UserId = MainClass.FindUserIdByUsername(textBox4.Text);
            transactionModel.Total = int.Parse(textBox10.Text);
            MainClass.SaveTransaction(transactionModel);

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                var transactionDetail = new TransactionDetail();
                transactionDetail.TransactionId = int.Parse(textBox2.Text);
                transactionDetail.DrugId = row.Cells["BarangId"].Value?.ToString();
                transactionDetail.Quantity = int.Parse(row.Cells["JmlBeli"].Value.ToString());
                transactionDetail.SubTotal = int.Parse(row.Cells["SubTotal"].Value.ToString());
                
                MainClass.SaveTransactionDetail(transactionDetail);
            }
            
            var factur = int.Parse(textBox2.Text);
            textBox2.Text = factur++.ToString();
            comboBox1.SelectedIndex = 0;
            textBox7.Text = "";
            dataGridView1.Rows.Clear();
            textBox10.Text = "";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = -1;
            textBox7.Text = "";
            dataGridView1.Rows.Clear();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            this.Hide();
            var login = new Login();
            login.Show();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex > -1)
            {
                var selectedCode = comboBox1.SelectedItem.ToString();
                Drug drug;
                try
                {
                    drug = MainClass.GetDrug(selectedCode);
                }
                catch (Exception exception)
                {
                    guna2MessageDialog1.Show(exception.Message);
                    throw;
                }

                textBox5.Text = drug.Name;
                textBox6.Text = drug.Price.ToString();
            }
            else
            {
                textBox5.Text = "";
                textBox6.Text = "";
                textBox11.Text = "";
            }
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox7.Text, out var quantity))
            {
                var subTotal = int.Parse(textBox6.Text) * quantity;
                textBox8.Text = subTotal.ToString();
            }
            else
            {
                textBox8.Text = "";
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            throw new System.NotImplementedException();
        }

        private void textBox11_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBox11.Text, out var money))
            {
                var total = money - int.Parse(textBox10.Text);
                textBox12.Text = total.ToString();
            }
            else
            {
                textBox12.Text = "";
            }
        }
    }
}