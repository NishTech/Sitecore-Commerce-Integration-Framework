select Id from [dbo].[CommerceEntities] where Id like '%job%'

select * from [dbo].[CommerceLists] where CommerceEntityId in (select Id from [dbo].[CommerceEntities] where Id like '%job%')

select * from [dbo].[CommerceLists] where ListName like '%fail%'

delete from [dbo].[CommerceLists] where CommerceEntityId in (select Id from [dbo].[CommerceEntities] where Id like '%job%')
delete from [dbo].[CommerceEntities] where Id like '%job%'

select * from CommerceEntities where Id like '%email.com%'
select * from CommerceEntities where Id like '%Entity-Customer%'
select * from [dbo].[CommerceLists] where CommerceEntityId like '%Entity-Customer%'

delete from [dbo].[CommerceLists] where CommerceEntityId like '%Entity-Customer%'
delete from CommerceEntities where Id like '%Entity-Customer%'
delete from CommerceEntities where Id like '%email.com%'

