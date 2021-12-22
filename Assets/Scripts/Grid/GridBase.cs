using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public abstract class GridBase<TValue> : MonoBehaviour
{
	[SerializeField] protected CellView _cellViewPrefab = null;
	[SerializeField] protected GridLayoutGroup _gridLayoutGroup = null;
	[SerializeField] protected int _disableRenderingAboveCellCount = 10000;

	public int columns { get; protected set; }
	public int rows { get; protected set; }
	public TValue[,] cells { get; protected set; }
	
	protected CellView[,] cellViews { get; set; }
	protected bool _disableRendering = false;

	protected abstract TValue ParseValue(string value);

	public virtual void Initialize(int numColumnsAndRows)
	{
		Initialize(numColumnsAndRows, numColumnsAndRows);
	}
	
	public virtual void Initialize(int numColumns, int numRows)
	{
		while (transform.childCount > 0)
		{
			DestroyImmediate(transform.GetChild(0).gameObject);
		}
		
		columns = numColumns;
		rows = numRows;
		cells = new TValue[columns, rows];

		_disableRendering = cells.Length >= _disableRenderingAboveCellCount;
		if (!_disableRendering)
		{
			CreateCellViews();
		}
	}

	public virtual void Initialize(string[] gridData, string delimiter = "")
	{
		ClearCellViews();
		
		if (!string.IsNullOrEmpty(delimiter))
		{
			columns = PuzzleBase.SplitString(gridData[0], delimiter).Length;
		}
		else
		{
			// If there's no delimiter, we'll turn each individual char of the line into a cell
			columns = gridData[0].Length;
		}

		rows = gridData.Length;
		
		cells = new TValue[columns, rows];
		for (int row = 0; row < rows; row++)
		{
			string lineData = gridData[row];
			
			string[] lineCellData;
			if (!string.IsNullOrEmpty(delimiter))
			{
				lineCellData = PuzzleBase.SplitString(lineData, delimiter);
			}
			else
			{
				// If there's no delimiter, split line into individual chars
				// eg. "12345" becomes { "1", "2", "3", "4", "5" }  
				lineCellData = lineData.ToCharArray().Select(c => c.ToString()).ToArray();
			}

			for (int column = 0; column < columns; column++)
			{
				string cellData = lineCellData[column];
				cells[column, row] = ParseValue(cellData);
			}
		}
		
		_disableRendering = cells.Length >= _disableRenderingAboveCellCount;
		if (!_disableRendering)
		{
			CreateCellViews();
		}
	}

	public virtual void ClearCellViews()
	{
		while (transform.childCount > 0)
		{
			DestroyImmediate(transform.GetChild(0).gameObject);
		}
	}

	protected virtual void CreateCellViews()
	{
		_gridLayoutGroup.constraintCount = columns;

		cellViews = new CellView[columns, rows];
		for (int row = 0; row < rows; row++)
		{
			for (int column = 0; column < columns; column++)
			{
				CellView cellView = Instantiate(_cellViewPrefab, transform);
				cellViews[column, row] = cellView;
				cellView.SetText(cells[column, row].ToString());
			}
		}
	}

	public virtual void HighlightCellView(int column, int row, Color color)
	{
		if (!_disableRendering)
		{
			cellViews[column, row].SetBackgroundColor(color);
		}
	}
	
	public void HighlightCellView(Vector2Int cell, Color color)
	{
		HighlightCellView(cell.x, cell.y, color);
	}
	
	public virtual void HighlightRow(int row, Color color)
	{
		for (int column = 0; column < rows; column++)
		{
			HighlightCellView(column, row, color);
		}
	}
	
	public virtual void HighlightColumn(int column, Color color)
	{
		for (int row = 0; row < rows; row++)
		{
			HighlightCellView(column, row, color);
		}
	}

	public TValue GetCellValue(int column, int row)
	{
		return cells[column, row];
	}

	public TValue GetCellValue(Vector2Int cell)
	{
		return GetCellValue(cell.x, cell.y);
	}

	public virtual void SetCellValue(int column, int row, TValue value)
	{
		cells[column, row] = value;

		if (!_disableRendering)
		{
			cellViews[column, row].SetText(value.ToString());
		}
	}

	public void SetCellValue(Vector2Int cell, TValue value)
	{
		SetCellValue(cell.x, cell.y, value);
	}

	public List<TValue> GetOrthogonalNeighbourValues(int column, int row)
	{
		return GetOrthogonalNeighbourCoords(column, row).Select(coord => cells[coord.x, coord.y]).ToList();
	}

	public List<TValue> GetOrthogonalNeighbourValues(Vector2Int cell)
	{
		return GetOrthogonalNeighbourValues(cell.x, cell.y);
	}

	public List<Vector2Int> GetOrthogonalNeighbourCoords(int column, int row)
	{
		List<Vector2Int> neighbours = new List<Vector2Int>();
		
		// Left
		if (column > 0)
		{
			neighbours.Add(new Vector2Int(column - 1, row));
		}
		
		// Right
		if (column < columns - 1)
		{
			neighbours.Add(new Vector2Int(column + 1, row));
		}
		
		// Above
		if (row > 0)
		{
			neighbours.Add(new Vector2Int(column, row - 1));
		}
		
		// Below
		if (row < rows - 1)
		{
			neighbours.Add(new Vector2Int(column, row + 1));
		}

		return neighbours;
	}

	public List<Vector2Int> GetOrthogonalNeighbourCoords(Vector2Int cell)
	{
		return GetOrthogonalNeighbourCoords(cell.x, cell.y);
	}

	public List<TValue> GetDiagonalNeighbourValues(int column, int row)
	{
		return GetDiagonalNeighbourCoords(column, row).Select(coord => cells[coord.x, coord.y]).ToList();
	}

	public List<TValue> GetDiagonalNeighbourValues(Vector2Int cell)
	{
		return GetDiagonalNeighbourValues(cell.x, cell.y);
	}

	public List<Vector2Int> GetDiagonalNeighbourCoords(int column, int row)
	{
		List<Vector2Int> neighbours = new List<Vector2Int>();
		
		// Up and left
		if (column > 0 && row > 0)
		{
			neighbours.Add(new Vector2Int(column - 1, row - 1));
		}
		
		// Up and right
		if (column < columns - 1 && row > 0)
		{
			neighbours.Add(new Vector2Int(column + 1, row - 1));
		}
		
		// Down and left
		if (column > 0 && row < rows - 1)
		{
			neighbours.Add(new Vector2Int(column - 1, row + 1));
		}
		
		// Down and right
		if (column < columns - 1 && row < rows - 1)
		{
			neighbours.Add(new Vector2Int(column + 1, row + 1));
		}

		return neighbours;
	}

	public List<Vector2Int> GetDiagonalNeighbourCoords(Vector2Int cell)
	{
		return GetDiagonalNeighbourCoords(cell.x, cell.y);
	}

	public List<TValue> GetAllNeighbourValues(int column, int row)
	{
		return GetAllNeighbourCoords(column, row).Select(coord => cells[coord.x, coord.y]).ToList();
	}

	public List<TValue> GetAllNeighbourValues(Vector2Int cell)
	{
		return GetAllNeighbourValues(cell.x, cell.y);
	}
	
	public List<Vector2Int> GetAllNeighbourCoords(int column, int row)
	{
		List<Vector2Int> neighbours = new List<Vector2Int>();
		neighbours.AddRange(GetOrthogonalNeighbourCoords(column, row));
		neighbours.AddRange(GetDiagonalNeighbourCoords(column, row));
		return neighbours;
	}
	
	public List<Vector2Int> GetAllNeighbourCoords(Vector2Int cell)
	{
		return GetAllNeighbourCoords(cell.x, cell.y);
	}
}
