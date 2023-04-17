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

        static string json = File.ReadAllText("Note.json");
        Dictionary<string, string> notesDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json) 
            ?? new Dictionary<string, string>();

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

        /// <summary>
        /// Takes in the direction and side of the note and calculates the position of the note
        /// </summary>
        /// <param name="dir"> Either a 0 or 1. 0 is x value, 1 is y value</param>
        /// <param name="side"> Either a 0 or 1. 0 is left note, 1 is right note</param>
        /// <returns> The calculated position of the note</returns>
        private int CalculateNotePosition(int dir, int side)
        {
            int calcPos = (side == 0) ? 
                initialNoteCoords[side, dir] + ((noteHeight + 50) * leftNoteNum) 
                : initialNoteCoords[side, dir] + ((noteHeight + 50) * rightNoteNum);
            return calcPos;
        }

        /// <summary>
        /// Calculates the width of the note
        /// </summary>
        /// <returns> Calculated width</returns>
        private int CalculateNoteWidth()
        {
            int calcWidth = (width - 100) / 2;
            return calcWidth;
        }

        /// <summary>
        /// Initializes the note with the given parameters
        /// </summary>
        /// <param name="text"> Takes in the text that will be displayed</param>
        private void NoteInit(string text)
        {
            note = new TextBox();
            note.Text = text;
            note.Name = $"Note{noteNum}";
            note.Location = (noteNum % 2 == 0) ? 
                new Point(initialNoteCoords[0, 0], CalculateNotePosition(1, 0)) 
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

        /// <summary>
        /// Initiallizes the delete button
        /// </summary>
        private void DeleteButtonInit()
        {
            int buttonWidth = 23;
            int buttonHeight = 350;
            deleteButton = new Button();
            deleteButton.Text = "Delete";
            deleteButton.Name = $"DeleteButton{noteNum}";
            deleteButton.Location = (noteNum % 2 == 0) ? 
                new Point(initialNoteCoords[0, 0] + CalculateNoteWidth() - buttonWidth, CalculateNotePosition(1, 0)) 
                : new Point(initialNoteCoords[1, 0] + CalculateNoteWidth() - buttonWidth, CalculateNotePosition(1, 1));
            deleteButton.Size = new Size(buttonWidth, buttonHeight);
            deleteButton.Anchor = (noteNum % 2 == 0) ? leftNoteAnchor : rightNoteAnchor;
            deleteButton.BringToFront();
            //deleteButton.Click += DeleteButton_Click;
            this.Controls.Add(deleteButton);
        }

        /// <summary>
        /// Used for note updates. When note text is changed, the text is re-saved to the json file
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Note_TextChanged(object sender, EventArgs e)
        {
            TextBox noteTextBox = sender as TextBox;
            string noteName = noteTextBox.Name;
            string noteText = noteTextBox.Text;
            currText = noteText;

            notesDic[noteName] = currText;
            json = JsonConvert.SerializeObject(notesDic);
            File.WriteAllText("Note.json", json);
        }

        /// <summary>
        /// Gets the typed text from the note taking textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoteTitle_TextChanged(object sender, EventArgs e)
        {
            currText = NoteTitle.Text;
        }
        
        /// <summary>
        /// Opens and reads the json file when the program opens. If there is a note saved, it will be displayed.
        /// </summary>
        private void CheckNoteFile()
        {
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

        /// <summary>
        /// Adds a note to the json file and displays it on the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddNote_Click(object sender, EventArgs e)
        {
            if (noteNum < MAX_NOTES && currText != null)
            {
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
