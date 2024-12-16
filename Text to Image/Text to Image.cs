using System.Drawing.Imaging;
using System.Drawing.Text;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using static System.Net.Mime.MediaTypeNames;

namespace Text_to_Image
{
    public class TextToImageForm : Form
    {

        public TextToImageForm()
        {
            InitializeComponent();
            PopulateFontPicker();
        }

        public void outBox(string input)
        {
            if (OutputBox.Text.Equals(input))
                OutputBox.Text = " " + input;
            else
                OutputBox.Text = input;
        }

        public static Bitmap CropBitmap(Bitmap bitmap)
        {
            // find bounds
            int minX = bitmap.Width;
            int minY = bitmap.Height;
            int maxX = 0;
            int maxY = 0;

            for (int x = 0; x < bitmap.Width; x++)
            {
                for (int y = 0; y < bitmap.Height; y++)
                {
                    Color pixelColor = bitmap.GetPixel(x, y);

                    if (pixelColor.A != 0)
                    {
                        minX = Math.Min(minX, x);
                        minY = Math.Min(minY, y);
                        maxX = Math.Max(maxX, x);
                        maxY = Math.Max(maxY, y);
                    }
                }
            }

            Rectangle size = new(minX, minY, maxX - minX + 1, maxY - minY + 1);

            // crop bitmap
            Bitmap croppedBitmap = new Bitmap(size.Width, size.Height);
            Graphics.FromImage(croppedBitmap).DrawImage(bitmap, 0, 0, size, GraphicsUnit.Pixel);

            return croppedBitmap;

        }

        public void PopulateFontPicker()
        {
            // Get all installed fonts
            InstalledFontCollection fonts = new InstalledFontCollection();

            foreach (FontFamily font in fonts.Families)
            {
                FontPicker.Items.Add(font.Name);
            }

            // Set default selection
            if (FontPicker.Items.Count > 0)
            {
                FontPicker.SelectedItem = "Comic Sans MS";
            }
        }

        private void InitializeComponent()
        {
            InputText = new TextBox();
            CroppingCheck = new CheckBox();
            OutputBox = new TextBox();
            GenerateImageButton = new Button();
            label1 = new Label();
            FontSize = new TextBox();
            CustomFontCheck = new CheckBox();
            FontPicker = new ComboBox();
            SuspendLayout();
            // 
            // InputText
            // 
            InputText.Location = new Point(12, 12);
            InputText.Multiline = true;
            InputText.Name = "InputText";
            InputText.Size = new Size(400, 80);
            InputText.TabIndex = 0;
            // 
            // CroppingCheck
            // 
            CroppingCheck.AutoSize = true;
            CroppingCheck.Location = new Point(12, 98);
            CroppingCheck.Name = "CroppingCheck";
            CroppingCheck.Size = new Size(145, 19);
            CroppingCheck.TabIndex = 0;
            CroppingCheck.Text = "crop image to text size";
            CroppingCheck.UseVisualStyleBackColor = true;
            // 
            // OutputBox
            // 
            OutputBox.Location = new Point(12, 192);
            OutputBox.Name = "OutputBox";
            OutputBox.ReadOnly = true;
            OutputBox.Size = new Size(400, 23);
            OutputBox.TabIndex = 1;
            OutputBox.TabStop = false;
            OutputBox.Text = "no output yet";
            // 
            // GenerateImageButton
            // 
            GenerateImageButton.Location = new Point(12, 159);
            GenerateImageButton.Name = "GenerateImageButton";
            GenerateImageButton.Size = new Size(120, 27);
            GenerateImageButton.TabIndex = 2;
            GenerateImageButton.Text = "Generate Image";
            GenerateImageButton.UseVisualStyleBackColor = true;
            GenerateImageButton.Click += GenerateImageButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(12, 126);
            label1.Name = "label1";
            label1.Size = new Size(54, 15);
            label1.TabIndex = 3;
            label1.Text = "font size:";
            // 
            // FontSize
            // 
            FontSize.Location = new Point(72, 123);
            FontSize.Name = "FontSize";
            FontSize.Size = new Size(42, 23);
            FontSize.TabIndex = 4;
            FontSize.Text = "12";
            FontSize.WordWrap = false;
            FontSize.KeyPress += FontSize_KeyPress;
            FontSize.Leave += FontSize_Leave;
            // 
            // CustomFontCheck
            // 
            CustomFontCheck.AutoSize = true;
            CustomFontCheck.Location = new Point(218, 98);
            CustomFontCheck.Name = "CustomFontCheck";
            CustomFontCheck.Size = new Size(91, 19);
            CustomFontCheck.TabIndex = 5;
            CustomFontCheck.Text = "custom font";
            CustomFontCheck.UseVisualStyleBackColor = true;
            CustomFontCheck.CheckedChanged += CustomFontCheck_CheckedChanged;
            // 
            // FontPicker
            // 
            FontPicker.DropDownStyle = ComboBoxStyle.DropDownList;
            FontPicker.FormattingEnabled = true;
            FontPicker.Location = new Point(218, 123);
            FontPicker.Name = "FontPicker";
            FontPicker.Size = new Size(194, 23);
            FontPicker.TabIndex = 6;
            FontPicker.Visible = false;
            // 
            // TextToImageForm
            // 
            ClientSize = new Size(424, 229);
            Controls.Add(FontPicker);
            Controls.Add(CustomFontCheck);
            Controls.Add(FontSize);
            Controls.Add(label1);
            Controls.Add(GenerateImageButton);
            Controls.Add(OutputBox);
            Controls.Add(CroppingCheck);
            Controls.Add(InputText);
            Name = "TextToImageForm";
            Text = "Text to Image";
            ResumeLayout(false);
            PerformLayout();
        }

