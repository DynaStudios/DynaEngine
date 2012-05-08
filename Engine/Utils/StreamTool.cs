using System.Collections.Generic;
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
		
		// HACK: testme
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
		
		// HACK: bugy
		public static byte[] getBytes(int nummber)
		{
			if (nummber == 0)
			{
				return new byte[]{0};
			}
			int tmpNumber = nummber;
			List<byte> bytes = new List<byte>();
			byte b;
			bool first = true;
			do
			{
				b = (byte) (tmpNumber % 128);
				if (!first) 
				{
					b += 128;
				}
				bytes.Add(b);
				tmpNumber = tmpNumber / 128;
				first = false;
			} while (b>=128);
			//return bytes.ToArray();
			byte[] ret = bytes.ToArray();
			System.Array.Reverse(ret);
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
