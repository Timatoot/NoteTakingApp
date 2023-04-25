namespace NoteTakingApp
{
    public partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        public int formWidth = 900;
        public int formHeight = 600;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.NoteTitle = new System.Windows.Forms.TextBox();
            this.AddNote = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // NoteTitle
            // 
            this.NoteTitle.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.NoteTitle.Location = new System.Drawing.Point(50, 150);
            this.NoteTitle.Name = "NoteTitle";
            this.NoteTitle.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.NoteTitle.Size = new System.Drawing.Size(750, 27);
            this.NoteTitle.TabIndex = 0;
            this.NoteTitle.TextChanged += new System.EventHandler(this.NoteTitle_TextChanged);
            // 
            // AddNote
            // 
            this.AddNote.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.AddNote.AutoEllipsis = true;
            this.AddNote.BackColor = System.Drawing.Color.White;
            this.AddNote.Cursor = System.Windows.Forms.Cursors.Hand;
            this.AddNote.FlatAppearance.BorderSize = 0;
            this.AddNote.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddNote.Location = new System.Drawing.Point(810, 150);
            this.AddNote.Name = "AddNote";
            this.AddNote.Size = new System.Drawing.Size(50, 27);
            this.AddNote.TabIndex = 0;
            this.AddNote.Text = "Add";
            this.AddNote.Click += new System.EventHandler(this.AddNote_Click);
            // 
            // Form1
            // 
            this.AutoScroll = true;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(130)))), ((int)(((byte)(147)))), ((int)(((byte)(172)))));
            this.ClientSize = new System.Drawing.Size(900, 600);
            this.Controls.Add(this.NoteTitle);
            this.Controls.Add(this.AddNote);
            this.Name = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox NoteTitle;
        private Button AddNote;
    }
}
