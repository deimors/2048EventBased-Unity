using UnityEngine;
using Zenject;
using _2048EventBased;

namespace Assets.Code
{
	public class GameOverHandler : MonoBehaviour
	{
		[SerializeField] private GameObject _gameLostBanner;

		[SerializeField] private GameObject _gameWonBanner;

		[SerializeField] private GameObject _curtain;

		[Inject] private InputLock InputLock { get; set; }

		[Inject]
		public void Initialize(Game game)
		{
			game.GameLost += OnGameLost;
			game.GameWon += OnGameWon;
		}

		private void OnGameLost()
		{
			_gameLostBanner.SetActive(true);
			ShowCurtain();
		}

		private void OnGameWon()
		{
			_gameWonBanner.SetActive(true);
			ShowCurtain();
		}

		private void ShowCurtain()
		{
			_curtain.SetActive(true);
			InputLock.Lock = true;
		}
	}
}