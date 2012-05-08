using System.IO;

using NUnit.Framework;

namespace DynaStudios.Utils
{
    [TestFixture(0, new byte[] { 0 })]
    [TestFixture(3, new byte[] { 3 })]
    [TestFixture(3000, new byte[] { 151, 56 })]
    [TestFixture(30000, new byte[] { 129, 234, 48 })]
    public class StreamToolTests
    {
        private int _integer;
        private byte[] _bytes;

        public StreamToolTests(int integer, byte[] bytes)
        {
            _integer = integer;
            _bytes = bytes;
        }

        [Test]
        public void intToBytesTest()
        {
            byte[] bytesFromStreamTool = StreamTool.intToBytes(_integer);
            Assert.That(bytesFromStreamTool, Is.EquivalentTo(_bytes));
        }

        [Test]
        public void getInt() {
            MemoryStream src = new MemoryStream(_bytes);
            int intFromStream = StreamTool.getInt(src);
            Assert.That(intFromStream, Is.EqualTo(_integer));
        }
    }
}
