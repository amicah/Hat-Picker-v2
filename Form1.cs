/*
    HatPicker v2.0
    Author: A Micah McClain
    Email: a.micah.mcclain@gmail.com
    Date: 10/4/2022
    Description:
        An updated version of my old HatPicker program. Enter names into a digital Hat, and draw them! Perfect for
        Secret Santa-type situations. Implements new features as well as bugfixes.
*/

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO;

namespace HatPicker
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            saveToolStripMenuItem.Enabled = false;
            copyToolStripMenuItem.Enabled = false;
        }

        private Random rand = new Random();
        private int listSize;
        private List<string> saveList;
        private StreamWriter sw;

        // Add item to hatBox and clear the entryBox
        private void insertItem(string item)
        {
            addButton.Enabled = false;
            if (item != "Name" && item != "")
            {
                if (!hatBox.Items.Contains(item))
                {
                    hatBox.Items.Add(item);
                }
                else
                {
                    MessageBox.Show("Duplicate entry.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            entryBox.Clear();
            addButton.Enabled = true;
        }

        // Adds item to hatBox by pressing 'Enter'
        private void entryBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == ((char)Keys.Return))
            {
                insertItem(entryBox.Text);
                e.Handled = true;
            }
        }

        // Adds item to hatBox using the addButton
        private void addButton_Click(object sender, EventArgs e)
        {
            insertItem(entryBox.Text);
        }

        // Removes selected item from hatBox
        private void deleteButton_Click(object sender, EventArgs e)
        {
            deleteButton.Enabled = false;
            if (hatBox.SelectedIndex != -1) 
            { 
                hatBox.Items.RemoveAt(hatBox.SelectedIndex);
            }
            deleteButton.Enabled = true;
        }

        // Shuffles the Hat, draws names, and display it in the drawBox
        private void drawButton_Click(object sender, EventArgs e)
        {
            drawButton.Enabled = false;
            this.listSize = hatBox.Items.Count;
            this.saveList = new List<string>(this.listSize);
            List<string> hatList = new List<string>(this.listSize);
            List<string> drawList = new List<string>(this.listSize);

            if (this.listSize > 0)
            {
                // Clear previous contents of outputBox and get the contents of inputsBox
                drawBox.Items.Clear();
                for (int i = 0; i < this.listSize; i++)
                {
                    hatList.Add(hatBox.Items[i].ToString());
                }
                // Shuffle the contents of inputsBox and display in outputsBox
                bool reroll = true;
                int counter = 0;
                while (reroll)
                {
                    int rNum = this.rand.Next(0, this.listSize);
                    // Last pick
                    if (counter == (this.listSize - 1))
                    {
                        // Start over if the last person picks themself
                        if (!drawList.Contains(hatList[counter]))
                        {
                            counter = 0;
                            drawList.Clear();
                        }
                        // Otherwise add the last pick and end the reroll
                        else if (!drawList.Contains(hatList[rNum]))
                        {
                            drawList.Add(hatList[rNum]);
                            reroll = false;
                        }
                    }
                    // Otherwise, pick a random name. If it's invalid, try again.
                    else
                    {
                        if (counter != rNum && !drawList.Contains(hatList[rNum]))
                        {
                            drawList.Add(hatList[rNum]);
                            counter++;
                        }
                    }
                }
                // Display the shuffled items and prepare saveList
                for (int i = 0; i < this.listSize; i++)
                {
                    drawBox.Items.Add(drawList[i]);
                    this.saveList.Add(hatBox.Items[i].ToString() + " -> " + drawBox.Items[i].ToString());
                }
            }
            saveToolStripMenuItem.Enabled = true;
            copyToolStripMenuItem.Enabled = true;
            drawButton.Enabled = true;
        }

        // Close program via menu
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        // Save file as .txt formatted as 'Name1 -> Name2' list
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveFile = new SaveFileDialog();
            saveFile.Filter = "Text File (*.txt)|*.txt";
            saveFile.Title = "Save text file";
            if (saveFile.ShowDialog() == DialogResult.OK)
            {
                sw = new StreamWriter(saveFile.FileName);
                for (int i = 0; i < listSize; i++)
                {
                    sw.WriteLine(saveList[i].ToString());
                }
                sw.Close();
            }
        }

        // Clear the hatBox and the drawBox
        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            hatBox.Items.Clear();
            drawBox.Items.Clear();
        }

        // Copy text formatted as 'Name1 -> Name2' list to clipboard
        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string copyText = "";
            for (int i = 0; i < listSize; i++)
            {
                copyText += saveList[i].ToString();
                if (i != listSize - 1)
                {
                    copyText += "\n";
                }
            }
            Clipboard.SetText(copyText);
        }

        // Small info box
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Add names to the digital hat,\nthen shuffle and draw names." +
                "\n\n\nAuthor:\nA Micah McClain\na.micah.mcclain@gmail.com",
                "HatPicker v2.0",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information
            );
        }
    }
}