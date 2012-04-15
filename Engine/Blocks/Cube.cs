using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using OpenTK.Graphics.OpenGL;

namespace DynaStudios.Blocks
{
    public class Cube : IDrawable
    {
        public Direction Direction { get; set; }
        public WorldPosition Position { get; set; }

        /// <summary>
        /// Creates a cube at given world position
        /// </summary>
        /// <param name="worldX">World X Coordinate</param>
        /// <param name="worldZ">World Z Coordinate</param>
        /// <param name="worldY">World Y Coordinate</param>
        public Cube(double worldX, double worldZ, double worldY)
        {
            Position = new WorldPosition(worldX, worldY, worldZ);
        }

        public void Render()
        {

            const float sizex = 0.5f;
            const float sizey = 0.5f;
            const float sizez = 0.5f;

            var x = Position.x;
            var y = Position.y;
            var z = Position.z;

            GL.Translate(-x, -y, -z);

            GL.Begin(BeginMode.Quads);

            GL.Color3(Color.Honeydew);

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

            GL.Color3(Color.Honeydew);

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

            GL.Color3(Color.Honeydew);

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

            GL.Translate(x, y, z);
        }
    }
}
