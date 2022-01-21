create table CancelledServiceDetails(
ServiceID int foreign key references ServiceDetails(ServiceID) not null,
Reason varchar(70) not null
);