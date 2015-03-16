using System;
using Toub.Sound.Midi;

namespace LeeHolmes.Music.Keys
{
    /// <summary>
    /// Support for keys with no marked sharps or flats.
    /// </summary>
    public class PlainKey : Key
    {
        private string friendlyName;
        private string[] noteScale;

        public PlainKey(string friendlyName, string[] noteScale)
        {
            this.friendlyName = friendlyName;
            this.noteScale = noteScale;
        }

        public override String FriendlyName 
        { 
            get { return friendlyName; } 
        }

        public override string[] NoteScale
        {
            get
            {
                return noteScale;
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