        [STAThread]
        public static void Main()
        {
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.Run(new TextToImageForm());
        }

        private TextBox InputText;
        private CheckBox CroppingCheck;
        private Button GenerateImageButton;
        private Label label1;
        private TextBox FontSize;
        private CheckBox CustomFontCheck;
        private ComboBox FontPicker;
        private TextBox OutputBox;

        private void GenerateImageButton_Click(object sender, EventArgs e)
        {
            if (InputText.Text.Equals(""))
            {
                outBox("Please input your text first.");
                return;
            }

            if (float.Parse(FontSize.Text, CultureInfo.InvariantCulture) < 1)
            {
                outBox("Please use a higher font size.");
                return;
            }

            string text = InputText.Text;
            System.Drawing.Font customFont;

            if (CustomFontCheck.Checked)
            {
                customFont = new System.Drawing.Font(FontPicker.SelectedItem.ToString(), float.Parse(FontSize.Text, CultureInfo.InvariantCulture));
            }
            else
            { 
                // Load the custom font from embedded resource
                PrivateFontCollection fontCollection = new PrivateFontCollection();
                using (Stream fontStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Text_to_Image.Resources.DigitalDisco-Thin.ttf"))
                {
                    if (fontStream == null)
                    {
                        outBox("Font couldn't be found. :(");
                        return;
                    }

            using (Graphics tempGraphics = Graphics.FromImage(new Bitmap(1, 1)))
            {
                int width = (int)Math.Ceiling(tempGraphics.MeasureString(text, customFont).Width);
                int height = (int)Math.Ceiling(tempGraphics.MeasureString(text, customFont).Height);

                string directory;
                Bitmap bitmap = new Bitmap(width, height);
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    graphics.Clear(Color.Transparent);
                    graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SingleBitPerPixelGridFit;
                    graphics.DrawString(text, customFont, Brushes.Black, new PointF(0, 0));
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "PNG files (*.png)|*.png|All files (*.*)|*.*"; // filter for .png files
                    saveFileDialog.Title = "Save As";
                    saveFileDialog.DefaultExt = "png";

                    string firstWord = Regex.Match(InputText.Text, @"^\S+") + ".png";
                    if (firstWord.Length < 11)
                    {
                        saveFileDialog.FileName = firstWord;
                    }
                    else
                    {
                        saveFileDialog.FileName = "bb text.png";
                    }

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        directory = saveFileDialog.FileName;
                    }
                    else
                    {
                        outBox("Something went wrong while picking the directory.");
                        return;
                    }
                }

                if (CroppingCheck.Checked)
                {
                    CropBitmap(bitmap).Save(directory, ImageFormat.Png);
                }
                else
                {
                    bitmap.Save(directory, ImageFormat.Png);
                }

                outBox($"Image saved to: {directory}");
            }
        }

        private void FontSize_KeyPress(object sender, KeyPressEventArgs e)
        {
            char keyChar = e.KeyChar;

            // Allow control keys (e.g., Backspace) and one dot
            if (char.IsControl(keyChar) || keyChar == '.' && !((TextBox)sender).Text.Contains('.'))
            {
                return;
            }

            // Allow digits
            if (!char.IsDigit(keyChar))
            {
                e.Handled = true; // Block the key
            }
        }

        private void FontSize_Leave(object sender, EventArgs e)
        {
            if (FontSize.Text.Length == 0)
            {
                FontSize.Text = "12";
            }
        }

        private void CustomFontCheck_CheckedChanged(object sender, EventArgs e)
        {
            if (CustomFontCheck.Checked)
            {
                FontPicker.Show();
            }
            else
            {
                FontPicker.Hide();
            }
        }
    }
}
