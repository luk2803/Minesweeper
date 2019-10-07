namespace Assets.Scripts_C_
{
    public interface IMineData
    {
        int Position { get; set; }
        bool IsMine { get; set; }
        int MinesInNear { get; set; }
        bool IsNotAllowedToBeBomb { get; set; }
        MineState State { get; set; }
       
    }
}