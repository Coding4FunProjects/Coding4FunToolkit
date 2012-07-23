using System.Windows.Media.Animation;

namespace testPeopleTile
{
	public struct ImageTileState
	{
		public Storyboard Storyboard { get; set; }
		public int Row { get; set; }
		public int Column { get; set; }
	}

    public struct ImageLocation
	{
        public ImageLocation(int row, int column) : this()
		{
			Row = row;
			Column = column;
		}

		public int Row { get; set; }
		public int Column { get; set; }

        public int Index { get; set; }

		public static bool operator ==(ImageLocation point1, ImageLocation point2)
		{
			return point1.Column == point2.Column && point1.Row == point2.Row;
		}

		public static bool operator !=(ImageLocation point1, ImageLocation point2)
		{
			return !(point1 == point2);
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ImageLocation))
			{
				return false;
			}

			var point = (ImageLocation)obj;
			return ((point.Row == Row) && (point.Column == Column));
		}

		public override int GetHashCode()
		{
			return (Row ^ Column) + Index;
		}


	}
}