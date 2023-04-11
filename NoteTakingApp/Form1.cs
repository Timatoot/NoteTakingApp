using System;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace NoteTakingApp
{
    public partial class Form1 : Form
    {
        AnchorStyles leftNoteAnchor = AnchorStyles.Top | AnchorStyles.Left;
        AnchorStyles rightNoteAnchor = AnchorStyles.Top | AnchorStyles.Right;

        TextBox note;

        const int MAX_NOTES = 20;

        int[,] initialNoteCoords = 
        {
            { 50, 200 }, { 450, 200 }
        };

        int noteHeight = 350;
        
        int noteNum = 0;
        string? currText;

        int rightNoteNum = 0;
        int leftNoteNum = 0;

        int width;

        public Form1()
        {
            InitializeComponent();
            width = ClientSize.Width;
            CheckNoteFile();
        }

        private int CalculateNotePosition(int dir, int side)
        {
            int calcPos;
            if (side == 0)
            {
                calcPos = initialNoteCoords[side, dir] + ((noteHeight + 50) * leftNoteNum);
                leftNoteNum++;
                return calcPos;
            }
            else
            {
                calcPos = initialNoteCoords[side, dir] + ((noteHeight + 50) * rightNoteNum);
                rightNoteNum++;
                return calcPos;
            }
        }

        private void NoteInit(string text)
        {
            note = new TextBox();
            note.Text = text;
            note.Location = (noteNum % 2 == 0) ? new Point(initialNoteCoords[0, 0], CalculateNotePosition(1, 0)) 
                : new Point(initialNoteCoords[1, 0], CalculateNotePosition(1, 1));
            note.Size = new Size(width / 2 - 100, noteHeight);
            note.Anchor = (noteNum % 2 == 0) ? leftNoteAnchor : rightNoteAnchor;
            note.Multiline = true;
            note.ScrollBars = ScrollBars.Vertical;
            this.Controls.Add(note);
        }

        private void NoteTitle_TextChanged(object sender, EventArgs e)
        {
            currText = NoteTitle.Text;
        }

        private void CheckNoteFile()
        {
            for (int i = 0; i <= MAX_NOTES; i++)
            {
                try
                {
                    string fileText = File.ReadAllText($"Note{i}.json");
                    NoteInit(fileText);
                    noteNum++;
                }
                catch
                {
                    break;
                }
            }
        }

        private void AddNote_Click(object sender, EventArgs e)
        {
            if (noteNum < MAX_NOTES && currText != null)
            {
                NoteInit(currText);

                string json = JsonConvert.SerializeObject(currText);
                File.WriteAllText($"Note{noteNum}.json", json);
               
                noteNum++;
                NoteTitle.Text = null;
                currText = null;
            }
        }

        /*void DeleteNote_Click(object sender, EventArgs e)
        {
            if (noteNum > 0)
            {
                this.Controls.Remove(note);
                File.Delete($"Note{noteNum - 1}.json");
                noteNum--;
            }
        }*/

        private void UpdateNote()
        {
            for (int i = 0; i < noteNum; i++)
            {
                string fileText = File.ReadAllText($"Note{i}.json");
                if (fileText != note.Text)
                {
                    string json = JsonConvert.SerializeObject(note.Text);
                    File.WriteAllText($"Note{i}.json", json);
                }
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            UpdateNote();
        }
    }
}
