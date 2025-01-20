using System;
using System.Collections.Generic;
using System.Linq;

namespace Snake
{
	internal class Snake
	{
		public delegate void AppleEatenHandler();
		public event AppleEatenHandler AppleEaten;

		public Snake(int headPositionX, int headPositionY, int length = 3)
		{
			if (length < 1)
			{
				throw new ArgumentOutOfRangeException(nameof(length));
			}

			var head = new SnakePart(headPositionX, headPositionY, null, isHead: true);
			
			var positionX = headPositionX - 1;

			Parts.Add(head);

			for (int i = 1; i < length; i++)
			{
				Parts.Add(new SnakePart(positionX, headPositionY, Parts[i - 1]));

				positionX--;
			}
		}

		public int Length { get { return Parts.Count; } }
		public List<SnakePart> Parts { get; set; } = new List<SnakePart>();

		/// <summary>
		/// Changes direction of the snake
		/// </summary>
		/// <param name="direction">New direction.</param>
		public void ChangeDirection(MovingDirection? direction)
		{
			Parts[0].ChangeHeadDirection(direction);
		}

		public void Move()
		{
			Parts[0].Move();
		}
		
		public SnakePart IncreaseSnake(int x, int y)
		{
			var newHead = new SnakePart(x, y, null, Parts[0].CurrentDirection, isHead: true);
			newHead.Move();
			Parts[0].ChangeHead(newHead);
			Parts = Parts.Prepend(newHead).ToList();
			AppleEaten.Invoke();
			return newHead;
		}
	}
}
