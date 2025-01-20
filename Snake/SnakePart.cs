using System;
using static Snake.MainForm;

namespace Snake
{


	internal class SnakePart
	{
		private int _x = 0;
		private int _y = 0;
		private MovingDirection _currentDirection;
		private SnakePart AheadSnakePart;

		public MovingDirection CurrentDirection { get { return _currentDirection; } }

		public event SnakePartMovingEventHandler MovingHappened;
		public event SnakePartMovingEventHandler SnakeRemoved;
		public event SnakePartMovingEventHandler SnakeMoved;

		public SnakePart(int x, int y, SnakePart aheadPart, MovingDirection? direction = null, bool isHead = false)
		{
			IsHead = isHead;
			_x = x;
			_y = y;
			if(direction != null)
			{
				_currentDirection = direction.Value;
			}
			else 
			{
				_currentDirection = MovingDirection.Right;
			}
			
			AheadSnakePart = aheadPart;

			if (!isHead)
			{
				AheadSnakePart.MovingHappened += AheadPart_MovingHappened;
			}
		}

		public void ChangeHead(SnakePart newHead)
		{
			if (!IsHead)
			{
				throw new InvalidOperationException("Only head can be changed by new head.");
			}
			AheadSnakePart = newHead;
			AheadSnakePart.MovingHappened += AheadPart_MovingHappened;
		}

		private void AheadPart_MovingHappened(int nextX, int nextY)
		{

			var oldX = X;
			var oldY = Y;
			X = nextX;
			Y = nextY;

			SnakeRemoved?.Invoke(oldX, oldY);
			MovingHappened?.Invoke(oldX, oldY);
			SnakeMoved?.Invoke(X, Y);
		}

		public void ChangeHeadDirection(MovingDirection? newDirection)
		{
			if (!IsHead)
			{
				throw new Exception("Only head can change direction");
			}

			if (newDirection == null)
			{
				return;
			}
			if (
					(_currentDirection == MovingDirection.Right && newDirection == MovingDirection.Left)
				|| (_currentDirection == MovingDirection.Left && newDirection == MovingDirection.Right)
				|| (_currentDirection == MovingDirection.Up && newDirection == MovingDirection.Down)
				|| (_currentDirection == MovingDirection.Down && newDirection == MovingDirection.Up)
				)
			{
				return;
			}
			else
			{
				_currentDirection = newDirection.Value;
			}
		}

		public void Move()
		{
			var oldX = X;
			var oldY = Y;

			switch (_currentDirection)
			{
				case MovingDirection.Left: X--; break;
				case MovingDirection.Right: X++; break;
				case MovingDirection.Up: Y--; break;
				case MovingDirection.Down: Y++; break;
				default: break;
			}


			SnakeRemoved?.Invoke(oldX, oldY);
			MovingHappened?.Invoke(oldX, oldY);
			SnakeMoved?.Invoke(X, Y);
		}

		public int X
		{
			get => _x;
			private set
			{
				if (value < 0)
				{
					_x = MainForm.ColumnsCount - 1;
				}
				else if (value > MainForm.ColumnsCount - 1)
				{
					_x = 0;
				}
				else
				{
					_x = value;
				}
			}
		}

		public int Y
		{
			get => _y;
			private set
			{
				if (value < 0)
				{
					_y = MainForm.RowsCount - 1;
				}
				else if (value > MainForm.RowsCount - 1)
				{
					_y = 0;
				}
				else
				{
					_y = value;
				}
			}
		}

		public bool IsHead { get; private set; }
	}
}
