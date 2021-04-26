namespace VanillaPong.GameCode
{
    public class PlayerLocation
    {
        public long TimeStamp { get; set; }
        public int Position { get; set; }
        public bool Sent { get; set; } = false;
    }
}