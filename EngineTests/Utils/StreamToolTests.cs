using System.IO;

using NUnit.Framework;

namespace DynaStudios.Utils
{
    public class StreamToolTests
    {
        [Test]
        public void intToBytesTest()
        {
            byte[] bytesFromStreamTool = StreamTool.intToBytes(0);
            byte[] bytesExpected = new byte[] { 0 };
            Assert.That(bytesFromStreamTool, Is.EquivalentTo(bytesExpected));

            bytesFromStreamTool = StreamTool.intToBytes(3);
            bytesExpected = new byte[] { 3 };
            Assert.That(bytesFromStreamTool, Is.EquivalentTo(bytesExpected));

            bytesFromStreamTool = StreamTool.intToBytes(3000);
            bytesExpected = new byte[] { 151, 56 };
            Assert.That(bytesFromStreamTool, Is.EquivalentTo(bytesExpected));

            bytesFromStreamTool = StreamTool.intToBytes(30000);
            bytesExpected = new byte[] { 129, 234, 48 };
            Assert.That(bytesFromStreamTool, Is.EquivalentTo(bytesExpected));
        }

        [Test]
        public void getInt() {
            // added an not to read byte (value 123) to all test to ensure getInt stops where it is supposed to stop
            MemoryStream src = new MemoryStream(new byte[] {0, 123});
            int expected = 0;
            int intFromStream = StreamTool.getInt(src);
            Assert.That(intFromStream, Is.EqualTo(expected));

            src = new MemoryStream(new byte[] { 3, 123});
            expected = 3;
            intFromStream = StreamTool.getInt(src);
            Assert.That(intFromStream, Is.EqualTo(expected));

            src = new MemoryStream(new byte[] { 151, 56, 123 });
            expected = 3000;
            intFromStream = StreamTool.getInt(src);
            Assert.That(intFromStream, Is.EqualTo(expected));

            src = new MemoryStream(new byte[] { 129, 234, 48, 123 });
            expected = 30000;
            intFromStream = StreamTool.getInt(src);
            Assert.That(intFromStream, Is.EqualTo(expected));
        }
    }
}
