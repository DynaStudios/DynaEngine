using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace DynaStudios.Blocks {

    public class Region {
        private string _dataPath;
        private int _x;
        public int X {
            get { return _x; }
        }
        private int _z;
        public int Z {
            get { return _z; }
        }
        private bool decompressed = false;
        public bool Decompressed {
            get { return decompressed; }
        }
        private bool compressed = false;
        public bool Compressed {
            get { return compressed; }
        }
        private int preloadChunks = 1;
        public int PreloadChunks {
            get { return preloadChunks; }
        }

        private AsyncChunkLoader _chunkLoader = null;
        public AsyncChunkLoader ChunkLoader
        {
            get { return _chunkLoader; }
        }

        private Chunk[,] _chunks = new Chunk[16, 16];

        public Region(string dataPath, int x, int z, AsyncChunkLoader loader)
        {
            init(dataPath, x, z, loader);
        }

        public Region(string dataPath, int x, int z)
        {
            init(dataPath, x, z, new AsyncChunkLoader(_dataPath));
        }

        ~Region()
        {
            _chunkLoader.removeRegion();
        }

        private void init(string dataPath, int x, int z, AsyncChunkLoader loader)
        {
            _dataPath = dataPath;
            _x = x;
            _z = z;
            loader.addRegion();
            _chunkLoader = loader;
            _chunkLoader.ChunkLoaded += chunkLoader_ChunkLoaded;
        }

        private void chunkLoader_ChunkLoaded(Chunk chunk) {
            lock (_chunks) {
                if (chunkIsChild(chunk.X, chunk.Z))
                {
                    _chunks[chunk.X % 16, chunk.Z % 16] = chunk;
                }
            }
        }

        private void asyncDecompressRegion(ThreadPriority priority = ThreadPriority.BelowNormal) {
            Thread thread = new Thread(decompressRegion);
            thread.Priority = priority;
            thread.Start(getFileName());
        }

        private void asynCompressRegion(ThreadPriority priority = ThreadPriority.BelowNormal) {
            Thread thread = new Thread(compressRegion);
            thread.Priority = priority;
            thread.Start(getFileName());
        }

        private string getStringForFilesystem(int nummber) {
            if (nummber < 0) {
                return "m" + (-nummber);
            }
            return "" + nummber;
        }

        private string getFileName() {
            StringBuilder fileName = new StringBuilder();
            fileName.Append(getStringForFilesystem(_x));
            fileName.Append("_");
            fileName.Append(getStringForFilesystem(_z));
            fileName.Append(".derf");
            return fileName.ToString();
        }

        private void decompressRegion(object o) {
            decompressRegion(o.ToString());
        }

        private void compressRegion(object o) {
            compressRegion(o.ToString());
        }

        private void decompressRegion(string fileName) {
            FileInfo info = new FileInfo(fileName);
            if (!info.Exists) {
                throw new FileNotFoundException("mimimimi");
            }
            using (FileStream inData = info.OpenRead()) {
                ZipInputStream zipInputStream = new ZipInputStream(inData);
                ZipEntry entry = zipInputStream.GetNextEntry();
                byte[] buffer = new byte[4096];
                while (entry != null) {
                    string outFileName = Path.Combine(_dataPath, entry.Name);
                    using (FileStream outFile = new FileInfo(outFileName).OpenWrite()) {
                        StreamUtils.Copy(zipInputStream, outFile, buffer);
                    }
                }
            }
            decompressed = true;
        }

        private void compressRegion(string fileName) {
            // TODO: recompress region
        }

        public bool chunkIsChild(int x, int z) {
            return x >= X * 16 && x < X * 17
                && z >= Z * 16 && z < Z * 17;
        }

        private void checkForChunk(int x, int z, bool urgent = false) {
            if (!chunkIsChild(x, z)) {
                return;
            }

            lock (_chunks) {
                if (_chunks[x % 16, z % 16] == null) {
                    _chunkLoader.request(x, z, urgent);
                }
            }
        }

        private void checkPreloadedChunks(int x, int z) {
            // HACK: preloads a square but it should be a circle
            for (int iz = z - PreloadChunks; iz < z + PreloadChunks; ++iz) {
                for (int ix = x - PreloadChunks; ix < x + PreloadChunks; ++ix) {
                    checkForChunk(ix, iz, ix == x && iz == z);
                }
            }
        }

        public void setCurrentChunk(int x, int z) {
            checkPreloadedChunks(x, z);
        }

        public Chunk this[int x,int z] {
            get {
                if (!Decompressed) {
                    // dangerous state that should be avoided
                    return null;
                }

                if (!chunkIsChild(x, z)) {
                    return null;
                }
                lock (_chunks) {
                    return _chunks[x % 16, z % 16];
                }
            }
        }
    }
}
