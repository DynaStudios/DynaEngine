namespace DynaStudios.Blocks
{
    public interface IWorldObject
    {
        Direction Direction { get; set; }
        WorldPosition Position { get; set; }
    }
}
