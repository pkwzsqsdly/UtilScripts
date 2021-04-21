using System;


namespace TestMap
{
    class Program
    {
        
        static void Main(string[] args)
        {
            string preMap = @"wwwrwwwwwwrwwwwwwrwwwwwwrwwwwwwrwwwwwwrwwwwwwrwww";
            MapChunk ap = new MapChunk(preMap,7);
            MapChunkEntity entity = new MapChunkEntity(ap);
            entity.Rotate(MapChunk.RotateType.Rotate90);
        }
    }
}
