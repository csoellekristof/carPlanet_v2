create table users(
	user_id int unsigned not null auto_increment,
	password varchar(300) not null,
	email varchar(150) null,
	birthdate date null,
	gender int null,
	constraint user_id_PK primary key(user_id)
	);

insert into users values(null,sha2("Hallo123!", 512), "kristof@gmail.com","2002-12-31",0);
insert into users values(null,sha2("", 512), "johannes@gmail.com","2004-08-04",0);
insert into users values(null,sha2("sadas", 512), "gabi@gmail.com","2003-11-11",1);

create table Autos(
	auto_id int unsigned not null auto_increment,
	Beschreibung varchar(300) not null,
	Typ varchar(50) not null,
	Name varchar(50) not null,
	Link varchar(1000) not null,
	constraint auto_id_PK primary key(auto_id)
	);

insert into Autos values(null,"", "gabi@gmail.com","2003-11-11",1);