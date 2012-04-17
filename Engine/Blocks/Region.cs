using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;

namespace DynaStudios.Blocks {

    public class Region {
        private bool ready = false;
        public bool Ready {
            get { return ready; }
        }

        public Region(int x, int y) {
            asyncLoadRegion(x, y);
        }

        private void asyncLoadRegion(int x, int y) {
            Thread thread = new Thread(loadRegion);
            thread.Start(getFileName(x, y));
        }

        private string getStringForFilesystem(int nummber) {
            if (nummber < 0) {
                return "m" + (-nummber);
            }
            return "" + nummber;
        }

        private string getFileName(int x, int y) {
            StringBuilder fileName = new StringBuilder();
            fileName.Append(getStringForFilesystem(x));
            fileName.Append("_");
            fileName.Append(getStringForFilesystem(y));
            fileName.Append(".derf");
            return fileName.ToString();
        }

        private void loadRegion(object o) {
            loadRegion(o.ToString());
        }

        private void loadRegion(string fileName) {
            FileInfo info = new FileInfo(fileName);
            if (!info.Exists)
            {
                throw new FileNotFoundException("mimimimi");
            }
            Stream inData = info.OpenRead();
            inData.Close();
            ready = true;
        }
    }
}
