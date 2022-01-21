create table CustomerLanguage(
UserLanguage varchar(40) not null,
UserID int foreign key references Users(UserID) not null
);