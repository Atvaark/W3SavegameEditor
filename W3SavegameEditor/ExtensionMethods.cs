using System.IO;

namespace W3SavegameEditor
{
    internal static class ExtensionMethods
    {
        public static string ReadString(this BinaryReader reader, int count)
        {
            return new string(reader.ReadChars(count));
        }

        public static string PeekString(this BinaryReader reader, int count)
        {
            long position = reader.BaseStream.Position;
            string value = new string(reader.ReadChars(count));
            reader.BaseStream.Position = position;
            return value;
        }

        public static void Skip(this BinaryReader reader, int count)
        {
            reader.BaseStream.Position += count;
        }
    }
}
