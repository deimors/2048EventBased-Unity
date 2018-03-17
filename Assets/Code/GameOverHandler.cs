using UnityEngine;
using Zenject;
using _2048EventBased;

namespace Assets.Code
{
	public class GameOverHandler : MonoBehaviour
	{
		[SerializeField]
		private GameObject _gameLostBanner;

		[SerializeField]
		private GameObject _gameWonBanner;

		[Inject]
		public void Initialize(Game game)
		{
			game.GameLost += () => _gameLostBanner.SetActive(true);
			game.GameWon += () => _gameWonBanner.SetActive(true);
		}
	}
}