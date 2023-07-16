/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `device` (
  `user_uuid` char(36) NOT NULL,
  `device_uuid` char(36) NOT NULL DEFAULT '0',
  `ipv4_address` varchar(15) NOT NULL DEFAULT '0',
  `name` varchar(45) NOT NULL,
  `created` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `public_key` char(44) NOT NULL,
  PRIMARY KEY (`user_uuid`,`device_uuid`),
  UNIQUE KEY `public_key_UNIQUE` (`public_key`),
  UNIQUE KEY `device_uuid_UNIQUE` (`device_uuid`),
  UNIQUE KEY `ipv4_address_UNIQUE` (`ipv4_address`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 */ /*!50003 TRIGGER `device_BEFORE_INSERT` BEFORE INSERT ON `device` FOR EACH ROW BEGIN
	DECLARE uuid CHAR(36) DEFAULT '';

	WHILE (uuid = '' OR EXISTS (SELECT `device_uuid` FROM `device` where `device`.`device_uuid` = uuid)) DO
		SET uuid = UUID();
	END WHILE;
  
	SET NEW.`device_uuid` = uuid;
    
    SET NEW.`ipv4_address` = INET_NTOA((SELECT `ipv4_counter` FROM `static` LIMIT 1));
    UPDATE `static` SET `ipv4_counter` = `ipv4_counter` + 1 WHERE `index` = 1;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `static` (
  `index` int unsigned NOT NULL AUTO_INCREMENT,
  `ipv4_counter` int unsigned NOT NULL DEFAULT '167772162',
  PRIMARY KEY (`index`),
  UNIQUE KEY `index_UNIQUE` (`index`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `user` (
  `user_uuid` char(36) NOT NULL DEFAULT '0',
  `user_id` bigint unsigned NOT NULL DEFAULT '0',
  `paid_until` bigint unsigned NOT NULL DEFAULT '0',
  `refresh_token` varchar(90) DEFAULT NULL,
  `refresh_token_expiry` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  PRIMARY KEY (`user_uuid`),
  UNIQUE KEY `public_id_UNIQUE` (`user_id`),
  UNIQUE KEY `user_uuid_UNIQUE` (`user_uuid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
/*!50003 SET @saved_cs_client      = @@character_set_client */ ;
/*!50003 SET @saved_cs_results     = @@character_set_results */ ;
/*!50003 SET @saved_col_connection = @@collation_connection */ ;
/*!50003 SET character_set_client  = utf8mb4 */ ;
/*!50003 SET character_set_results = utf8mb4 */ ;
/*!50003 SET collation_connection  = utf8mb4_0900_ai_ci */ ;
/*!50003 SET @saved_sql_mode       = @@sql_mode */ ;
/*!50003 SET sql_mode              = 'ONLY_FULL_GROUP_BY,STRICT_TRANS_TABLES,NO_ZERO_IN_DATE,NO_ZERO_DATE,ERROR_FOR_DIVISION_BY_ZERO,NO_ENGINE_SUBSTITUTION' */ ;
DELIMITER ;;
/*!50003 CREATE*/ /*!50017 */ /*!50003 TRIGGER `user_BEFORE_INSERT` BEFORE INSERT ON `user` FOR EACH ROW BEGIN
	DECLARE id BIGINT UNSIGNED DEFAULT 0;
    DECLARE uuid CHAR(36) DEFAULT '';

	WHILE (id = 0 OR EXISTS (SELECT `user_id` FROM `user` where `user`.`user_id` = id)) DO
		SET id = FLOOR(1000000000000000 + RAND() * 9000000000000000);
	END WHILE;
  
	SET NEW.`user_id` = id;

	WHILE (uuid = '' OR EXISTS (SELECT `user_uuid` FROM `user` where `user`.`user_uuid` = uuid)) DO
		SET uuid = LOWER(CONCAT(
			LPAD(HEX(ROUND(rand()*POW(2,32))), 8, '0'), '-',
			LPAD(HEX(ROUND(rand()*POW(2,16))), 4, '0'), '-',
			LPAD(HEX(ROUND(rand()*POW(2,16))), 4, '0'), '-',
			LPAD(HEX(ROUND(rand()*POW(2,16))), 4, '0'), '-',
			LPAD(HEX(ROUND(rand()*POW(2,48))), 12, '0')
		));
	END WHILE;
  
	SET NEW.`user_uuid` = uuid;
END */;;
DELIMITER ;
/*!50003 SET sql_mode              = @saved_sql_mode */ ;
/*!50003 SET character_set_client  = @saved_cs_client */ ;
/*!50003 SET character_set_results = @saved_cs_results */ ;
/*!50003 SET collation_connection  = @saved_col_connection */ ;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

