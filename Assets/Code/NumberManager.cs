﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using _2048EventBased;

namespace Assets.Code
{
	public class NumberManager : MonoBehaviour
	{
		private const float Velocity = 1000;
		private const float UpdateRate = 0.005f;
		private static readonly Vector3 CenterOffset = new Vector3(-72 * 1.5f, 72 * 1.5f);
		private readonly Dictionary<Position, GameObject> _numbers = new Dictionary<Position, GameObject>();

		private readonly Queue<NumberAddedEvent> _addedEvents = new Queue<NumberAddedEvent>(10);

		private int _numbersMoving;

		[SerializeField]
		private GameObject _numberPrefab;

		[Inject]
		private InputLock InputLock { get; set; }

		[Inject]
		public void Initialize(Game game)
		{
			game.NumberAdded += OnNumberAdded;
			game.NumberMoved += OnNumberMoved;
			game.NumbersMerged += OnNumbersMerged;
		}
		
		private void OnNumberAdded(NumberAddedEvent numberAddedEvent)
		{
			if (_numbersMoving == 0)
				AddNewNumber(numberAddedEvent);
			else
				_addedEvents.Enqueue(numberAddedEvent);
		}

		private void AddNewNumber(NumberAddedEvent numberAddedEvent)
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

		private async void OnNumberMoved(NumberMovedEvent numberMovedEvent)
		{
			var number = RemoveNumber(numberMovedEvent.Origin);

			var originPosition = numberMovedEvent.Origin;
			var targetPosition = numberMovedEvent.Target;

			_numbersMoving++;
			InputLock.Lock = true;

			await AnimateMoveNumber(number, originPosition, targetPosition);
			
			AddNumber(number, numberMovedEvent.Target);

			_numbersMoving--;
			InputLock.Lock = _numbersMoving > 0;
			ProcessAddedQueue();
		}

		private void ProcessAddedQueue()
		{
			if (_numbersMoving == 0)
			{
				while (_addedEvents.Any())
					AddNewNumber(_addedEvents.Dequeue());
			}
		}

		private async void OnNumbersMerged(NumbersMergedEvent numbersMergedEvent)
		{
			var firstNumber = RemoveNumber(numbersMergedEvent.Origin1);
			var secondNumber = RemoveNumber(numbersMergedEvent.Origin2);

			_numbersMoving++;
			InputLock.Lock = true;

			await Task.WhenAll(
				AnimateMoveNumber(firstNumber, numbersMergedEvent.Origin1, numbersMergedEvent.Target),
				AnimateMoveNumber(secondNumber, numbersMergedEvent.Origin2, numbersMergedEvent.Target)
			);
			
			Destroy(secondNumber);
			
			AddNumber(firstNumber, numbersMergedEvent.Target);
			SetNumberText(firstNumber, numbersMergedEvent.NewNumber);


			_numbersMoving--;
			InputLock.Lock = _numbersMoving > 0;
			ProcessAddedQueue();
		}

		private static async Task AnimateMoveNumber(GameObject number, Position originPosition, Position targetPosition)
		{
			var origin = PositionToVector3(originPosition);
			var toTarget = PositionToVector3(targetPosition) - origin;
			var distanceToTarget = toTarget.magnitude;
			var increment = toTarget.normalized * Velocity * UpdateRate;

			var rectTransform = number.GetComponent<RectTransform>();
			while ((rectTransform.localPosition - origin).magnitude < distanceToTarget)
			{
				rectTransform.localPosition += increment;
				await Task.Delay(TimeSpan.FromSeconds(UpdateRate));
			}
		}

		private void AddNumber(GameObject number, Position position)
		{
			number.GetComponent<RectTransform>().localPosition = PositionToVector3(position);
			_numbers[position] = number;
		}

		private static Vector3 PositionToVector3(Position position) 
			=> new Vector3(position.Column * 72, position.Row * -72) + CenterOffset;

		private GameObject RemoveNumber(Position position)
		{
			var number = _numbers[position];
			_numbers.Remove(position);
			return number;
		}
	}
}