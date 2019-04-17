using UnityEngine;

namespace Assets.TestAsset.Scripts
{
    public class MoveController : MonoBehaviour
    {
        public float MoveTime = .15f;

        public Vector3 Offset = Vector3.up / 2;

        public float ThresholdPosition = 0.2f;

        public GridController GridMap;

        private GridCell _targetCell;
        private Vector3 _targetPosition;

        private GridCell _startCell;
        private MovableObject _movableObject;
        private Vector3 _startPosition;

        private float _currentMoveTime;

        public void SetTargetCell(GridCell target)
        {
            if (_startCell != null
                && _startCell != target
                && _startCell.Item != null
                && IsValidMove(_startCell, target))
            {
                _targetCell = target;
                if (_movableObject != null)
                {
                    _targetCell.Item = _movableObject;
                    _startCell.Item = null;
                    _startCell = target;
                    _movableObject.transform.position = _targetPosition;
                    _startPosition = _movableObject.transform.position;
                }

                _targetPosition = _targetCell.transform.position + Offset;
                _currentMoveTime = 0;
            }
        }

        public void StartMove(GridCell target)
        {
            if (target == null || target.Item == null)
            {
                return;
            }

            _startCell = target;
            _movableObject = target.Item;
            _targetPosition = _movableObject.transform.position;
            _startPosition = _movableObject.transform.position;
        }

        public void EndMove()
        {
            if (_targetCell != null)
            {
                _targetCell.Item = _movableObject;
                _targetCell = null;
            }

            if (_startCell != null)
            {
                _startCell = null;
            }

            if (_movableObject)
            {
                _movableObject.transform.position = _targetPosition;
                _movableObject = null;
            }
        }

        private bool IsValidMove(GridCell origin, GridCell target)
        {
            var result = true;
            result &= target.Item == null;
            result &= origin.PosX - target.PosX == 0 || origin.PosY - target.PosY == 0;

            if (result)
            {
                result &= FreePath(origin, target);
            }

            return result;
        }

        private bool FreePath(GridCell origin, GridCell target)
        {
            var start = origin;
            var end = target;

            if (origin.PosX > target.PosX || origin.PosY > target.PosY)
            {
                start = target;
                end = origin;
            }

            for (var i = start.PosX; i <= end.PosX; i++)
            {
                for (var j = start.PosY; j <= end.PosY; j++)
                {
                    if (origin.PosX == i && origin.PosY == j)
                    {
                        continue;
                    }

                    if (GridMap[i, j].Item != null)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void Update()
        {
            if (_movableObject == null)
            {
                return;
            }

            if (MoveTime > 0 && _movableObject.transform.position != _targetPosition && _currentMoveTime <= MoveTime)
            {
                _movableObject.transform.position = Vector3.Lerp(_startPosition, _targetPosition, _currentMoveTime / MoveTime);
                _currentMoveTime += Time.deltaTime;
            }
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
