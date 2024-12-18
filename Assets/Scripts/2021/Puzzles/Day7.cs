using UnityEngine;

namespace AoC2021
{
	public class Day7 : PuzzleBase
	{
		protected override void ExecutePuzzle1()
		{
			int lowestFuelCost = int.MaxValue;
			int bestPosition = -1;
			int[] crabPositions = ParseIntArray(SplitString(_inputDataLines[0], ","));
			for (int checkPosition = Mathf.Min(crabPositions); checkPosition < Mathf.Max(crabPositions); checkPosition++)
			{
				int totalFuelCost = 0;
				foreach (int crabPosition in crabPositions)
				{
					totalFuelCost += Mathf.Abs(crabPosition - checkPosition);
				}

				if (totalFuelCost < lowestFuelCost)
				{
					lowestFuelCost = totalFuelCost;
					bestPosition = checkPosition;
				}
			}

			LogResult("Best position", bestPosition);
			LogResult("Total fuel cost", lowestFuelCost);
		}

		protected override void ExecutePuzzle2()
		{
			int lowestFuelCost = int.MaxValue;
			int bestPosition = -1;
			int[] crabPositions = ParseIntArray(SplitString(_inputDataLines[0], ","));
			for (int checkPosition = Mathf.Min(crabPositions); checkPosition < Mathf.Max(crabPositions); checkPosition++)
			{
				int totalFuelCost = 0;
				foreach (int crabPosition in crabPositions)
				{
					int distance = Mathf.Abs(crabPosition - checkPosition);
					int fuelCost = 0;
					for (int d = 1; d <= distance; d++)
					{
						fuelCost += d;
					}
				
					totalFuelCost += fuelCost;
				}

				if (totalFuelCost < lowestFuelCost)
				{
					lowestFuelCost = totalFuelCost;
					bestPosition = checkPosition;
				}
			}

			LogResult("Best position", bestPosition);
			LogResult("Total fuel cost", lowestFuelCost);
		}
	}
}
