using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Test_LYF.ModelsLYF;
using DeviceLibrary.Models.Enums;
using DeviceLibrary.Models;
using Test_LYF.ServicesSqlite;
using System.Data.SQLite;

namespace Test_LYF.ViewsForms
{
    public partial class PaymentView : Form
    {
        private Account account;
        private ServicesHttp.ServicesHttp servicesHttp = new ServicesHttp.ServicesHttp();
        private BackgroundWorker backgroundWorker1 = new BackgroundWorker();
        private DeviceLibrary.DeviceLibrary deviceLibrary = new DeviceLibrary.DeviceLibrary();
        private DataAccess dataAccess = new DataAccess();

        public PaymentView(Account _account)
        {
            InitializeComponent();
            InitializeBackgroundWorker();
            dataAccess.InitializeDatabase();
            initDeviceLibrary();
            account = _account;
            lblDebt.Text = "$" + account.debt;
            lblRemaining.Text = "$" + account.debt;
        }

        private void initDeviceLibrary()
        {
            try
            {
                deviceLibrary.Open();
                deviceLibrary.Enable();
                DeviceStatus status = deviceLibrary.Status;


                if (status == DeviceStatus.Disconnected)
                {
                    MessageBox.Show("El dispositivo NO está conectado.");
                    goBack();
                    return;
                }

                if (status == DeviceStatus.Disabled)
                {
                    MessageBox.Show("El dispositivo NO acepta monedas ni billetes en este momento.");
                    goBack();
                    return;
                }

                lblCargando.Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void goBack()
        {
            WelcomeView myAccount = new WelcomeView();
            myAccount.Show();
            deviceLibrary.Close();
            this.Hide();
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
            pay.paid = double.Parse(lblDesposited.Text.Substring(1, lblDesposited.Text.Length - 1)) - double.Parse(lblChange.Text.Substring(1, lblChange.Text.Length - 1));
            var response = servicesHttp.post("transaction?token=be2c7efc27d7fbfc8d3b1ee4979def9d", pay);
            response.Wait();
            string json = response.Result;
            return JsonConvert.DeserializeObject<Account>(json);
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            if (lblDesposited.Text == "$0")
            {
                goBack();
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
            DeviceStatus status = deviceLibrary.Status;

            if (status != DeviceStatus.Enabled)
            {
                MessageBox.Show("Error de conexion.");
                goBack();
                return;
            }

            if (lblRemaining.Text == "$0") return;

            Document document = new Document(Decimal.Parse(pay.ToString()), pay > 20 ? DocumentType.Bill : DocumentType.Coin, 1);
            deviceLibrary.SimulateInsertion(document);

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

            dataAccess.addPayment(account.user, int.Parse(account.account), newDebt, double.Parse(lblDesposited.Text.Substring(1, lblDesposited.Text.Length - 1)) + pay);
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


            goBack();
        }

        private void lblCargando_Click(object sender, EventArgs e)
        {

        }
    }
}
