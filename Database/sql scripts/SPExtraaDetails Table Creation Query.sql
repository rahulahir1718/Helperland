create table SPExtraDetails(
UserID int foreign key references Users(UserID) not null,
Nationality varchar(30),
Gender varchar(20) not null,
AccountStatus BIT not null,
Avtar int not null
);