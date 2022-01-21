create table NewsLetterDetails(
ID int primary key identity(1,1) not null,
Email varchar(60) not null,
unique(Email)
);