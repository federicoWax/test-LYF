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
    public partial class MyAccountView : Form
    {
        private Account account;
        public MyAccountView(Account _account)
        {
            InitializeComponent();
            account = _account;
            lblAccount.Text = account.account;
            lblDebt.Text = "$" + account.debt;
            lblUser.Text = account.user;
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            AccountView accountView = new AccountView();
            accountView.Show();
            this.Hide();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            PaymentView paymentView = new PaymentView(account);
            paymentView.Show();
            this.Hide();
        }

        private void tableLayoutPanel4_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
