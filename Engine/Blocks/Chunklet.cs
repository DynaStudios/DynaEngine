
namespace DynaStudios.Blocks
{
    public class Chunklet
    {

        int[, ,] blocks;

        public int startX;
        public int startZ;
        public int yLevel;

        public Chunklet()
        {
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
