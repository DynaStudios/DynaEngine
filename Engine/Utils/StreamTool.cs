using System.IO;

namespace DynaStudios.Utils
{
    public class StreamTool
    {
        private static string _DIR = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static string DIR
        {
            get { return _DIR; }
        }
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

        public static string getStringForFilesystem(int nummber)
        {
            if (nummber < 0)
            {
                return "m" + (-nummber).ToString();
            }
            return nummber.ToString();
        }
    }
}
