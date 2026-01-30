-- MySQL dump 10.13  Distrib 8.0.45, for Linux (aarch64)
--
-- Host: localhost    Database: shotsync
-- ------------------------------------------------------
-- Server version	8.0.45

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Table structure for table `__EFMigrationsHistory`
--

DROP TABLE IF EXISTS `__EFMigrationsHistory`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `__EFMigrationsHistory` (
  `MigrationId` varchar(150) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `ProductVersion` varchar(32) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  PRIMARY KEY (`MigrationId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `__EFMigrationsHistory`
--

LOCK TABLES `__EFMigrationsHistory` WRITE;
/*!40000 ALTER TABLE `__EFMigrationsHistory` DISABLE KEYS */;
INSERT INTO `__EFMigrationsHistory` VALUES ('20260128112231_InitialFullSchema_Production','8.0.13'),('20260128140647_AddDatabaseIndexes','8.0.13'),('20260128180823_AddEventFields','8.0.13'),('20260128182433_AddHostIdToEvent','8.0.13'),('20260129145551_AddPlayerEmail','8.0.13');
/*!40000 ALTER TABLE `__EFMigrationsHistory` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `events`
--

DROP TABLE IF EXISTS `events`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `events` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `venue` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `location` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `date` datetime(6) NOT NULL,
  `number_of_players` int NOT NULL,
  `status` int NOT NULL,
  `is_displayed` tinyint(1) NOT NULL,
  `is_happen` tinyint(1) NOT NULL,
  `create_at` datetime(6) DEFAULT NULL,
  `update_at` datetime(6) DEFAULT NULL,
  `description` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `entry_fee` decimal(65,30) DEFAULT NULL,
  `format` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `slogan` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `total_prize` decimal(65,30) DEFAULT NULL,
  `host_id` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_events_host_id` (`host_id`),
  CONSTRAINT `FK_events_users_host_id` FOREIGN KEY (`host_id`) REFERENCES `users` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=3 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `events`
--

LOCK TABLES `events` WRITE;
/*!40000 ALTER TABLE `events` DISABLE KEYS */;
INSERT INTO `events` VALUES (2,'ncbncvbn','bcvncvbn','cvbncvbn','2026-01-21 21:40:00.000000',32,1,1,1,'2026-01-29 01:40:41.689717','2026-01-30 00:51:14.992177','Nhập mô tả về giải đấu...',NULL,'Round Robin','cvbncvbn',NULL,2);
/*!40000 ALTER TABLE `events` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `matches`
--

DROP TABLE IF EXISTS `matches`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `matches` (
  `id` int NOT NULL AUTO_INCREMENT,
  `event_id` int NOT NULL,
  `first_player_id` int DEFAULT NULL,
  `second_player_id` int DEFAULT NULL,
  `table_number` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `first_player_point` int NOT NULL,
  `second_player_point` int NOT NULL,
  `race_to` int NOT NULL,
  `is_start` tinyint(1) NOT NULL,
  `is_finish` tinyint(1) NOT NULL,
  `round_name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `round_type` int NOT NULL,
  `next_match_id_win` int DEFAULT NULL,
  `next_match_id_lose` int DEFAULT NULL,
  `next_match_position` int NOT NULL,
  `created_at` datetime(6) NOT NULL,
  `updated_at` datetime(6) DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_matches_event_id` (`event_id`),
  KEY `IX_matches_first_player_id` (`first_player_id`),
  KEY `IX_matches_second_player_id` (`second_player_id`),
  CONSTRAINT `FK_matches_events_event_id` FOREIGN KEY (`event_id`) REFERENCES `events` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_matches_players_first_player_id` FOREIGN KEY (`first_player_id`) REFERENCES `players` (`id`) ON DELETE RESTRICT,
  CONSTRAINT `FK_matches_players_second_player_id` FOREIGN KEY (`second_player_id`) REFERENCES `players` (`id`) ON DELETE RESTRICT
) ENGINE=InnoDB AUTO_INCREMENT=221 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `matches`
--

LOCK TABLES `matches` WRITE;
/*!40000 ALTER TABLE `matches` DISABLE KEYS */;
INSERT INTO `matches` VALUES (166,2,106,70,'T-5',0,2,9,1,0,'Vòng loại 1 (Nhánh thắng)',1,182,190,1,'2026-01-30 00:51:14.336005','2026-01-30 01:02:25.403062'),(167,2,102,82,'T-2',0,0,9,1,0,'Vòng loại 1 (Nhánh thắng)',1,182,190,2,'2026-01-30 00:51:14.392685','2026-01-30 00:51:14.643542'),(168,2,83,107,'T-4',0,0,9,1,0,'Vòng loại 1 (Nhánh thắng)',1,183,191,1,'2026-01-30 00:51:14.404620','2026-01-30 00:51:14.652702'),(169,2,72,104,'T-10',0,0,9,1,0,'Vòng loại 1 (Nhánh thắng)',1,183,191,2,'2026-01-30 00:51:14.415467','2026-01-30 00:51:14.657033'),(170,2,1,98,'T-3',0,0,9,1,0,'Vòng loại 1 (Nhánh thắng)',1,184,192,1,'2026-01-30 00:51:14.429627','2026-01-30 00:51:14.667304'),(171,2,85,66,'T-8',0,0,9,1,0,'Vòng loại 1 (Nhánh thắng)',1,184,192,2,'2026-01-30 00:51:14.440656','2026-01-30 00:51:14.670085'),(172,2,68,79,'T-7',0,0,9,1,0,'Vòng loại 1 (Nhánh thắng)',1,185,193,1,'2026-01-30 00:51:14.452239','2026-01-30 00:51:14.680535'),(173,2,97,73,'T-1',0,0,9,1,0,'Vòng loại 1 (Nhánh thắng)',1,185,193,2,'2026-01-30 00:51:14.460655','2026-01-30 00:51:14.683704'),(174,2,78,84,'T-6',0,0,9,1,0,'Vòng loại 1 (Nhánh thắng)',1,186,194,1,'2026-01-30 00:51:14.469102','2026-01-30 00:51:14.692310'),(175,2,76,81,'T-9',0,0,9,1,0,'Vòng loại 1 (Nhánh thắng)',1,186,194,2,'2026-01-30 00:51:14.475666','2026-01-30 00:51:14.696233'),(176,2,69,103,'T-5',0,0,9,1,0,'Vòng loại 1 (Nhánh thắng)',1,187,195,1,'2026-01-30 00:51:14.484305','2026-01-30 00:51:14.704873'),(177,2,99,67,'T-2',0,0,9,1,0,'Vòng loại 1 (Nhánh thắng)',1,187,195,2,'2026-01-30 00:51:14.492123','2026-01-30 00:51:14.709901'),(178,2,80,101,'T-4',0,0,9,1,0,'Vòng loại 1 (Nhánh thắng)',1,188,196,1,'2026-01-30 00:51:14.499478','2026-01-30 00:51:14.719661'),(179,2,77,105,'T-10',0,0,9,1,0,'Vòng loại 1 (Nhánh thắng)',1,188,196,2,'2026-01-30 00:51:14.505247','2026-01-30 00:51:14.722774'),(180,2,75,74,'T-3',0,0,9,1,0,'Vòng loại 1 (Nhánh thắng)',1,189,197,1,'2026-01-30 00:51:14.513884','2026-01-30 00:51:14.734449'),(181,2,71,100,'T-8',0,0,9,1,0,'Vòng loại 1 (Nhánh thắng)',1,189,197,2,'2026-01-30 00:51:14.519939','2026-01-30 00:51:14.738766'),(182,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 2 (Nhánh thắng)',1,206,198,1,'2026-01-30 00:51:14.526422','2026-01-30 00:51:14.848496'),(183,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 2 (Nhánh thắng)',1,206,199,2,'2026-01-30 00:51:14.540827','2026-01-30 00:51:14.850885'),(184,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 2 (Nhánh thắng)',1,207,200,1,'2026-01-30 00:51:14.555858','2026-01-30 00:51:14.858053'),(185,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 2 (Nhánh thắng)',1,207,201,2,'2026-01-30 00:51:14.568838','2026-01-30 00:51:14.860586'),(186,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 2 (Nhánh thắng)',1,208,202,1,'2026-01-30 00:51:14.582187','2026-01-30 00:51:14.867584'),(187,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 2 (Nhánh thắng)',1,208,203,2,'2026-01-30 00:51:14.594605','2026-01-30 00:51:14.869762'),(188,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 2 (Nhánh thắng)',1,209,204,1,'2026-01-30 00:51:14.609329','2026-01-30 00:51:14.875730'),(189,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 2 (Nhánh thắng)',1,209,205,2,'2026-01-30 00:51:14.624305','2026-01-30 00:51:14.879259'),(190,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 1 (Nhánh thua)',2,198,NULL,1,'2026-01-30 00:51:14.636205','2026-01-30 00:51:14.749579'),(191,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 1 (Nhánh thua)',2,199,NULL,1,'2026-01-30 00:51:14.648240','2026-01-30 00:51:14.763164'),(192,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 1 (Nhánh thua)',2,200,NULL,1,'2026-01-30 00:51:14.660146','2026-01-30 00:51:14.776105'),(193,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 1 (Nhánh thua)',2,201,NULL,1,'2026-01-30 00:51:14.674158','2026-01-30 00:51:14.788266'),(194,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 1 (Nhánh thua)',2,202,NULL,1,'2026-01-30 00:51:14.687208','2026-01-30 00:51:14.800964'),(195,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 1 (Nhánh thua)',2,203,NULL,1,'2026-01-30 00:51:14.699624','2026-01-30 00:51:14.814312'),(196,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 1 (Nhánh thua)',2,204,NULL,1,'2026-01-30 00:51:14.714024','2026-01-30 00:51:14.824272'),(197,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 1 (Nhánh thua)',2,205,NULL,1,'2026-01-30 00:51:14.727349','2026-01-30 00:51:14.836518'),(198,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 2 (Nhánh thua)',2,210,NULL,1,'2026-01-30 00:51:14.742814','2026-01-30 00:51:14.886033'),(199,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 2 (Nhánh thua)',2,210,NULL,2,'2026-01-30 00:51:14.757289','2026-01-30 00:51:14.888915'),(200,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 2 (Nhánh thua)',2,211,NULL,1,'2026-01-30 00:51:14.770664','2026-01-30 00:51:14.896067'),(201,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 2 (Nhánh thua)',2,211,NULL,2,'2026-01-30 00:51:14.783638','2026-01-30 00:51:14.898764'),(202,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 2 (Nhánh thua)',2,212,NULL,1,'2026-01-30 00:51:14.795538','2026-01-30 00:51:14.904288'),(203,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 2 (Nhánh thua)',2,212,NULL,2,'2026-01-30 00:51:14.808101','2026-01-30 00:51:14.906873'),(204,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 2 (Nhánh thua)',2,213,NULL,1,'2026-01-30 00:51:14.819912','2026-01-30 00:51:14.913643'),(205,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại 2 (Nhánh thua)',2,213,NULL,2,'2026-01-30 00:51:14.831821','2026-01-30 00:51:14.916004'),(206,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại trực tiếp 1/16',3,214,NULL,1,'2026-01-30 00:51:14.844182','2026-01-30 00:51:14.925794'),(207,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại trực tiếp 1/16',3,214,NULL,2,'2026-01-30 00:51:14.853510','2026-01-30 00:51:14.929143'),(208,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại trực tiếp 1/16',3,215,NULL,1,'2026-01-30 00:51:14.863453','2026-01-30 00:51:14.935934'),(209,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại trực tiếp 1/16',3,215,NULL,2,'2026-01-30 00:51:14.871936','2026-01-30 00:51:14.938129'),(210,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại trực tiếp 1/16',3,216,NULL,1,'2026-01-30 00:51:14.882062','2026-01-30 00:51:14.944091'),(211,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại trực tiếp 1/16',3,216,NULL,2,'2026-01-30 00:51:14.891370','2026-01-30 00:51:14.948355'),(212,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại trực tiếp 1/16',3,217,NULL,1,'2026-01-30 00:51:14.901068','2026-01-30 00:51:14.955580'),(213,2,NULL,NULL,NULL,0,0,9,0,0,'Vòng loại trực tiếp 1/16',3,217,NULL,2,'2026-01-30 00:51:14.908733','2026-01-30 00:51:14.959941'),(214,2,NULL,NULL,NULL,0,0,9,0,0,'Tứ kết',3,218,NULL,1,'2026-01-30 00:51:14.917972','2026-01-30 00:51:14.967661'),(215,2,NULL,NULL,NULL,0,0,9,0,0,'Tứ kết',3,218,NULL,2,'2026-01-30 00:51:14.931891','2026-01-30 00:51:14.969881'),(216,2,NULL,NULL,NULL,0,0,9,0,0,'Tứ kết',3,219,NULL,1,'2026-01-30 00:51:14.940178','2026-01-30 00:51:14.976005'),(217,2,NULL,NULL,NULL,0,0,9,0,0,'Tứ kết',3,219,NULL,2,'2026-01-30 00:51:14.951037','2026-01-30 00:51:14.980045'),(218,2,NULL,NULL,NULL,0,0,12,0,0,'Bán kết',3,220,NULL,1,'2026-01-30 00:51:14.963266','2026-01-30 00:51:14.986509'),(219,2,NULL,NULL,NULL,0,0,12,0,0,'Bán kết',3,220,NULL,2,'2026-01-30 00:51:14.972311','2026-01-30 00:51:14.989024'),(220,2,NULL,NULL,NULL,0,0,13,0,0,'Chung kết',3,NULL,NULL,0,'2026-01-30 00:51:14.982097',NULL);
/*!40000 ALTER TABLE `matches` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `player_histories`
--

DROP TABLE IF EXISTS `player_histories`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `player_histories` (
  `id` int NOT NULL AUTO_INCREMENT,
  `player_id` int NOT NULL,
  `event_id` int NOT NULL,
  `match_id` int NOT NULL,
  `result` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `opponent_id` int NOT NULL,
  `match_date` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `IX_player_histories_event_id` (`event_id`),
  KEY `IX_player_histories_player_id` (`player_id`),
  CONSTRAINT `FK_player_histories_events_event_id` FOREIGN KEY (`event_id`) REFERENCES `events` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_player_histories_players_player_id` FOREIGN KEY (`player_id`) REFERENCES `players` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `player_histories`
--

LOCK TABLES `player_histories` WRITE;
/*!40000 ALTER TABLE `player_histories` DISABLE KEYS */;
/*!40000 ALTER TABLE `player_histories` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `player_in_events`
--

DROP TABLE IF EXISTS `player_in_events`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `player_in_events` (
  `id` int NOT NULL AUTO_INCREMENT,
  `player_id` int NOT NULL,
  `event_id` int NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_player_in_events_player_id_event_id` (`player_id`,`event_id`),
  KEY `IX_player_in_events_event_id` (`event_id`),
  KEY `IX_player_in_events_player_id` (`player_id`),
  CONSTRAINT `FK_player_in_events_events_event_id` FOREIGN KEY (`event_id`) REFERENCES `events` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_player_in_events_players_player_id` FOREIGN KEY (`player_id`) REFERENCES `players` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=46 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `player_in_events`
--

LOCK TABLES `player_in_events` WRITE;
/*!40000 ALTER TABLE `player_in_events` DISABLE KEYS */;
INSERT INTO `player_in_events` VALUES (1,1,2),(4,66,2),(5,67,2),(6,68,2),(7,69,2),(8,70,2),(9,71,2),(10,72,2),(11,73,2),(12,74,2),(13,75,2),(14,76,2),(15,77,2),(16,78,2),(17,79,2),(18,80,2),(2,81,2),(19,82,2),(3,83,2),(20,84,2),(21,85,2),(35,97,2),(36,98,2),(37,99,2),(38,100,2),(39,101,2),(40,102,2),(41,103,2),(42,104,2),(43,105,2),(44,106,2),(45,107,2);
/*!40000 ALTER TABLE `player_in_events` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `players`
--

DROP TABLE IF EXISTS `players`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `players` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `nation` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `portrait` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `point` int DEFAULT NULL,
  `is_active` tinyint(1) NOT NULL,
  `create_at` datetime(6) DEFAULT NULL,
  `update_at` datetime(6) DEFAULT NULL,
  `email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL DEFAULT '',
  `user_id` int DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_players_email` (`email`),
  KEY `IX_players_name` (`name`),
  KEY `IX_players_user_id` (`user_id`),
  CONSTRAINT `FK_players_users_user_id` FOREIGN KEY (`user_id`) REFERENCES `users` (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=108 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `players`
--

LOCK TABLES `players` WRITE;
/*!40000 ALTER TABLE `players` DISABLE KEYS */;
INSERT INTO `players` VALUES (1,'Ko Pin Yi','Vietnam',NULL,NULL,1,'2026-01-29 22:09:28.624486',NULL,'kpy@shotsync.com',12),(66,'Ferdis','Indonesia',NULL,0,1,NULL,NULL,'ferdis@shotsync.com',13),(67,'Kyle Arcenal','Philippines',NULL,0,1,NULL,NULL,'kylearcenal@shotsync.com',14),(68,'Ian Bayot','Philippines',NULL,0,1,NULL,NULL,'ianbayot@shotsync.com',15),(69,'Jack Beggs','New Zealand',NULL,0,1,NULL,NULL,'jackbeggs@shotsync.com',16),(70,'Sean Beggs','New Zealand',NULL,0,1,NULL,NULL,'seanbeggs@shotsync.com',17),(71,'Hugo Boyle','Spain',NULL,0,1,NULL,NULL,'hugoboyle@shotsync.com',18),(72,'Bui Tuan Anh','Viet Nam',NULL,0,1,NULL,NULL,'buituananh@shotsync.com',19),(73,'Bui Viet Anh','Viet Nam',NULL,0,1,NULL,NULL,'buivietanh@shotsync.com',20),(74,'Lau Caden','Hong Kong',NULL,0,1,NULL,NULL,'laucaden@shotsync.com',21),(75,'Chang Tzu Ching','Chinese Taipei',NULL,0,1,NULL,NULL,'changtzuching@shotsync.com',22),(76,'Po Chun Chao','Chinese Taipei',NULL,0,1,NULL,NULL,'pochunchao@shotsync.com',23),(77,'Chien Ching Yuan','Chinese Taipei',NULL,0,1,NULL,NULL,'chienchingyuan@shotsync.com',24),(78,'Chu Tien Manh','Viet Nam',NULL,0,1,NULL,NULL,'chutienmanh@shotsync.com',25),(79,'Chung Duc Vinh','Viet Nam',NULL,0,1,NULL,NULL,'chungducvinh@shotsync.com',26),(80,'Dan Gia Linh','Viet Nam',NULL,0,1,NULL,NULL,'dangialinh@shotsync.com',27),(81,'Jed De Castro','Philippines',NULL,0,1,NULL,NULL,'jeddecastro@shotsync.com',28),(82,'Prince Delos Santos','Philippines',NULL,0,1,NULL,NULL,'princedelossantos@shotsync.com',29),(83,'Prince Dizon','Philippines',NULL,0,1,NULL,NULL,'princedizon@shotsync.com',30),(84,'Do Hai Anh','Viet Nam',NULL,0,1,NULL,NULL,'dohaianh@shotsync.com',31),(85,'Le Quang Do','Viet Nam',NULL,0,1,NULL,NULL,'lequangdo@shotsync.com',32),(97,'Extra Player 1','VN',NULL,0,1,'2026-01-30 00:44:30.156100',NULL,'extra1_639053306701562210@test.com',NULL),(98,'Extra Player 2','VN',NULL,0,1,'2026-01-30 00:44:30.377510',NULL,'extra2_639053306703775160@test.com',NULL),(99,'Extra Player 3','VN',NULL,0,1,'2026-01-30 00:44:30.391728',NULL,'extra3_639053306703917320@test.com',NULL),(100,'Extra Player 4','VN',NULL,0,1,'2026-01-30 00:44:30.401517',NULL,'extra4_639053306704015210@test.com',NULL),(101,'Extra Player 5','VN',NULL,0,1,'2026-01-30 00:44:30.416884',NULL,'extra5_639053306704168890@test.com',NULL),(102,'Extra Player 6','VN',NULL,0,1,'2026-01-30 00:44:30.432623',NULL,'extra6_639053306704326280@test.com',NULL),(103,'Extra Player 7','VN',NULL,0,1,'2026-01-30 00:44:30.444260',NULL,'extra7_639053306704442650@test.com',NULL),(104,'Extra Player 8','VN',NULL,0,1,'2026-01-30 00:44:30.454343',NULL,'extra8_639053306704543470@test.com',NULL),(105,'Extra Player 9','VN',NULL,0,1,'2026-01-30 00:44:30.465442',NULL,'extra9_639053306704654480@test.com',NULL),(106,'Extra Player 10','VN',NULL,0,1,'2026-01-30 00:44:30.474189',NULL,'extra10_639053306704741930@test.com',NULL),(107,'Extra Player 11','VN',NULL,0,1,'2026-01-30 00:44:30.484242',NULL,'extra11_639053306704842460@test.com',NULL);
/*!40000 ALTER TABLE `players` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `users`
--

DROP TABLE IF EXISTS `users`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `users` (
  `id` int NOT NULL AUTO_INCREMENT,
  `name` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `email` varchar(255) CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `password` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci NOT NULL,
  `role` int NOT NULL,
  `nation` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `avatar` longtext CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci,
  `is_active` tinyint(1) NOT NULL,
  `created_at` datetime(6) NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `IX_users_email` (`email`)
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `users`
--

LOCK TABLES `users` WRITE;
/*!40000 ALTER TABLE `users` DISABLE KEYS */;
INSERT INTO `users` VALUES (1,'Administrator','admin@shotsync.com','$2a$11$aBSQJXLKw33qiX0qhiP2KOTXPaU2Iz50b001oNnrm.iQEK6Y8WXr.',1,'None',NULL,1,'2026-01-28 21:19:16.698596'),(2,'XBilliard','xbilliard@shotsync.com','$2a$11$a.bqha8HNlJCVl.IHkffjOFxj1bMxRNFhTLeC1gZ4V939gJ03EJg6',2,'None',NULL,1,'2026-01-28 22:57:07.352663'),(3,'McPhu','macphu@shotsync.com','$2a$11$SAZM/8WZAJpvQeo16pKeqOELZUsVH0c5cTXjr57DOWQWPB6DIVjxG',3,'None',NULL,1,'2026-01-28 23:29:04.436203'),(4,'ABC','abc@example.com','$2a$11$xdcpXHp1zlwfzhLzQ5yZju1drOP.X81UE.pswkdyE0w0GZrgnb2vK',3,'string',NULL,1,'2026-01-29 00:29:22.190719'),(5,'Eklent Kaci','kaci@shotsync.com','$2a$11$oW2rDW4uffgDs0OkTZuvreAecV8DsSIslx0/WxSImoQNmQzUQYBmS',3,'Vietnam',NULL,1,'2026-01-29 21:39:41.761430'),(6,'Kledio Kaci','kledio@shotsync.com','$2a$11$jpK5pUXFisBZK73qLFmnQekHqHceYB02VY5oYG4Q6DW3lrm2XQ70y',3,'Vietnam',NULL,1,'2026-01-29 21:44:57.069569'),(10,'string','user@example.com','$2a$11$mKN99AzAh4LIaQVok2Uq1eGr1G8.IwZPwAiWbWgEJHxnbDtNruchK',3,'string',NULL,1,'2026-01-29 21:57:13.719385'),(11,'Shane Van Boening','svb@shotsync.com','$2a$11$wnqdbQEeZ2xlVps6TYUbtOF7CqgBFBOPVK3eydROSqbOAa.vq0dGW',3,'USA',NULL,1,'2026-01-29 22:02:40.691601'),(12,'Ko Pin Yi','kpy@shotsync.com','$2a$11$cTNosMDdrDumduJ1bUuKY.4ccSxQ97bu3Li4cNtfz5RvQW/60eC.e',3,'Vietnam',NULL,1,'2026-01-29 22:09:28.269234'),(13,'Ferdis','ferdis@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Indonesia',NULL,1,'2026-01-29 22:32:02.000000'),(14,'Kyle Arcenal','kylearcenal@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Philippines',NULL,1,'2026-01-29 22:32:02.000000'),(15,'Ian Bayot','ianbayot@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Philippines',NULL,1,'2026-01-29 22:32:02.000000'),(16,'Jack Beggs','jackbeggs@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'New Zealand',NULL,1,'2026-01-29 22:32:02.000000'),(17,'Sean Beggs','seanbeggs@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'New Zealand',NULL,1,'2026-01-29 22:32:02.000000'),(18,'Hugo Boyle','hugoboyle@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Spain',NULL,1,'2026-01-29 22:32:02.000000'),(19,'Bui Tuan Anh','buituananh@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Viet Nam',NULL,1,'2026-01-29 22:32:02.000000'),(20,'Bui Viet Anh','buivietanh@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Viet Nam',NULL,1,'2026-01-29 22:32:02.000000'),(21,'Lau Caden','laucaden@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Hong Kong',NULL,1,'2026-01-29 22:32:02.000000'),(22,'Chang Tzu Ching','changtzuching@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Chinese Taipei',NULL,1,'2026-01-29 22:32:02.000000'),(23,'Po Chun Chao','pochunchao@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Chinese Taipei',NULL,1,'2026-01-29 22:32:02.000000'),(24,'Chien Ching Yuan','chienchingyuan@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Chinese Taipei',NULL,1,'2026-01-29 22:32:02.000000'),(25,'Chu Tien Manh','chutienmanh@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Viet Nam',NULL,1,'2026-01-29 22:32:02.000000'),(26,'Chung Duc Vinh','chungducvinh@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Viet Nam',NULL,1,'2026-01-29 22:32:02.000000'),(27,'Dan Gia Linh','dangialinh@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Viet Nam',NULL,1,'2026-01-29 22:32:02.000000'),(28,'Jed De Castro','jeddecastro@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Philippines',NULL,1,'2026-01-29 22:32:02.000000'),(29,'Prince Delos Santos','princedelossantos@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Philippines',NULL,1,'2026-01-29 22:32:02.000000'),(30,'Prince Dizon','princedizon@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Philippines',NULL,1,'2026-01-29 22:32:02.000000'),(31,'Do Hai Anh','dohaianh@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Viet Nam',NULL,1,'2026-01-29 22:32:02.000000'),(32,'Le Quang Do','lequangdo@shotsync.com','$2a$11$ASAZM8WZAJpyQeo',3,'Viet Nam',NULL,1,'2026-01-29 22:32:02.000000');
/*!40000 ALTER TABLE `users` ENABLE KEYS */;
UNLOCK TABLES;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2026-01-30  7:00:01
