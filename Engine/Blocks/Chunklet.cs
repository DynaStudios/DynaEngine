using System;
using System.Drawing;
using System.IO;

namespace DynaStudios.Blocks
{
    public class Chunklet
    {
        private static Color[] colors = new Color[] { Color.Black, Color.White, Color.Red, Color.Green, Color.Blue, Color.Yellow };
        private Block[, ,] _blocks;

        public int _x;
        public int _z;
        public int _y;

        public Chunklet(int x, int y, int z)
        {
            _x = x;
            _y = y;
            _z = z;
            _blocks = new Block[16, 16, 16];
            //generateStupedWorld();
        }

        public void load(Stream stream)
        {
            int data = stream.ReadByte();
            if (data <= 0)
            {
                _blocks = null;
                return;
            }
            int offsetX = _x * 16;
            int offsetY = _y * 16;
            int offsetZ = _z * 16;
            for (int z = 0; z < 16; ++z)
            {
                for (int y = 0; y < 16; ++y)
                {
                    for (int x = 0; x < 16; ++x)
                    {
                        data = stream.ReadByte();
                        if (data < 0)
                        {
                            return;
                        }
                        Block block = new Block(x + offsetX, y + offsetY, offsetZ + z);
                        block.color = colors[data % colors.Length];
                        _blocks[x, y, z] = block;
                    }
                }
            }
        }

        /// <summary>
        /// for testing purpose only
        /// include it in the constructor to generate a randomly filled chunk
        /// </summary>
        private void generateStupedWorld()
        {
            Random rnd = new Random();
            for (int x = 0; x < 16; ++x)
            {
                for (int y = 0; y < 16; ++y)
                {
                    for (int z = 0; z < 16; ++z)
                    {
                        if (rnd.Next(6) == 0)
                        {
                            Block block = new Block(x, y, z);
                            block.color = colors[rnd.Next(colors.Length)];
                            _blocks[x, y, z] = block;
                        }
                    }
                }
            }
        }

        internal void render(Camera camerMan)
        {
            foreach( Block block in _blocks) {
                if (block != null)
                {
                    block.doRender();
                }
            }
        }
    }
}
