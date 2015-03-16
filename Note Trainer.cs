using System;
using System.Drawing;
using System.Threading;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace LeeHolmes.Music
{
	/// <summary>
	/// Summary description for NoteTrainer.
	/// </summary>
	public class NoteTrainer : System.Windows.Forms.Form
	{
		private System.Windows.Forms.Timer countdown;
		private System.ComponentModel.IContainer components;
        private LeeHolmes.Music.NoteDisplay noteDisplay1;
		bool[] noteSelected;
        int lastNote;
        bool rising;
        private System.Windows.Forms.Label tempoLabel;
        private System.Windows.Forms.CheckBox randomCheckBox;
        Random randomNum;
        private System.Windows.Forms.ComboBox keyCombo;
        private System.Windows.Forms.Label keyLabel;

        private System.Windows.Forms.TrackBar noteSpeed; 

		public NoteTrainer()
		{
			InitializeComponent();

			noteDisplay1.NoteNotify += new NoteDisplay.NoteClick(NoteClicked);
            noteDisplay1.CurrentKey = new Keys.C();
            lastNote = -1; rising = true;
            randomNum = new Random();

            keyCombo.Items.Clear();
            keyCombo.Items.AddRange(new object[]
                {
                    new Keys.C(), 
                    new Keys.G(), 
                    new Keys.D(),
                    new Keys.A(),
					new Keys.PlainKey(
                        "Second Position",
                        new string[]
                        { 
                            "F#3", "G3", "G#3", "A3",
					        "B3", "C4", "C#4", "D4",
					        "E4", "F4", "F#4", "G4", 
                            "A4", "A#4", "B4", "C5",
					        "C#5", "D5", "D#5", "E5",
					        "F#5", "G5", "G#5", "A5"
                        }
                    ),

                    new Keys.PlainKey(
                        "Third Position",
                        new string[]
                        { 
                            "G3", "G#3", "A3", "A#3",
					        "C4", "C#4", "D4", "D#4",
					        "F4", "F#4", "G4", "G#4",
                            "A#4", "B4", "C5", "C#5",
					        "D5", "D#5", "E5", "F5",
					        "G5", "G#5", "A5", "A#5"
                        }
                    ),

                    new Keys.PlainKey(
                        "Fourth Position",
                        new string[]
                        { 
                            "G#3", "A3", "A#3", "B3",
					        "C#4", "D4", "D#4", "E4",
					        "F#4", "G4", "G#4", "A4",
                            "B4", "C5", "C#5", "D5",
					        "D#5", "E5", "F5", "F#5",
					        "G#5", "A5", "A#5", "B5"
                        }
                    ),

                    new Keys.PlainKey(
                        "Fifth Position",
                        new string[]
                        { 
                            "A3", "A#3", "B3", "C4",
					        "D4", "D#4", "E4", "F4",
					        "G4", "G#4", "A4", "A#4",
                            "C5", "C#5", "D5", "D#5",
					        "E5", "F5", "F#5", "G5",
					        "A5", "A#5", "B5", "C6"
                        }
                    ),

                    new Keys.PlainKey(
                        "Sixth Position",
                        new string[]
                        { 
                            "A#3", "B3", "C4", "C#4",
					        "D#4", "E4", "F4", "F#4",
					        "G#4", "A4", "A#4", "B4",
                            "C#5", "D5", "D#5", "E5",
					        "F5", "F#5", "G5", "G#5",
					        "A#5", "B5", "C6", "C#6"
                        }
                    ),

                    new Keys.PlainKey(
                        "Seventh Position",
                        new string[]
                        { 
                            "B3", "C4", "C#4", "D4",
					        "E4", "F4", "F#4", "G4",
					        "A4", "A#4", "B4", "C5",
                            "D5", "D#5", "E5", "F5",
					        "F#5", "G5", "G#5", "A5",
					        "B5", "C6", "C#6", "D6"
                        }
                    ),

                    new Keys.PlainKey(
                        "Eigth Position",
                        new string[]
                        { 
                            "C4", "C#4", "D4", "D#4",
					        "F4", "F#4", "G4", "G#4",
					        "A#4", "B4", "C5", "C#5",
                            "D#5", "E5", "F5", "F#5",
					        "G5", "G#5", "A5", "A#5",
					        "C6", "C#6", "D6", "D#6"
                        }
                    ),

                    new Keys.PlainKey(
                        "Ninth Position",
                        new string[]
                        { 
                            "C#4", "D4", "D#4", "E4",
					        "F#4", "G4", "G#4", "A4",
					        "B4", "C5", "C#5", "D5",
                            "E5", "F5", "F#5", "G5",
					        "G#5", "A5", "A#5", "B5",
					        "C#6", "D6", "D#6", "E6"
                        }
                    ),

                    new Keys.PlainKey(
                        "Tenth Position",
                        new string[]
                        { 
                            "D4", "D#4", "E4", "F4",
					        "G4", "G#4", "A4", "A#4",
					        "C5", "C#5", "D5", "D#5",
                            "F5", "F#5", "G5", "G#5",
					        "A5", "A#5", "B5", "C6",
					        "D6", "D#6", "E6", "F6",
                        }
                    ),

                    new Keys.PlainKey(
                        "Eleventh Position",
                        new string[]
                        { 
                            "D#4", "E4", "F4", "F#4",
					        "G#4", "A4", "A#4", "B4",
					        "C#5", "D5", "D#5", "E5",
                            "F#5", "G5", "G#5", "A5",
					        "A#5", "B5", "C6", "C#6",
					        "D#6", "E6", "F6", "F#6"
                        }
                    )
                });
			
			InitNotes();
		}

		private void InitNotes()
		{
			noteSelected = new bool[noteDisplay1.NoteScale.Length];

			for(int noteIndex = 0; noteIndex < noteDisplay1.NoteScale.Length; noteIndex++)
				noteSelected[noteIndex] = true;
		}

		private void NoteClicked(string whichNote)
		{
			int clickedNoteLocation = Array.IndexOf(noteDisplay1.NoteScale, whichNote);
			noteSelected[clickedNoteLocation] = !noteSelected[clickedNoteLocation];

			if(noteSelected[clickedNoteLocation])
				MessageBox.Show("Adding note " + whichNote + " to testing");
			else
				MessageBox.Show("Removing note " + whichNote + " from testing");
//
//			// How many notes do we have
//			int noteCounter = 0;
//            for(int noteIndex = 0; noteIndex < noteSelected.Length; noteIndex++)
//				if(noteSelected[noteIndex]) noteCounter++;

//			playNotes = new string[noteCounter];
//			noteCounter = 0;
//			for(int noteIndex = 0; noteIndex < noteSelected.Length; noteIndex++)
//			{
//				if(noteSelected[noteIndex])
//				{
//					playNotes[noteCounter] = noteDisplay1.NoteScale[noteIndex];
//					noteCounter++;
//				}
//			}
//
//			string arrayString = "";
//			foreach(string currentNote in playNotes)
//				arrayString += currentNote + " ";
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            LeeHolmes.Music.Keys.A a1 = new LeeHolmes.Music.Keys.A();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NoteTrainer));
            this.countdown = new System.Windows.Forms.Timer(this.components);
            this.noteDisplay1 = new LeeHolmes.Music.NoteDisplay();
            this.noteSpeed = new System.Windows.Forms.TrackBar();
            this.tempoLabel = new System.Windows.Forms.Label();
            this.randomCheckBox = new System.Windows.Forms.CheckBox();
            this.keyCombo = new System.Windows.Forms.ComboBox();
            this.keyLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.noteSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // countdown
            // 
            this.countdown.Enabled = true;
            this.countdown.Interval = 3000;
            this.countdown.Tick += new System.EventHandler(this.countdown_Tick);
            // 
            // noteDisplay1
            // 
            this.noteDisplay1.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.noteDisplay1.CurrentKey = a1;
            this.noteDisplay1.Location = new System.Drawing.Point(0, 1);
            this.noteDisplay1.Name = "noteDisplay1";
            this.noteDisplay1.Note = "A4";
            this.noteDisplay1.NoteNotify = null;
            this.noteDisplay1.NoteScale = new string[] {
        "E3",
        "F3",
        "G3",
        "A4",
        "B4",
        "C4",
        "D4",
        "E4",
        "F4",
        "G4",
        "A5",
        "B5",
        "C5",
        "D5",
        "E5",
        "F5",
        "G5"};
            this.noteDisplay1.PauseLength = 700;
            this.noteDisplay1.PlayLength = 300;
            this.noteDisplay1.Size = new System.Drawing.Size(266, 280);
            this.noteDisplay1.TabIndex = 0;
            // 
            // noteSpeed
            // 
            this.noteSpeed.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.noteSpeed.Location = new System.Drawing.Point(0, 208);
            this.noteSpeed.Name = "noteSpeed";
            this.noteSpeed.Size = new System.Drawing.Size(104, 45);
            this.noteSpeed.TabIndex = 1;
            // 
            // tempoLabel
            // 
            this.tempoLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.tempoLabel.Location = new System.Drawing.Point(32, 243);
            this.tempoLabel.Name = "tempoLabel";
            this.tempoLabel.Size = new System.Drawing.Size(40, 23);
            this.tempoLabel.TabIndex = 2;
            this.tempoLabel.Text = "Tempo";
            // 
            // randomCheckBox
            // 
            this.randomCheckBox.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.randomCheckBox.Location = new System.Drawing.Point(200, 211);
            this.randomCheckBox.Name = "randomCheckBox";
            this.randomCheckBox.Size = new System.Drawing.Size(72, 24);
            this.randomCheckBox.TabIndex = 4;
            this.randomCheckBox.Text = "Random";
            this.randomCheckBox.UseVisualStyleBackColor = false;
            // 
            // keyCombo
            // 
            this.keyCombo.Location = new System.Drawing.Point(110, 211);
            this.keyCombo.Name = "keyCombo";
            this.keyCombo.Size = new System.Drawing.Size(80, 21);
            this.keyCombo.TabIndex = 5;
            this.keyCombo.Text = "C";
            this.keyCombo.SelectedIndexChanged += new System.EventHandler(this.keyCombo_SelectedIndexChanged);
            // 
            // keyLabel
            // 
            this.keyLabel.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.keyLabel.Location = new System.Drawing.Point(116, 243);
            this.keyLabel.Name = "keyLabel";
            this.keyLabel.Size = new System.Drawing.Size(150, 23);
            this.keyLabel.TabIndex = 6;
            this.keyLabel.Text = "Training Set";
            // 
            // NoteTrainer
            // 
            this.ClientSize = new System.Drawing.Size(264, 278);
            this.Controls.Add(this.keyLabel);
            this.Controls.Add(this.keyCombo);
            this.Controls.Add(this.randomCheckBox);
            this.Controls.Add(this.tempoLabel);
            this.Controls.Add(this.noteSpeed);
            this.Controls.Add(this.noteDisplay1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "NoteTrainer";
            this.Text = "Note Trainer";
            ((System.ComponentModel.ISupportInitialize)(this.noteSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
		#endregion

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main() 
		{
			Application.Run(new NoteTrainer());
		}

		private void countdown_Tick(object sender, System.EventArgs e)
		{
            countdown.Interval = 5000 - (480 * noteSpeed.Value);
            noteDisplay1.PauseLength = 1500 - (144 * noteSpeed.Value);
            noteDisplay1.PlayLength = countdown.Interval - noteDisplay1.PauseLength;

            int nextNote = 0;

            do
            {
                if(randomCheckBox.Checked)
                    nextNote = randomNum.Next(noteDisplay1.NoteScale.Length);
                else
                {
                    if(rising && (lastNote == (noteDisplay1.NoteScale.Length - 1)))
                        rising = false;
                    if((! rising) && (lastNote == 0))
                        rising = true;

                    if(rising)
                        nextNote = lastNote + 1;
                    else
                        nextNote = lastNote - 1;
                    
                    lastNote = nextNote;
                }
            } while(! noteSelected[nextNote]);
              
			noteDisplay1.Note = noteDisplay1.NoteScale[nextNote];
		}

        private void keyCombo_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            noteDisplay1.CurrentKey = (Keys.Key) keyCombo.SelectedItem;
			InitNotes();
        }
	}
}
