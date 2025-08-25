using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.Windows.Forms;

namespace DVLD.Login
{
    public partial class frmLogin : Form
    {
        public frmLogin()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string enteredUserName = txtUserName.Text.Trim();
            string enteredPassword = txtPassword.Text.Trim();

            clsUser user = clsUser.FindByUsernameAndPassword(enteredUserName, enteredPassword);

            if (user != null)
            {
                if (chkRememberMe.Checked)
                    clsGlobal.RememberUsernameAndPassword(enteredUserName, enteredPassword);
                else
                    clsGlobal.RememberUsernameAndPassword("", "");

                if (!user.IsActive)
                {
                    txtUserName.Focus();
                    MessageBox.Show("Your account is not Active, Contact Admin.", "Inactive Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsGlobal.CurrentUser = user;

                this.Hide();
                frmMain frm = new frmMain(this);
                frm.ShowDialog();
                this.Show();
            }
            else
            {
                txtUserName.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong Credentials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
            string storedUserName = "", storedPassword = "";

            if (clsGlobal.GetStoredCredential(ref storedUserName, ref storedPassword))
            {
                txtUserName.Text = storedUserName;
                txtPassword.Text = storedPassword;
                chkRememberMe.Checked = true;
            }
            else
            {
                chkRememberMe.Checked = false;
            }
        }
    }
}
