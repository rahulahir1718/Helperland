create table AddressDetails(
AddressID int primary key identity(1,1) not null,
UserID int foreign key references Users(UserID) not null,
StreetName varchar(80) not null,
HouseNo int not null,
City varchar(30) not null,
PostalCode char(6) not null
);