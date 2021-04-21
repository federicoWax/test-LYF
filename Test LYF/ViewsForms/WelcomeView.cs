using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Test_LYF.ViewsForms
{
    public partial class WelcomeView : Form
    {
        public WelcomeView()
        {
            InitializeComponent();
        }

        private void LabelOnClick(object sender, EventArgs e)
        {
            this.Hide();
            AccountView acountView = new AccountView();
            acountView.Show();
        }
    }
}
