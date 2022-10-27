-- create a table
CREATE TABLE userProfile (
  id INTEGER PRIMARY KEY,
  name TEXT NOT NULL,
  gender TEXT NOT NULL,
  retired bool NOT NULL,
  age INTEGER NOT NULL
);
CREATE TABLE UserPortfolio (
  id INTEGER PRIMARY KEY,
  transfersID list NOT NULL,
  expensesID list NOT NULL
);
CREATE TABLE Transfer (
  id INTEGER PRIMARY KEY,
  toInvestmentId NOT NULL,
  fromInvestmentId NOT NULL,
  date datetime NOT NULL,
  amount double NOT NULL
);
CREATE TABLE Expense (
  id INTEGER PRIMARY KEY,
  startDate datetime NOT NULL,
  endDate datetime NOT NULL,
  frenquency double NOT NULL,
  amount double NOT NULL
);
CREATE TABLE InvestmentVehicle (
  id Integer PRIMARY KEY,
  investments list NOT NULL
);

CREATE TABLE Investments(
    id Integer PRIMARY KEY,
    datas data NOT NULL,
    type TEXT NOT NULL
);

-- insert some values
INSERT INTO userProfile VALUES (1, 'Ryan', 'M', true, 62);
INSERT INTO userProfile VALUES (2, 'Joanna', 'F', false, 45);
INSERT INTO userProfile VALUES (3, 'William', 'M', false, 27);
-- fetch some values
SELECT * FROM userProfile;