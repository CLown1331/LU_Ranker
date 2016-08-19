<?php 
	$cache = "cache/index.html";
	$cachetime = 3600 * 5;
	if( file_exists( $cache ) && ( time() - $cachetime ) < filemtime( $cache ) ) {
		include_once( $cache );
		exit();
	}
	
	ob_start(); 
?>
<!DOCTYPE html>
<html lang="en">
	<head>
	  <title> LU_Ranklist </title>
	  <meta charset="utf-8">
	  <meta name="viewport" content="width=device-width, initial-scale=1">
	  <link rel="stylesheet" href="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/css/bootstrap.min.css">
	  <script src="https://ajax.googleapis.com/ajax/libs/jquery/1.12.4/jquery.min.js"></script>
	  <script src="http://maxcdn.bootstrapcdn.com/bootstrap/3.3.7/js/bootstrap.min.js"></script>
	</head>
	<body>
	<div class="container">
	<?php	
		set_time_limit( 0 );
		include_once( 'user.php' );
		
		$file = fopen( "input.txt", "r" );
		
		while( $handles = fscanf( $file, "%s %s" ) ) {
			list( $cfName, $tcName ) = $handles;
			$userList[] = new User( $cfName, $tcName );
		}
		
		//print_r( $userList );
				
		foreach( $userList as $user ) {
			$user->cfRating = get_cf_rating( $user->cfName );
			$user->tcRating = get_tc_rating( $user->tcName );
			$user->cfColor = get_cf_color( $user->cfRating );
			$user->tcColor = get_tc_color( $user->tcRating );
			$user->points = get_points( $user->cfRating, $user->tcRating );
			$user->points = number_format( $user->points, 2 );
			usleep( 200000 );
		}
			
		usort( $userList, 'cmp' );
		
		echo "\t<table class=\"table\">\n";
		echo "\t\t<thead>\n";
		echo "\t\t<tr>\n";
		echo "\t\t\t<th> Rank <th>\n";
		echo "\t\t\t<th> Codeforces Handle <th>\n";
		echo "\t\t\t<th> Codeforces Rating <th>\n";
		echo "\t\t\t<th> TopCoder Handle <th>\n";
		echo "\t\t\t<th> TopCoder Rating <th>\n";
		echo "\t\t\t<th> Points <th>\n";
		echo "\t\t</tr>\n";
		echo "\t\t</thead>\n";
		echo "\t\t<tbody\n";
		$prev = 0;
		$rank = 0;
		foreach( $userList as $user ) {
			if( $prev != $user->points ) $rank++;
			$prev = $user->points;
			echo "\t\t<tr>\n";
			echo "\t\t\t<td> $rank <td>\n";
			echo "\t\t\t<td> <a href=\"http://www.codeforces.com/profile/$user->cfName\" style=\"text-decoration:none\" > <font color=\"$user->cfColor\">  $user->cfName </font> </a> <td>\n";
			echo "\t\t\t<td> $user->cfRating <td>\n";
			echo "\t\t\t<td> <a href=\"https://www.topcoder.com/members/$user->tcName\" style=\"text-decoration:none\" > <font color=\"$user->tcColor\">  $user->tcName </font> </a> <td>\n";
			echo "\t\t\t<td> $user->tcRating <td>\n";
			echo "\t\t\t<td> $user->points <td>\n";
			echo "\t\t</tr>\n";
		}
		
		echo "\t\t<tbody\n";
		echo "\t\t</table>";
		echo "\n";
		fclose( $file );
		
		function get_cf_rating( $handle ) {
			if( $handle == "null" ) return 0;
			$cfUrl = "http://codeforces.com/api/user.rating?handle=";
			$data = file_get_contents( $cfUrl . $handle );
			$json = json_decode( $data );
			$latest = array_pop( $json->result );
			if( !$latest ) return 0;
			$rating = $latest->newRating;
			$last_contest_date = $latest->ratingUpdateTimeSeconds;
			$value_date = mktime(0,0,0,date('m')-1,date('d'),date('Y'));
			if( $last_contest_date < $value_date ) $rating = 0;
			return $rating;
		}
		
		function get_tc_rating( $handle ) {
			if( $handle == "null" ) return 0;
			$tcUrl = str_replace('{handle}', $handle, 'https://api.topcoder.com/v2/users/{handle}/statistics/data/srm');
			$data = file_get_contents( $tcUrl );
			$json = json_decode( $data );
			$rating = $json->rating;
			$last_contest_date = strtotime( $json->mostRecentEventDate );
			$value_date = mktime(0,0,0,date('m')-1,date('d'),date('Y'));
			if( $last_contest_date < $value_date ) $rating = 0;
			return $rating;
		}
		
		function get_cf_color( $rating ) {
			if( !$rating || $rating < 1200 ) return "d3d1c2";
			if( $rating >= 1200 && $rating < 1400 ) return "008000";
			if( $rating >= 1400 && $rating < 1600 ) return "00cccc";
			if( $rating >= 1600 && $rating < 1900 ) return "0000FF";
			if( $rating >= 1900 && $rating< 2200 ) return "ff33cc";
			if( $rating >= 2200 && $rating < 2400 ) return "FFA500";
			if( $rating >= 2200 && $rating < 2400 ) return "ff1a1a";
			if( $rating >= 2600 && $rating < 2900 ) return "e60000";
			if( $rating >= 2900 ) return "800000";
			return "d3d1c2";
		}
		
		function get_tc_color( $rating ) {
			if( !$rating || $rating < 900 ) return "d3d1c2";
			if( $rating >= 900 && $rating < 1200 ) return "008000";
			if( $rating >= 1200 && $rating < 1500 ) return "0000FF";
			if( $rating >= 1500 && $rating < 2200 ) return "FFFF00";
			if( $rating >= 2200 && $rating < 2600 ) return "ff1a1a";
			if( $rating >= 2600 && $rating < 2900 ) return "e60000";
			if( $rating >= 2900 ) return "800000";
			return "d3d1c2";
		}
		
		function get_points( $cfRating, $tcRating ) {
			return $cfRating / 1500.0 * 50 + $tcRating / 1200.0 * 50;
		}
		
		function cmp( $a, $b ) {
			return $a->points > $b->points ? -1 : 1;
		}
	?>
	</div>
	</body>
</html>

<?php
	$fp = fopen( $cache, "w+" );
	fwrite( $fp, ob_get_contents() );
	fclose( $fp );
	ob_end_flush();
?>