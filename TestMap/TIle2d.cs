
public class Tile2d
{
    public Coord2d coord2d;
    public bool isTopPassable;
    public bool isBottomPassable;
    public bool isLeftPassable;
    public bool isRightPassable;
    
    public Tile2d(int x, int y)
    {
        coord2d = new Coord2d(x, y);
        isTopPassable = true;
        isBottomPassable = true;
        isLeftPassable = true;
        isRightPassable = true;
    }
    
}