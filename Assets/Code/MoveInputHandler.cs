using UnityEngine;
using Zenject;
using _2048EventBased;

namespace Assets.Code
{
	public class MoveInputHandler : MonoBehaviour
	{
		[Inject]
		private Game Game { get; set; }

		void Update()
		{
			if (Input.GetKeyDown(KeyCode.UpArrow))
				Game.Move(Direction.Up);

			if (Input.GetKeyDown(KeyCode.DownArrow))
				Game.Move(Direction.Down);

			if (Input.GetKeyDown(KeyCode.LeftArrow))
				Game.Move(Direction.Left);

			if (Input.GetKeyDown(KeyCode.RightArrow))
				Game.Move(Direction.Right);
		}
	}
}