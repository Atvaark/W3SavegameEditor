using System;
using System.IO;
using System.Text;

namespace W3SavegameEditor.ChunkedLz4
{
    public class ChunkedLz4FileHeader
    {
        public int ChunkCount { get; set; }
        public int OffsetFirstChunk { get; set; }

        public static ChunkedLz4FileHeader Read(Stream input)
        {
            using (var reader = new BinaryReader(input, Encoding.ASCII, true))
            {
                string saveFileHeader = new string(reader.ReadChars(4));
                if (saveFileHeader != "SNFH")
                {
                    throw new InvalidOperationException();
                }

                string chunkedLz4FileHeader = new string(reader.ReadChars(4));
                if (chunkedLz4FileHeader != "FZLC")
                {
                    throw new InvalidOperationException();
                }

                return new ChunkedLz4FileHeader
                {
                    ChunkCount = reader.ReadInt32(),
                    OffsetFirstChunk = reader.ReadInt32()
                };
            }
        }
    }
}