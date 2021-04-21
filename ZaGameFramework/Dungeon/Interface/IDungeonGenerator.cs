
namespace Amaranth.Engine
{
    public interface IDungeonGenerator
    {
        void Create(Dungeon dungeon, bool isDescending, int depth, object options);
    }
}
