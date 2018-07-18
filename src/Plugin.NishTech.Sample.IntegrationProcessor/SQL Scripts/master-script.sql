Select top 100
'Insert Into Customer Select ' + 
	'Id=' + isnull('''' + convert(varchar(50),Id) + '''','null') + ', ' +
	'AccountNumber=' + isnull('''' + replace(CustomerNumber,'''','''''') + '''','null') + ', ' +
	'AccountStatus=' + isnull('''' + replace('ActiveAccount','''','''''') + '''','null') + ', ' +
	'FirstName=' + isnull('''' + replace(FirstName,'''','''''') + '''','null') + ', ' +
	'LastName=' + isnull('''' + replace(LastName,'''','''''') + '''','null') + ', ' +
	'Email=' + isnull('''' + replace(CustomerNumber + '@email.com','''','''''') + '''','null')  + ', ' +
	'PhoneNumber=' + isnull('''' + replace('999-999-9999','''','''''') + '''','null') + ''
from [Customer] where CustomerSequence=0 and State<>'' and Country<>'' and postalcode<>'' and Country='US' order by CustomerNumber

Select top 100
'Insert Into Address Select ' + 
	'Id=' + isnull('''' + convert(varchar(50),NEWID()) + '''','null') + ', ' +
	'CustomerId=' + isnull('''' + convert(varchar(50),Id) + '''','null') + ', ' +
	'AddressName=' + isnull('''' + replace('Home','''','''''') + '''','null') + ', ' +
	'FirstName=' + isnull('''' + replace(FirstName,'''','''''') + '''','null') + ', ' +
	'LastName=' + isnull('''' + replace(LastName,'''','''''') + '''','null') + ', ' +
	'Address1=' + isnull('''' + replace(Address1,'''','''''') + '''','null') + ', ' +
	'Address2=' + isnull('''' + replace(Address2,'''','''''') + '''','null') + ', ' +
	'City=' + isnull('''' + replace(City,'''','''''') + '''','null') + ', ' +
	'State=' + isnull('''' + replace(State,'''','''''') + '''','null') + ', ' +
	'StateCode=' + isnull('''' + replace(State,'''','''''') + '''','null') + ', ' +
	'Country=' + isnull('''' + replace(Country,'''','''''') + '''','null') + ', ' +
	'CountryCode=' + isnull('''' + replace(Country,'''','''''') + '''','null') + ', ' +
	'ZipCode=' + isnull('''' + replace(PostalCode,'''','''''') + '''','null') + ', ' +
	'Email=' + isnull('''' + replace(CustomerNumber + '@email.com','''','''''') + '''','null') + ', ' +
	'PhoneNumber=' + isnull('''' + replace('555-555-5555','''','''''') + '''','null') + ', ' +
	'IsPrimary=' + isnull(convert(varchar,1),'null') + ', ' +
	'Company=' + isnull('''' + replace('1','''','''''') + '''','null') + ''
from [Customer] where CustomerSequence=0 and State<>'' and Country<>'' and postalcode<>'' and Country='US' order by CustomerNumber