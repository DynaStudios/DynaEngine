using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using DynaStudios.IO;
using DynaStudios.Utils;

namespace DynaStudios.Blocks
{
    public class Room
    {
        private List<Block> _blocks = new List<Block>();
        public List<Block> Blocks
        {
            get { return _blocks; }
        }

        public Room(string filePath, TextureController textureController)
        {
            using (FileStream fileStream = new FileInfo(filePath).OpenRead())
            {
                init(fileStream, textureController);
            }
        }

        public Room(Stream dataStream, TextureController textureController)
        {
            init(dataStream, textureController);
        }
        private void init (Stream dataStream, TextureController textureController)
        {
            Dictionary<int, int> fileToGpuIdMap = loadTextures(dataStream, textureController);
            readBlocks(dataStream, fileToGpuIdMap);
        }

        public void render()
        {
            int size = _blocks.Count;
            for (int i = 0; i < size; ++i)
            {
                _blocks[i].doRender();
            }
        }

        private Dictionary<int, int> loadTextures(Stream dataStream, TextureController textureController)
        {
            Dictionary<int, int> fileToGpuIdMap = new Dictionary<int, int>();
            int count = StreamTool.getInt(dataStream);
            for (int i = 0; i < count; ++i)
            {
                string path = getPath(dataStream);
                int textureId = textureController.getTexture(path);
                fileToGpuIdMap[i] = textureId;
            }
            return fileToGpuIdMap;
        }

        private string getPath(Stream stream) {
            int size = StreamTool.getInt(stream);
            byte[] rawPathBytes = new byte[size];
            stream.Read(rawPathBytes, 0, size);
            return Encoding.UTF8.GetString(rawPathBytes);
        }

        private void readBlocks(Stream dataStream, Dictionary<int, int> fileToGpuIdMap)
        {
            int count = StreamTool.getInt(dataStream);
            for (int i = 0; i < count; ++i)
            {
                int x = StreamTool.getInt(dataStream);
                int y = StreamTool.getInt(dataStream);
                int z = StreamTool.getInt(dataStream);
                int texturID = StreamTool.getInt(dataStream);
                _blocks.Add(new Block(x, y, z, fileToGpuIdMap[texturID]));
            }
        }
    }
}
