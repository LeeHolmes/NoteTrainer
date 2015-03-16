using System;

namespace LeeHolmes.Music.Keys
{
	/// <summary>
	/// Class for a musical Key
	/// </summary>
    public abstract class Key
    {
        public abstract string[] NoteScale { get; }
        
        public abstract String FriendlyName { get; }
        public abstract Toub.Sound.Midi.KeySignature KeySignature  { get; }
        public abstract int Sharps  { get; }
        public abstract int Flats  { get; }
        public override string ToString()
        {
            return FriendlyName;
        }
	}
}
