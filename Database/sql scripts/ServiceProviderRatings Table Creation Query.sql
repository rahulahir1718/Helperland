create table ServiceProviderRatings(
ServiceProviderID int foreign key references Users(UserID) not null,
ratings decimal(2,2) not null
);