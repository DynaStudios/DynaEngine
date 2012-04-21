﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

using DynaStudios.IO;
using DynaStudios.Utils;

namespace DynaStudios.Blocks
{
    class XmlTextureParser
    {
        private Dictionary<string, int> _texturesMap = new Dictionary<string, int>();
        private TextureController _textureController;

        public XmlTextureParser(TextureController textureController)
        {
            _textureController = textureController;
        }

        public int getTextureIdFromXmlElement(XmlElement texElement)
        {
            string texturePath = texElement.InnerText;
            if (_texturesMap.ContainsKey(texturePath))
            {
                return _texturesMap[texturePath];
            }

            int textureId = _textureController.getTexture(texturePath);
            _texturesMap[texturePath] = textureId;
            return textureId;
        }
    }
    public class Room
    {
        private List<Block> _blocks = new List<Block>();
        public List<Block> Blocks
        {
            get { return _blocks; }
        }

        public Room(XmlDocument doc, TextureController textureController)
        {
            xmlInit(doc, textureController);
        }

        public Room(string filePath, TextureController textureController)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            xmlInit(doc, textureController);
        }

        private void xmlInit (XmlDocument doc, TextureController textureController) {
            XmlTextureParser xmlTextureParser = new XmlTextureParser(textureController);
            XmlNodeList nodes = doc.GetElementsByTagName("block");
            foreach (XmlNode node in nodes)
            {
                XmlElement blockElement = (XmlElement) node;
                XmlElement positionElement = (XmlElement) blockElement.GetElementsByTagName("position")[0];
                XmlElement textureElement = (XmlElement) blockElement.GetElementsByTagName("texture")[0];

                int x = int.Parse(positionElement.GetAttribute("x"));
                int y = int.Parse(positionElement.GetAttribute("y"));
                int z = int.Parse(positionElement.GetAttribute("z"));

                int textureId  = xmlTextureParser.getTextureIdFromXmlElement(textureElement);

                _blocks.Add(new Block(x, y, z, textureId));
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
