using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using Toub.Sound.Midi;
using System.Threading;

namespace LeeHolmes.Music
{
	/// <summary>
	/// Summary description for NoteDisplay.
	/// </summary>
	public class NoteDisplay : System.Windows.Forms.UserControl
	{
        double basePosition = 160;
        double multiplier = 4.5;

		private System.Windows.Forms.Label Staff;
		private string currentNote;
		private System.Windows.Forms.Label NoteLabel;
        private Keys.Key currentKey;
        private int pauseLength;
        private int playLength;

        string[] notes;

		/// <summary> 
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
        private System.Windows.Forms.Label noteNameDisplay;
        private System.Windows.Forms.Label keyLabel;
        private System.Windows.Forms.Label sharp6;
        private System.Windows.Forms.Label sharp5;
        private System.Windows.Forms.Label sharp4;
        private System.Windows.Forms.Label sharp3;
        private System.Windows.Forms.Label sharp2;
        private System.Windows.Forms.Label sharp1;
        private System.Windows.Forms.Label sharp7;
        private System.Windows.Forms.Panel panel1;
		
		// Support click notification
		public delegate void NoteClick(string whichNote);
		private NoteClick onClicked;

		public NoteDisplay()
		{
            InitializeComponent();

            try
            {
                MidiPlayer.OpenMidi();
            }
            catch
            {
                MessageBox.Show("Error: Could not open MIDI output device.", 
                    "Error opening MIDI device", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            CurrentKey = new Keys.A();

            this.Paint += new PaintEventHandler(OnPaint);
            this.Click += new EventHandler(OnClick);
            currentNote = "A4";

            PauseLength = 700;
            PlayLength = 300;
		}

		/// <summary>
		/// Delegate to notify you when someone's clicked a note
		/// </summary>
		public NoteClick NoteNotify
		{
			get { return onClicked; }
			set { onClicked = value; }
		}

		public string[] NoteScale
		{
			get { return notes; }
			set { if(value != null) notes = value;}
		}

        public int PauseLength
        {
            get { return pauseLength; }
            set { pauseLength = value; }
        }

        public int PlayLength
        {
            get { return playLength; }
            set { playLength = value; }
        }

        public LeeHolmes.Music.Keys.Key CurrentKey
        {
            get { return currentKey; }
            set 
            { 
                currentKey = value; 
                keyLabel.Text = "Training Set: " + currentKey.FriendlyName + ", Note:";

                NoteScale = currentKey.NoteScale;

                // Draw the key signature
                sharp1.Visible = false;
                sharp2.Visible = false;
                sharp3.Visible = false;
                sharp4.Visible = false;
                sharp5.Visible = false;
                sharp6.Visible = false;
                sharp7.Visible = false;

                // Draw the key signature
                if(currentKey.Sharps > 0)
                    sharp1.Visible = true;
                if(currentKey.Sharps > 1)
                    sharp2.Visible = true;
                if(currentKey.Sharps > 2)
                    sharp3.Visible = true;
                if(currentKey.Sharps > 3)
                    sharp4.Visible = true;
                if(currentKey.Sharps > 4)
                    sharp5.Visible = true;
                if(currentKey.Sharps > 5)
                    sharp6.Visible = true;
                if(currentKey.Sharps > 6)
                    sharp7.Visible = true;
            }
        }

		public string Note
		{
			get
			{
				return currentNote;
			}
			set
			{
				currentNote = value;
                noteNameDisplay.Text = currentNote.Substring(0, (currentNote.Length - 1));
				this.Refresh();
				PlayThread pt = new PlayThread(currentNote, currentKey, PauseLength, PlayLength);
				Thread t = new Thread(new ThreadStart(pt.PlayNote));
				t.Start();
			}
		}

		private int CalculatePosition(string whichNote)
		{
			Hashtable positions = InitializePositions();
			return (int) (double) positions[whichNote];
		}

		private Hashtable InitializePositions()
		{
			Hashtable positions = new Hashtable();
			positions["E3"] = basePosition;
			positions["F3"] = basePosition - (1.0 * multiplier);
			positions["F#3"] = basePosition - (1.0 * multiplier);
			positions["G3"] = basePosition - (2.0 * multiplier);
			positions["G#3"] = basePosition - (2.0 * multiplier);
			positions["A3"] = basePosition - (3.0 * multiplier);
			positions["A#3"] = basePosition - (3.0 * multiplier);
			positions["B3"] = basePosition - (4.0 * multiplier);
			positions["C4"] = basePosition - (5.0 * multiplier);
			positions["C#4"] = basePosition - (5.0 * multiplier);
			positions["D4"] = basePosition - (6.0 * multiplier);
			positions["D#4"] = basePosition - (6.0 * multiplier);
			positions["E4"] = basePosition - (7.0 * multiplier);
			positions["F4"] = basePosition - (8.0 * multiplier);
			positions["F#4"] = basePosition - (8.0 * multiplier);
			positions["G4"] = basePosition - (9.0 * multiplier);
			positions["G#4"] = basePosition - (9.0 * multiplier);
			positions["A4"] = basePosition - (10.0 * multiplier);
			positions["A#4"] = basePosition - (10.0 * multiplier);
			positions["B4"] = basePosition - (11.0 * multiplier);
			positions["C5"] = basePosition - (12.0 * multiplier);
			positions["C#5"] = basePosition - (12.0 * multiplier);
			positions["D5"] = basePosition - (13.0 * multiplier);
			positions["D#5"] = basePosition - (13.0 * multiplier);
			positions["E5"] = basePosition - (14.0 * multiplier);
			positions["F5"] = basePosition - (15.0 * multiplier);
			positions["F#5"] = basePosition - (15.0 * multiplier);
			positions["G5"] = basePosition - (16.0 * multiplier);
			positions["G#5"] = basePosition - (16.0 * multiplier);
			positions["A5"] = basePosition - (17.0 * multiplier);
            positions["A#5"] = basePosition - (18.0 * multiplier);
            positions["B5"] = basePosition - (19.0 * multiplier);
            positions["B#5"] = basePosition - (20.0 * multiplier);
            positions["C6"] = basePosition - (21.0 * multiplier);
            positions["C#6"] = basePosition - (22.0 * multiplier);
            positions["D6"] = basePosition - (23.0 * multiplier);
            positions["D#6"] = basePosition - (24.0 * multiplier);
            positions["E6"] = basePosition - (25.0 * multiplier);

			return positions;
		}

		// Process clicks
		protected void OnClick(Object sender, EventArgs e)
		{
			if(onClicked != null)
			{
				System.Drawing.Point pt = this.PointToClient(Cursor.Position);
				string[] notes = NoteScale; 

				foreach(string note in notes)
				{
					int notePosition = (CalculatePosition(note) + 40);
					if (Math.Abs(pt.Y - notePosition) < 3)
						onClicked(note);
				}
			}
		}

		// Draw the staff
		protected void OnPaint(Object sender, PaintEventArgs pe)
		{
            SuspendLayout();

			Graphics graphics = pe.Graphics;
			Pen drawPen = new Pen(Color.Black, 2);

			// Draw the lower ledger lines (if required)
            for (int y = (int)(basePosition + (3.5 * multiplier)); y < (CalculatePosition(Note.ToString()) + 93); y += 9) 
				graphics.DrawLine(drawPen, 83, y, 98, y);

			// Set the note font
			if(Note.ToString().IndexOf("#") > 0)
				this.NoteLabel.Text = "FM";
			else
				this.NoteLabel.Text = "M";

            // Position the note
			this.NoteLabel.Location = 
                new System.Drawing.Point(80 - (5 * (NoteLabel.Text.Length - 1)), 
                    CalculatePosition(Note.ToString()));
            
			// Draw the upper ledger lines (if required)
            // Todo

            //string[] positions = { "A5" };
            string[] positions = { "E3", "G3", "B3", "D4", "F4"  };
			foreach(string position in positions)
			{
				int y = CalculatePosition(position);
				graphics.DrawLine(drawPen, 0, y, 400, y);
			}

            ResumeLayout(true);
		}


		/// <summary> 
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
                    MidiPlayer.CloseMidi();
				}
			}
			base.Dispose( disposing );
		}

