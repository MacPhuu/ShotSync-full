CREATE TABLE IF NOT EXISTS `__EFMigrationsHistory` (
    `MigrationId` varchar(150) CHARACTER SET utf8mb4 NOT NULL,
    `ProductVersion` varchar(32) CHARACTER SET utf8mb4 NOT NULL,
    CONSTRAINT `PK___EFMigrationsHistory` PRIMARY KEY (`MigrationId`)
) CHARACTER SET=utf8mb4;

START TRANSACTION;

ALTER DATABASE CHARACTER SET utf8mb4;

CREATE TABLE `events` (
    `id` int NOT NULL AUTO_INCREMENT,
    `name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `venue` longtext CHARACTER SET utf8mb4 NOT NULL,
    `location` longtext CHARACTER SET utf8mb4 NOT NULL,
    `date` datetime(6) NOT NULL,
    `number_of_players` int NOT NULL,
    `status` int NOT NULL,
    `is_displayed` tinyint(1) NOT NULL,
    `is_happen` tinyint(1) NOT NULL,
    `create_at` datetime(6) NULL,
    `update_at` datetime(6) NULL,
    CONSTRAINT `PK_events` PRIMARY KEY (`id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `players` (
    `id` int NOT NULL AUTO_INCREMENT,
    `name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `nation` longtext CHARACTER SET utf8mb4 NOT NULL,
    `portrait` longtext CHARACTER SET utf8mb4 NULL,
    `point` int NULL,
    `is_active` tinyint(1) NOT NULL,
    `create_at` datetime(6) NULL,
    `update_at` datetime(6) NULL,
    CONSTRAINT `PK_players` PRIMARY KEY (`id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `user` (
    `id` int NOT NULL AUTO_INCREMENT,
    `name` longtext CHARACTER SET utf8mb4 NOT NULL,
    `password` longtext CHARACTER SET utf8mb4 NOT NULL,
    `email` longtext CHARACTER SET utf8mb4 NOT NULL,
    `is_active` tinyint(1) NOT NULL,
    `role` int NOT NULL,
    `create_at` datetime(6) NULL,
    `update_at` datetime(6) NULL,
    CONSTRAINT `PK_user` PRIMARY KEY (`id`)
) CHARACTER SET=utf8mb4;

CREATE TABLE `matches` (
    `id` int NOT NULL AUTO_INCREMENT,
    `event_id` int NOT NULL,
    `first_player_id` int NULL,
    `second_player_id` int NULL,
    `table` longtext CHARACTER SET utf8mb4 NULL,
    `first_player_point` int NOT NULL,
    `second_player_point` int NOT NULL,
    `race_to` int NOT NULL,
    `is_start` tinyint(1) NOT NULL,
    `is_finish` tinyint(1) NOT NULL,
    `phase` longtext CHARACTER SET utf8mb4 NULL,
    `round_name` longtext CHARACTER SET utf8mb4 NULL,
    `stage` int NULL,
    `branch` int NULL,
    `next_match_id_win` int NULL,
    `next_match_id_lose` int NULL,
    `create_at` datetime(6) NULL,
    `update_at` datetime(6) NULL,
    CONSTRAINT `PK_matches` PRIMARY KEY (`id`),
    CONSTRAINT `FK_matches_events_event_id` FOREIGN KEY (`event_id`) REFERENCES `events` (`id`) ON DELETE CASCADE,
    CONSTRAINT `FK_matches_players_first_player_id` FOREIGN KEY (`first_player_id`) REFERENCES `players` (`id`) ON DELETE RESTRICT,
    CONSTRAINT `FK_matches_players_second_player_id` FOREIGN KEY (`second_player_id`) REFERENCES `players` (`id`) ON DELETE RESTRICT
) CHARACTER SET=utf8mb4;

CREATE TABLE `player_histories` (
    `id` int NOT NULL AUTO_INCREMENT,
    `player_id` int NOT NULL,
    `event_id` int NOT NULL,
    `match_id` int NOT NULL,
    `result` longtext CHARACTER SET utf8mb4 NOT NULL,
    `opponent_id` int NOT NULL,
    `match_date` datetime(6) NOT NULL,
    CONSTRAINT `PK_player_histories` PRIMARY KEY (`id`),
    CONSTRAINT `FK_player_histories_events_event_id` FOREIGN KEY (`event_id`) REFERENCES `events` (`id`) ON DELETE CASCADE,
    CONSTRAINT `FK_player_histories_players_player_id` FOREIGN KEY (`player_id`) REFERENCES `players` (`id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE TABLE `player_in_events` (
    `id` int NOT NULL AUTO_INCREMENT,
    `player_id` int NOT NULL,
    `event_id` int NOT NULL,
    CONSTRAINT `PK_player_in_events` PRIMARY KEY (`id`),
    CONSTRAINT `FK_player_in_events_events_event_id` FOREIGN KEY (`event_id`) REFERENCES `events` (`id`) ON DELETE CASCADE,
    CONSTRAINT `FK_player_in_events_players_player_id` FOREIGN KEY (`player_id`) REFERENCES `players` (`id`) ON DELETE CASCADE
) CHARACTER SET=utf8mb4;

CREATE INDEX `IX_matches_event_id` ON `matches` (`event_id`);

CREATE INDEX `IX_matches_first_player_id` ON `matches` (`first_player_id`);

CREATE INDEX `IX_matches_second_player_id` ON `matches` (`second_player_id`);

CREATE INDEX `IX_player_histories_event_id` ON `player_histories` (`event_id`);

CREATE INDEX `IX_player_histories_player_id` ON `player_histories` (`player_id`);

CREATE INDEX `IX_player_in_events_event_id` ON `player_in_events` (`event_id`);

CREATE INDEX `IX_player_in_events_player_id` ON `player_in_events` (`player_id`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260128094449_InitialFullSchema', '8.0.13');

COMMIT;

START TRANSACTION;

DROP PROCEDURE IF EXISTS `POMELO_BEFORE_DROP_PRIMARY_KEY`;
DELIMITER //
CREATE PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`(IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255))
BEGIN
	DECLARE HAS_AUTO_INCREMENT_ID TINYINT(1);
	DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
	DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
	DECLARE SQL_EXP VARCHAR(1000);
	SELECT COUNT(*)
		INTO HAS_AUTO_INCREMENT_ID
		FROM `information_schema`.`COLUMNS`
		WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
			AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
			AND `Extra` = 'auto_increment'
			AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
	IF HAS_AUTO_INCREMENT_ID THEN
		SELECT `COLUMN_TYPE`
			INTO PRIMARY_KEY_TYPE
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
		SELECT `COLUMN_NAME`
			INTO PRIMARY_KEY_COLUMN_NAME
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_KEY` = 'PRI'
			LIMIT 1;
		SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL;');
		SET @SQL_EXP = SQL_EXP;
		PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
		EXECUTE SQL_EXP_EXECUTE;
		DEALLOCATE PREPARE SQL_EXP_EXECUTE;
	END IF;
END //
DELIMITER ;

DROP PROCEDURE IF EXISTS `POMELO_AFTER_ADD_PRIMARY_KEY`;
DELIMITER //
CREATE PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`(IN `SCHEMA_NAME_ARGUMENT` VARCHAR(255), IN `TABLE_NAME_ARGUMENT` VARCHAR(255), IN `COLUMN_NAME_ARGUMENT` VARCHAR(255))
BEGIN
	DECLARE HAS_AUTO_INCREMENT_ID INT(11);
	DECLARE PRIMARY_KEY_COLUMN_NAME VARCHAR(255);
	DECLARE PRIMARY_KEY_TYPE VARCHAR(255);
	DECLARE SQL_EXP VARCHAR(1000);
	SELECT COUNT(*)
		INTO HAS_AUTO_INCREMENT_ID
		FROM `information_schema`.`COLUMNS`
		WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
			AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
			AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
			AND `COLUMN_TYPE` LIKE '%int%'
			AND `COLUMN_KEY` = 'PRI';
	IF HAS_AUTO_INCREMENT_ID THEN
		SELECT `COLUMN_TYPE`
			INTO PRIMARY_KEY_TYPE
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
				AND `COLUMN_TYPE` LIKE '%int%'
				AND `COLUMN_KEY` = 'PRI';
		SELECT `COLUMN_NAME`
			INTO PRIMARY_KEY_COLUMN_NAME
			FROM `information_schema`.`COLUMNS`
			WHERE `TABLE_SCHEMA` = (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA()))
				AND `TABLE_NAME` = TABLE_NAME_ARGUMENT
				AND `COLUMN_NAME` = COLUMN_NAME_ARGUMENT
				AND `COLUMN_TYPE` LIKE '%int%'
				AND `COLUMN_KEY` = 'PRI';
		SET SQL_EXP = CONCAT('ALTER TABLE `', (SELECT IFNULL(SCHEMA_NAME_ARGUMENT, SCHEMA())), '`.`', TABLE_NAME_ARGUMENT, '` MODIFY COLUMN `', PRIMARY_KEY_COLUMN_NAME, '` ', PRIMARY_KEY_TYPE, ' NOT NULL AUTO_INCREMENT;');
		SET @SQL_EXP = SQL_EXP;
		PREPARE SQL_EXP_EXECUTE FROM @SQL_EXP;
		EXECUTE SQL_EXP_EXECUTE;
		DEALLOCATE PREPARE SQL_EXP_EXECUTE;
	END IF;
END //
DELIMITER ;

CALL POMELO_BEFORE_DROP_PRIMARY_KEY(NULL, 'user');
ALTER TABLE `user` DROP PRIMARY KEY;

ALTER TABLE `user` DROP COLUMN `update_at`;

ALTER TABLE `user` RENAME `users`;

ALTER TABLE `users` CHANGE `create_at` `created_at` datetime(6) NOT NULL;

ALTER TABLE `users` MODIFY COLUMN `created_at` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

ALTER TABLE `users` ADD `avatar` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `users` ADD `nation` longtext CHARACTER SET utf8mb4 NULL;

ALTER TABLE `users` ADD CONSTRAINT `PK_users` PRIMARY KEY (`id`);
CALL POMELO_AFTER_ADD_PRIMARY_KEY(NULL, 'users', 'id');

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260128102358_AddNameToUser', '8.0.13');

DROP PROCEDURE `POMELO_BEFORE_DROP_PRIMARY_KEY`;

DROP PROCEDURE `POMELO_AFTER_ADD_PRIMARY_KEY`;

COMMIT;

START TRANSACTION;

ALTER TABLE `matches` DROP COLUMN `branch`;

ALTER TABLE `matches` DROP COLUMN `phase`;

ALTER TABLE `matches` DROP COLUMN `stage`;

ALTER TABLE `matches` CHANGE `update_at` `updated_at` datetime(6) NULL;

ALTER TABLE `matches` CHANGE `create_at` `created_at` datetime(6) NOT NULL;

ALTER TABLE `matches` CHANGE `table` `table_number` longtext NULL;

ALTER TABLE `matches` MODIFY COLUMN `created_at` datetime(6) NOT NULL DEFAULT '0001-01-01 00:00:00';

ALTER TABLE `matches` ADD `next_match_position` int NOT NULL DEFAULT 0;

ALTER TABLE `matches` ADD `round_type` int NOT NULL DEFAULT 0;

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20260128110920_UpdateMatchForHybridFlow', '8.0.13');

COMMIT;

