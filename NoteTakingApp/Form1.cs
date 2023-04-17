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
        Button deleteButton;

        static string json;
        Dictionary<string, string> notesDic;

        const int MAX_NOTES = 19;
        int noteNum = 0;
        int rightNoteNum = 0;
        int leftNoteNum = 0;

        int[,] initialNoteCoords = 
        {
            { 50, 200 }, { 470, 200 }
        };

        int noteHeight = 350;
        int width;

        string? currText;

        public Form1()
        {
            InitializeComponent();
            width = ClientSize.Width;
            CheckNoteFile();
        }

        private int CalculateNotePosition(int dir, int side)
        {
            int calcPos = (side == 0) ? initialNoteCoords[side, dir] + ((noteHeight + 50) * leftNoteNum) 
                : initialNoteCoords[side, dir] + ((noteHeight + 50) * rightNoteNum);
            return calcPos;
        }

        private int CalculateNoteWidth()
        {
            int calcWidth = (width - 100) / 2;
            return calcWidth;
        }

        private void NoteInit(string text)
        {
            note = new TextBox();
            note.Text = text;
            note.Name = $"Note{noteNum}";
            note.Location = (noteNum % 2 == 0) ? new Point(initialNoteCoords[0, 0], CalculateNotePosition(1, 0)) 
                : new Point(initialNoteCoords[1, 0], CalculateNotePosition(1, 1));
            if (noteNum % 2 == 0)
            {
                leftNoteNum++;
            }
            else
            { 
                rightNoteNum++;
            }

            note.Size = new Size(CalculateNoteWidth(), noteHeight);
            note.Anchor = (noteNum % 2 == 0) ? leftNoteAnchor : rightNoteAnchor;
            note.Multiline = true;
            note.ScrollBars = ScrollBars.Vertical;
            note.TextChanged += Note_TextChanged;
            this.Controls.Add(note);
        }

        private void DeleteButtonInit()
        {
            int buttonWidth = 23;
            int buttonHeight = 350;
            deleteButton = new Button();
            deleteButton.Text = "Delete";
            deleteButton.Name = $"DeleteButton{noteNum}";
            deleteButton.Location = (noteNum % 2 == 0) ? new Point(initialNoteCoords[0, 0] + CalculateNoteWidth() - buttonWidth, CalculateNotePosition(1, 0)) 
                : new Point(initialNoteCoords[1, 0] + CalculateNoteWidth() - buttonWidth, CalculateNotePosition(1, 1));
            deleteButton.Size = new Size(buttonWidth, buttonHeight);
            deleteButton.Anchor = (noteNum % 2 == 0) ? leftNoteAnchor : rightNoteAnchor;
            deleteButton.BringToFront();
            // deleteButton.Click += DeleteButton_Click;
            this.Controls.Add(deleteButton);
        }

        private void Note_TextChanged(object sender, EventArgs e)
        {
            TextBox noteTextBox = sender as TextBox;
            string noteName = noteTextBox.Name;
            string noteText = noteTextBox.Text;
            currText = noteText;

            json = File.ReadAllText("Note.json");
            notesDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();

            notesDic[noteName] = currText;

            json = JsonConvert.SerializeObject(notesDic);
            File.WriteAllText("Note.json", json);
        }
        private void NoteTitle_TextChanged(object sender, EventArgs e)
        {
            currText = NoteTitle.Text;
        }

        private void CheckNoteFile()
        {
            json = File.ReadAllText("Note.json");
            notesDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();

            foreach (string key in notesDic.Keys)
            {
                if (notesDic.TryGetValue(key, out string noteText) && !string.IsNullOrEmpty(noteText))
                {
                    DeleteButtonInit();
                    NoteInit(noteText);
                    noteNum++;
                }
            }
        }

        private void AddNote_Click(object sender, EventArgs e)
        {
            if (noteNum < MAX_NOTES && currText != null)
            {
                notesDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();

                notesDic.Add($"Note{noteNum}", currText);

                json = JsonConvert.SerializeObject(notesDic);
                File.WriteAllText("Note.json", json);

                NoteInit(currText);

                noteNum++;
                NoteTitle.Text = null;
                currText = null;
            }
        }

        private void DeleteButton_Click()
        {

        }
    }
}
