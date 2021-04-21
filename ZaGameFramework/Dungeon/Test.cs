using System.Collections;
using System.Collections.Generic;
using Amaranth.Engine;
using Bramble.Core;

public class Test
{
    private int TileSize = 1;
    // Start is called before the first frame update
    public void Start()
    {
            int seed = Rng.Int(System.Int32.MaxValue);

            Rng.Seed(seed);

            var dungeon = new Dungeon(61,101);
            // fill the dungeon with default tiles
            dungeon.Generate(false,3);
            // draw(dungeon);
    }
    
}
