using System;
using Toub.Sound.Midi;

namespace LeeHolmes.Music.Keys
{
    /// <summary>
    /// The key of D.
    /// </summary>
    public class D : Key
    {
        public override String FriendlyName 
        { 
            get { return "D"; } 
        }

        public override string[] NoteScale
        {
            get
            {
                return new string[]
                { 
                    "E3", "F#3", "G3", "A3", "B3", "C#4", "D4", "E4", "F#4", "G4", 
                    "A4", "B4", "C#5", "D5", "E5", "F#5", "G5" 
                };
            }
        }

        public override Toub.Sound.Midi.KeySignature KeySignature
        { 
            get { return new KeySignature(0, Toub.Sound.Midi.Key.Sharp2, Tonality.Major); }
        }

        public override int Sharps 
        { 
            get { return 2; }
        }

        public override int Flats
        { 
            get { return 0; }
        }
    }
}