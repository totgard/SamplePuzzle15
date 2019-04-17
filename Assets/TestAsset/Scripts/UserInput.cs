using UnityEngine;

namespace Assets.TestAsset.Scripts
{
    public class UserInput : MonoBehaviour
    {
        public KeyCode MoveKey = KeyCode.Mouse0;
        public KeyCode AddKey = KeyCode.Mouse1;

        public Color SelectedColor;

        public LayerMask GridLayer;

        public MoveController MoveController;
        public ObjectCreator Creator;

        private Ray _mouseRay;
        private RaycastHit _hitInfo;
        private GridCell _selectedCell;
        private MovableObject _selectedObject;

        private Color _oldColor;

        private void Update()
        {
            _mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);

            var result = GetTarget(GridLayer);
            if (result != null)
            {
                _selectedCell = result.GetComponent<GridCell>();
            }

            if (Input.GetKey(MoveKey))
            {
                if (_selectedCell != null)
                {
                    MoveController.SetTargetCell(_selectedCell);
                }
            }
            else if (Input.GetKeyDown(AddKey)
                    && _selectedCell != null)
            {
                _selectedCell.Item = Creator.Create(_selectedCell);
            }

            if (Input.GetKeyDown(MoveKey))
            {
                if (_selectedCell != null)
                {
                    MoveController.StartMove(_selectedCell);
                    if (_selectedCell.Item != null)
                    {
                        _selectedObject = _selectedCell.Item;
                    }
                    if (_selectedObject != null)
                    {
                        _oldColor = _selectedObject.Renderer.material.color;
                        _selectedObject.Renderer.material.color = SelectedColor;
                    }
                }
            }

            if (Input.GetKeyUp(MoveKey))
            {
                MoveController.EndMove();
                if (_selectedObject != null)
                {
                    _selectedObject.Renderer.material.color = _oldColor;
                }
            }
        }

        private GameObject GetTarget(LayerMask mask)
        {
            var hits = Physics.RaycastAll(_mouseRay, 10000, mask);

            if (hits != null && hits.Length > 0)
            {
                return hits[0].collider.gameObject;
            }

            return null;
        }

#if UNITY_EDITOR

        private void OnValidate()
        {
            if (MoveController == null)
            {
                MoveController = FindObjectOfType<MoveController>();
            }

            if (Creator == null)
            {
                Creator = FindObjectOfType<ObjectCreator>();
            }
        }

        private void Reset()
        {
            if (MoveController == null)
            {
                MoveController = FindObjectOfType<MoveController>();
            }

            if (Creator == null)
            {
                Creator = FindObjectOfType<ObjectCreator>();
            }
        }

#endif
    }
}