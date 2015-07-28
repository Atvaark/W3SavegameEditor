using System.Diagnostics;
using System.IO;
using System.Linq;

namespace W3SavegameEditor.ChunkedLz4
{
    public static class ChunkedLz4File
    {
        public static byte[] Read(Stream input)
        {

            ChunkedLz4FileHeader header = ChunkedLz4FileHeader.Read(input);
            var table = ChunkedLz4FileTable.Read(input, header.ChunkCount);
            input.Position = header.OffsetFirstChunk;

            var data = new byte[table.Chunks.Sum(c => c.DecompressedChunkSize)];
            using (var memoryStream = new MemoryStream(data))
            {
                foreach (var chunk in table.Chunks)
                {
                    byte[] chunkData = chunk.Read(input);
                    memoryStream.Write(chunkData, 0, chunkData.Length);
                    Debug.Assert(input.Position == chunk.EndOfChunkOffset || chunk.EndOfChunkOffset == 0);
                }
            }

            return data;
        }
    }
}