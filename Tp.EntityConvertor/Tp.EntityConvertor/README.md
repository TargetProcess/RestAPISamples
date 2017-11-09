**Tp.EntityConvertor**

Simple bulk convertor from one general entity type to another.

Usage: `Tp.EntityConvertor.exe args`

where `args`:
* -u, --instance_url      Base url to the Targetprocess instance where generals are converted.
* -t, --access_token      Access token for access to Targetprocess instance. See Targetprocess / My profile / Access Tokens.
* -time, --timeout        Time to wait for conversion to complete (in HH:mm:ss[.FFFF] format).
* -ekid, --entity_kind_id Entity kind id to convert general to.
* -ids, --general_ids     Separated by comma array of general's ids to convert.

Samples:

*Tp.EntityConvertor.exe -u https://md5.tpondemand.com -t MSAd1j2s31a54dk567fkidsa5iksa5l67asdfll56f7asdvcrfsadfla567sdfsakl576df5asd5fmaa== -time 0:5:30 -ekid 43 -ids 1599,1600,1683*

*Tp.EntityConvertor.exe --instance_url https://md5.tpondemand.com --access_token MSAd1j2s31a54dk567fkidsa5iksa5l67asdfll56f7asdvcrfsadfla567sdfsakl576df5asd5fmaa== --timeout 0:5:30 --entity_kind_id 47 --general_ids 123,451,898*

Some Entity Kind Ids (ekid, entity_kind_id):

* UserStory  - 43
* Task       - 45
* Request    - 46
* Feature    - 47
* Bug        - 48
* Release    - 51
* Project    - 52
* Program    - 53
* Iteration  - 54
* Impediment - 55
* Epic       - 87