		#region Component Designer generated code
		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.Staff = new System.Windows.Forms.Label();
            this.NoteLabel = new System.Windows.Forms.Label();
            this.noteNameDisplay = new System.Windows.Forms.Label();
            this.keyLabel = new System.Windows.Forms.Label();
            this.sharp7 = new System.Windows.Forms.Label();
            this.sharp6 = new System.Windows.Forms.Label();
            this.sharp5 = new System.Windows.Forms.Label();
            this.sharp4 = new System.Windows.Forms.Label();
            this.sharp3 = new System.Windows.Forms.Label();
            this.sharp2 = new System.Windows.Forms.Label();
            this.sharp1 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // Staff
            // 
            this.Staff.BackColor = System.Drawing.Color.Transparent;
            this.Staff.Font = new System.Drawing.Font("MusetteMusic", 22.25F);
            this.Staff.Location = new System.Drawing.Point(0, 109);
            this.Staff.Name = "Staff";
            this.Staff.Size = new System.Drawing.Size(40, 80);
            this.Staff.TabIndex = 0;
            this.Staff.Text = "A";
            // 
            // NoteLabel
            // 
            this.NoteLabel.BackColor = System.Drawing.Color.Transparent;
            this.NoteLabel.Font = new System.Drawing.Font("MusetteMusic", 18.25F);
            this.NoteLabel.Location = new System.Drawing.Point(0, 63);
            this.NoteLabel.Name = "NoteLabel";
            this.NoteLabel.Size = new System.Drawing.Size(40, 48);
            this.NoteLabel.TabIndex = 1;
            this.NoteLabel.Text = "M";
            // 
            // noteNameDisplay
            // 
            this.noteNameDisplay.Font = new System.Drawing.Font("Arial", 8.25F);
            this.noteNameDisplay.Location = new System.Drawing.Point(223, 19);
            this.noteNameDisplay.Name = "noteNameDisplay";
            this.noteNameDisplay.Size = new System.Drawing.Size(24, 23);
            this.noteNameDisplay.TabIndex = 2;
            this.noteNameDisplay.Text = "A";
            // 
            // keyLabel
            // 
            this.keyLabel.Font = new System.Drawing.Font("Arial", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.keyLabel.Location = new System.Drawing.Point(75, 19);
            this.keyLabel.Name = "keyLabel";
            this.keyLabel.Size = new System.Drawing.Size(135, 31);
            this.keyLabel.TabIndex = 3;
            this.keyLabel.Text = "Key of C:";
            // 
            // sharp7
            // 
            this.sharp7.Font = new System.Drawing.Font("MusetteMusic", 18.25F);
            this.sharp7.Location = new System.Drawing.Point(80, 113);
            this.sharp7.Name = "sharp7";
            this.sharp7.Size = new System.Drawing.Size(16, 40);
            this.sharp7.TabIndex = 14;
            this.sharp7.Text = "F";
            this.sharp7.Visible = false;
            // 
            // sharp6
            // 
            this.sharp6.Font = new System.Drawing.Font("MusetteMusic", 18.25F);
            this.sharp6.Location = new System.Drawing.Point(72, 103);
            this.sharp6.Name = "sharp6";
            this.sharp6.Size = new System.Drawing.Size(16, 40);
            this.sharp6.TabIndex = 13;
            this.sharp6.Text = "F";
            this.sharp6.Visible = false;
            // 
            // sharp5
            // 
            this.sharp5.Font = new System.Drawing.Font("MusetteMusic", 18.25F);
            this.sharp5.Location = new System.Drawing.Point(63, 119);
            this.sharp5.Name = "sharp5";
            this.sharp5.Size = new System.Drawing.Size(16, 40);
            this.sharp5.TabIndex = 12;
            this.sharp5.Text = "F";
            this.sharp5.Visible = false;
            // 
            // sharp4
            // 
            this.sharp4.Font = new System.Drawing.Font("MusetteMusic", 18.25F);
            this.sharp4.Location = new System.Drawing.Point(56, 108);
            this.sharp4.Name = "sharp4";
            this.sharp4.Size = new System.Drawing.Size(16, 40);
            this.sharp4.TabIndex = 11;
            this.sharp4.Text = "F";
            this.sharp4.Visible = false;
            // 
            // sharp3
            // 
            this.sharp3.Font = new System.Drawing.Font("MusetteMusic", 18.25F);
            this.sharp3.Location = new System.Drawing.Point(48, CalculatePosition("G5"));
            this.sharp3.Name = "sharp3";
            this.sharp3.Size = new System.Drawing.Size(16, 40);
            this.sharp3.TabIndex = 10;
            this.sharp3.Text = "F";
            this.sharp3.Visible = false;
            // 
            // sharp2
            // 
            this.sharp2.Font = new System.Drawing.Font("MusetteMusic", 18.25F);
            this.sharp2.Location = new System.Drawing.Point(40, CalculatePosition("C5"));
            this.sharp2.Name = "sharp2";
            this.sharp2.Size = new System.Drawing.Size(16, 40);
            this.sharp2.TabIndex = 9;
            this.sharp2.Text = "F";
            this.sharp2.Visible = false;
            // 
            // sharp1
            // 
            this.sharp1.Font = new System.Drawing.Font("MusetteMusic", 18.25F);
            this.sharp1.Location = new System.Drawing.Point(30, CalculatePosition("F5"));
            this.sharp1.Name = "sharp1";
            this.sharp1.Size = new System.Drawing.Size(16, 40);
            this.sharp1.TabIndex = 8;
            this.sharp1.Text = "F";
            this.sharp1.Visible = false;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.ControlText;
            this.panel1.Font = new System.Drawing.Font("MusetteMusic", 18.25F);
            this.panel1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.panel1.Location = new System.Drawing.Point(0, 152);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(264, 2);
            this.panel1.TabIndex = 15;

            // 
            // NoteDisplay
            // 
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.sharp7);
            this.Controls.Add(this.sharp6);
            this.Controls.Add(this.sharp5);
            this.Controls.Add(this.sharp4);
            this.Controls.Add(this.sharp3);
            this.Controls.Add(this.sharp2);
            this.Controls.Add(this.sharp1);
            this.Controls.Add(this.keyLabel);
            this.Controls.Add(this.noteNameDisplay);
            this.Controls.Add(this.NoteLabel);
            this.Controls.Add(this.Staff);
            this.Name = "NoteDisplay";
            this.Size = new System.Drawing.Size(344, 293);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }
		#endregion
	}

	public class PlayThread
	{
		private string whichNote;
        private Keys.Key currentKey;
        private int pauseLength;
        private int playLength;

		public PlayThread(string note, Keys.Key currentKey, int pauseLength, int playLength)
		{
			whichNote = note;
            this.pauseLength = pauseLength;
            this.playLength = playLength;
            this.currentKey = currentKey;
		}

		public void PlayNote()
		{
			Thread.Sleep(pauseLength);

            try
            {
                MidiPlayer.Play(new ProgramChange(0, 0, GeneralMidiInstruments.NylonAcousticGuitar));
                MidiPlayer.Play(new NoteOn(0, 0, whichNote, 127));
                Thread.Sleep(playLength);
                MidiPlayer.Play(new NoteOff(0, 0, whichNote, 127));
            }
            catch { }
		}
	}
}
