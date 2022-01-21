create table ServiceDetails(
ServiceID int primary key identity(1,1) not null,
CustomerID int foreign key references Users(UserID) not null,
ServiceDate date not null,
ServiceTime time not null,
ServiceDuration int not null,
ExtraServices int,
Comments varchar(100),
HasPet bit not null,
AcceptedBy int foreign key references Users(UserID),
Payment decimal(4,2) not null,
ServiceStatus varchar(10) not null
);