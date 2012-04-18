using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DynaStudios.Blocks {

    public class Chunk {
        private int x;
        public int X {
            get { return x; }
        }

        private int y;
        public int Y {
            get { return y; }
        }

        private Chunklet[] chunklets;

        public Chunk(int x, int y) {
            this.x = x;
            this.y = y;
            chunklets = new Chunklet[16];
        }

        public void load(string pathToFile) {
            // TODO: implement me
        }
    }
}
