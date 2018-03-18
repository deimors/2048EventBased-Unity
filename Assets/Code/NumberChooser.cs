using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using _2048EventBased;

namespace Assets.Code
{
	public class NumberChooser : IChooseNewNumber
	{
		public Position ChoosePosition(IEnumerable<Position> emptyPositions)
		{
			var positions = emptyPositions.ToArray();

			var index = Random.Range(0, positions.Length - 1);

			return positions[index];
		}

		public int ChooseValue()
			=> Random.Range(0f, 1f) < .25f ? 4 : 2;
	}
}