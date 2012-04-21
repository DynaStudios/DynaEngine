using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

using OpenTK;

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
            XmlNodeList objectNodes = doc.GetElementsByTagName("object");
            foreach (XmlNode objectNode in objectNodes)
            {
                XmlElement objectElement = (XmlElement) objectNode;

                loadObject(objectElement, textureController);
            }
            
        }

        private void loadObject(XmlElement objectElement, TextureController textureController)
        {
            Vector3 offset = new Vector3();
            offset.X = int.Parse(objectElement.GetAttribute("x"));
            offset.Y = int.Parse(objectElement.GetAttribute("y"));
            offset.Z = int.Parse(objectElement.GetAttribute("z"));

            loadBlocks(offset, objectElement, textureController);
            loadBigBlocks(offset, objectElement, textureController);
        }

        private void loadBlocks(Vector3 offset, XmlElement objectElement, TextureController textureController)
        {
            XmlNodeList nodes = objectElement.GetElementsByTagName("block");
            foreach (XmlNode node in nodes)
            {
                XmlElement blockElement = (XmlElement) node;
                XmlElement positionElement = (XmlElement) blockElement.GetElementsByTagName("position")[0];
                XmlElement textureElement = (XmlElement) blockElement.GetElementsByTagName("texture")[0];

                int x = (int) offset.X + int.Parse(positionElement.GetAttribute("x"));
                int y = (int) offset.Y + int.Parse(positionElement.GetAttribute("y"));
                int z = (int) offset.Z + int.Parse(positionElement.GetAttribute("z"));

                int textureId = textureController.getTexture(textureElement.InnerText);

                _blocks.Add(new Block(x, y, z, textureId));
            }
        }

        private void loadBigBlocks(Vector3 offset, XmlElement objectElement, TextureController textureController)
        {
            XmlNodeList nodes = objectElement.GetElementsByTagName("bigBlock");
            foreach (XmlNode node in nodes)
            {
                XmlElement blockElement = (XmlElement) node;
                XmlElement startPositionElement = (XmlElement) blockElement.GetElementsByTagName("start")[0];
                XmlElement sizeElement = (XmlElement) blockElement.GetElementsByTagName("size")[0];
                XmlElement textureElement = (XmlElement) blockElement.GetElementsByTagName("texture")[0];

                int startX = (int) offset.X + int.Parse(startPositionElement.GetAttribute("x"));
                int startY = (int) offset.Y + int.Parse(startPositionElement.GetAttribute("y"));
                int startZ = (int) offset.Z + int.Parse(startPositionElement.GetAttribute("z"));

                int sizeX = int.Parse(sizeElement.GetAttribute("x"));
                int sizeY = int.Parse(sizeElement.GetAttribute("y"));
                int sizeZ = int.Parse(sizeElement.GetAttribute("z"));

                int textureId = textureController.getTexture(textureElement.InnerText);

                for (int x = startX; x < startX + sizeX; ++x)
                {
                    for (int y = startY; y < startY + sizeY; ++y)
                    {
                        for (int z = startZ; z < startZ + sizeZ; ++z)
                        {
                            _blocks.Add(new Block(x, y, z, textureId));
                        }
                    }
                }
            }
        }

        public void render()
        {
            int size = _blocks.Count;
            for (int i = 0; i < size; ++i)
            {
                _blocks[i].doRender();
            }
        }
    }
}
