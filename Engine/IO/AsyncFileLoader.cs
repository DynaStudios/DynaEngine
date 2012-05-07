using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DynaStudios.IO {
    class IntHolder
    {
        public int value = 0;
    }

    public class AsyncFileLoader {
        public delegate void FileLoadedDelegate(ILoadableFile file);
        public event FileLoadedDelegate FilesLoaded;

        private LinkedList<ILoadableFile> _queriedFiles = new LinkedList<ILoadableFile>();
        private string _dataPath;
        private bool _keepAlive = true;
        private IntHolder _regions = new IntHolder();

        public AsyncFileLoader(string dataPath) {
            _dataPath = dataPath;
            new Thread(loadFiles).Start();
        }

        public bool fileIsQueried(ILoadableFile search) {
            lock (_queriedFiles) {
                LinkedListNode<ILoadableFile> file = _queriedFiles.First;
                while (file != null) {
                    if (file.Value == search) {
                        return true;
                    }
                    file = file.Next;
                }
            }
            return false;
        }

        public void request(ILoadableFile search, bool urgent = false) {
            if (!fileIsQueried(search)) {
                registerFileForLoad(search, urgent);
            }
        }

        private void registerFileForLoad(ILoadableFile file, bool urgent) {
            lock (_queriedFiles) {
                if (urgent) {
                    _queriedFiles.AddFirst(file);
                } else {
                    _queriedFiles.AddLast(file);
                }
                Monitor.Pulse(_queriedFiles);
            }
        }

        private LinkedListNode<ILoadableFile> getNextFileNode() {
            LinkedListNode<ILoadableFile> fileNode;
            do {
                lock (_queriedFiles) {
                    fileNode = _queriedFiles.First;
                    if (fileNode != null) {
                        break;
                    }
                    Monitor.Wait(_queriedFiles);
                }
            } while (_keepAlive);
            return fileNode;
        }

        private void loadFiles() {
            while (_keepAlive) {
                LinkedListNode<ILoadableFile> fileNode = getNextFileNode();
                // no need to load if its time to quit
                if (!_keepAlive) {
                    return;
                }
                ILoadableFile file = fileNode.Value;
                file.load();
                lock (_queriedFiles) {
                    _queriedFiles.Remove(fileNode);
                }
                FilesLoaded(file);
            }
        }

        public void stop() {
            _keepAlive = false;
            lock (_queriedFiles) {
                Monitor.Pulse(_queriedFiles);
            }
        }

        public void addRegion()
        {
            lock (_regions)
            {
                ++_regions.value;
            }
        }

        public void removeRegion()
        {
            lock (_regions)
            {
                --_regions.value;
                if (_regions.value == 0)
                {
                    stop();
                }
            }
        }
    }
}
