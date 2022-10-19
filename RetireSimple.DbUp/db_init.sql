--brew services start mariadb
--mysql

-- create schema

CREATE DATABASE retire_simple;
SHOW SCHEMAS;

USE retire_simple;

-- create tables
CREATE TABLE UserProfile (
  id INTEGER PRIMARY KEY AUTO_INCREMENT,
  name TEXT NOT NULL,
  gender TEXT NOT NULL,
  retired bool NOT NULL,
  age INTEGER NOT NULL
);
CREATE TABLE UserPortfolio (
  id INTEGER PRIMARY KEY AUTO_INCREMENT,
  transfersID SET(""),
  expensesID SET("")
);
CREATE TABLE Transfer (
  id INTEGER PRIMARY KEY AUTO_INCREMENT,
  toInvestmentId INTEGER NOT NULL,
  fromInvestmentId INTEGER NOT NULL,
  date datetime NOT NULL,
  amount double NOT NULL
);
CREATE TABLE Expense (
  id INTEGER PRIMARY KEY AUTO_INCREMENT,
  startDate datetime NOT NULL,
  endDate datetime NOT NULL,
  frenquency double NOT NULL,
  amount double NOT NULL
);
CREATE TABLE InvestmentVehicle (
  id Integer PRIMARY KEY AUTO_INCREMENT,
  investments SET("") NOT NULL
);

CREATE TABLE Investments(
    id Integer PRIMARY KEY AUTO_INCREMENT,
    datas JSON NOT NULL,
    type TEXT NOT NULL
);

SHOW TABLES;

-- insert some values
INSERT INTO UserProfile (name, gender, retired, age) VALUES ('Ryan', 'M', true, 62);
INSERT INTO UserProfile (name, gender, retired, age) VALUES ('Joanna', 'F', false, 45);
INSERT INTO UserProfile (name, gender, retired, age) VALUES ('William', 'M', false, 27);

-- fetch values
SELECT * FROM UserProfile;