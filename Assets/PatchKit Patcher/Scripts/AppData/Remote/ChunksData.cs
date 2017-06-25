using PatchKit.Patcher.AppData.Remote.Downloaders;

namespace PatchKit.Patcher.AppData.Remote
{
    public struct ChunksData
    {
        public long ChunkSize;

        public Chunk[] Chunks;
    }
}