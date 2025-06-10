create table bank_accounts (
    account_id int primary key,
    balance int not null
);
insert into bank_accounts (account_id, balance) values 
(1, 5000),
(2, 10000),
(3, 7500),
(4, 12000),
(5, 3000);

--1 locking 
--session1
update bank_accounts set balance=balance-1000 where account_id=1;
--session2
update bank_accounts set balance=balance+1000 where account_id=1;
 
--2
--session1
select * from bank_accounts where account_id=1 for update;
 
--session2
update bank_accounts set balance=balance-1000 where account_id=1;
 
--3 deadlock
rollback;
--session1
begin;
select * from bank_accounts where account_id=2 for update;
--session2
begin;
select * from bank_accounts where account_id=1 for update;
 
--session1
select * from bank_accounts where account_id=1 for update;
 
--session2
select * from bank_accounts where account_id=2 for update;
 
--4 pg lock table
select * from pg_locks;