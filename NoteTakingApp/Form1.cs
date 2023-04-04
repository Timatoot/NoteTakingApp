using System;
using System.Linq.Expressions;
using Newtonsoft.Json;

namespace NoteTakingApp
{
    public partial class Form1 : Form
    {
        AnchorStyles leftNoteAnchor = AnchorStyles.Top | AnchorStyles.Left;
        AnchorStyles rightNoteAnchor = AnchorStyles.Top | AnchorStyles.Right;

        TextBox note;

        int[,] initialNoteCoords = 
        {
            { 50, 150 }, { 450, 150 }
        };

        int noteWidth = 350;
        int noteHeight = 350;
        
        int noteNum = 0;
        string? currText;

        int rightNoteNum = 0;
        int leftNoteNum = 0;



        public Form1()
        {
            InitializeComponent();
            CheckNoteFile();
        }

        private int CalculateNotePosition(int dir, int side)
        {
            int calcPos;
            if (side == 0)
            {
                calcPos = initialNoteCoords[side, dir] + (noteHeight * leftNoteNum) + 50;
                leftNoteNum++;
                return calcPos;
            }
            else
            {
                calcPos = initialNoteCoords[side, dir] + (noteHeight * rightNoteNum) + 50;
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
            note.Size = new Size(noteWidth, noteHeight);
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
            for (int i = 0; i <= 20; i++)
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
            if (noteNum < 20 && currText != null)
            {
                NoteInit(currText);

                string json = JsonConvert.SerializeObject(currText);
                File.WriteAllText($"Note{noteNum}.json", json);
               
                noteNum++;
                NoteTitle.Text = null;
                currText = null;
            }
        }
    }
}
