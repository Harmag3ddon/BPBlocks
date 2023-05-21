using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using IniParser;
using IniParser.Model;

namespace BPBlocks
{
    public partial class BPBlocksGUI : Form
    {
        public BPBlocksGUI()
        {
            InitializeComponent();
            this.TopMost = true; // Set the TopMost property to true
            flowLayoutPanel = new FlowLayoutPanel();
            flowLayoutPanel.Dock = DockStyle.Top; // Set the docking style to control expansion
            flowLayoutPanel.AutoSize = true; // Enable automatic sizing
            this.Controls.Add(flowLayoutPanel); // Add the FlowLayoutPanel to the form
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            //TODO Settings for default starting pos of GUI
            //TODO Check how to make edit file from GUI and reset GUI to new values

            string userName = Environment.UserName;
            string customConfigPath = $@"C:\Users\{userName}\BPBlockConfig.ini";
            // Check if file Config file exists
            if (File.Exists(customConfigPath))
            {
                //do nothing
            }
            else
            {
                CreateIniFile(customConfigPath);
            }

            // Read the INI file
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(customConfigPath);

            // Specify the section you want to retrieve buttons from
            string sectionName = "Buttons";
            var section = data.Sections[sectionName];

            // Create buttons dynamically based on the keys in the section
            foreach (var buttonData in section)
            {
                string buttonName = buttonData.KeyName;
                string buttonText = buttonData.Value;

                // Create a new button
                var button = new Button
                {
                    Name = buttonName,
                    Text = buttonName, // Text on the button
                    Tag = buttonText, // value to be copy pasted
                    AutoSize = true,
                    Margin = new Padding(5), // Adjust the margin as needed
                    TextAlign = ContentAlignment.MiddleCenter
                };
                button.Click += Button_Click;
                // Add the button to the form
                flowLayoutPanel.Controls.Add(button);
            }
            ResizeGUI();

        }
        private void Button_Click(object sender, EventArgs e)
        {
            // Handle button click event here
            Button button = (Button)sender;
            Clipboard.SetText(button.Tag.ToString());
            //MessageBox.Show($"Button '{button.Text}' clicked! Value: '{button.Tag}'"); Testing purpouses
        }

        private void CreateIniFile(string ConfigFilePath)
        {
            using (StreamWriter writer = new StreamWriter(ConfigFilePath))
            {
                writer.WriteLine("[Buttons]");
                writer.WriteLine("Input=testas1");
                writer.WriteLine("Output=testas2");
            }
        }

        private void ResizeGUI()
        {
            int padding = 20; // Additional padding for aesthetic purposes
            int desiredWidth = flowLayoutPanel.Width; // Desired width of the form
            int desiredHeight = flowLayoutPanel.Height + (1 * padding); // Desired height of the form
            this.ClientSize = new Size(desiredWidth, desiredHeight);
        }
    }
}
