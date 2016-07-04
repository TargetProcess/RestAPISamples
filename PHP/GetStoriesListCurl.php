<?php

//Working with user stories in account julia.tpondemand.com
$baseurl = "https://julia.tpondemand.com/api/v1/userstories";

//I want to see all not closed stories from the project #2
$url = $baseurl . "?where=(project.id eq '2')and(entitystate.isfinal eq 'false')&include=[id,name,entitystate]&format=json&take=20";

while ($url != "") {
	$url = str_replace(" ", "%20", $url);
	echo "GET: " . $url . "<br><br>";
	
	//Send request
	$ch = curl_init();
	curl_setopt ($ch, CURLOPT_URL, $url);
	curl_setopt($ch,CURLOPT_HTTPHEADER,array (
		"Accept: application/json",
		"Authorization: Basic YWRtaW46YWRtaW4="
	));
	ob_start();
	curl_exec ($ch);
	curl_close ($ch);
	
	//Read response
	$response = ob_get_contents();
	ob_clean();
	
	//Uncomment the following line to see the raw response
	//echo $response;
	
	//Parse response
	$stories = json_decode($response);
	
	//The maximum amount of items per page is 1000, so we have to check for the Next link
	if(isset($stories->Next)) {
		$url = $stories->Next;
	} else {
		$url = "";
	}
	
	//Reading individuals stories
	echo "Pulled " . count($stories->Items) . "stories.<br><br>";	
	foreach ($stories->Items as $story) {
		echo "Story #" . $story->Id . " " . $story->Name . " (" . $story ->EntityState->Name . ")<br><br>";
	}
}

?>
