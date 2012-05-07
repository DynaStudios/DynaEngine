using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using DynaStudios.IO;
using DynaStudios.Utils;

namespace DynaStudios.Chunks
{

    public class Chunk : ILoadableFile
    {
        private string _fileName;
        private int x;
        public int X
        {
            get { return x; }
        }

        private int z;
        public int Z
        {
            get { return z; }
        }

        private Chunklet[] _chunklets = new Chunklet[16];

        public Chunk(string fileName, int x, int z)
        {
            this.x = x;
            this.z = z;
            _fileName = fileName;
            _chunklets = new Chunklet[16];
        }

        public void load()
        {
            using (FileStream file = new FileInfo(_fileName).OpenRead())
            {
                for (int y = 0; y < 16; ++y)
                {
                    Chunklet chunklet = new Chunklet(x, y, z);
                    chunklet.load(file);
                    _chunklets[y] = chunklet;
                }
            }
        }

        public void render()
        {
            for (int i = 0; i < 16; ++i)
            {
                _chunklets[i].render();
            }
        }

        public void generateStupedWorld()
        {
            for (int y = 0; y < 16; ++y)
            {
                Chunklet chunklet = new Chunklet(x, y, z);
                chunklet.generateStupedWorld();
                _chunklets[y] = chunklet;
            }
        }

        public bool Equals(ILoadableFile leftFile, ILoadableFile rightFile)
        {
            Chunk left = leftFile as Chunk;
            Chunk right = rightFile as Chunk;
            if (left == null || right == null)
            {
                return false;
            }
            return left.X == right.X && left.Z == right.Z;
        }

        public int GetHashCode(ILoadableFile file)
        {
            return _fileName.GetHashCode();
        }
    }
}
