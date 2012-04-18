using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.IO.Pipes;
using System.Threading;

using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

namespace DynaStudios.Blocks {

    public class Region {
        string dataPath;
        private int x;
        private int y;
        private bool decompressed = false;
        public bool Decompressed
        {
            get { return decompressed; }
        }
        private bool compressed = false;
        public bool Compressed
        {
            get { return compressed; }
        }

        public Region(string dataPath, int x, int y) {
            this.dataPath = dataPath;
            this.x = x;
            this.y = y;
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
            fileName.Append(getStringForFilesystem(x));
            fileName.Append("_");
            fileName.Append(getStringForFilesystem(y));
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
                    string outFileName = Path.Combine(dataPath, entry.Name);
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
    }
}
