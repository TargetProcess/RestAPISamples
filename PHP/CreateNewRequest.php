<?php

//Working with requests in account julia.tpondemand.com
$url = "https://julia.tpondemand.com/api/v1/requests";
echo "POST to: " . $url . "<br><br>";

//I want to create a new request
$request = [
	"Project" => ["Id"=>2], //in the project #2
	"Name"=>"Test" //with a name 'Test'
];
$json = json_encode($request);
echo "JSON payload: " . $json . "<br><br>";

//I need the following custom headers
$streamOpts = array(
	"http" => array(
		"method" => "POST",
		"header" => "Content-type: application/json\r\n" .
			"Authorization: Basic YWRtaW46YWRtaW4=",
		"content" => $json
	),
);

//Send the request with my custom headers and read the response
$jsonPosterStreamContext = stream_context_create($streamOpts);
$response = file_get_contents($url, false, $jsonPosterStreamContext);
echo "Response: " . $response;

?>
