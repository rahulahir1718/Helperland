create table Feedback(
CustomerID int foreign key references Users(UserID) not null,
ServiceProviderID int foreign key references Users(UserID) not null,
OnTimeArrival int not null,
Friendly int not null,
Quality int not null,
Feedback varchar(100),
Rating decimal(2,2) not null
);