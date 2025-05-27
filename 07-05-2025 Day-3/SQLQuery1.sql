use pubs
go

select au_id as 'Author Id', title as 'Book name' from titleauthor join titles on titleauthor.title_id = titles.title_id;

select pub_name as Publisher_Name, title, ord_date from publishers JOIN titles on publishers.pub_id = titles.pub_id JOIN sales ON titles.title_id = sales.title_id;

select pub_name as Publisher_Name, title, ord_date from publishers JOIN titles on publishers.pub_id = titles.pub_id JOIN sales ON titles.title_id = sales.title_id order by ord_date;

select p.pub_name as Publisher_Name, min(t.pubdate) as First_published_Date from publishers p join titles t on p.pub_id = t.pub_id group by p.pub_name ;

select pub_name, min(ord_date) as first_order_date from publishers p left outer join titles t on p.pub_id = t.pub_id left outer join sales s on t.title_id = s.title_id group by pub_name order by min(ord_date); 

select title as Book_Name, stor_address as Store_Address from titles join sales ON titles.title_id = sales.title_id join stores on stores.stor_id = sales.stor_id order by Book_Name;

create procedure proc_firstprocedure
as begin
 print 'Hello World!'
end
Go
exec proc_firstprocedure

create table Products
(id int identity(1,1) constraint pk_productId primary key,
name nvarchar(100) not null,
details nvarchar(max))
Go
create proc proc_InsertProduct(@pname nvarchar(100),@pdetails nvarchar(max))
as
begin
    insert into Products(name,details) values(@pname,@pdetails)
end
go
proc_InsertProduct 'Laptop','{"brand":"Dell","spec":{"ram":"16GB","cpu":"i5"}}'
go
select * from Products

create or alter proc proc_InsertProduct(@pname nvarchar(100),@pdetails nvarchar(max))
as
begin
    insert into Products(name,details) values(@pname,@pdetails)
end

select JSON_QUERY(details, '$.spec') Product_Specification from products

create proc proc_UpdateProductSpec(@pid int,@newvalue varchar(20))
as
begin
   update products set details = JSON_MODIFY(details, '$.spec.ram',@newvalue) where id = @pid
end

proc_UpdateProductSpec 1, '24GB'

select id, name, JSON_VALUE(details, '$.brand') Brand_Name
from Products

create table Posts
  (id int primary key,
  title nvarchar(100),
  user_id int,
  body nvarchar(max))
Go
	select * from Posts
go
create proc proc_BulInsertPosts(@jsondata nvarchar(max))
as
begin
	insert into Posts(user_id,id,title,body)
	select userId,id,title,body from openjson(@jsondata)
	with (userId int,id int, title varchar(100), body varchar(max))
end

go  
delete from Posts

  proc_BulInsertPosts '
[
  {
    "userId": 1,
    "id": 1,
    "title": "sunt aut facere repellat provident occaecati excepturi optio reprehenderit",
    "body": "quia et suscipit\nsuscipit recusandae consequuntur expedita et cum\nreprehenderit molestiae ut ut quas totam\nnostrum rerum est autem sunt rem eveniet architecto"
  },
  {
    "userId": 1,
    "id": 2,
    "title": "qui est esse",
    "body": "est rerum tempore vitae\nsequi sint nihil reprehenderit dolor beatae ea dolores neque\nfugiat blanditiis voluptate porro vel nihil molestiae ut reiciendis\nqui aperiam non debitis possimus qui neque nisi nulla"
  }]'
  
  select * from products where 
  try_cast(json_value(details,'$.spec.cpu') as nvarchar(20)) ='i7'

