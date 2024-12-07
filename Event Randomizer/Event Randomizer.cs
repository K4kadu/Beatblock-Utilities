using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;

namespace Event_Randomizer
{
    public class EventRandomizerForm : Form
    {
        public EventRandomizerForm()
        {
            InitializeComponents();
        }

        public void outBox(string input)
        {
            if (outputBox.Text.Equals(input))
                outputBox.Text = " " + input;
            else
                outputBox.Text = input;
        }

        private void InitializeComponents()
        {

            eventLabel = new Label
            {
                Text = "Paste the event that you want to be randomized, starting with { and ending with }.",
                Top = 10,
                Left = 20,
                AutoSize = true,
            };

            // EVENT INPUT
            eventInput = new TextBox
            {
                Multiline = true,
                Width = 600,
                Height = 40,
                Top = 32,
                Left = 20
            };

            List<string> foundProperties = new List<string> { "time", "angle", "order" };

            eventInput.TextChanged += (sender, e) =>
            {
                TextBox? eventInput = sender as TextBox; // Cast the sender to a TextBox
                string text = eventInput.Text;

                // Clear the list to update with new found properties
                foundProperties.Clear();

                // Use a regular expression to find words in quotation marks
                var matches = System.Text.RegularExpressions.Regex.Matches(text, "\"([^\"]+?)\":");

                foreach (var match in matches)
                {
                    foundProperties.Add(match.ToString().Trim(':').Trim('"')); // Remove quotation marks and add to the list
                }

                // Populate property box with elements from the list
                propertyBox.Items.Clear();
                propertyBox.Items.AddRange(foundProperties.ToArray());
                // propertyBox.Items.Remove("time");

                if (propertyBox.Items.Count == 0)
                {
                    propertyBox.Items.Add("no properties found");
                    propertyBox.SelectedIndex = 0;
                }
            };

            propertyLabel = new Label
            {
                Text = "randomized property:",
                Top = 80,
                Left = 20,
                AutoSize = true,
            };

            // PROPERTY BOX
            propertyBox = new ComboBox
            {
                Width = 140,
                Top = 80,
                Left = 160,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Items = { "no properties found" },
                SelectedIndex = 0
            };

            propertyBox.SelectedIndexChanged += (sender, e) =>
            {
                if (propertyBox.SelectedItem != null && propertyBox.SelectedItem.ToString() == "time")
                    acrossTime.Checked = false;
            };

            minValueLabel = new Label
            {
                Text = "minimum value:",
                Top = 120,
                Left = 20,
                AutoSize = true,
            };

            minValueBox = new TextBox
            {
                Text = "0",
                Width = 80,
                Height = 20,
                Top = 120,
                Left = 130
            };

            minValueBox.KeyPress += (sender, e) =>
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
            };

            maxValueLabel = new Label
            {
                Text = "maximum value:",
                Top = 150,
                Left = 20,
                AutoSize = true,
            };

            maxValueBox = new TextBox
            {
                Text = "0",
                Width = 80,
                Height = 20,
                Top = 150,
                Left = 130
            };

            maxValueBox.KeyPress += (sender, e) =>
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
            };

            roundValueLabel = new Label
            {
                Text = "rounded to\na multiple of:",
                Top = 180,
                Left = 20,
                AutoSize = true,
            };

            roundValueBox = new TextBox
            {
                Text = "1",
                Width = 80,
                Height = 20,
                Top = 185,
                Left = 130
            };

