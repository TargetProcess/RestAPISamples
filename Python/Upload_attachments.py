import requests
url = 'https://account_name.tpondemand.com/UploadFile.ashx'
filename='file.xxx'
f = open (filename, 'rb')
token = 'xxxxxxxxxxxxxxx'
params = {'access_token':token}
data = {'generalid':'453'}
files = {'file':(filename, f, "multipart/form-data")}
r = requests.post(url, data = data, params = params, files = files)
print(r.text)
print(r.headers)
