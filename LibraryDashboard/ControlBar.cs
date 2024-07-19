using System.Drawing;
using System.Windows.Forms;

namespace LibraryDashboard
{
    public class ControlBar : Panel
    {
        public ControlBar(Form parentForm)
        {
            this.Dock = DockStyle.Top;
            this.Height = 40;
            this.BackColor = Color.DarkGray;
            parentForm.Controls.Add(this);

            CreateControlButtons(parentForm);
        }

        private void CreateControlButtons(Form parentForm)
        {
            Button btnClose = new Button
            {
                Text = "X",
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 1 },
                ForeColor = Color.Red,
                BackColor = Color.Transparent,
                Dock = DockStyle.Right
            };
            btnClose.Click += (sender, e) => { parentForm.Close(); };
            this.Controls.Add(btnClose);

            Button btnMaximize = new Button
            {
                Text = "□",
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 1 },
                ForeColor = Color.Yellow,
                BackColor = Color.Transparent,
                Dock = DockStyle.Right
            };
            btnMaximize.Click += (sender, e) =>
            {
                parentForm.WindowState = parentForm.WindowState == FormWindowState.Maximized ?
                                        FormWindowState.Normal : FormWindowState.Maximized;
            };
            this.Controls.Add(btnMaximize);

            Button btnMinimize = new Button
            {
                Text = "-",
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 1 },
                ForeColor = Color.Green,
                BackColor = Color.Transparent,
                Dock = DockStyle.Right
            };
            btnMinimize.Click += (sender, e) => { parentForm.WindowState = FormWindowState.Minimized; };
            this.Controls.Add(btnMinimize);
        }
    }
}
