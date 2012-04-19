using System;
using System.Drawing;

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

        /// <summary>
        /// for testing purpose only
        /// include it in the constructor to generate a randomly filled chunk
        /// </summary>
        private void generateStupedWorld()
        {
            Random rnd = new Random();
            Color[] colors = new Color[] {Color.Black, Color.White, Color.Red, Color.Green, Color.Blue, Color.Yellow};
            for (int x = 0; x < 16; ++x)
            {
                for (int y = 0; y < 16; ++y)
                {
                    for (int z = 0; z < 16; ++z)
                    {
                        if (rnd.Next(3) == 0)
                        {
                            Block block = new Block(x, y, z);
                            block.color = colors[rnd.Next(colors.Length)];
                            _blocks[x, y, z] = block;
                        }
                    }
                }
            }
        }

        internal void render(CameraMan camerMan)
        {
            
            foreach( Block block in _blocks) {

                block.render();

            }


        }
    }
}
