using System;


namespace TestMap
{
    class Program
    {
        
        static void Main(string[] args)
        {
            // MapChunk ap = new MapChunk(preMap,7);
            // MapChunkEntity entity = new MapChunkEntity(ap,Coord2d.zero);
            // entity.Rotate(MapChunk.RotateType.Rotate90);
            // MapContext map = new MapContext();
            // map.Generate();
            STSMap map = new STSMap();
            map.AddRandomHandler(new STSMapTestHandler());
            map.Init(10,6);
            map.ConnectNodes();
            map.Print();
            // Console.WriteLine(ConfigRandom.IsContain(4,3));
        }
    }
}