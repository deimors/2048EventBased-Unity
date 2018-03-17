using System.Collections.Generic;
using System.Linq;
using _2048EventBased;

namespace Assets.Code
{
	public class NumberChooser : IChooseNewNumber
	{
		public Position ChoosePosition(IEnumerable<Position> emptyPositions) 
			=> emptyPositions.First();

		public int ChooseValue()
			=> 2;
	}
}