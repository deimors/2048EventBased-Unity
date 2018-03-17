using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using _2048EventBased;

namespace Assets.Code
{
	public class NumberManager : MonoBehaviour
	{
		private readonly Dictionary<Position, GameObject> _numbers = new Dictionary<Position, GameObject>();

		[SerializeField]
		private GameObject _numberPrefab;

		[Inject]
		public void Initialize(Game game)
		{
			game.NumberAdded += OnNumberAdded;
			game.NumberMoved += OnNumberMoved;
			game.NumbersMerged += OnNumbersMerged;
		}
		
		private void OnNumberAdded(NumberAddedEvent numberAddedEvent)
		{
			var newNumber = Instantiate(_numberPrefab);

			newNumber.transform.SetParent(transform, false);

			AddNumber(newNumber, numberAddedEvent.Position);

			SetNumberText(newNumber, numberAddedEvent.Number);
		}

		private static void SetNumberText(GameObject newNumber, int newNumberValue)
		{
			var newText = newNumberValue.ToString();
			var textComponent = newNumber.GetComponentInChildren<Text>();

			textComponent.text = newText;

			if (newText.Length <= 2)
				textComponent.fontSize = 48;
			else if (newText.Length == 3)
				textComponent.fontSize = 30;
			else if (newText.Length == 4)
				textComponent.fontSize = 24;
		}

		private void OnNumberMoved(NumberMovedEvent numberMovedEvent)
		{
			var number = RemoveNumber(numberMovedEvent.Origin);

			AddNumber(number, numberMovedEvent.Target);
		}

		private void OnNumbersMerged(NumbersMergedEvent numbersMergedEvent)
		{
			var firstNumber = RemoveNumber(numbersMergedEvent.Origin1);
			var secondNumber = RemoveNumber(numbersMergedEvent.Origin2);

			Destroy(secondNumber);

			AddNumber(firstNumber, numbersMergedEvent.Target);
			SetNumberText(firstNumber, numbersMergedEvent.NewNumber);
		}


		private void AddNumber(GameObject number, Position position)
		{
			number.GetComponent<RectTransform>().localPosition = new Vector3(position.Column * 72, position.Row * -72);
			_numbers[position] = number;
		}

		private GameObject RemoveNumber(Position position)
		{
			var number = _numbers[position];
			_numbers.Remove(position);
			return number;
		}
	}
}