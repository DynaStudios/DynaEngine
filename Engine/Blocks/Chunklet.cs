
namespace DynaStudios.Blocks
{
    public class Chunklet
    {

        private Block[, ,] _blocks;

        public int startX;
        public int startZ;
        public int yLevel;

        public Chunklet(int x, int y, int z)
        {
            startX = x;
            yLevel = y;
            startZ = z;
            _blocks = new Block[16, 16, 16];
        }


        internal void render(CameraMan camerMan)
        {
            
            foreach( Block block in _blocks) {

                block.render();

            }


        }
    }
}
