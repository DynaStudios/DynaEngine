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

                _blocks[x,y,z] = new Block(x, y, z, textureId);
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
                            _blocks[x,y,z] = new Block(x, y, z, textureId);
                        }
                    }
                }
            }
        }
		public double collisionX (double posx, double posy, double posz, double dist)
		{
			// a neighbouring block (only then only 1, if floory==0.5&&floorz==0.5, else 1 or none of 2 or 4) can limit the collision distance,
			// everything else is not considered
			double floory=posy-(int)posy;
			double floorz=posz-(int)posz;
			//negative or positve direction? -> signum, i.e. 1 or -1
			int dirsig;
			//the distance to the neighbouring blocks, air or solid
			double rest;
			if (dist>0.0) {
				dirsig=1;
				rest=(int)posx+1-posx;
			} else if (dist<0.0) {
				dirsig=-1;
				rest=posx-(int)posx;
			}
			else /* if dist==0.0 */ return 0.0;
			if (floory<0.5)
			{
				if (floorz<0.5)
				{
					if ( _blocks[(int)posx+dirsig,(int)posy-1,(int)posz-1]!=null || _blocks[(int)posx+dirsig,(int)posy,(int)posz-1]!=null ||
					     _blocks[(int)posx+dirsig,(int)posy-1,(int)posz] != null || _blocks[(int)posx+dirsig,(int)posy,(int)posz] != null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
				else if (floorz>0.5)
				{
					if ( _blocks[(int)posx+dirsig,(int)posy-1,(int)posz] != null || _blocks[(int)posx+dirsig,(int)posy,(int)posz] != null ||
					     _blocks[(int)posx+dirsig,(int)posy-1,(int)posz+1]!=null || _blocks[(int)posx+dirsig,(int)posy,(int)posz+1]!=null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
				else /*if floorz==0.5*/
				{
					if (_blocks[(int)posx+dirsig,(int)posy-1,(int)posz] != null || _blocks[(int)posx+dirsig,(int)posy,(int)posz] != null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
			}
			else if (floory>0.5)
			{
				if (floorz<0.5)
				{
					if ( _blocks[(int)posx+dirsig,(int)posy,(int)posz-1]!=null || _blocks[(int)posx+dirsig,(int)posy+1,(int)posz-1]!=null ||
					     _blocks[(int)posx+dirsig,(int)posy,(int)posz] != null || _blocks[(int)posx+dirsig,(int)posy+1,(int)posz] != null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
				else if (floorz>0.5)
				{
					if ( _blocks[(int)posx+dirsig,(int)posy,(int)posz] != null || _blocks[(int)posx+dirsig,(int)posy+1,(int)posz] != null ||
					     _blocks[(int)posx+dirsig,(int)posy,(int)posz+1]!=null || _blocks[(int)posx+dirsig,(int)posy+1,(int)posz+1]!=null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
				else /*if floorz==0.5*/
				{
					if (_blocks[(int)posx+dirsig,(int)posy,(int)posz] != null || _blocks[(int)posx+dirsig,(int)posy+1,(int)posz] != null )
				{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
			}
			else /*if floory==0.5*/
			{
				if (floorz<0.5)
				{
					if ( _blocks[(int)posx+dirsig,(int)posy,(int)posz-1]!=null || _blocks[(int)posx+dirsig,(int)posy,(int)posz] != null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
				else if (floorz>0.5)
				{
					if ( _blocks[(int)posx+dirsig,(int)posy,(int)posz] != null || _blocks[(int)posx+dirsig,(int)posy,(int)posz+1]!=null )
					{
						if (rest>dist) return dist;
						else            return rest;
					}
					else return dist;
				}
				else /*if floorz==0.5*/
				{
					if (_blocks[(int)posx+dirsig,(int)posy,(int)posz] != null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
			}
		}
		public double collisionY (double posx, double posy, double posz, double dist)
		{
			// a neighbouring block can limit the collision distance,
			// everything else is not considered
			double floorx=posx-(int)posx;
			double floorz=posz-(int)posz;
			//negative or positve direction? -> signum, i.e. 1 or -1
			int dirsig;
			//the distance to the neighbouring blocks, air or solid
			double rest;
			if (dist>0.0) {
				dirsig=1;
				rest=(int)posy+1-posy;
			} else if (dist<0.0) {
				dirsig=-1;
				rest=posy-(int)posy;
			}
			else /* if dist==0.0 */ return 0.0;
			if (floorx<0.5)
			{
				if (floorz<0.5)
				{
					if ( _blocks[(int)posx-1,(int)posy+dirsig,(int)posz-1]!=null  || _blocks[(int)posx,(int)posy+dirsig,(int)posz-1]!=null ||
					     _blocks[(int)posx-1,(int)posy+dirsig,(int)posz]  != null || _blocks[(int)posx,(int)posy+dirsig,(int)posz] != null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
				else if (floorz>0.5)
				{
					if ( _blocks[(int)posx-1,(int)posy+dirsig,(int)posz] != null || _blocks[(int)posx-1,(int)posy+dirsig,(int)posz] != null ||
					     _blocks[(int)posx-1,(int)posy+dirsig,(int)posz+1]!=null || _blocks[(int)posx-1,(int)posy+dirsig,(int)posz+1]!=null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
				else /*if floorz==0.5*/
				{
					if (_blocks[(int)posx-1,(int)posy+dirsig,(int)posz] != null || _blocks[(int)posx-1,(int)posy+dirsig,(int)posz] != null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
			}
			else if (floorx>0.5)
			{
				if (floorz<0.5)
				{
					if ( _blocks[(int)posx+1,(int)posy+dirsig,(int)posz-1]!=null || _blocks[(int)posx+1,(int)posy+dirsig,(int)posz-1]!=null ||
					     _blocks[(int)posx+1,(int)posy+dirsig,(int)posz] != null || _blocks[(int)posx+1,(int)posy+dirsig,(int)posz] != null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
				else if (floorz>0.5)
				{
					if ( _blocks[(int)posx+1,(int)posy+dirsig,(int)posz] != null || _blocks[(int)posx+1,(int)posy+dirsig,(int)posz] != null ||
					     _blocks[(int)posx+1,(int)posy+dirsig,(int)posz+1]!=null || _blocks[(int)posx+1,(int)posy+dirsig,(int)posz+1]!=null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
				else /*if floorz==0.5*/
				{
					if (_blocks[(int)posx+1,(int)posy+dirsig,(int)posz] != null || _blocks[(int)posx+1,(int)posy+dirsig,(int)posz] != null )
				{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
			}
			else /*if floorx==0.5*/
			{
				if (floorz<0.5)
				{
					if ( _blocks[(int)posx,(int)posy+dirsig,(int)posz-1]!=null || _blocks[(int)posx,(int)posy+dirsig,(int)posz] != null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
				else if (floorz>0.5)
				{
					if ( _blocks[(int)posx,(int)posy+dirsig,(int)posz] != null || _blocks[(int)posx,(int)posy+dirsig,(int)posz+1]!=null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
				else /*if floorz==0.5*/
				{
					if (_blocks[(int)posx,(int)posy+dirsig,(int)posz] != null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
			}
		}
		public double collisionZ (double posx, double posy, double posz, double dist)
		{
			// a neighbouring block can limit the collision distance,
			// everything else is not considered
			double floorx=posx-(int)posx;
			double floory=posy-(int)posy;
			//negative or positve direction? -> signum, i.e. 1 or -1
			int dirsig;
			//the distance to the neighbouring blocks, air or solid
			double rest;
			if (dist>0.0) {
				dirsig=1;
				rest=(int)posy+1-posy;
			} else if (dist<0.0) {
				dirsig=-1;
				rest=posy-(int)posy;
			}
			else /* if dist==0.0 */ return 0.0;
			if (floorx<0.5)
			{
				if (floory<0.5)
				{
					if ( _blocks[(int)posx-1,(int)posy-1,(int)posz+dirsig]!=null || _blocks[(int)posx,(int)posy-1,(int)posz+dirsig]!=null ||
					     _blocks[(int)posx-1,(int)posy,(int)posz+dirsig] != null || _blocks[(int)posx,(int)posy,(int)posz+dirsig]  != null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
				else if (floory>0.5)
				{
					if ( _blocks[(int)posx-1,(int)posy+1,(int)posz+dirsig]!= null || _blocks[(int)posx,(int)posy+1,(int)posz+dirsig] != null ||
					     _blocks[(int)posx-1,(int)posy,(int)posz+dirsig]!=null    || _blocks[(int)posx,(int)posy,(int)posz+dirsig]!=null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
				else /*if floorz==0.5*/
				{
					if (_blocks[(int)posx-1,(int)posy,(int)posz+dirsig] != null || _blocks[(int)posx,(int)posy,(int)posz+dirsig] != null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
			}
			else if (floorx>0.5)
			{
				if (floory<0.5)
				{
					if ( _blocks[(int)posx+1,(int)posy-1,(int)posz+dirsig]!=null || _blocks[(int)posx,(int)posy-1,(int)posz+dirsig]!=null ||
					     _blocks[(int)posx+1,(int)posy,(int)posz+dirsig]  !=null || _blocks[(int)posx,(int)posy,(int)posz+dirsig]  !=null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
				else if (floory>0.5)
				{
					if ( _blocks[(int)posx+1,(int)posy+1,(int)posz+dirsig]!=null || _blocks[(int)posx,(int)posy+1,(int)posz+dirsig]!=null ||
					     _blocks[(int)posx+1,(int)posy,(int)posz+dirsig]  !=null || _blocks[(int)posx,(int)posy,(int)posz+dirsig]  !=null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
				else /*if floory==0.5*/
				{
					if (_blocks[(int)posx+1,(int)posy,(int)posz+dirsig] != null || _blocks[(int)posx,(int)posy,(int)posz+dirsig] != null )
				{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
			}
			else /*if floorx==0.5*/
			{
				if (floory<0.5)
				{
					if ( _blocks[(int)posx,(int)posy-1,(int)posz+dirsig]!=null || _blocks[(int)posx,(int)posy,(int)posz+dirsig] != null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
				else if (floory>0.5)
				{
					if ( _blocks[(int)posx,(int)posy+1,(int)posz+dirsig] != null || _blocks[(int)posx,(int)posy,(int)posz+dirsig]!=null )
					{
						if (rest>dist) return dist;
						else            return rest;
					}
					else return dist;
				}
				else /*if floorz==0.5*/
				{
					if (_blocks[(int)posx,(int)posy,(int)posz+dirsig] != null )
					{
						if (rest>dist) return dist;
						else           return rest;
					}
					else return dist;
				}
			}
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
