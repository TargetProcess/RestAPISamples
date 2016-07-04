<?php

//Working with requests in account julia.tpondemand.com
$baseurl = "https://julia.tpondemand.com/api/v1/requests";

//I want to see all not closed requests from the project #578
$filters = array(
	"project.id eq '578'",
	"entitystate.isfinal eq 'false'"
);

//Encode the filters
foreach ($filters as $key => $filter) {
    $filters[$key] = "(" . rawurlencode($filter) . ")";
}

//Add filters and other parameters to the base URL
$url = $baseurl . "?where=" . implode("and", $filters) . "&include=[id,name,entitystate,owner]&take=1000";
echo "GET: " . $url . "<br><br>";

//I need the following custom headers
$streamOpts = array(
	"http" => array(
		"method" => "GET",
		"header" => "Authorization: Basic YWRtaW46YWRtaW4="
	),
);

//Send the request with my custom headers and read the response
$response = file_get_contents($url, false, stream_context_create($streamOpts));
echo "Response: " . $response;

?>
