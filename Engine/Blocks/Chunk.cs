using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DynaStudios.Blocks {

    public class Chunk {
        private int x;
        public int X {
            get { return x; }
        }

        private int z;
        public int Z {
            get { return z; }
        }

        private Chunklet[] chunklets = new Chunklet[16];

        public Chunk(int x, int z) {
            this.x = x;
            this.z = z;
            chunklets = new Chunklet[16];
        }

        public void load(string pathToFile) {
            using (FileStream file = new FileInfo(pathToFile).OpenRead())
            {
                for (int y = 0; y < 16; ++y)
                {
                    Chunklet chunklet = new Chunklet(x, y, z);
                    chunklet.load(file);
                    chunklets[y] = chunklet;
                }
            }
        }
    }
}
