# -*- coding: utf-8 -*-
import requests
url = 'https://restapi.tpondemand.com/UploadFile.ashx'
filename = '/Users/targetprocess/Downloads/qwe.png'
f = open (filename, 'rb')
token = 'NjplaXdQeTJDOHVITFBta0QyME85QlhEOWZ0NKWG1RPQ=='
params = {'access_token':token}
data = {'generalid':'34'}
files = {'file':('qwe.png', f, "multipart/form-data")}
r = requests.post(url, params = params, files = files, data = data)
print(r.text)
print(data)
print(r.headers)