<html>
	<head>
		<title> LU_RankList </title>
	</head>
	<?php	
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
			$user->points = get_points( $user->cfRating, $user->tcRating );
		}
			
		usort( $userList, 'cmp' );
		
		foreach( $userList as $user ) {
			echo " $user->cfName: $user->cfRating, $user->tcName: $user->tcRating <br>";
		}
		
		fclose( $file );
		
		function get_cf_rating( $handle ) {
			$cfUrl = "http://codeforces.com/api/user.rating?handle=";
			$data = file_get_contents( $cfUrl . $handle );
			$json = json_decode( $data );
			$latest = array_pop( $json->result );
			if( !$latest ) return 0;
			$rating = $latest->newRating;
			return $rating;
		}
		
		function get_tc_rating( $handle ) {
			return 0;
		}
		
		function get_points( $cfRating, $tcRating ) {
			return $cfRating / 1500.0 * 50 + $tcRating / 1200.0 * 50;
		}
		
		function cmp( $a, $b ) {
			return $a->points > $b->points ? -1 : 1;
		}
	?>
</html>