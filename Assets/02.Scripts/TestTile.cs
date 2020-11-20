using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ETileType
{
    FieldTile,
    TowerTile,
    ObstacleTile,

    None
}

public enum ETowerFitType
{
	Fits,
	Overlaps,
	OutOfBounds
}

public class TestTile : MonoBehaviour
{
    [SerializeField] ETileType _tileType = ETileType.FieldTile;
	[SerializeField] TestNode _nodePrefab = null;
	[SerializeField] TestIntVector2 _dimensions = TestIntVector2.zero;
	[SerializeField] float gridSize = 1;

	bool[,] _availableNodes;
	TestNode[,] _nodeTiles;

	private void Awake()
    {
		_nodeTiles = new TestNode[_dimensions.x, _dimensions.y];
		_availableNodes = new bool[_dimensions.x, _dimensions.y];
		for (int y = 0; y < _dimensions.y; y++)
		{
			for (int x = 0; x < _dimensions.x; x++)
			{
				Vector3 targetPos = new Vector3(x + 0.5f, 0.01f, y + 0.5f) * gridSize;
				TestNode nodeTile = Instantiate(_nodePrefab, transform);
				nodeTile._parentTile = this;
				nodeTile.transform.localPosition = targetPos;
				_nodeTiles[x, y] = nodeTile;
				nodeTile._pos = new TestIntVector2(x, y);
				nodeTile.StateSet(_tileType);
			}
		}
	}

	public TestIntVector2 WorldToGrid(Vector3 worldLocation, TestIntVector2 sizeOffset)
	{
		Vector3 localLocation = transform.InverseTransformPoint(worldLocation);

		var offset = new Vector3(sizeOffset.x * 0.5f, 0.0f, sizeOffset.y * 0.5f);
		localLocation -= offset;

		int xPos = Mathf.RoundToInt(localLocation.x);
		int yPos = Mathf.RoundToInt(localLocation.z);

		return new TestIntVector2(xPos, yPos);
	}

	public Vector3 NodeToPosition(TestIntVector2 nodePos, TestIntVector2 size)
    {
		Vector3 nodePosition = transform.position;
		nodePosition.x += nodePos.x + size.x -1;
		nodePosition.z += nodePos.y + size.y -1;
		return nodePosition;
    }

	public ETowerFitType Fits(TestIntVector2 gridPos, TestIntVector2 size)
	{
		if ((size.x > _dimensions.x) || (size.y > _dimensions.y))
		{
			return ETowerFitType.OutOfBounds;
		}

		TestIntVector2 extents = gridPos + size;

		if ((gridPos.x < 0) || (gridPos.y < 0) ||
			(extents.x > _dimensions.x) || (extents.y > _dimensions.y))
		{
			return ETowerFitType.OutOfBounds;
		}

		for (int y = gridPos.y; y < extents.y; y++)
		{
			for (int x = gridPos.x; x < extents.x; x++)
			{
				if (_availableNodes[x, y])
				{
					return ETowerFitType.Overlaps;
				}
			}
		}

		return ETowerFitType.Fits;
	}

	public void Occupy(TestIntVector2 gridPos, TestIntVector2 size)
	{
		TestIntVector2 extents = gridPos + size;

		for (int y = gridPos.y; y < extents.y; y++)
		{
			for (int x = gridPos.x; x < extents.x; x++)
			{
				_availableNodes[x, y] = true;
				_nodeTiles[x, y].StateSet(_tileType, ENodeState.Filled);
			}
		}
	}

	public void Clear(TestIntVector2 gridPos, TestIntVector2 size)
	{
		TestIntVector2 extents = gridPos + size;

		for (int y = gridPos.y; y < extents.y; y++)
		{
			for (int x = gridPos.x; x < extents.x; x++)
			{
				_availableNodes[x, y] = false;
				_nodeTiles[x, y].StateSet(_tileType);
			}
		}
	}

#if UNITY_EDITOR
	void OnValidate()
	{
		// Validate grid size
		if (gridSize <= 0)
		{
			gridSize = 1;
		}

		// Validate dimensions
		if (_dimensions.x <= 0)
		{
			_dimensions.x = 1;
		}
        if (_dimensions.y <= 0)
        {
			_dimensions.y = 1;
        }
	}

	/// <summary>
	/// Draw the grid in the scene view
	/// </summary>
	void OnDrawGizmos()
	{
		Color prevCol = Gizmos.color;
		if (_tileType == ETileType.FieldTile)
		{
			Gizmos.color = Color.green;
		}
		else if (_tileType == ETileType.TowerTile)
		{
			Gizmos.color = Color.cyan;
		}
		else if (_tileType == ETileType.ObstacleTile)
		{
			Gizmos.color = Color.yellow;
		}

		else
		{
			Gizmos.color = Color.black;
		}

		Matrix4x4 originalMatrix = Gizmos.matrix;
		Gizmos.matrix = transform.localToWorldMatrix;

		// Draw local space flattened cubes
		for (int y = 0; y < _dimensions.y; y++)
		{
			for (int x = 0; x < _dimensions.x; x++)
			{
				var position = new Vector3((x + 0.5f) * gridSize, 0, (y + 0.5f) * gridSize);
				Gizmos.DrawWireCube(position, new Vector3(gridSize, 0, gridSize));
			}
		}

		Gizmos.matrix = originalMatrix;
		Gizmos.color = prevCol;
	}
#endif
}
