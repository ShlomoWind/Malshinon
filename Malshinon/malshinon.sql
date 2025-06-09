CREATE TABLE IF NOT EXISTS people (
    id INT PRIMARY KEY AUTO_INCREMENT,
    first_name VARCHAR(100),
    last_name VARCHAR(100),
    secret_code VARCHAR(100),
    type ENUM('reporter', 'target', 'both', 'potential_agent'),
    num_reports INT DEFAULT 0,
    num_mentions INT DEFAULT 0
) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;

CREATE TABLE IF NOT EXISTS IntelReports (
    id INT PRIMARY KEY AUTO_INCREMENT,
    reporter_id INT,
    target_id INT,
    text TEXT,
    timestamp DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (reporter_id) REFERENCES people(id),
    FOREIGN KEY (target_id) REFERENCES people(id)
) CHARACTER SET utf8mb4 COLLATE utf8mb4_general_ci;