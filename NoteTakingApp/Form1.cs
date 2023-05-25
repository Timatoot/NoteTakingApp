using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Runtime.InteropServices;
using MongoDB.Bson;
using System.Net.Sockets;
using System.Text;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using Newtonsoft.Json.Linq;
using System.Timers;

namespace NoteTakingApp
{
    public partial class Form1 : Form
    { 
        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]

        private static extern IntPtr CreateRoundRectRgn
        (
            int nLeftRect, // x-coordinate of upper-left corner
            int nTopRect, // y-coordinate of upper-left corner
            int nRightRect, // x-coordinate of lower-right corner
            int nBottomRect, // y-coordinate of lower-right corner
            int nWidthEllipse, // width of ellipse
            int nHeightEllipse // height of ellipse
        );

        string dbUser;
        string dbPass;
        string dbName;
        string dbCollection;

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
            dbUser = Program.DbUser;
            dbPass = Program.DbPass;
            dbName = "NoteApp";
            dbCollection = "Notes";

            json = ReadDB(dbUser, dbPass, dbName, dbCollection);
            notesDic = JsonConvert.DeserializeObject<Dictionary<string, string>>(json)
                ?? new Dictionary<string, string>();

            InitializeComponent();
            width = ClientSize.Width;
            CheckNoteFile();
            FormClosed += OnFormClose;
        }

        public void WriteDB()
        {
            var client = new MongoClient($"mongodb+srv://{dbUser}:{dbPass}@noteapptim.9l2spze.mongodb.net/?authSource=admin");
            var database = client.GetDatabase(dbName);
            var collection = database.GetCollection<BsonDocument>(dbCollection);
            var objectId = new ObjectId("64679d70d2432a77fbaa9fe9");

            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);

            string jsonFilePath = "C:\\Users\\Tim\\source\\repos\\NoteTakingApp\\NoteTakingApp\\bin\\Debug\\net6.0-windows\\Note.json";
            JObject json = JObject.Parse(File.ReadAllText(jsonFilePath));

            BsonDocument bson = BsonDocument.Parse(json.ToString());

            var document = collection.Find(filter).FirstOrDefault();
            if (document != null)
            {
                collection.ReplaceOne(filter, bson);
            }
            else
            {
                bson["_id"] = ObjectId.GenerateNewId();
            }
        }

        public static string ReadDB(string dbUser, string dbPass, string dbName, string dbCollection)
        {
            var client = new MongoClient($"mongodb+srv://{dbUser}:{dbPass}@noteapptim.9l2spze.mongodb.net/?authSource=admin");
            var database = client.GetDatabase(dbName);
            var collection = database.GetCollection<BsonDocument>(dbCollection);
            var objectId = new ObjectId("64679d70d2432a77fbaa9fe9");
            var filter = Builders<BsonDocument>.Filter.Eq("_id", objectId);

            // Define a projection that excludes the '_id' field.
            var projection = Builders<BsonDocument>.Projection.Exclude("_id");

            var document = collection.Find(filter).Project(projection).FirstOrDefault();
            var jsons = document.ToJson();

            return jsons;
        }

        /// <summary>
        /// Runs when the form is loaded
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            AddNote.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, AddNote.Width, AddNote.Height, 20, 20));
            NoteTitle.Region = Region.FromHrgn(CreateRoundRectRgn(1, 0, NoteTitle.Width, NoteTitle.Height, 20, 20));

            if (!File.Exists("Note.json"))   
            {

                File.WriteAllText("Note.json", JsonConvert.SerializeObject(notesDic));
            }
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
            note.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, note.Width, note.Height, 20, 20));
            note.TextChanged += Note_TextChanged;
            Controls.Add(note);
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
            deleteButton.FlatAppearance.BorderSize = 0;
            deleteButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            deleteButton.BackColor = System.Drawing.Color.Gray;
            deleteButton.Location = (noteNum % 2 == 0) ?
                new Point(initialNoteCoords[0, 0] + CalculateNoteWidth() - buttonWidth, CalculateNotePosition(1, 0)) 
                : new Point(initialNoteCoords[1, 0] + CalculateNoteWidth() - buttonWidth, CalculateNotePosition(1, 1));
            deleteButton.Size = new Size(buttonWidth, buttonHeight);
            deleteButton.Anchor = (noteNum % 2 == 0) ? leftNoteAnchor : rightNoteAnchor;
            deleteButton.Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, deleteButton.Width, deleteButton.Height, 20, 20));
            deleteButton.BringToFront();
            deleteButton.Click += new System.EventHandler(DeleteButton_Click);
            Controls.Add(deleteButton);
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

                DeleteButtonInit();
                NoteInit(currText);

                noteNum++;
                NoteTitle.Text = null;
                currText = null;
            }
        }

        /// <summary>
        /// Deletes a note from the json file and removes it from the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            Button deleteButton = sender as Button;
            char noteNumber = deleteButton.Name[deleteButton.Name.Length-1];
            string noteName = $"Note{noteNumber}";

            notesDic.Remove(noteName);
            Controls.RemoveByKey(noteName);
            Controls.RemoveByKey(deleteButton.Name);
            noteNum--;
            for (int i = 0; i < notesDic.Count; i++)
            {
                string oldName = $"Note{i + 1}";
                if (notesDic.ContainsKey(oldName))
                {
                    string newName = $"Note{i}";
                    string value = notesDic[oldName];
                    notesDic.Remove(oldName);
                    if (!notesDic.ContainsKey(newName))
                    {
                        notesDic.Add(newName, value);
                    }
                    else
                    {
                        notesDic.Add(oldName, value);
                    }
                }
            }
            json = JsonConvert.SerializeObject(notesDic);
            File.WriteAllText("Note.json", json);
            leftNoteNum = 0;
            rightNoteNum = 0;
            for (int i = 0; i <= noteNum; i++)
            {
                Controls.RemoveByKey($"Note{i}");
                Controls.RemoveByKey($"DeleteButton{i}");
            }
            noteNum = 0;
            CheckNoteFile();
        }
        private void OnFormClose(object sender, FormClosedEventArgs e)
        {
            WriteDB();
        }
    }
}
