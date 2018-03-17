using _2048EventBased;
using UnityEngine;
using Zenject;

namespace Assets.Code
{
	public class BoardBuilder : MonoBehaviour
	{
		[Inject]
		private Game Game { get; set; }

		private void Start()
		{
			Game[1, 1] = 2;
			Game[2, 2] = 2;
		}
	}
}