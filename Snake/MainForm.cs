using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
	public partial class MainForm : Form
	{
		public const int ColumnsCount = 17;
		public const int RowsCount = 15;
		private readonly Cell[,] _cells = new Cell[ColumnsCount, RowsCount];
		private Snake _snake;
		private Random _random;

		public delegate void SnakePartMovingEventHandler(int x, int y);

		public MainForm()
		{
			InitializeComponent();
			timer1.Enabled = false;
			_random = new Random();
		}

		private void InitNewGame()
		{
			timer1.Enabled = false;
			tableLayoutPanel1.Controls.Clear();
			_snake = new Snake(3, 0);
			_snake.AppleEaten += AddApple;


			bool isCellDark = false;

			for (int i = 0; i < 17; i++)
			{
				for (int j = 0; j < 15; j++)
				{
					var cell = new Cell(i, j, isCellDark);
					_cells[i, j] = cell;
					tableLayoutPanel1.Controls.Add(cell, i, j);
					isCellDark = !isCellDark;
				}
			}

			foreach (var part in _snake.Parts)
			{
				_cells[part.X, part.Y].AddSnakePart();
				part.SnakeRemoved += RemoveSnakePart;
				part.SnakeMoved += MoveSnakePart;
			}

			AddApple();
		}
		private void Form1_Load(object sender, EventArgs e)
		{
			InitNewGame();
		}

		private void AddApple()
		{
			bool appleAdded = false;
			do
			{
				var applePostionX = _random.Next(0, ColumnsCount - 1);
				var applePostionY = _random.Next(0, RowsCount - 1);
				if (_cells[applePostionX, applePostionY].IsCellFree)
				{
					_cells[applePostionX, applePostionY].AddApple();
					appleAdded = true;
				}
			}
			while (!appleAdded);
		}

		private void MoveSnakePart(int x, int y)
		{
			var cell = _cells[x, y];
			if (!cell.IsCellFree && !cell.IsAppleInCell)
			{
				throw new Exception("Game over");
			}
			else if (cell.IsAppleInCell)
			{
				if(timer1.Interval > 80)
				{
					timer1.Interval -= 10;
				}
				
				var newHead = _snake.IncreaseSnake(x, y);
				newHead.SnakeRemoved += RemoveSnakePart;
				newHead.SnakeMoved += MoveSnakePart;
			}

			_cells[x, y].AddSnakePart();
		}

		private void RemoveSnakePart(int oldX, int oldY)
		{
			_cells[oldX, oldY].RemoveSnakePart();
		}

		private void timer1_Tick(object sender, EventArgs e)
		{
			try
			{
				_snake.Move();
			}
			catch (Exception ex)
			{
				timer1.Enabled = false;
				MessageBox.Show(ex.Message, "Please try again", MessageBoxButtons.OK, MessageBoxIcon.Error);
				InitNewGame();
			}
		}

		private void btnStartGame_Click(object sender, EventArgs e)
		{
			timer1.Enabled = !timer1.Enabled;
			if (timer1.Enabled)
			{
				btnStartGame.Text = "Stop game";
			}
			else
			{
				btnStartGame.Text = "Start game";
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			var part = _snake.Parts[0];
			MovingDirection? newDirection = null;

			switch (keyData)
			{
				case Keys.Up:
					newDirection = MovingDirection.Up;
					break;
				case Keys.Down:
					newDirection = MovingDirection.Down;
					break;
				case Keys.Left:
					newDirection = MovingDirection.Left;
					break;
				case Keys.Right:
					newDirection = MovingDirection.Right;
					break;
				case Keys.Enter:
					btnStartGame_Click(new object(), new EventArgs());
					break;
				default:
					break;
			}
			
			_snake.ChangeDirection(newDirection);
			return base.ProcessCmdKey(ref msg, keyData);
		}
	}
}
