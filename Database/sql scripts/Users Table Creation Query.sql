create table Users(
UserID int primary key identity(1,1) not null,
FirstName varchar(40) not null,
LastName varchar(40) not null,
Email varchar(60) not null,
MobileNumber char(10) not null,
UserPassword varchar(20) not null,
DOB date not null,
RoleID int foreign key references Roles(RoleID) not null,
unique(Email)
);