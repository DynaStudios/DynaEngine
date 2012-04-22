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
    class RoomModel
    {
        public List<Block> blocks = new List<Block>();
    }

    class RoomObject
    {
        public RoomModel model;
        public int x;
        public int y;
        public int z;

        public List<Block> createBlocks()
        {
            List<Block> blocks = new List<Block>();
            foreach (Block srcBlock in model.blocks)
            {
                int blockX = (int) srcBlock.Position.x + x;
                int blockY = (int) srcBlock.Position.y + y;
                int blockZ = (int) srcBlock.Position.z + z;
                Block block = new Block(blockX, blockY, blockZ, srcBlock.TextureId);
                blocks.Add(block);
            }
            return blocks;
        }
    }

    class RoomParser
    {
        private XmlDocument _doc;
        private TextureController _textureController;
        private Dictionary<string, RoomModel> _models = new Dictionary<string,RoomModel>();
        private List<RoomObject> _roomObjects = new List<RoomObject>();

        public RoomParser(XmlDocument doc, TextureController textureController)
        {
            _doc = doc;
            _textureController = textureController;

            loadModels();
            loadObjects();
        }

        public List<Block> getBlocks()
        {
            List<Block> blocks = new List<Block>();
            foreach (RoomObject roomObject in _roomObjects)
            {
                blocks.AddRange(roomObject.createBlocks());
            }
            return blocks;
        }

        private void loadObjects()
        {
            XmlNodeList objectNodes = _doc.GetElementsByTagName("object");
            foreach (XmlNode objectNode in objectNodes)
            {
                loadObject((XmlElement) objectNode);
            }
        }

        private void loadObject(XmlElement objectNode)
        {
            RoomObject roomObject = new RoomObject();
            roomObject.model = _models[objectNode.GetAttribute("name")];
            roomObject.x = int.Parse(objectNode.GetAttribute("x"));
            roomObject.y = int.Parse(objectNode.GetAttribute("y"));
            roomObject.z = int.Parse(objectNode.GetAttribute("z"));
            _roomObjects.Add(roomObject);
        }

        private void loadModels()
        {
            XmlNodeList modelNodes = _doc.GetElementsByTagName("model");
            foreach (XmlNode modelNode in modelNodes)
            {
                loadModel((XmlElement) modelNode);
            }
        }

        private void loadModel(XmlElement modelElement)
        {
            RoomModel model = new RoomModel();
            string name = modelElement.GetAttribute("name");

            model.blocks.AddRange(loadBlocks(modelElement));
            model.blocks.AddRange(loadBigBlocks(modelElement));
            _models.Add(name, model);
        }

        private List<Block> loadBlocks(XmlElement objectElement)
        {
            List<Block> blocks = new List<Block>();
            XmlNodeList nodes = objectElement.GetElementsByTagName("block");
            foreach (XmlNode node in nodes)
            {
                XmlElement blockElement = (XmlElement) node;
                XmlElement positionElement = (XmlElement) blockElement.GetElementsByTagName("position")[0];
                XmlElement textureElement = (XmlElement) blockElement.GetElementsByTagName("texture")[0];

                int x = (int) int.Parse(positionElement.GetAttribute("x"));
                int y = (int) int.Parse(positionElement.GetAttribute("y"));
                int z = (int) int.Parse(positionElement.GetAttribute("z"));

                int textureId = _textureController.getTexture(textureElement.InnerText);

                blocks.Add(new Block(x, y, z, textureId));
            }
            return blocks;
        }

        private List<Block> loadBigBlocks(XmlElement objectElement)
        {
            List<Block> blocks = new List<Block>();
            XmlNodeList nodes = objectElement.GetElementsByTagName("bigBlock");
            foreach (XmlNode node in nodes)
            {
                XmlElement blockElement = (XmlElement) node;
                XmlElement startPositionElement = (XmlElement) blockElement.GetElementsByTagName("start")[0];
                XmlElement sizeElement = (XmlElement) blockElement.GetElementsByTagName("size")[0];
                XmlElement textureElement = (XmlElement) blockElement.GetElementsByTagName("texture")[0];

                int startX = int.Parse(startPositionElement.GetAttribute("x"));
                int startY = int.Parse(startPositionElement.GetAttribute("y"));
                int startZ = int.Parse(startPositionElement.GetAttribute("z"));

                int sizeX = int.Parse(sizeElement.GetAttribute("x"));
                int sizeY = int.Parse(sizeElement.GetAttribute("y"));
                int sizeZ = int.Parse(sizeElement.GetAttribute("z"));

                int textureId = _textureController.getTexture(textureElement.InnerText);

                for (int x = startX; x < startX + sizeX; ++x)
                {
                    for (int y = startY; y < startY + sizeY; ++y)
                    {
                        for (int z = startZ; z < startZ + sizeZ; ++z)
                        {
                            blocks.Add(new Block(x, y, z, textureId));
                        }
                    }
                }
            }
            return blocks;
        }
    }

    public class Room
    {
        private Block[, ,] _blocks;
        public Block[, ,] Blocks
        {
            get { return _blocks; }
        }

        public Room(int sizeX, int sizeY, int sizeZ)
        {
            _blocks = new Block[sizeX, sizeY, sizeZ];
        }

        public void loadXml(string filePath, TextureController textureController)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(filePath);
            loadXml(doc, textureController);
        }

        public void loadXml (XmlDocument doc, TextureController textureController) {
            RoomParser parser = new RoomParser(doc, textureController);
            List<Block> blocks = parser.getBlocks();
            foreach (Block block in blocks)
            {
                int x = (int) block.Position.x;
                int y = (int) block.Position.y;
                int z = (int) block.Position.z;
                _blocks[x, y, z] = block;
            }
        }

		public double collisionX (double posx, double posy, double posz, double dist)
		{
			// a neighbouring block (only then only 1, if floory==0.5&&floorz==0.5, else 1 or none of 2 or 4) can limit the collision distance,
			// everything else is not 
			double floory=posy-(int)posy;
			double floorz=posz-(int)posz;
			//negative or positve direction? -> signum, i.e. 1 or -1
			int dirsig;
			//the distance to the neighbouring block
			double xrest;
			if (dist>0.0) {
				dirsig=1;
				xrest=(int)posx+1-posx;
			} else if (dist<0.0) {
				dirsig=-1;
				xrest=posx-(int)posx;
			}
			else /* if dist==0.0 */ return 0.0;
			if (floory<0.5)
			{
				if (floorz<0.5)
				{
					if ( _blocks[(int)posx+dirsig,(int)posy-1,(int)posz-1]!=null || _blocks[(int)posx+dirsig,(int)posy,(int)posz-1]!=null ||
					     _blocks[(int)posx+dirsig,(int)posy-1,(int)posz] != null || _blocks[(int)posx+dirsig,(int)posy,(int)posz] != null )
					{
						if (xrest>dist) return dist;
						else            return xrest;
					}
				}
				else if (floorz>0.5)
				{
					if ( _blocks[(int)posx+dirsig,(int)posy-1,(int)posz] != null || _blocks[(int)posx+dirsig,(int)posy,(int)posz] != null ||
					     _blocks[(int)posx+dirsig,(int)posy-1,(int)posz+1]!=null || _blocks[(int)posx+dirsig,(int)posy,(int)posz+1]!=null )
					{
						if (xrest>dist) return dist;
						else            return xrest;
					}
				}
				else /*if floorz==0.5*/
				{
					if (_blocks[(int)posx+dirsig,(int)posy-1,(int)posz] != null || _blocks[(int)posx+dirsig,(int)posy,(int)posz] != null )
					{
						if (xrest>dist) return dist;
						else            return xrest;
					}
				}
			}
			else if (floory>0.5)
			{
				if (floorz<0.5)
				{
					if ( _blocks[(int)posx+dirsig,(int)posy,(int)posz-1]!=null || _blocks[(int)posx+dirsig,(int)posy+1,(int)posz-1]!=null ||
					     _blocks[(int)posx+dirsig,(int)posy,(int)posz] != null || _blocks[(int)posx+dirsig,(int)posy+1,(int)posz] != null )
					{
						if (xrest>dist) return dist;
						else            return xrest;
					}
				}
				else if (floorz>0.5)
				{
					if ( _blocks[(int)posx+dirsig,(int)posy,(int)posz] != null || _blocks[(int)posx+dirsig,(int)posy+1,(int)posz] != null ||
					     _blocks[(int)posx+dirsig,(int)posy,(int)posz+1]!=null || _blocks[(int)posx+dirsig,(int)posy+1,(int)posz+1]!=null )
					{
						if (xrest>dist) return dist;
						else            return xrest;
					}
				}
				else /*if floorz==0.5*/
				{
					if (_blocks[(int)posx+dirsig,(int)posy,(int)posz] != null || _blocks[(int)posx+dirsig,(int)posy+1,(int)posz] != null )
				{
						if (xrest>dist) return dist;
						else            return xrest;
					}
				}
			}
			else /*if floory==0.5*/
			{
				if (floorz<0.5)
				{
					if ( _blocks[(int)posx+dirsig,(int)posy,(int)posz-1]!=null || _blocks[(int)posx+dirsig,(int)posy,(int)posz] != null )
					{
						if (xrest>dist) return dist;
						else            return xrest;
					}
				}
				else if (floorz>0.5)
				{
					if ( _blocks[(int)posx+dirsig,(int)posy,(int)posz] != null || _blocks[(int)posx+dirsig,(int)posy,(int)posz+1]!=null )
					{
						if (xrest>dist) return dist;
						else            return xrest;
					}
				}
				else /*if floorz==0.5*/
				{
					if (_blocks[(int)posx+dirsig,(int)posy,(int)posz] != null )
					{
						if (xrest>dist) return dist;
						else            return xrest;
					}
				}
			}
			return 0.0; // why? oO
		}

        public void render()
        {

            foreach (Block block in _blocks)
            {
                if (block != null)
                {
                    block.doRender();
                }
            }
        }
    }
}
