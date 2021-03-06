using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoolGrid : GridBase<bool, StaticCellCollection<bool>, StaticCellCollection<CellView>>
{
	protected override bool ParseValue(string value)
	{
		return bool.Parse(value);
	}

	protected override bool Compare(bool a, bool b)
	{
		return a == b;
	}
}
