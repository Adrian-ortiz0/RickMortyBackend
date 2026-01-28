CREATE DATABASE IF NOT EXISTS rickmorty_db
CHARACTER SET utf8mb4
COLLATE utf8mb4_unicode_ci;

USE rickmorty_db;

CREATE TABLE IF NOT EXISTS Characters (
    Id INT PRIMARY KEY,
    Name VARCHAR(255) NOT NULL,
    Status VARCHAR(50),
    Species VARCHAR(100),
    Type VARCHAR(100),
    Gender VARCHAR(50),
    OriginName VARCHAR(255),
    OriginUrl VARCHAR(500),
    LocationName VARCHAR(255),
    LocationUrl VARCHAR(500),
    Image VARCHAR(500),
    Episodes JSON,
    Url VARCHAR(500),
    Created DATETIME,
    LastSync DATETIME,
    INDEX idx_name (Name),
    INDEX idx_status (Status),
    INDEX idx_species (Species),
    INDEX idx_lastsync (LastSync)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;