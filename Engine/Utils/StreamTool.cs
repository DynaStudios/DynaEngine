using System.IO;

namespace DynaStudios.Utils
{
    class StreamTool
    {
        public static int getInt(Stream stream)
        {
            int ret = 0;
            int b;
            do
            {
                ret *= 128;
                b = stream.ReadByte();
                ret += b % 128;
            } while (b > 127);
            return ret;
        }
    }
}
