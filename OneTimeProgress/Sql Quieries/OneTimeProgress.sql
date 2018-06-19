create database OneTimeProgress
go
create table LoginDetails(
userName varchar(max),
secretPassword varchar(max),
userType varchar(max)
)
go
------------------
drop table LoginDetails
---------------------
insert into LoginDetails values('tharundintakurthi@gmail.com','tharun','Staff')
insert into LoginDetails values('ajaydintakurthi@gmail.com','ajay','Super Visor')
------------------------------------------------
go
alter procedure LoginValidator(
@userName varchar(max),
@password varchar(max)
) as
begin
select userType from LoginDetails
where userName=@userName and secretPassword=@password
end
-----------------------------------------------
go
create table AllFlightDetails
(
equipmentName varchar(max),
flightNumber int,
airCraftModel varchar(max),
currentStation varchar(max),
bayNumber int,
taskStartTime varchar(max),
departure varchar(max),
)
---------------------------------------------
drop table AllFlightDetails
-----------------------------------------------------
go
insert into AllFlightDetails values('Flight1',1001,'Airbus A340-600','MAA',2,'12:00','13:00')
insert into AllFlightDetails values('Flight2',1002,'Airbus A320-100/200','MAA',5,'13:00','14:20')
insert into AllFlightDetails values('Flight3',1003,'Airbus A340-600','MAA',4,'14:00','15:30')
insert into AllFlightDetails values('Flight4',1004,'Airbus A320-100/200','MAA',8,'16:00','17:00')
---------------------------
go
create procedure GetAllFlightsDetails as
begin
select flightNumber,airCraftModel,currentStation,bayNumber,taskStartTime,departure from AllFlightDetails
ORDER BY taskStartTime
end
--------------------------------
go
create table TaskList
(
id int identity(1,1),
flightNumber varchar(max),
taskDetail varchar(max),
duration int,
startTime int,
statusOfTask varchar(max)
)
----------------------------------
drop table TaskList
---------------------
create procedure InsertIntoTaskList
(
@flightNumber varchar(max),
@taskDetail varchar(max),
@duration varchar(max),
@startTime varchar(max),
@statusOfTask varchar(max)
) as
begin
insert into TaskList values(@flightNumber,@taskDetail,@duration,@startTime,@statusOfTask)
end
------------------------------
drop procedure InsertIntoTaskList
---------
alter procedure GetTasksForParticularFlight(@flightNumber varchar(max))
as
begin
select Id,taskDetail,duration,startTime,statusOfTask from TaskList
where flightNumber=@flightNumber
order by startTime desc
end
----------------
create procedure GetDetailsForOneFlight(@flightNumber varchar(max))
as
begin
select flightNumber,airCraftModel,currentStation,bayNumber,taskStartTime,departure from AllFlightDetails
where flightNumber=@flightNumber
end
---------------
create procedure GetStatusOfAllTasks(@flightNumber varchar(max))
as
begin
select taskDetail,statusOfTask from TaskList
where flightNumber=@flightNumber
order by startTime desc
end
---------------------
alter procedure UpdateTaskStatus
(
@flightNumber varchar(max),
@id varchar(max),
@statusUpdate varchar(max)
) as 
begin
update TaskList
set statusOfTask=@statusUpdate
where flightNumber=@flightNumber and id=@id
end
---------------------
select * from tasklist where flightNumber='1001'
GetTasksForParticularFlight '1001'
UpdateTaskStatus '1001','4','hhhd'
