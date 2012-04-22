using System;
using System.Collections.Generic;
using System.IO;

using OpenTK;

namespace DynaStudios.Blocks
{
    // HACK: Incomplete should be completed after the Ludum Dare
    class FacePoint
    {
        public int vertex;
        public int texture = 1;
        public int normal = 1;
    }

    class WavefrontFace
    {
        public string material = null;
        public List<FacePoint> points = new List<FacePoint>();

        public WavefrontFace(string[] face)
        {
            for (int i = 1; i < face.Length; ++i)
            {
                readPoint(face[i]);
            }
        }

        private void readPoint (string strPoint) {
            string[] data = strPoint.Split('/');
            FacePoint point = new FacePoint();
            if (!int.TryParse(data[0], out point.vertex))
            {
                throw new ArgumentOutOfRangeException("vertex 0 given but vertexes starts with 1");
            }
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

    class WavefrontObjectParser
    {
        public string currentMaterial = null;
        List<WavefrontFace> faces = new List<WavefrontFace>();
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
            WavefrontFace face = new WavefrontFace(line);
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

    public class WavefrontObject
    {
        List<WavefrontFace> faces = new List<WavefrontFace>();

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
            
        }
    }
}
