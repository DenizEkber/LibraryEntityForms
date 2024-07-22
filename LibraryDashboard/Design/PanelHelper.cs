using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LibraryDashboard.Helpers
{
    internal static class PanelHelper
    {
        public static PictureBox CreatePictureBox(byte[] photoData, string fallbackImagePath, Size size, Point location)
        {
            PictureBox pictureBox = new PictureBox
            {
                Size = size,
                Location = location,
                SizeMode = PictureBoxSizeMode.StretchImage,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Load the image from byte array
            if (photoData != null && photoData.Length > 0)
            {
                try
                {
                    using (var ms = new MemoryStream(photoData))
                    {
                        pictureBox.Image = Image.FromStream(ms);
                    }
                }
                catch (Exception ex)
                {
                    pictureBox.Image = Image.FromFile(fallbackImagePath); // Fallback image
                    Console.WriteLine($"Error loading image: {ex.Message}");
                }
            }
            else
            {
                pictureBox.Image = Image.FromFile(fallbackImagePath); // Fallback image
            }

            return pictureBox;
        }

        public static Label CreateLabel(string text, Font font, Color color, Point location, bool autoSize = true)
        {
            return new Label
            {
                Text = text,
                Font = font,
                ForeColor = color,
                Location = location,
                AutoSize = autoSize
            };
        }

        public static Panel CreatePanel(Size size, Padding margin, Color backColor, Padding padding)
        {
            return new Panel
            {
                Size = size,
                Margin = margin,
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = backColor,
                Padding = padding
            };
        }
    }
}
