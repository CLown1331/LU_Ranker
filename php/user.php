<?php 
	class User{
		public $cfName;
		public $tcName;
		public $cfRating;
		public $tcRating;
		public $points;
		
		public function __construct( $cfName, $tcName ) {
			$this->cfName = $cfName;
			$this->tcName = $tcName;
		}
	}
?>