use mydb;

DROP TABLE IF EXISTS level, leveldetail, resourcedetail;
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

DELIMITER $$

DROP PROCEDURE IF EXISTS CreateLevelInfo$$
CREATE PROCEDURE CreateLevelInfo(
      --   IN id_array VARCHAR(1000), OUT outputsql VARCHAR(1000)
      IN id_array VARCHAR(1000)
    )
    READS SQL DATA
    SQL SECURITY INVOKER
BEGIN
    DECLARE id_array_local VARCHAR(1000);
    DECLARE start_pos SMALLINT;
    DECLARE comma_pos SMALLINT;
    DECLARE current_id VARCHAR(1000);
    DECLARE end_loop TINYINT;

   --  DECLARE @finalsql VARCHAR(1000);
    DECLARE startofsql VARCHAR(100);
    DECLARE midofsql VARCHAR(100);
    DECLARE endofsql VARCHAR(100);
    DECLARE selectofsql VARCHAR(1000);
    DECLARE joinofsql VARCHAR(1000);
    DECLARE loopindex SMALLINT;
    DECLARE loopindex2 SMALLINT;
    
    SET @finalsql = '';
    SET startofsql = 'create table levelinfo as select ';
    SET midofsql = ' FROM leveldetail AS t1 ';
    SET endofsql = ' WHERE t1.levelid = 1';
    SET selectofsql = '';
    SET joinofsql = '';
    SET loopindex = 1;
    SET loopindex2 = 0;

    SET id_array_local = id_array;
    SET start_pos = 1;
    SET comma_pos = locate(',', id_array_local);
    REPEAT
        IF comma_pos > 0 THEN
            SET current_id = substring(id_array_local, start_pos, comma_pos - start_pos);
            SET end_loop = 0;
        ELSE
            SET current_id = substring(id_array_local, start_pos);
            SET end_loop = 1;
        END IF;
        
        SET selectofsql = CONCAT(selectofsql,' t',loopindex,'.leveldetailname AS ',current_id,',');
        IF loopindex > 1 THEN
            SET loopindex2 = loopindex-1;
        	SET joinofsql = CONCAT(joinofsql,' LEFT JOIN leveldetail AS t',loopindex,' ON t',loopindex,'.superleveldetailid = t',loopindex2,'.leveldetailid ');
        END IF;     
        # Place here your code that uses current_id
       
        IF end_loop = 0 THEN
            SET id_array_local = substring(id_array_local, comma_pos + 1);
            SET comma_pos = locate(',', id_array_local);
        END IF;
        SET loopindex = loopindex + 1;
    UNTIL end_loop = 1
    END REPEAT;
    IF end_loop = 1 THEN
       SET loopindex = loopindex - 1;
    END IF;
    SET selectofsql = CONCAT(selectofsql,' t',loopindex,'.leveldetailid');
    SET @finalsql = CONCAT(startofsql,selectofsql,midofsql,joinofsql,endofsql); 
   --  SET outputsql = @finalsql;
   --  select @outputsql;
   PREPARE stmt FROM @finalsql;
   EXECUTE stmt;
   DEALLOCATE PREPARE stmt;
END$$

DELIMITER ;



CREATE USER 'aksuser'@'10.0.0.%' IDENTIFIED BY 'password123';
GRANT ALL PRIVILEGES ON mydb.* TO 'aksuser'@'10.0.0.%';