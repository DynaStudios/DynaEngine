using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace DynaStudios.Blocks
{
    public class Block : AbstractDrawable
    {
        public Color color = Color.AliceBlue;

        public override Direction Direction { get; set; }
        public override WorldPosition Position { get; set; }

        /// <summary>
        /// Creates a block at given world position
        /// </summary>
        /// <param name="worldX">World X Coordinate</param>
        /// <param name="worldZ">World Z Coordinate</param>
        /// <param name="worldY">World Y Coordinate</param>
        public Block(double worldX, double worldZ, double worldY)
        {
            Position = new WorldPosition(worldX, worldY, worldZ);
        }

        public override void render()
        {

            const float sizex = 1.0f;
            const float sizey = 1.0f;
            const float sizez = 1.0f;

            GL.Begin(BeginMode.Quads);

            GL.Color3(color);

            // FRONT
            GL.Vertex3(-sizex, -sizey, sizez);
            GL.Vertex3(sizex, -sizey, sizez);
            GL.Vertex3(sizex, sizey, sizez);
            GL.Vertex3(-sizex, sizey, sizez);

            // BACK
            GL.Vertex3(-sizex, -sizey, -sizez);
            GL.Vertex3(-sizex, sizey, -sizez);
            GL.Vertex3(sizex, sizey, -sizez);
            GL.Vertex3(sizex, -sizey, -sizez);

            GL.Color3(color);

            // LEFT
            GL.Vertex3(-sizex, -sizey, sizez);
            GL.Vertex3(-sizex, sizey, sizez);
            GL.Vertex3(-sizex, sizey, -sizez);
            GL.Vertex3(-sizex, -sizey, -sizez);

            // RIGHT
            GL.Vertex3(sizex, -sizey, -sizez);
            GL.Vertex3(sizex, sizey, -sizez);
            GL.Vertex3(sizex, sizey, sizez);
            GL.Vertex3(sizex, -sizey, sizez);

            GL.Color3(color);

            // TOP
            GL.Vertex3(-sizex, sizey, sizez);
            GL.Vertex3(sizex, sizey, sizez);
            GL.Vertex3(sizex, sizey, -sizez);
            GL.Vertex3(-sizex, sizey, -sizez);

            // BOTTOM
            GL.Vertex3(-sizex, -sizey, sizez);
            GL.Vertex3(-sizex, -sizey, -sizez);
            GL.Vertex3(sizex, -sizey, -sizez);
            GL.Vertex3(sizex, -sizey, sizez);

            GL.End();
        }
    }
}
