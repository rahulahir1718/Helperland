create table FavouriteSP(
CustomerID int foreign key references Users(UserID) not null,
FavouriteSPID int foreign key references Users(UserID) not null
);