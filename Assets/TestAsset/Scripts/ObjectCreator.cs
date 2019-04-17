using UnityEngine;

namespace Assets.TestAsset.Scripts
{
    public class ObjectCreator : MonoBehaviour
    {
        public MovableObject Prefab;
        public GridController GridMap;

        public Vector3 Offset = Vector3.up / 2;

        public MovableObject Create(GridCell cell)
        {
            return Instantiate(Prefab, cell.transform.position + Offset, Quaternion.identity);
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (GridMap == null)
            {
                GridMap = FindObjectOfType<GridController>();
            }
        }

        private void Reset()
        {
            if (GridMap == null)
            {
                GridMap = FindObjectOfType<GridController>();
            }
        }

#endif
    }
}
