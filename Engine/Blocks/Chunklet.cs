
namespace DynaStudios.Blocks
{
    public class Chunklet
    {

        int[, ,] blocks;

        public int startX;
        public int startZ;
        public int yLevel;

        public Chunklet(int x, int y, int z)
        {
            startX = x;
            yLevel = y;
            startZ = z;
            blocks = new int[16, 16, 16];
        }


        internal void render(CameraMan camerMan)
        {
            bool isHeadingFromWest;
            bool isHeadingFromNorth;

            //if(camerMan.Position.x < )

        }
    }
}
