using LibraryDashboard.Design;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace LibraryDashboard.Helpers
{
    internal static class PanelHelper
    {

        public static Image ResizeImage(Image imgToResize, Size size)
        {
            if (imgToResize == null)
                throw new ArgumentNullException(nameof(imgToResize));

            // Create a new bitmap with the specified size
            Bitmap resizedImage = new Bitmap(size.Width, size.Height);
            using (Graphics graphics = Graphics.FromImage(resizedImage))
            {
                // Set the quality of the resized image
                graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighQuality;
                graphics.DrawImage(imgToResize, 0, 0, size.Width, size.Height);
            }
            return resizedImage;
        }
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

        public static Panel CreatePanel(Size size, Point location, Color backColor, BorderStyle borderStyle)
        {
            return new Panel
            {
                Size = size,
                Location = location,
                BackColor = backColor,
                BorderStyle = borderStyle,
                Region = System.Drawing.Region.FromHrgn(RoundCorner.CreateRoundRectRgn(0, 0, size.Width, size.Height, 15, 15))
            };
        }
        public static Button CreateButton(string text, Point location, Size size, Color backColor, Color foreColor, Font font, EventHandler clickEventHandler)
        {
            Button button = new Button
            {
                Text = text,
                Location = location,
                Size = size,
                BackColor = backColor,
                ForeColor = foreColor,
                Font = font,
                FlatStyle = FlatStyle.Flat
            };
            button.FlatAppearance.BorderSize = 0;
            button.Click += clickEventHandler;
            return button;
        }

        public static TextBox CreateTextBox(Point location, Size size, Font font, bool useSystemPasswordChar = false)
        {
            return new TextBox
            {
                Location = location,
                Size = size,
                Font = font,
                UseSystemPasswordChar = useSystemPasswordChar
            };
        }

        public static ComboBox CreateComboBox(Point location, Size size, object[] items, int selectedIndex)
        {
            var comboBox = new ComboBox
            {
                Location = location,
                Size = size,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point)
            };
            comboBox.Items.AddRange(items);
            comboBox.SelectedIndex = selectedIndex;

            return comboBox;
        }
    }
}
