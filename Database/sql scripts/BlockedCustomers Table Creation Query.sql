create table BlockedCustomers(
ServiceProviderID int foreign key references Users(UserID) not null,
CustomerID int foreign key references Users(UserID) not null
);