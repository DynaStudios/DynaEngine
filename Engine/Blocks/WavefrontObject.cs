using System;
using System.Collections.Generic;
using System.IO;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace DynaStudios.Blocks
{
    // HACK: Incomplete should be completed after the Ludum Dare
    #region parsing
    // holds the indexes of the face point
    class FacePointParser
    {
        public Vector3d vertex;
        public int texture = 1;
        public int normal = 1;
    }

    // parses and holds and the indexes of a face
    class WavefrontFaceParser
    {
        public string material = null;
        private int _textureId = -1;
        public List<FacePointParser> points = new List<FacePointParser>();

        public WavefrontFaceParser(string[] face, List<Vector3d> vertexs)
        {
            for (int i = 1; i < face.Length; ++i)
            {
                readPoint(face[i], vertexs);
            }
        }

        public WavefrontPolygon getFace()
        {
            WavefrontPolygon polygon = new WavefrontPolygon();
            int size = points.Count;
            for (int i = 0; i < size; ++i)
            {
                polygon.points.Add(new Vector3d(points[i].vertex));
            }
            return polygon;
        }

        private void readPoint(string strPoint, List<Vector3d> vertexs)
        {
            string[] data = strPoint.Split('/');
            FacePointParser point = new FacePointParser();
            int vertexId;
            if (!int.TryParse(data[0], out vertexId))
            {
                throw new ArgumentOutOfRangeException("vertex 0 given but vertexes starts with 1");
            }
            point.vertex = vertexs[vertexId];
            if (data.Length > 1)
            {
                if (!int.TryParse(data[1], out point.texture))
                {
                    point.texture = 1;
                }
            }
            if (data.Length > 2)
            {
                if (!int.TryParse(data[2], out point.normal))
                {
                    point.normal = 1;
                }
            }
            points.Add(point);
        }
    }

    // parses and holdes the object
    class WavefrontObjectParser
    {
        public string currentMaterial = null;
        List<WavefrontFaceParser> faces = new List<WavefrontFaceParser>();
        List<Vector3d> vertexs = new List<Vector3d>();
        List<Vector2d> textures = new List<Vector2d>();
        List<Vector3d> normals = new List<Vector3d>();

        public WavefrontObjectParser(Stream raw)
        {
            StreamReader reader = new StreamReader(raw);
            string line = reader.ReadLine();
            while (line != null)
            {
                parseLine(line);
                line = reader.ReadLine();
            }
        }

        public List<WavefrontPolygon> getFaces()
        {
            List<WavefrontPolygon> wavefrontPolygons = new List<WavefrontPolygon>();
            int size = faces.Count;
            for (int i = 0; i < size; ++i)
            {
                wavefrontPolygons.Add(faces[i].getFace());
            }
            return wavefrontPolygons;
        }

        void parseLine(string line)
        {
            string[] parts = line.Split(' ');
            switch (parts[0])
            {
                case "v":
                    readVertex(parts);
                break;
                case "f":
                    readFace(parts);
                break;
            }
        }

        void readFace(string[] line)
        {
            WavefrontFaceParser face = new WavefrontFaceParser(line, vertexs);
            face.material = currentMaterial;
            faces.Add(face);
        }

        void readVertex(string[] line)
        {
            Vector3d vertex = new Vector3d();
            vertex.X = double.Parse(line[1]);
            vertex.Y = double.Parse(line[2]);
            vertex.Z = double.Parse(line[3]);
            vertexs.Add(vertex);
        }
    }
    #endregion parsing

    class WavefrontPolygon
    {
        public int texturId = -1;
        public List<Vector2d> textures;
        public List<Vector3d> points = new List<Vector3d>();

        public void render()
        {
            if (texturId >= 0)
            {
                renderTexture();
            }
            else
            {
                renderPlain();
            }
        }

        private void renderTexture()
        {
            int size = points.Count;
            GL.BindTexture(TextureTarget.Texture2D, texturId);
            GL.Begin(BeginMode.Polygon);
            for (int i = 0; i < size; ++i)
            {
                GL.TexCoord2(textures[i]);
                GL.Vertex3(points[i]);
            }
            GL.End();
        }

        private void renderPlain()
        {
            int size = points.Count;
            GL.Begin(BeginMode.Polygon);
            for (int i = 0; i < size; ++i)
            {
                GL.Vertex3(points[i]);
            }
            GL.End();
        }
    }

    public class WavefrontObject : AbstractDrawable
    {
        private List<WavefrontPolygon> _faces = new List<WavefrontPolygon>();

        public WavefrontObject loadFile(string filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                return null;
            }
            using (FileStream file = fileInfo.OpenRead())
            {
                return new WavefrontObject(file);
            }
        }

        public WavefrontObject(Stream raw)
        {
            WavefrontObjectParser parser = new WavefrontObjectParser(raw);
            _faces = parser.getFaces();
        }

        public override void render()
        {
            int size = _faces.Count;
            for (int i = 0; i < size; ++i)
            {
                _faces[i].render();
            }
        }
    }
}
