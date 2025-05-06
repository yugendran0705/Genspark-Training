create table item (
    itemname varchar(255) primary key,
    itemtype varchar(100) not null,
    itemcolor varchar(100) not null
);

create table department (
    deptname varchar(255) primary key,
    floor int not null check(floor > 0),
    phone varchar(20) unique not null,
    mgrid int not null,
    foreign key (mgrid)
);

create table emp (
    empno int primary key,
    empname varchar(255) not null,
    salary decimal(10,2) check(salary > 0),
    deptname varchar(255) null,
    bossno int null,
    foreign key (deptname) references department(deptname),
    foreign key (bossno) references emp(empno)
);

create table sales (
    salesno int primary key,
    saleqty int not null check(saleqty > 0),
    itemname varchar(255) not null,
    deptname varchar(255) not null,
    foreign key (itemname) references item(itemname),
    foreign key (deptname) references department(deptname)
);

ALTER TABLE department 
ALTER COLUMN mgrid INT NULL;

INSERT INTO department (deptname, floor, phone, mgrid) VALUES
('Management', 5, 34, NULL);

INSERT INTO department (deptname, floor, phone) VALUES
('Marketing', 2, '1234567891'),
('Accounting', 3, '1234567892'),
('Purchasing', 4, '1234567893'),
('Personnel', 5, '1234567894'),
('Navigation', 6, '1234567895'),
('Books', 7, '1234567896'),
('Clothes', 8, '1234567897'),
('Equipment', 9, '1234567898'),
('Furniture', 10, '1234567899'),
('Recreation', 11, '1234567800');

INSERT INTO emp (empno, empname, salary, deptname, bossno) VALUES
(1, 'Alice', 75000, 'Management', NULL),
(2, 'Ned', 45000, 'Marketing', 1),
(3, 'Andrew', 25000, 'Marketing', 2),
(4, 'Clare', 22000, 'Marketing', 2),
(5, 'Todd', 38000, 'Accounting', 1),
(6, 'Nancy', 22000, 'Accounting', 5),
(7, 'Brier', 43000, 'Purchasing', 1),
(8, 'Sarah', 56000, 'Purchasing', 7),
(9, 'Sophile', 35000, 'Personnel', 1),
(10, 'Sanjay', 15000, 'Navigation', 3),
(11, 'Rita', 15000, 'Books', 4),
(12, 'Gigi', 16000, 'Clothes', 4),
(13, 'Maggie', 11000, 'Clothes', 4),
(14, 'Paul', 15000, 'Equipment', 3),
(15, 'James', 15000, 'Equipment', 3),
(16, 'Pat', 15000, 'Furniture', 3),
(17, 'Mark', 15000, 'Recreation', 3);

ALTER TABLE item 
ALTER COLUMN itemcolor VARCHAR(100) NULL;

INSERT INTO item (itemname, itemtype, itemcolor) VALUES
('Pocket Knife-Nile', 'E', 'Brown'),
('Pocket Knife-Avon', 'E', 'Brown'),
('Compass', 'N', NULL),
('Geo positioning system', 'N', NULL),
('Elephant Polo stick', 'R', 'Bamboo'),
('Camel Saddle', 'R', 'Brown'),
('Sextant', 'N', NULL),
('Map Measure', 'N', NULL),
('Boots-snake proof', 'C', 'Green'),
('Pith Helmet', 'C', 'Khaki'),
('Hat-polar Explorer', 'C', 'White'),
('Exploring in 10 Easy Lessons', 'B', NULL),
('Hammock', 'F', 'Khaki'),
('How to win Foreign Friends', 'B', NULL),
('Map case', 'E', 'Brown'),
('Safari Chair', 'F', 'Khaki'),
('Safari cooking kit', 'F', 'Khaki'),
('Stetson', 'C', 'Black'),
('Tent - 2 person', 'F', 'Khaki'),
('Tent - 8 person', 'F', 'Khaki');

INSERT INTO sales (salesno, saleqty, itemname, deptname) VALUES
(101, 2, 'Boots-snake proof', 'Clothes'),
(102, 1, 'Pith Helmet', 'Clothes'),
(103, 1, 'Sextant', 'Navigation'),
(104, 3, 'Hat-polar Explorer', 'Clothes'),
(105, 5, 'Pith Helmet', 'Equipment'),
(106, 2, 'Pocket Knife-Nile', 'Clothes'),
(107, 3, 'Pocket Knife-Nile', 'Recreation'),
(108, 1, 'Compass', 'Navigation'),
(109, 2, 'Geo positioning system', 'Navigation'),
(110, 5, 'Map Measure', 'Navigation'),
(111, 1, 'Geo positioning system', 'Books'),
(112, 1, 'Sextant', 'Books'),
(113, 3, 'Pocket Knife-Nile', 'Books'),
(114, 1, 'Pocket Knife-Nile', 'Navigation'),
(115, 1, 'Pocket Knife-Nile', 'Equipment'),
(116, 1, 'Sextant', 'Clothes'),
(117, 1, 'Sextant', 'Equipment'),
(118, 1, 'Sextant', 'Recreation'),
(119, 1, 'Sextant', 'Furniture'),
(120, 1, 'Pocket Knife-Nile', 'Furniture'),
(121, 1, 'Exploring in 10 easy lessons', 'Books'),
(122, 1, 'How to win foreign friends', 'Books'),
(123, 1, 'Compass', 'Books'),
(124, 1, 'Pith Helmet', 'Books'),
(125, 1, 'Elephant Polo stick', 'Recreation'),
(126, 1, 'Camel Saddle', 'Recreation');


UPDATE department SET mgrid = 1 WHERE deptname = 'Management';
UPDATE department SET mgrid = 4 WHERE deptname = 'Books';
UPDATE department SET mgrid = 4 WHERE deptname = 'Clothes';
UPDATE department SET mgrid = 3 WHERE deptname = 'Equipment';
UPDATE department SET mgrid = 3 WHERE deptname = 'Furniture';
UPDATE department SET mgrid = 3 WHERE deptname = 'Navigation';
UPDATE department SET mgrid = 4 WHERE deptname = 'Recreation';
UPDATE department SET mgrid = 5 WHERE deptname = 'Accounting';
UPDATE department SET mgrid = 7 WHERE deptname = 'Purchasing';
UPDATE department SET mgrid = 9 WHERE deptname = 'Personnel';
UPDATE department SET mgrid = 2 WHERE deptname = 'Marketing';
