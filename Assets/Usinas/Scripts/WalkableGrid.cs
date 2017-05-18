using UnityEngine;
using System.Collections;

public class WalkableGrid : MonoBehaviour
{

    public int xSize = 5;
    public int ySize = 5;
    public float tileSize = 3f;

}
	

    /*public Vector3 CoordToPosition(int x, int y)
    {
        return new Vector3(-xSize / 2f + 0.5f + x, 0, -ySize / 2f + 0.5f + y) * tileSize;
    }

    public Vector3 CoordToPosition(Coord coord)
    {
        return new Vector3(-xSize / 2f + 0.5f + coord.x, 0, -ySize / 2f + 0.5f + coord.y) * tileSize;
    }

    public Coord GetTileFromPosition(Vector3 position)
    {
        int x = Mathf.RoundToInt(position.x / tileSize + (xSize - 1) / 2f);
        int y = Mathf.RoundToInt(position.z / tileSize + (ySize - 1) / 2f);

        x = Mathf.Clamp(x, 0, xSize - 1);
        y = Mathf.Clamp(y, 0, ySize - 1);

        return new Coord(x, y);
    }
}

public class Coord
{
    public int x, y;

    public Coord(int _x, int _y)
    {
        x = _x;
        y = _y;
    }
}*/
