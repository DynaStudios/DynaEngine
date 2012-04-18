using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace DynaStudios.Blocks {
    class AsyncChunkLoader {
        public delegate void ChunkLoadedDelegate(Chunk chunk);
        public event ChunkLoadedDelegate ChunkLoaded;

        private LinkedList<Chunk> _queriedChunks = new LinkedList<Chunk>();
        private string _dataPath;
        private bool _keepAlive = true;

        public AsyncChunkLoader(string dataPath) {
            _dataPath = dataPath;
            new Thread(loadChunks).Start();
        }

        public bool chunkIsQueried(int x, int y) {
            lock (_queriedChunks) {
                LinkedListNode<Chunk> chunk = _queriedChunks.First;
                while (chunk != null) {
                    if (chunk.Value.X == x && chunk.Value.Y == y) {
                        return true;
                    }
                    chunk = chunk.Next;
                }
            }
            return false;
        }

        public void request(int x, int y, bool urgent = false) {
            if (!chunkIsQueried(x, y)) {
                registerChunkForLoad(x, y, urgent);
            }
        }

        private void registerChunkForLoad(int x, int y, bool urgent) {
            Chunk chunk = new Chunk(x, y);
            lock(_queriedChunks) {
                if (urgent) {
                    _queriedChunks.AddFirst(chunk);
                } else {
                    _queriedChunks.AddLast(chunk);
                }
                Monitor.Pulse(_queriedChunks);
            }
        }

        private string getStringForFilesystem(int nummber) {
            if (nummber < 0) {
                return "m" + (-nummber).ToString();
            }
            return nummber.ToString();
        }

        private LinkedListNode<Chunk> getNextChunkNode() {
            LinkedListNode<Chunk> chunkNode;
            do {
                lock (_queriedChunks) {
                    chunkNode = _queriedChunks.First;
                    if (chunkNode != null) {
                        break;
                    }
                    Monitor.Wait(_queriedChunks);
                }
            } while (_keepAlive);
            return chunkNode;
        }

        private void loadChunks() {
            while (_keepAlive) {
                LinkedListNode<Chunk> chunkNode = getNextChunkNode();
                if (!_keepAlive) {
                    return;
                }
                Chunk chunk = chunkNode.Value;

                string chunkFileName = string.Format(
                    "{0}_{1}.dscf",
                    getStringForFilesystem(chunk.X),
                    getStringForFilesystem(chunk.Y));
                string chunkFullPath = Path.Combine(_dataPath, chunkFileName);

                chunk.load(chunkFullPath);
                lock (_queriedChunks) {
                    _queriedChunks.Remove(chunkNode);
                }
                ChunkLoaded(chunk);
            }
        }

        public void stop() {
            _keepAlive = false;
            lock (_queriedChunks) {
                Monitor.Pulse(_queriedChunks);
            }
        }
    }

    public class Region {
        private string _dataPath;
        private int _x;
        public int X {
            get { return _x; }
        }
        private int _y;
        public int Y {
            get { return _y; }
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

        private AsyncChunkLoader _chunkLoader;
        private Chunk[,] _chunks = new Chunk[16, 16];

        public Region(string dataPath, int x, int y) {
            _dataPath = dataPath;
            _x = x;
            _y = y;
            _chunkLoader = new AsyncChunkLoader(_dataPath);
            _chunkLoader.ChunkLoaded += chunkLoader_ChunkLoaded;
        }

        private void chunkLoader_ChunkLoaded(Chunk chunk) {
            lock (_chunks) {
                _chunks[chunk.X % 16, chunk.Y % 16] = chunk;
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
            fileName.Append(getStringForFilesystem(_y));
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

        public bool chunkIsChild(int x, int y) {
            return x >= X * 16 && x < X * 17
                && y >= Y * 16 && y < Y * 17;
        }

        private void checkForChunk(int x, int y, bool urgent = false) {
            if (!chunkIsChild(x, y)) {
                return;
            }

            lock (_chunks) {
                if (_chunks[x % 16, y % 16] == null) {
                    _chunkLoader.request(x, y, urgent);
                }
            }
        }

        private void checkPreloadedChunks(int x, int y) {
            // HACK: preloads a square but it should be a circle
            for (int iy = y - PreloadChunks; iy < y + PreloadChunks; ++iy) {
                for (int ix = x - PreloadChunks; ix < x + PreloadChunks; ++ix) {
                    checkForChunk(ix, iy, ix == x && iy == y);
                }
            }
        }

        public void setCurrentChunk(int x, int y) {
            checkPreloadedChunks(x, y);
        }

        public Chunk this[int x,int y] {
            get {
                if (!Decompressed) {
                    // dangerous state that should be avoided
                    return null;
                }
                // TODO: complete this
                return null;
            }
        }
    }
}
