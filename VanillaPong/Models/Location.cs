using System;

namespace VanillaPong.Models
{
    public class Location
    {
        public long TimeStamp { get; set; }
        public int Position { get; set; }
        public int PositionX { get; set; }

        public Location ShallowCopy()
        {
            return (Location)this.MemberwiseClone();
        }
    }
}