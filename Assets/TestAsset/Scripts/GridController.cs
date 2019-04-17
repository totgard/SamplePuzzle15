using UnityEngine;

namespace Assets.TestAsset.Scripts
{
    public class GridController : MonoBehaviour
    {
        public GridCreator Creator;

        public GridCell this[int i, int j] => _cells[i, j];

        private GridCell[,] _cells;

        private void Start()
        {
            _cells = Creator.Create();
            Creator.CreateCubes(_cells);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (Creator == null)
            {
                Creator = FindObjectOfType<GridCreator>();
            }
        }

#endif
    }
}
