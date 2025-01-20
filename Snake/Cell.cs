using System.Drawing;
using System.Windows.Forms;

namespace Snake
{
	public partial class Cell : UserControl
	{
		public int Column { get; set; }
		public int Row { get; set; }
		private readonly Color EmptyColor; 
		//private readonly Color SnakeColor = Color.FromArgb(45,139,212);
		private readonly Color SnakeColor = Color.DarkBlue;
		private readonly Color AppleColor = Color.Yellow;
		
		public Cell(int column, int row, bool isDark)
		{
			InitializeComponent();
			Column = column;
			Row = row;

			if(isDark)
			{
				EmptyColor = Color.DarkKhaki;
			}
			else
			{
				EmptyColor = Color.Olive;
				//EmptyColor = Color.DarkGoldenrod;
				//Color.FromArgb(64, 64, 64);
			}

			BackColor = EmptyColor;
		}

		public void AddSnakePart()
		{ 
			BackColor = SnakeColor;
		}

		public void RemoveSnakePart()
		{
			BackColor = EmptyColor;
		}

		public void AddApple()
		{
			BackColor = AppleColor;
		}

		public bool IsCellFree { get => BackColor == EmptyColor; }

		public bool IsAppleInCell { get => BackColor == AppleColor; }
	}
}
