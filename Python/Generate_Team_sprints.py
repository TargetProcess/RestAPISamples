import requests
import datetime

#needs to be modified in order to be used by specific teams. Also please update dates and number of sprints to be generated below
access_token="xxxxxxxxxxx"
team_id=123
team_name="My Team"

start_date = "01-04-17"
start_datetime = datetime.datetime.strptime(start_date,"%m-%d-%y")
end_date = "01-17-17"
end_datetime = datetime.datetime.strptime(end_date,"%m-%d-%y")

sprint_number = 1
while sprint_number <= 26:
    url = "https://mytp.tpondemand.com/api/v1/TeamIterations/?access_token={0}".format(access_token)
    iteration_name = "Sprint {0}-{1}".format(sprint_number,start_date.replace("-",""))
    payload_iteration_name = "<TeamIteration Name=\"{0}\">\n\t".format(iteration_name)
    payload_start = "<StartDate>{0}T00:00:00</StartDate>\n\t".format(start_datetime.strftime("%Y-%m-%d"))
    payload_end = "<EndDate>{0}T23:59:59</EndDate>\n\t".format(end_datetime.strftime("%Y-%m-%d"))
    payload_team = "<Team ResourceType=\"Team\" Id=\"{0}\" Name=\"{1}\"/>\n</TeamIteration>".format(team_id,team_name)
    payload = payload_iteration_name + payload_start + payload_end + payload_team
    
    headers = {
        'content-type': "application/xml",
        'cache-control': "no-cache",
      		}
    start_datetime = start_datetime + datetime.timedelta(days=14)
    start_date = start_datetime.strftime("%m-%d-%y")
    end_datetime = end_datetime + datetime.timedelta(days=14)
    end_date = end_datetime.strftime("$m-$d-%y")
    response = requests.request("POST", url, data=payload, headers=headers)
    sprint_number = sprint_number + 1
    print(response.text)