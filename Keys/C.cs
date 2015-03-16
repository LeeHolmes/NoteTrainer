using System;
using Toub.Sound.Midi;

namespace LeeHolmes.Music.Keys
{
	/// <summary>
	/// The key of C.
	/// </summary>
    public class C : Key
    {
        public override String FriendlyName 
        { 
            get { return "C"; } 
        }

        public override string[] NoteScale
        {
            get
            {
                return new string[]
                { 
                    "E3", "F3", "G3", "A3", "B3", "C4", "D4", "E4", "F4", "G4", 
                    "A4", "B4", "C5", "D5", "E5", "F5", "G5" 
                };
            }
        }

        
        public override Toub.Sound.Midi.KeySignature KeySignature
        { 
            get { return new KeySignature(0, Toub.Sound.Midi.Key.NoFlatsOrSharps, Tonality.Major); }
        }

        public override int Sharps 
        { 
            get { return 0; }
        }

        public override int Flats
        { 
            get { return 0; }
        }
    }
}
