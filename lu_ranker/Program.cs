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
			StreamReader read = new StreamReader( "..\\..\\..\\input.txt" );
			StreamWriter file = new StreamWriter( "..\\..\\..\\index.html" );

			List < User > UserList = new List < User >();
			HashSet < double > uniqueRatings = new HashSet < double > ();

			string cfURL = "http://codeforces.com/api/user.rating?handle=";
			string tcURL = "http://api.topcoder.com/v2/users/";
			string sLine = "", tempString;
			string[] words; 
			WebRequest wrGETURL;
			Stream objStream;
			StreamReader objReader;
			int rating, len;

			while( ( sLine = read.ReadLine() ) != null )
			{
				words = sLine.Split( ' ' );
				UserList.Add( new User( words[0], words[1] ) ); 
			}

			//UserList.Add( new User( "CLown1331" ) );  
			//UserList.Add( new User( "DarkknightRHZ" ) );
			//UserList.Add( new User( "Dipta_Paul" ) );
			//UserList.Add( new User( "Ahmed_Maruf" ) );
			//UserList.Add( new User( "Sabbir345" ) );
		
			for( int i=0; i<UserList.Count; i++ )
			{
				wrGETURL = WebRequest.Create( cfURL + UserList[i].cfname );
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

				wrGETURL = WebRequest.Create( tcURL + UserList[i].tcname );
				objStream = wrGETURL.GetResponse().GetResponseStream();
				objReader = new StreamReader( objStream );

				sLine = "";

				while( ( tempString = objReader.ReadLine () ) != null ) 
				{
					sLine += tempString;
				}

				len = sLine.Length;
				rating = 0;
					
				for( int k = sLine.IndexOf( "\"rating\":" ) + 10; k < len; k++ ) 
				{
					if( sLine [k] < '0' || sLine [k] > '9' ) break;
					rating *= 10;
					rating += ( sLine[k] - '0' );
				}

				UserList[i].tcRating = rating;

				UserList[i].calcPoints();

				//Console.WriteLine( "{0} : {1}", UserList[i].name, UserList[i].cfRating ); 
				System.Threading.Thread.Sleep( 100 );
			}

			UserList.Sort(); 

			file.Write( "<!DOCTYPE html>\n<html>\n\t<head>\n\t\t<title> LU Ranklist </title>\n\t\t<style> table, th, td { border: 1px solid black; } </style>\n\t</head>\n\t<body bgcolor=\"#fffbdc\">" );
			file.WriteLine( "<table style=\"width:68%\" align=\"center\">" );
			file.WriteLine ("<tr>");
			file.WriteLine ("<th align=\"center\"> Rank </th>");
			file.WriteLine ("<th align=\"center\"> Codeforces ID </th> "  );
			file.WriteLine ("<th align=\"center\"> Codeforces Rating </th> " );
			file.WriteLine ("<th align=\"center\"> TopCoder ID </th> " );
			file.WriteLine ("<th align=\"center\"> TopCoder Rating </th> " );
			file.WriteLine ("<th align=\"center\"> Points </th> " );
			file.WriteLine ("</tr>" ); 
			for( int i = 0, id = 0, pls = 1; i < UserList.Count; i++, pls++ ) 
			{
				//Console.WriteLine( "CF: {0}, TC: {1}", UserList[i].cfRating, UserList[i].tcRating );
				if( uniqueRatings.Contains( UserList[i].point ) == false) {
					id = pls;
					uniqueRatings.Add( UserList[i].point ); 
				}
				file.WriteLine( "<tr>" );
				file.WriteLine( "<td align=\"center\">{0}</td>,  ", id );
				file.WriteLine( "<td align=\"center\"><a href=\"http:www.codeforces.com/profile/{0}\" style=\"text-decoration:none\"><font color=\"{1}\">{2}</font></a></td> <td align=\"center\"> {3}</td>", UserList[i].cfname, getCFColor( UserList[i].cfRating ), UserList[i].cfname, UserList[i].cfRating ); 
				file.WriteLine( "<td align=\"center\"><a href=\"https://www.topcoder.com/members/{0}\" style=\"text-decoration:none\"><font color=\"{1}\">{2}</font></a></td> <td align=\"center\"> {3}</td>", UserList[i].tcname, getTCColor( UserList[i].tcRating ), UserList[i].tcname, UserList[i].tcRating ); 
				file.WriteLine( "<td align=\"center\"> {0} </td>", UserList[i].point.ToString ("#.##") );
				file.WriteLine( "</tr>" );
			}
				
			//Console.ReadLine(); 
			file.WriteLine( "</table>\t</body>\n</html>" ); 
			file.Close();

			read.Close ();
		}

		public static string getCFColor( int rating )
		{
			if( rating < 1200 ) return "d3d1c2";
			if( rating >= 1200 && rating < 1400 ) return "008000";
			if( rating >= 1400 && rating < 1600 ) return "00cccc";
			if( rating >= 1600 && rating < 1900 ) return "0000FF";
			if( rating >= 1900 && rating < 2200 ) return "ff33cc";
			if( rating >= 2200 && rating < 2400 ) return "FFA500";
			if( rating >= 2200 && rating < 2400 ) return "ff1a1a";
			if( rating >= 2600 && rating < 2900 ) return "e60000";
			if( rating >= 2900 ) return "800000";
			return "d3d1c2";
		}

		public static string getTCColor( int rating )
		{
			if( rating < 900 ) return "d3d1c2";
			if( rating >= 900 && rating < 1200 ) return "008000";
			if( rating >= 1200 && rating < 1500 ) return "0000FF";
			if( rating >= 1500 && rating < 2200 ) return "FFFF00";
			if( rating >= 2200 && rating < 2600 ) return "ff1a1a";
			if( rating >= 2600 && rating < 2900 ) return "e60000";
			if( rating >= 2900 ) return "800000";
			return "d3d1c2";
		}
	}
}