            roundValueBox.KeyPress += (sender, e) =>
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
            };

            addMinimumLabel = new Label
            {
                Text = "add minimum value to each result",
                Top = 220,
                Left = 20,
                AutoSize = true,
            };

            addMinimumLabel.Click += (sender, e) =>
             {
                 addMinimumCheck.Checked = !addMinimumCheck.Checked;
             };

            addMinimumCheck = new CheckBox
            {
                Top = 220,
                Left = 210,
                Width = 20,
                Height = 20,
                Checked = true
            };

            acrossTimeLabel = new Label
            {
                Text = "repeat with different timings",
                Top = 80,
                Left = 380,
                AutoSize = true,
            };

            acrossTimeLabel.Click += (sender, e) =>
            {
                acrossTime.Checked = !acrossTime.Checked;
            };

            acrossTime = new CheckBox
            {
                Width = 20,
                Top = 78,
                Left = 360,
                Checked = true
            };

            acrossTime.CheckedChanged += (sender, e) =>
            {
                if (acrossTime.Checked)
                {
                    if (propertyBox.SelectedItem != null && propertyBox.SelectedItem.ToString() == "time")
                        propertyBox.SelectedItem = null;

                    stepSize.Show();
                    stepSizeLabel.Show();
                    stepSizeLabelToo.Show();
                    startingBeatLabel.Show();
                    startingBeat.Show();
                    endingBeatLabel.Show();
                    endingBeat.Show();
                }
                else
                {
                    stepSize.Hide();
                    stepSizeLabel.Hide();
                    stepSizeLabelToo.Hide();
                    startingBeatLabel.Hide();
                    startingBeat.Hide();
                    endingBeatLabel.Hide();
                    endingBeat.Hide();
                }
            };

            stepSizeLabel = new Label
            {
                Text = "Place an event every",
                Top = 120,
                Left = 360,
                AutoSize = true,
            };

            stepSize = new TextBox
            {
                Text = "1",
                Width = 40,
                Height = 20,
                Top = 120,
                Left = 476
            };

            stepSize.KeyPress += (sender, e) =>
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
            };

            stepSizeLabelToo = new Label
            {
                Text = "beats,",
                Top = 120,
                Left = 518,
                AutoSize = true,
            };

            startingBeatLabel = new Label
            {
                Text = "from beat",
                Top = 150,
                Left = 360,
                AutoSize = true,
            };

            startingBeat = new TextBox
            {
                Text = "0",
                Width = 40,
                Height = 20,
                Top = 150,
                Left = 425
            };

            startingBeat.KeyPress += (sender, e) =>
            {
                char keyChar = e.KeyChar;

                // Allow control keys (e.g., Backspace) and one dot
                if (char.IsControl(keyChar) || keyChar == '.' && !((TextBox)sender).Text.Contains('.'))
                {
                    return; // Allow the key
                }

                // Allow digits
                if (!char.IsDigit(keyChar))
                {
                    e.Handled = true; // Block the key
                }
            };

            endingBeatLabel = new Label
            {
                Text = "until beat",
                Top = 180,
                Left = 360,
                AutoSize = true,
            };

            endingBeat = new TextBox
            {
                Text = "0",
                Width = 40,
                Height = 20,
                Top = 180,
                Left = 425
            };

            endingBeat.KeyPress += (sender, e) =>
            {
                char keyChar = e.KeyChar;

                // Allow control keys (e.g., Backspace) and one dot
                if (char.IsControl(keyChar) || keyChar == '.' && !((TextBox)sender).Text.Contains('.'))
                {
                    return; // Allow the key
                }

                // Allow digits
                if (!char.IsDigit(keyChar))
                {
                    e.Handled = true; // Block the key
                }
            };

            // RANDOMIZER BUTTON
            randomizeButton = new Button
            {
                Text = "Randomize",
                Top = 250,
                Left = 20,
                Width = 100
            };

            randomizeButton.Click += (sender, e) =>
            {
                if (propertyBox.Items.Count == 0)
                { outBox("Please provide an event."); return; }

                if (propertyBox.SelectedItem == null)
                { outBox("Please select a property."); return; }

                if (propertyBox.SelectedItem.Equals("no properties found"))
                { outBox("Please provide an event."); return; }

                if (stepSize.Text.Equals("0"))
                { outBox("Placing an event every 0 beats is not possible."); return; }

                if (double.Parse(minValueBox.Text, CultureInfo.InvariantCulture) > double.Parse(maxValueBox.Text, CultureInfo.InvariantCulture))
                { outBox("Make sure that the minimum value is not greater than the maximum value."); return; }

                eventOutput = "";
                Random random = new Random();

                // remove start and end commas to make the tag syntax work
                string inputEvent = eventInput.Text;
                if (inputEvent[0].Equals(','))
                    inputEvent = inputEvent.Substring(1);
                if (inputEvent[inputEvent.Length - 1].Equals(','))
                    inputEvent = inputEvent.Substring(0, inputEvent.Length - 1);

                double counter = double.Parse(startingBeat.Text, CultureInfo.InvariantCulture);
                double threshold = double.Parse(endingBeat.Text, CultureInfo.InvariantCulture);
                string replaced = "";
                for (; counter <= threshold; counter = counter + double.Parse(stepSize.Text, CultureInfo.InvariantCulture))
                {
                    try
                    {
                        // Parse min, max, and round values from input
                        double minValue = double.Parse(minValueBox.Text, CultureInfo.InvariantCulture);
                        double maxValue = double.Parse(maxValueBox.Text, CultureInfo.InvariantCulture);
                        double valueRound = double.Parse(roundValueBox.Text, CultureInfo.InvariantCulture);

                        double randomValue = maxValue + 1;
                        while (randomValue > maxValue || randomValue < minValue)
                        {
                            // Generate a random value between minValue and maxValue
                            randomValue = minValue + (random.NextDouble() * (maxValue - minValue));

                            // Round to the nearest multiple of valueRound
                            if (valueRound != 0)
                                randomValue = Math.Round(randomValue / valueRound) * valueRound;

                            if (addMinimumCheck.Checked)
                                randomValue = randomValue + minValue;
                        }

                        try
                        {
                            // Regex pattern to find "property" and its value
                            string pattern = @$"\""{propertyBox.Text}\"":-?[0-9]*\.?[0-9]+"; // Matches "property": followed by a number
                            // Replacement string with the new value
                            string replacement = $"\"{propertyBox.Text}\":{randomValue.ToString(CultureInfo.InvariantCulture)}";
                            // Replace the "property" value in the input string
                            replaced = Regex.Replace(inputEvent, pattern, replacement) + ",\n";

                            // Regex pattern to find "time" and its value
                            pattern = @"\""time\"":-?[0-9]*\.?[0-9]+"; // Matches "time": followed by a number
                            // Replacement string with the new value
                            replacement = $"\"time\":{counter.ToString(CultureInfo.InvariantCulture)}";
                            // Replace the "time" value in the input string
                            eventOutput = eventOutput + Regex.Replace(replaced, pattern, replacement);
                        }
                        catch
                        {
                            outBox("Something went wrong.");
                        }

                        outBox("all done. :)");
                    }
                    catch
                    {
                        outBox("Please make sure min value and max value are set to a number.");
                    }
                }
            };

            // OUTPUT BOX
            outputBox = new TextBox
            {
                Text = "no output yet",
                Top = 250,
                Left = 140,
                Width = 460,
                ReadOnly = true
            };

            openOutput = new Button
            {
                Text = "view output events",
                Top = 290,
                Left = 20,
                Width = 130,
                Height = 30
            };

            openOutput.Click += (sender, e) =>
            {
                // Create a new Form
                Form outputForm = new Form();
                outputForm.Text = "Event Output";
                outputForm.Size = new Size(800, 600); // Set the size of the new window

                // Create a TextBox to display the eventOutput string
                TextBox textBox = new TextBox();
                textBox.Multiline = true; // Enable multiline to display large content
                textBox.ScrollBars = ScrollBars.Both; // Add both horizontal and vertical scrollbars
                textBox.WordWrap = false; // Disable word wrap
                textBox.Dock = DockStyle.Fill; // Fill the entire form with the TextBox
                textBox.ReadOnly = true; // Make the TextBox read-only

                // Set the text to eventOutput, minus commas
                textBox.Text = eventOutput.Replace("\n", "\r\n"); ;

                // Add the TextBox to the new Form
                outputForm.Controls.Add(textBox);

                // Show the new Form
                outputForm.Show();
            };

            copyOutput = new Button
            {
                Text = "copy to clipboard",
                Top = 290,
                Left = 160,
                Width = 130,
                Height = 30
            };

            copyOutput.Click += (sender, e) =>
            {
                Clipboard.SetText(eventOutput);
            };

            saveAsTag = new Button
            {
                Text = "save as tag",
                Top = 290,
                Left = 300,
                Width = 130,
                Height = 30
            };

            saveAsTag.Click += (sender, e) =>
            {
                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*"; // filter for .json files
                    saveFileDialog.Title = "Save As Tag";
                    saveFileDialog.DefaultExt = "json";
                    saveFileDialog.FileName = "random events.json";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string filePath = saveFileDialog.FileName;
                        File.WriteAllText(filePath, "[\n" + eventOutput + "]");
                        outBox($"File saved successfully to: {filePath}");
                    }
                }
            };

            // Add controls to the form
            Controls.Add(eventLabel);
            Controls.Add(eventInput);

            Controls.Add(propertyLabel);
            Controls.Add(propertyBox);

            Controls.Add(minValueLabel);
            Controls.Add(minValueBox);
            Controls.Add(maxValueLabel);
            Controls.Add(maxValueBox);
            Controls.Add(roundValueLabel);
            Controls.Add(roundValueBox);
            Controls.Add(addMinimumLabel);
            Controls.Add(addMinimumCheck);

            Controls.Add(acrossTime);
            Controls.Add(acrossTimeLabel);
            Controls.Add(stepSizeLabel);
            Controls.Add(stepSize);
            Controls.Add(stepSizeLabelToo);
            Controls.Add(startingBeatLabel);
            Controls.Add(startingBeat);
            Controls.Add(endingBeatLabel);
            Controls.Add(endingBeat);

            Controls.Add(randomizeButton);
            Controls.Add(outputBox);
            Controls.Add(openOutput);
            Controls.Add(copyOutput);
            Controls.Add(saveAsTag);

            // Set form properties
            Text = "Event Randomizer";
            Width = 660;
            Height = 380;
        }

        [STAThread]
        public static void Main()
        { 
            Application.EnableVisualStyles();
            Application.Run(new EventRandomizerForm());
        }


        public string eventOutput = "";
        private Label eventLabel;
        private Label propertyLabel;
        private Label minValueLabel;
        private Label maxValueLabel;
        private Label roundValueLabel;
        private Label addMinimumLabel;
        private CheckBox addMinimumCheck;
        private TextBox eventInput;
        private ComboBox propertyBox;
        private TextBox minValueBox;
        private TextBox maxValueBox;
        private TextBox roundValueBox;
        private Label stepSizeLabel;
        private TextBox stepSize;
        private Label stepSizeLabelToo;
        private Label startingBeatLabel;
        private TextBox startingBeat;
        private Label endingBeatLabel;
        private TextBox endingBeat;
        private Button randomizeButton;
        private TextBox outputBox;
        private Button openOutput;
        private Button saveAsTag;
        private Button copyOutput;
        private Label acrossTimeLabel;
        private CheckBox acrossTime;

    }
}
