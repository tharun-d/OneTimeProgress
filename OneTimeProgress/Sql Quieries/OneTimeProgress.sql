create database OneTimeProgress
go
create table LoginDetails(
userName varchar(max),
secretPassword varchar(max),
userType varchar(max)
)
go

drop table LoginDetails
insert into LoginDetails values('tharundintakurthi@gmail.com','tharun','staff')
------------------------------------------------
create procedure LoginValidator(
@userName varchar(max),
@password varchar(max)
) as
begin
select * from LoginDetails
where userName=@userName and secretPassword=@password
end
-----------------------------------------------
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
insert into AllFlightDetails values('Flight1',1001,'Airbus A340-600','MAA',2,'12:00','13:00')
insert into AllFlightDetails values('Flight2',1002,'Airbus A320-100/200','MAA',5,'13:00','14:20')
insert into AllFlightDetails values('Flight3',1003,'Airbus A340-600','MAA',4,'14:00','15:30')
insert into AllFlightDetails values('Flight4',1004,'Airbus A320-100/200','MAA',8,'16:00','17:00')
---------------------------
create procedure GetAllFlightsDetails as
begin
select flightNumber,airCraftModel,currentStation,bayNumber,taskStartTime,departure from AllFlightDetails
ORDER BY taskStartTime
end
--------------------------------
create table TaskList
(
flightNumber varchar(max),
taskDetail varchar(max),
duration int,
startTime int
)
----------------------------------
drop table TaskList
---------------------
create procedure InsertIntoTaskList
(
@flightNumber varchar(max),
@taskDetail varchar(max),
@duration varchar(max),
@startTime varchar(max)
) as
begin
insert into TaskList values(@flightNumber,@taskDetail,@duration,@startTime)
end
------------------------------
drop procedure InsertIntoTaskList
---------
alter procedure GetTasksForParticularFlight(@flightNumber varchar(max))
as
begin
select taskDetail,duration,startTime from TaskList
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
-----
select * from tasklist where flightNumber='1001'
GetTasksForParticularFlight '1001'
