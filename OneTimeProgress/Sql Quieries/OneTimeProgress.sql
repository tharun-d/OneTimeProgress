create database OneTimeProgress
go
create table LoginDetails(
email varchar(max),
secretPassword varchar(max),
userName varchar(max),
userType varchar(max),
UserDepartment varchar(max)
)
go
------------------
drop table LoginDetails
delete from LoginDetails
select * from logindetails
---------------------
create procedure InsertIntoLoginDetails
(
@email varchar(max),
@secretPassword varchar(max),
@userName varchar(max),
@userType varchar(max),
@UserDepartment varchar(max)
)
as
begin
insert into LoginDetails values(@email,@secretPassword,@userName,@userType,@UserDepartment)
end
-----------------------
insert into LoginDetails values('tharun@gmail.com','tharun','Tharun','Staff','Ramp')
insert into LoginDetails values('Athanu@gmail.com','athanu','Athanu','Staff','Gate')
insert into LoginDetails values('ajay@gmail.com','ajay','Ajay','Super Visor','Ramp')
insert into LoginDetails values('vijay@gmail.com','vijay','Vijay','Super Visor','Gate')
insert into LoginDetails values('arjun@gmail.com','arjun','Arjun','Manager','Manager')
------------------------------------------------
go
alter procedure LoginValidator(
@userName varchar(max),
@password varchar(max)
) as
begin
select userName,userType,UserDepartment from LoginDetails
where Email=@userName and secretPassword=@password
end		
-----------------------------------------------
LoginValidator 'tharun@gmail.com','tharun'
go
create table AllFlightDetails
(
equipmentName varchar(max),
flightNumber varchar(max),
airCraftModel varchar(max),
currentStation varchar(max),
bayNumber int,
taskStartTime datetime,
departure dateTime
)
---------------------------------------------
drop table AllFlightDetails
-----------------------------------------------------
go
insert into AllFlightDetails values('Flight0',1000,'Airbus A320-600','MAA',5,'9:00','11:00')
insert into AllFlightDetails values('Flight1',1001,'Airbus A340-600','MAA',2,'12:00','13:00')
insert into AllFlightDetails values('Flight2',1002,'Airbus A320-100/200','MAA',5,'13:00','14:20')
insert into AllFlightDetails values('Flight3',1003,'Airbus A340-600','MAA',4,'14:00','15:30')
insert into AllFlightDetails values('Flight4',1004,'Airbus A320-100/200','MAA',8,'16:00','17:00')
---------------------------
create procedure InsertIntoAllFlightDetails(@equipmentName varchar(max),@flightNumber varchar(max),@airCraftModel varchar(max),@currentStation varchar(max),@bayNumber int,@taskStartTime datetime,@departureTime datetime)as
begin
insert into AllFlightDetails values(@equipmentName,@flightNumber,@airCraftModel,@currentStation,@bayNumber,@taskStartTime,@departureTime)
end
---------------------------
select * from AllFlightDetails
delete from AllFlightDetails
go
alter procedure GetAllFlightsDetails as
begin
select flightNumber,airCraftModel,currentStation,bayNumber,taskStartTime,departure from AllFlightDetails
end
--------------------------------
go
create table TaskList
(
id int identity(1,1),
flightNumber varchar(max),
taskDetail varchar(max),
duration int,
startTime datetime,
endTime datetime,
statusOfTask varchar(max),
actualStartTime datetime,
actualEndTime datetime,
timeDifference int,
department varchar(max),
staffName varchar(max)
)
----------------------------------
drop table TaskList
---------------------
create procedure InsertIntoTaskList
(
@flightNumber varchar(max),
@taskDetail varchar(max),
@duration int,
@startTime datetime,
@endTime datetime,
@statusOfTask varchar(max),
@actualStartTime datetime,
@actualEndTime datetime,
@timeDifference int,
@department varchar(max),
@staffName varchar(max)
) as
begin
insert into TaskList values(@flightNumber,@taskDetail,@duration,@startTime,@endTime,@statusOfTask,@actualStartTime,@actualEndTime,@timeDifference,@department,@staffName)
end
------------------------------
drop procedure InsertIntoTaskList
---------
alter procedure GetTasksForParticularFlight(@flightNumber varchar(max),@staffName varchar(max),@staffDepartment varchar(max))
as
begin
select Id,taskDetail,duration,startTime,endTime,statusOfTask,actualStartTime,actualEndTime,timedifference from TaskList
where flightNumber=@flightNumber and staffName=@staffName and department=@staffDepartment
order by startTime
end
----------------
drop procedure GetTasksForParticularFlight '1002'
----
alter procedure GetTasksForParticularFlightDepartmentWise(@flightNumber varchar(max),@superVisorDepartment varchar(max))
as
begin
select Id,taskDetail,duration,startTime,endTime,statusOfTask,actualStartTime,actualEndTime,timedifference,staffName from TaskList
where flightNumber=@flightNumber and department=@superVisorDepartment
order by startTime
end
----------------
drop procedure GetTasksForParticularFlightDepartmentWise '1002'
----
create procedure GetDetailsForOneFlight(@flightNumber varchar(max))
as
begin
select flightNumber,airCraftModel,currentStation,bayNumber,taskStartTime,departure from AllFlightDetails
where flightNumber=@flightNumber
end
---------------
alter procedure GetStatusOfAllTasks(@flightNumber varchar(max))
as
begin
select taskDetail,duration,startTime,EndTime,actualStartTime,actualEndTime,timedifference,statusOfTask from TaskList
where flightNumber=@flightNumber and userName=@staffName and userDepartment=@staffDepartment
order by startTime
end
---------------------
create procedure UpdateTaskStartTime
(
@flightNumber varchar(max),
@id varchar(max),
@statusUpdate varchar(max),
@currentTime datetime
) as 
begin
update TaskList
set statusOfTask=@statusUpdate,actualStartTime=@currentTime
where flightNumber=@flightNumber and id=@id
end
---------------------
drop procedure UpdateTaskStartTime
---------------------------
create procedure UpdateTaskEndTime
(
@flightNumber varchar(max),
@id varchar(max),
@statusUpdate varchar(max),
@currentTime datetime,
@timeDifference int
) as 
begin
update TaskList
set statusOfTask=@statusUpdate,actualEndTime=@currentTime,timeDifference=@timeDifference
where flightNumber=@flightNumber and id=@id
end
---------------------
drop procedure UpdateTaskEndTime
---------------------------
alter procedure GettingStartTime(@flightnumber varchar(max),@id int) as
begin
select starttime from TaskList
where flightNumber=@flightnumber and id=@id
end
---------
drop procedure GettingStartTime
-----------------------------
create procedure GettingEndTime(@flightnumber varchar(max),@id int) as
begin
select Endtime from TaskList
where flightNumber=@flightnumber and id=@id
end
---------
drop procedure GettingEndTime
-----------------------------
create procedure GettingActualStartTime(@flightnumber varchar(max),@id int) as
begin
select actualStartTime from TaskList
where flightNumber=@flightnumber and id=@id
end
---------
drop procedure GettingActualStartTime
-----------------------------
delete from tasklist
select timedifference from tasklist where flightNumber='1001'
GetTasksForParticularFlight '1001'
UpdateTaskEndTime '1001','6','ggg','sd'
--------------------------------------------------------------------------------------------------------------------
select taskdetail,duration,startTime,endTime,actualStartTime,actualEndTime,DateDiff(MINUTE,actualEndTime,actualStartTime) 
as diff from tasklist
---------------------------------------------------------------------------------
update tasklist
set timeDifference = 30
where taskDetail='CABIN APPEARANCE' and flightNumber='1001'
----------------------------------------------------------------------
create table Departments
(
flightNumber varchar(max),
departmentName varchar(max),
superVisorName varchar(max),
sheduledStartTime datetime,
sheduledEndTime datetime,
sheduledDuration int,
actualStartTime datetime,
actualEndTime datetime,
statusOfDepartment varchar(max)
)
---
drop table Departments
--------------
select * from departments
delete from departments
--------------
alter procedure InsertIntoDepartments
(
@flightNumber varchar(max),
@departmentName varchar(max),
@superVisorName varchar(max),
@sheduledStartTime datetime,
@sheduledEndTime datetime,
@sheduledDuration int,
@actualStartTime datetime,
@actualEndTime datetime,
@statusOfDepartment varchar(max)
) as 
begin
insert into Departments values
(
@flightNumber,
@departmentName,
@superVisorName,
@sheduledStartTime,
@sheduledEndTime,
@sheduledDuration,
@actualStartTime,
@actualEndTime,
@statusOfDepartment
)
end
-------------
alter procedure GettingAllDepartmentsStatus(@flightNumber varchar(max))
as
begin
select departmentName,superVisorName,sheduledStartTime,sheduledEndTime,sheduledDuration,actualStartTime,actualEndTime,statusOfDepartment from Departments
where flightNumber=@flightNumber
end
----------------------
select * from tasklist
delete from tasklist
select * from AllFlightDetails
delete from AllFlightDetails
select * from LoginDetails
delete from LoginDetails
select * from Departments
delete from Departments