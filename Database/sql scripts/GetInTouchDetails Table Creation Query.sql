create table GetInTouchDetials(
ID int primary key identity(1,1) not null,
FirstName varchar(40) not null,
LastName varchar(40) not null,
MobileNumber char(10),
Email varchar(60) not null,
UserSubject varchar(30) not null,
UserMessage varchar(200) not null,
unique(Email)
);