using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Test_LYF.ModelsLYF;

namespace Test_LYF.ViewsForms
{
    public partial class AccountView : Form
    {

        private ServicesHttp.ServicesHttp servicesHttp = new ServicesHttp.ServicesHttp();
        private BackgroundWorker backgroundWorker1 = new BackgroundWorker();

        public AccountView()
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            lblCargando.Visible = false;
        }

        private void InitializeBackgroundWorker()
        {
            backgroundWorker1.DoWork += new DoWorkEventHandler(backgroundWorker1_DoWork);
            backgroundWorker1.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                backgroundWorker1_RunWorkerCompleted
            );
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = getAccount();
        }

        private Account getAccount()
        {
            var response = servicesHttp.get("account_balance?token=be2c7efc27d7fbfc8d3b1ee4979def9d&account=" +txtAccount.Text);
            response.Wait();
            string json = response.Result;
            return JsonConvert.DeserializeObject<Account>(json);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lblCargando.Visible = false;

            if (e.Error != null)
            {
                MessageBox.Show(e.Error.Message);
                return;
            }
            
            Account account = e.Result as Account;

            if (!string.IsNullOrEmpty(account.message))
            {
                MessageBox.Show(account.message);
                return;
            }

            account.account = txtAccount.Text;
            MyAccountView myAccount = new MyAccountView(account);

            myAccount.Show();
            this.Close();
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            WelcomeView welcomeView = new WelcomeView();
            welcomeView.Show();
            this.Close();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            txtAccount.Text = txtAccount.Text + "1";
        }

        private void btn2_Click(object sender, EventArgs e)
        {
            txtAccount.Text = txtAccount.Text + "2";
        }

        private void btn3_Click(object sender, EventArgs e)
        {
            txtAccount.Text = txtAccount.Text + "3";
        }

        private void btn4_Click(object sender, EventArgs e)
        {
            txtAccount.Text = txtAccount.Text + "4";
        }

        private void btn5_Click(object sender, EventArgs e)
        {
            txtAccount.Text = txtAccount.Text + "5";
        }

        private void btn6_Click(object sender, EventArgs e)
        {
            txtAccount.Text = txtAccount.Text + "6";
        }

        private void btn7_Click(object sender, EventArgs e)
        {
            txtAccount.Text = txtAccount.Text + "7";
        }

        private void btn8_Click(object sender, EventArgs e)
        {
            txtAccount.Text = txtAccount.Text + "8";
        }

        private void btn9_Click(object sender, EventArgs e)
        {
            txtAccount.Text = txtAccount.Text + "9";
        }

        private void btn0_Click(object sender, EventArgs e)
        {
            txtAccount.Text = txtAccount.Text + "0";
        }

        private void btnBorrar_Click(object sender, EventArgs e)
        {
            if(txtAccount.Text.Length > 0)
                txtAccount.Text = txtAccount.Text.Remove(txtAccount.Text.Length - 1);
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtAccount.Text))
            {
                MessageBox.Show("Favor de escribir el número de cuenta.");
                return;
            }

            lblCargando.Visible = true;
            backgroundWorker1.RunWorkerAsync();
        }

        private void AccountView_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
