use mydb;

create table level(
   levelid INT NOT NULL AUTO_INCREMENT,
   levelname VARCHAR(100) NOT NULL,
   superlevelid INT NOT NULL,
   PRIMARY KEY ( levelid )
);

create table leveldetail(
   leveldetailid INT NOT NULL AUTO_INCREMENT,
   leveldetailname VARCHAR(100) NOT NULL,
   levelid INT NOT NULL,
   superleveldetailid INT NOT NULL,
   PRIMARY KEY ( leveldetailid )
);

create table resourcedetail(
   resourcedetailid INT NOT NULL AUTO_INCREMENT,
   projectname VARCHAR(200) NOT NULL,
   resourcegroupname VARCHAR(200) NOT NULL,
   subscriptionid VARCHAR(50) NOT NULL,
   projectowneremail VARCHAR(100) NOT NULL,
   leveldetailid INT NOT NULL,
   PRIMARY KEY ( resourcedetailid )
);

CREATE USER 'aksuser'@'10.0.0.%' IDENTIFIED BY 'password123';
GRANT ALL PRIVILEGES ON mydb.* TO 'aksuser'@'10.0.0.%';