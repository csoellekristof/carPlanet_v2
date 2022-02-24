create table users(
	user_id int unsigned not null auto_increment,
	password varchar(300) not null,
	email varchar(150) null,
	birthdate date null,
	gender int null,
	constraint user_id_PK primary key(user_id)
	);

insert into users values(null, "Kristof",sha2("Hallo123!", 512), "kristof@gmail.com","2002-12-31",0);
insert into users values(null, "Johannes",sha2("Hallo123!", 512), "johannes@gmail.com","2004-08-04",0);
insert into users values(null, "Gabi",sha2("sadas", 512), "gabi@gmail.com","2003-11-11",1);