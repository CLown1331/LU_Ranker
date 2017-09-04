<?php 
	class User{
		public $fName;
		public $lName;
		public $cfName;
		public $tcName;
		public $cfRating;
		public $tcRating;
		public $cfColor;
		public $tcColor;
		public $points;
		
		public function __construct( $fName, $lName, $cfName, $tcName ) {
			$this->fName = $fName;
			$this->lName = $lName;
			$this->cfName = $cfName;
			$this->tcName = $tcName;
		}
	}
?>