using System;

namespace lu_ranker
{
	public class User : IComparable < User >
	{
		public string name;
		public int cfRating;
		public int tcRating;
		public int cfColor;
		public int tcColor;
		public double point;

		public User( string name )
		{
			this.name = name;
			this.cfRating = 0;
			this.tcRating = 0;
			this.cfColor = 0;
			this.tcColor = 0;
		}

		public void calcPoints()
		{
			point = ( cfRating / 1500.0 ) * 50.0 + ( tcRating / 1200.0 ) * 50.0 ;
		}

		public int CompareTo( User other )
		{
			return other.point.CompareTo( this.point ); 
		}


	}
}