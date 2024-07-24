using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LibraryDashboard.Helpers;
using LibraryEntityForms.CodeFirst.Context;
using Microsoft.EntityFrameworkCore;

namespace LibraryDashboard
{
    internal class Setting : Panel
    {
        private TextBox txtUserName;
        private TextBox txtFirstName;
        private TextBox txtLastName;
        private TextBox txtEmail;
        private ComboBox cboBackgroundColor;
        private Button btnSave;
        private Button btnColor;
        protected dynamic userData;

        public Setting(Form parentForm, dynamic userData)
        {
            this.userData = userData;
            this.Size = new Size(1575, 1075);
            this.BackColor = Color.White;
            this.BorderStyle = BorderStyle.FixedSingle;
            this.Location = new Point(345, 120);
            this.Visible = false;

            parentForm.Controls.Add(this);

            InitializeComponent(parentForm, userData);
        }

       /* int lblLoc = 20;
        int txtLoc = 20;
        void CreateLabelProcess(string usernameTxt, TextBox tb)
        {
            Label lbl = PanelHelper.CreateLabel($"{usernameTxt} Name:", new Font("Segoe UI", 12F, FontStyle.Regular), Color.Black, new Point(10, lblLoc+=40));
            tb = PanelHelper.CreateTextBox(new Point(120, txtLoc += 40), new Size(200, 25), new Font("Segoe UI", 12F, FontStyle.Regular));
            //tb.Text = TxtReturn(tb.Name);
            MessageBox.Show(tb.ToString());

            this.Controls.Add(lbl);
            this.Controls.Add(tb);

        }*/
        /*dynamic TxtReturn(string textBoxName)
        {
            return textBoxName switch
            {
                "txtUserName" => userData.UserName,
                "txtFirstName" => userData.FirstName,
                "txtLastName" => userData.LastName,

            };
        }*/

        private void InitializeComponent(Form parentForm, dynamic userData)
        {
           /* CreateLabelProcess("User", txtUserName);
            CreateLabelProcess("First", txtFirstName);
            CreateLabelProcess("Last", txtLastName);*/

            Label lblUserName = PanelHelper.CreateLabel("User Name:", new Font("Segoe UI", 12F, FontStyle.Regular), Color.Black, new Point(10, 20));
            txtUserName = PanelHelper.CreateTextBox(new Point(120, 20), new Size(200, 25), new Font("Segoe UI", 12F, FontStyle.Regular));
            txtUserName.Text = userData.UserName;

            Label lblFirstName = PanelHelper.CreateLabel("First Name:", new Font("Segoe UI", 12F, FontStyle.Regular), Color.Black, new Point(10, 60));
            txtFirstName = PanelHelper.CreateTextBox(new Point(120, 60), new Size(200, 25), new Font("Segoe UI", 12F, FontStyle.Regular));
            txtFirstName.Text = userData.FirstName;

            Label lblLastName = PanelHelper.CreateLabel("Last Name:", new Font("Segoe UI", 12F, FontStyle.Regular), Color.Black, new Point(10, 100));
            txtLastName = PanelHelper.CreateTextBox(new Point(120, 100), new Size(200, 25), new Font("Segoe UI", 12F, FontStyle.Regular));
            txtLastName.Text = userData.LastName;

            /*Label lblEmail = PanelHelper.CreateLabel("Email:", new Font("Segoe UI", 12F, FontStyle.Regular), Color.Black, new Point(10, 140));
            txtEmail = PanelHelper.CreateTextBox(new Point(120, 140), new Size(200, 25), new Font("Segoe UI", 12F, FontStyle.Regular));
            txtEmail.Text = userData.Email;*/

            /*Label lblBackgroundColor = PanelHelper.CreateLabel("Background Color:", new Font("Segoe UI", 12F, FontStyle.Regular), Color.Black, new Point(10, 180));
            cboBackgroundColor = PanelHelper.CreateComboBox(new Point(160, 180), new Size(160, 25), new string[] { "White", "Black" }, 0);*/

            btnSave = PanelHelper.CreateButton("Save", new Point(50, 230), new Size(100, 50), Color.Green, Color.White, new Font("Segoe UI", 12F, FontStyle.Regular), async (s, e) => await SaveSettingsAsync());
            //btnColor = PanelHelper.CreateButton("Cancel", new Point(200, 230), new Size(100, 30), Color.Red, Color.White, new Font("Segoe UI", 12F, FontStyle.Regular), async (s, e) => await SaveColorAsync());



            this.Controls.Add(lblUserName);
            this.Controls.Add(txtUserName);
            this.Controls.Add(lblFirstName);
            this.Controls.Add(txtFirstName);
            this.Controls.Add(lblLastName);
            this.Controls.Add(txtLastName);
            /*this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);*/
            //this.Controls.Add(lblBackgroundColor);
            this.Controls.Add(cboBackgroundColor);
            this.Controls.Add(btnSave);
            this.Controls.Add(btnColor);

            this.BringToFront();
        }

        /*private async Task SaveColorAsync()
        {
            var mainForm = this.FindForm();
            if (mainForm != null)
            {
                mainForm.BackColor = cboBackgroundColor.SelectedItem.ToString() == "Black" ? Color.Black : Color.White;
            }
        }*/
        private async Task SaveSettingsAsync()
        {
            try
            {
                using (var ctx = new LibraryContext())
                {
                    var Id = (int)userData.Id;
                    var user = await ctx.Users.FindAsync((int)userData.Id);
                    var userDetail = await ctx.UserDetail.FirstOrDefaultAsync(p => p.Id_User == Id);

                    if (user != null && userDetail != null)
                    {
                        user.Name = txtUserName.Text;
                        user.UpdatedDate = DateTime.Now;

                        userDetail.FirstName = txtFirstName.Text;
                        userDetail.LastName = txtLastName.Text;
                        userDetail.Email = txtEmail.Text;
                        userDetail.UpdatedDate = DateTime.Now;
                        //userDetail.BackgroundColor = cboBackgroundColor.SelectedItem.ToString();

                        await ctx.SaveChangesAsync();

                        // Change the background color of the main form
                        var mainForm = this.FindForm();
                        if (mainForm != null)
                        {
                            mainForm.BackColor = cboBackgroundColor.SelectedItem.ToString() == "Black" ? Color.Black : Color.White;
                        }

                        MessageBox.Show("Settings saved successfully!");
                    }
                    else
                    {
                        MessageBox.Show("User or User Detail not found!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving settings: {ex.Message}");
            }

            this.Visible = true;
        }
    }
}
