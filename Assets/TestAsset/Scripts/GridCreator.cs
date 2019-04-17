using Assets.TestAsset.Scripts;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    public GridCell CellPrefab;
    public MovableObject CubePrefab;
    public Transform ContentRoot;

    public int SizeH = 6;
    public int SizeW = 6;

    public Vector2 Spacing;

    public GridCell[,] Create()
    {
        return CreateGrid(SizeH, SizeW);
    }

    public GridCell[,] CreateGrid(int height, int width)
    {
        Clear();

        var _cells = new GridCell[height, width];

        for (var i = 0; i < SizeW; i++)
        {
            for (var j = 0; j < SizeH; j++)
            {
                var item = Instantiate(CellPrefab, ContentRoot);
                _cells[i, j] = item;
                item.PosX = i;
                item.PosY = j;
                item.transform.localPosition = new Vector3(j + Spacing.x * j, 0, i + Spacing.y * i);
            }
        }

        return _cells;
    }

    public void CreateCubes(GridCell[,] cells)
    {
        for (var i = 0; i < cells.GetLength(0); i++)
        {
            for (var j = 0; j < cells.GetLength(1); j++)
            {
                if (Random.value > 0.5f)
                {
                    var item = Instantiate(CubePrefab, cells[i, j].transform.position, Quaternion.identity);
                    item.transform.position += Vector3.up / 2;
                    cells[i, j].Item = item;
                }
            }
        }
    }

    public void Clear()
    {
        for (var i = 0; i < ContentRoot.transform.childCount; i++)
        {
#if UNITY_EDITOR
            DestroyImmediate(ContentRoot.transform.GetChild(i).gameObject);
#else        
            Destroy(ContentRoot.transform.GetChild(i).gameObject);
#endif
        }
    }
}
