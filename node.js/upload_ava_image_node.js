const fs = require('fs');
const request = require('request');
var sizeOf = require('image-size');
var image = 'ava.jpg';
var userId = 1;
var dimensions = sizeOf(image);
var token = 'xxxxxxxxxxxxxxxxxxxxxxxxxxxxxxx';
var size = dimensions.width > dimensions.height ? dimensions.height : dimensions.width;
console.log(dimensions.width, dimensions.height, size);
    const post_image = {
        method: "POST",
        url: `https://account_name.tpondemand.com/avatar.ashx`,
        qs:{'UserID':userId,'token':token},
        headers: {
                    "Content-Type": "multipart/form-data"
        },
        formData : {"image" : fs.createReadStream(image)}};
    const submit_image = {
        method: "GET",
        url: `https://account_name.tpondemand.com/avatar.ashx?source=temp&operation=crop&x=0&y=0&width=${size}&height=${size}`,
        qs:{'UserID':userId,'token':token}
         };
    request(post_image, function (err, res, body) {
        if (res.statusCode==200) {
            request(submit_image, function (err, res, body) {
                if(err) console.log(err);
                console.log(body);
            });
        } if(err) console.log(err);
        console.log(body);
    });
