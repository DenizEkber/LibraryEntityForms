using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryDashboard.Design
{
    internal class PanelCreated
    {
        public Panel CreatePanel(Point location, Size size, Color backColor)
        {
            Panel panel = new Panel
            {
                Location = location,
                Size = size,
                BackColor = backColor
            };
            return panel;
        }

        public Label CreateLabel(string text, Point location, Font font, Color color, Size size)
        {
            Label label = new Label
            {
                Text = text,
                Location = location,
                Font = font,
                AutoSize = false,
                Size = size,
                BackColor = color,

                TextAlign = ContentAlignment.TopLeft,
                AutoEllipsis = true
            };
            return label;

        }
    }
}
