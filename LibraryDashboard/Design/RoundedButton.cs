using System.Drawing;
using System.Drawing.Drawing2D;

using System.Windows.Forms;

namespace LibraryDashboard.Design
{
    public class RoundedButton : Button
    {
        public RoundedButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.White;
            Size = new Size(315, 60);
            Font = new Font("Segoe UI", 12, FontStyle.Regular);
            ForeColor = Color.Gray;
            TextAlign = ContentAlignment.MiddleRight;
            ImageAlign = ContentAlignment.MiddleLeft;
            Padding = new Padding(20, 0, 20, 0);
            Margin = new Padding(0, 10, 0, 10);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            
            GraphicsPath path = new GraphicsPath();
            int radius = 20; 

            path.AddArc(ClientRectangle.Width - radius, 0, radius, radius, 270, 90);
            path.AddArc(ClientRectangle.Width - radius, ClientRectangle.Height - radius, radius, radius, 0, 90);
            path.AddArc(0, ClientRectangle.Height - radius, radius, radius, 90, 90);
            path.AddArc(0, 0, radius, radius, 180, 90);

            Region = new Region(path);

            
        }
    }
}
