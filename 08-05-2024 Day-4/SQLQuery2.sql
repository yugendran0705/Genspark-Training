-- 1) List all orders with the customer name and the employee who handled the order.
select	o.*, c.CompanyName, e.FirstName as Employee from Customers c join Orders o on 
c.CustomerID = o.CustomerID join Employees e on e.EmployeeID = o.EmployeeID;

-- 2) Get a list of products along with their category and supplier name.
select	p.*, c.CategoryName, s.CompanyName as Supplier from Products p join Suppliers s on 
p.SupplierID = s.SupplierID join Categories c on c.CategoryID = p.CategoryID;

--3) Show all orders and the products included in each order with quantity and unit price.
select o.*, od.Quantity, od.UnitPrice , p.ProductName from Orders o join [Order Details] od on 
o.OrderID = od.OrderID join Products p on p.ProductID = od.ProductID;

-- 4) List employees who report to other employees (manager-subordinate relationship).
select concat(e1.FirstName,' ',e1.LastName) as Employee_Name, concat(e2.FirstName,' ',e2.LastName) as Reports_to
from Employees e1
join Employees e2 on e1.ReportsTo=e2.EmployeeID;
 
-- 5) Display each customer and their total order count.
select o.CustomerId, c.CompanyName, o.ordercount
from (select CustomerID, count(*)as ordercount from Orders group by CustomerID) o
join Customers c on o.CustomerID = c.CustomerID
  
-- 6) Find the average unit price of products per category.
select p.CategoryID, c.CategoryName,avg(UnitPrice) as Average_Unit_Price
from Products p join Categories c on p.CategoryID=c.CategoryID
group by p.CategoryID , c.CategoryName
  
-- 7) List customers where the contact title starts with 'Owner'.
select * from customers where ContactTitle like 'Owner';
  
-- 8) Show the top 5 most expensive products.
select top 5 * from Products order by UnitPrice desc;

--9) Return the total sales amount (quantity × unit price) per order.
select o.OrderID, sum(od.Quantity*od.UnitPrice)as Total from Orders o
join [Order Details] od on o.OrderID=od.OrderID
group by o.OrderID;
 
 
-- 10) Create a stored procedure that returns all orders for a given customer ID 
-- Input: @CustomerID
create or alter procedure proc_GetOrderByCustomerId (@Cusid nvarchar(20))
as
begin
    select * from Orders where CustomerID=@Cusid
end
go
proc_GetOrderByCustomerId 'VINET' 
 
-- 11) Write a stored procedure that inserts a new product.
-- Inputs: ProductName, SupplierID, CategoryID, UnitPrice, etc.
create or alter procedure proc_AddProduct
(@name nvarchar(20), @sid int,@cid int, @qnt nvarchar(100), @up int, @uis int, @uoo int, @rol int, @discontinued int)
as
begin
    insert into Products(ProductName,SupplierID,CategoryID,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued)
    values (@name,@sid,@cid,@qnt,@up,@uis,@uoo,@rol,@discontinued)
end
 
proc_AddProduct 'Tea', 1,1,'10 boxes x 20 bags', 20.0, 40, 0, 10,0
 
--12) Create a stored procedure that returns total sales per employee.
--Join Orders, Order Details, and Employees
create or alter proc gettotalsales
as 
begin
	select a.EmployeeId,sum(c.UnitPrice*c.Quantity) as totalsales from Employees a join Orders b on a.EmployeeID=b.EmployeeID join [Order Details] c on c.OrderID=b.OrderID group by a.EmployeeID;
end
 
gettotalsales

-- 13) Use a CTE to rank products by unit price within each category. 
-- Use ROW_NUMBER() or RANK() with PARTITION BY CategoryID
WITH ProductRanking AS (
    SELECT ProductID, ProductName, CategoryID, UnitPrice,
           ROW_NUMBER() OVER (PARTITION BY CategoryID ORDER BY UnitPrice DESC) AS Rank
    FROM Products
)

SELECT * FROM ProductRanking;

WITH ProductRanking AS (
    SELECT ProductID, ProductName, CategoryID, UnitPrice,
           RANK() OVER (PARTITION BY CategoryID ORDER BY UnitPrice DESC) AS Rank
    FROM Products
)

SELECT * FROM ProductRanking;

-- 14) Create a CTE to calculate total revenue per product and filter products with revenue > 10,000.
WITH ProductRevenue AS (
    SELECT p.ProductID, p.ProductName, SUM(od.Quantity * p.UnitPrice) AS TotalRevenue
    FROM [Order Details] od
    INNER JOIN Products p ON od.ProductID = p.ProductID
    GROUP BY p.ProductID, p.ProductName
)

SELECT ProductID, ProductName, TotalRevenue
FROM ProductRevenue
WHERE TotalRevenue > 10000
ORDER BY TotalRevenue DESC;

--15) Use a CTE with recursion to display employee hierarchy.
--Start from top-level employee (ReportsTo IS NULL) and drill down
WITH EmployeeHierarchy AS (
    SELECT EmployeeID, FirstName, LastName, ReportsTo, 0 AS hierarchy_level
    FROM Employees
    WHERE ReportsTo IS NULL

    UNION ALL

    SELECT e.EmployeeID, e.FirstName, e.LastName, e.ReportsTo, eh.hierarchy_level + 1
    FROM Employees e
    INNER JOIN EmployeeHierarchy eh ON e.ReportsTo = eh.EmployeeID
)

SELECT EmployeeID, FirstName, LastName, ReportsTo, hierarchy_level
FROM EmployeeHierarchy
ORDER BY hierarchy_level, EmployeeID;
