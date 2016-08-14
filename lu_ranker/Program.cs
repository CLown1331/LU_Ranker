using System;
using System.Net;
using System.IO;
using System.Collections.Generic;

namespace lu_ranker
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			StreamReader read = new StreamReader( "..\\..\\input.txt" );
			StreamWriter file = new StreamWriter( "..\\..\\..\\index.html" );

			List < User > UserList = new List < User >();

			string sURL = "http://codeforces.com/api/user.rating?handle=";
			string sLine = "";
			WebRequest wrGETURL;
			Stream objStream;
			StreamReader objReader;
			int rating, len;

			while( ( sLine = read.ReadLine() ) != null )
			{
				UserList.Add( new User( sLine ) ); 
			}

			//UserList.Add( new User( "CLown1331" ) );  
			//UserList.Add( new User( "DarkknightRHZ" ) );
			//UserList.Add( new User( "Dipta_Paul" ) );
			//UserList.Add( new User( "Ahmed_Maruf" ) );
			//UserList.Add( new User( "Sabbir345" ) );
		
			for( int i=0; i<UserList.Count; i++ )
			{
				wrGETURL = WebRequest.Create( sURL + UserList[i].name );
				objStream = wrGETURL.GetResponse().GetResponseStream();
				objReader = new StreamReader( objStream );

				sLine = objReader.ReadLine();

				len = sLine.Length;
				rating = 0;

				for( int k = len - 4, mul = 1; k >= len - 7; k-- ) 
				{
					if( sLine [k] < '0' || sLine [k] > '9' ) break;
					rating += ( sLine[k] - '0' ) * mul;
					mul *= 10;
				}

				UserList[i].cfRating = rating;

				UserList[i].calcPoints();

				//Console.WriteLine( "{0} : {1}", UserList[i].name, UserList[i].cfRating ); 
			}

			UserList.Sort(); 

			file.Write( "<!DOCTYPE html>\n<html>\n\t<head>\n\t\t<title> LU Ranklist </title>\n\t</head>\n\t<body bgcolor=\"#fffbdc\">" );

			for (int i = 0; i < UserList.Count; i++) 
			{
				file.WriteLine( "<a href=\"http:www.codeforces.com/profile/{0}\" style=\"text-decoration:none\"><font color=\"{1}\">{2}</font></a> : {3}<br>", UserList[i].name, getColor( UserList[i].cfRating ), UserList[i].name, UserList[i].cfRating ); 
			} 
				
			//Console.ReadLine(); 
			file.WriteLine( "\t</body>\n</html>" ); 
			file.Close();

			read.Close ();
		}

		public static string getColor( int rating )
		{
			if( rating < 1200 ) return "d3d1c2";
			if( rating >= 1200 && rating < 1400 ) return "008000";
			if( rating >= 1400 && rating < 1600 ) return "00cccc";
			if( rating >= 1600 && rating < 1900 ) return "0000FF";
			if( rating >= 1900 && rating < 2200 ) return "ff33cc";
			if( rating >= 2200 && rating < 2600 ) return "ff1a1a";
			if( rating >= 2600 && rating < 2900 ) return "e60000";
			if( rating >= 2900 ) return "800000";
			return "d3d1c2";
		}
	}
}
