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
    public partial class PaymentView : Form
    {
        private Account account;
        private ServicesHttp.ServicesHttp servicesHttp = new ServicesHttp.ServicesHttp();
        private BackgroundWorker backgroundWorker1 = new BackgroundWorker();

        public PaymentView(Account _account)
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            account = _account;
            lblDebt.Text = "$" + account.debt;
            lblRemaining.Text = "$" + account.debt;
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
            e.Result = transaction();
        }

        private Account transaction()
        {
            Pay pay = new Pay();
            pay.account = account.account;
            pay.pay = double.Parse(lblDesposited.Text.Substring(1, lblDesposited.Text.Length - 1)) - double.Parse(lblChange.Text.Substring(1, lblChange.Text.Length - 1));
            var response = servicesHttp.post("transaction?token=be2c7efc27d7fbfc8d3b1ee4979def9dW", pay);
            response.Wait();
            string json = response.Result;
            return JsonConvert.DeserializeObject<Account>(json);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (lblDebt.Text == "$0")
            {
                WelcomeView myAccount = new WelcomeView();
                myAccount.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Ya tiene un deposito, favor de terminar la transacción.");
            }
        }

        private void circleButton1_Click(object sender, EventArgs e)
        {
            updateData(10);
        }

        private void circleButton2_Click(object sender, EventArgs e)
        {
            updateData(5);
        }

        private void circleButton3_Click(object sender, EventArgs e)
        {
            updateData(2);
        }

        private void circleButton4_Click(object sender, EventArgs e)
        {
            updateData(1);
        }

        private void circleButton5_Click(object sender, EventArgs e)
        {
            updateData(0.5);
        }

        private void btn500_Click(object sender, EventArgs e)
        {
            updateData(500);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            updateData(200);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            updateData(100);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            updateData(50);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            updateData(20);
        }

        void updateData(double pay)
        {
            if (lblRemaining.Text == "$0") return;

            double newDebt = (double.Parse(lblRemaining.Text.Substring(1, lblRemaining.Text.Length - 1)) - pay);

            if (newDebt < 0)
            {
                newDebt = 0;
            }

            if (newDebt == 0)
            {
                lblChange.Text = "$" + ((double.Parse(lblDesposited.Text.Substring(1, lblDesposited.Text.Length - 1)) + pay) - double.Parse(lblDebt.Text.Substring(1, lblDebt.Text.Length - 1))).ToString();
            }

            lblRemaining.Text = "$" + newDebt.ToString();
            lblDesposited.Text = "$" + (double.Parse(lblDesposited.Text.Substring(1, lblDesposited.Text.Length - 1)) + pay).ToString();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(lblDesposited.Text == "$0")
            {
                MessageBox.Show("Favor de realizar un deposito.");
            }
            else
            {
                lblCargando.Visible = true;
                backgroundWorker1.RunWorkerAsync();
            }
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

           
            WelcomeView welcomeView = new WelcomeView();

            welcomeView.Show();
            this.Hide();
        }
    }
}
