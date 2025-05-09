--List all films with their length and rental rate, sorted by length descending.
--Columns: title, length, rental_rate
SELECT title, length, rental_rate
FROM public.film
ORDER BY length DESC;

--Find the top 5 customers who have rented the most films.
--Hint: Use the rental and customer tables.
SELECT c.customer_id, c.first_name, c.last_name, COUNT(r.rental_id) AS total_rentals
FROM public.rental r
JOIN customer c ON r.customer_id = c.customer_id
GROUP BY c.customer_id, c.first_name, c.last_name
ORDER BY total_rentals DESC
LIMIT 5;

--Display all films that have never been rented.
--Hint: Use LEFT JOIN between film and inventory → rental.
SELECT f.title
FROM public.film f
LEFT JOIN public.inventory i ON f.film_id = i.film_id
LEFT JOIN public.rental r ON i.inventory_id = r.inventory_id
WHERE r.rental_id IS NULL;

--List all actors who appeared in the film ‘Academy Dinosaur’.
--Tables: film, film_actor, actor
SELECT a.actor_id, a.first_name, a.last_name
FROM public.actor a
JOIN public.film_actor fa ON a.actor_id = fa.actor_id
JOIN public.film f ON fa.film_id = f.film_id
WHERE f.title = 'Academy Dinosaur';

--List each customer along with the total number of rentals they made and the total amount paid.
--Tables: customer, rental, payment
SELECT c.customer_id, c.first_name, c.last_name, 
       COUNT(r.rental_id) AS total_rentals, 
       SUM(p.amount) AS total_amount_paid
FROM public.customer c
LEFT JOIN public.rental r ON c.customer_id = r.customer_id
LEFT JOIN public.payment p ON c.customer_id = p.customer_id
GROUP BY c.customer_id, c.first_name, c.last_name
ORDER BY total_amount_paid DESC;

--Using a CTE, show the top 3 rented movies by number of rentals.
--Columns: title, rental_count
WITH MovieRentalCount AS (
    SELECT f.title, COUNT(r.rental_id) AS rental_count
    FROM public.film f
    JOIN public.inventory i ON f.film_id = i.film_id
    JOIN public.rental r ON i.inventory_id = r.inventory_id
    GROUP BY f.title
)

SELECT title, rental_count
FROM MovieRentalCount
ORDER BY rental_count DESC
LIMIT 3;

--Find customers who have rented more than the average number of films.
--Use a CTE to compute the average rentals per customer, then filter.
WITH AvgRentals AS (
    SELECT AVG(rental_count) AS average_rentals
    FROM (
        SELECT customer_id, COUNT(rental_id) AS rental_count
        FROM public.rental
        GROUP BY customer_id
    ) AS rental_summary
)

SELECT c.customer_id, c.first_name, c.last_name, COUNT(r.rental_id) AS total_rentals
FROM public.customer c
JOIN public.rental r ON c.customer_id = r.customer_id
GROUP BY c.customer_id, c.first_name, c.last_name
HAVING COUNT(r.rental_id) > (SELECT average_rentals FROM AvgRentals)
ORDER BY total_rentals DESC;

--Write a function that returns the total number of rentals for a given customer ID.
--Function: get_total_rentals(customer_id INT)
CREATE OR REPLACE FUNCTION get_total_rentals(p_customer_id INT)
RETURNS INT AS $$
DECLARE total_rentals INT;
BEGIN
    SELECT COUNT(r.rental_id) INTO total_rentals
    FROM public.rental r
    WHERE r.customer_id = p_customer_id;
    
    RETURN total_rentals;
END;
$$ LANGUAGE plpgsql;
DROP FUNCTION get_total_rentals(integer);
SELECT get_total_rentals(5);

--Write a stored procedure that updates the rental rate of a film by film ID and new rate.
--Procedure: update_rental_rate(film_id INT, new_rate NUMERIC)
CREATE OR REPLACE PROCEDURE update_rental_rate(
    p_film_id INT,
    p_new_rate NUMERIC
)
LANGUAGE plpgsql
AS $$
BEGIN
    UPDATE public.film
    SET rental_rate = p_new_rate
    WHERE film_id = p_film_id;
    
    RAISE NOTICE 'Rental rate updated for Film ID: %, New Rate: %', p_film_id, p_new_rate;
END;
$$;
CALL update_rental_rate(10, 4.99);

--Write a procedure to list overdue rentals (return date is NULL and rental date older than 7 days).
--Procedure: get_overdue_rentals() that selects relevant columns.
CREATE OR REPLACE PROCEDURE get_overdue_rentals()
LANGUAGE plpgsql
AS $$
DECLARE rec RECORD;
BEGIN
    FOR rec IN 
        SELECT r.rental_id, r.rental_date, c.customer_id, c.first_name, c.last_name
        FROM public.rental r
        JOIN public.customer c ON r.customer_id = c.customer_id
        WHERE r.return_date IS NULL
        AND r.rental_date < NOW() - INTERVAL '7 days'
        ORDER BY r.rental_date
    LOOP
        RAISE NOTICE 'Rental ID: %, Rental Date: %, Customer: % %', rec.rental_id, rec.rental_date, rec.first_name, rec.last_name;
    END LOOP;
END;
$$;

CALL get_overdue_rentals();

-- Cursor-Based Questions (5)
-- Write a cursor that loops through all films and prints titles longer than 120 minutes.
do $$
declare
    cursor_film cursor for select title from film where length > 120;
    title text;
begin
    for title in cursor_film loop
        raise notice 'Title: %', title;
    end loop;
end $$;
 
-- Create a cursor that iterates through all customers and counts how many rentals each made.
do $$
declare
    cursor_rentals cursor for
        select c.customer_id, c.first_name, c.last_name,count(*) as rental_count
        from rental r
        join customer c on c.customer_id = r.customer_id
        group by c.customer_id, c.first_name, c.last_name
        order by c.customer_id;
    rec record;
begin
    for rec in cursor_rentals
    loop
        raise notice 'cust_id: %, first_name: %, last name: %, count: %',
        rec.customer_id, rec.first_name, rec.last_name, rec.rental_count;
    end loop;
end $$;
 
 
-- Using a cursor, update rental rates: Increase rental rate by $1 for films with less than 5 rentals.
do $$
declare
    cursor_rental_update cursor for
        select f.film_id, f.rental_rate from film f
        join
        (
            select film_id, count(rental_id) as rental_count
            from inventory i
            left join rental r on i.inventory_id = r.inventory_id
            group by film_id
        ) rental_stats
        on f.film_id = rental_stats.film_id
        where rental_stats.rental_count < 5;
    rec record;
begin
    for rec in cursor_rental_update loop
        update film
        set rental_rate = rec.rental_rate + 1
        where film_id = rec.film_id;
        raise notice 'Updated film_id: %, new rental_rate: %', rec.film_id, rec.rental_rate + 1;
    end loop;
end $$;
 
 
-- Create a function using a cursor that collects titles of all films from a particular category.
 
create or replace function films_by_category(cname text)
returns void
as
$$
declare
    cursor_films_by_category cursor for
        select f.title, c.name from film_category fc
        join film f on fc.film_id=f.film_id
        join category c on fc.category_id=c.category_id
        where c.name= cname;
    rec record;
begin
    for rec in cursor_films_by_category loop
        raise notice 'Title: %',rec.title;
    end loop;
end;
$$
language plpgsql
 
select * from films_by_category('Comedy');
 
 
-- Loop through all stores and count how many distinct films are available in each store using a cursor.
do
$$
declare
    cursor_films_per_store cursor for
        select store_id, count(film_id) as film_count from inventory
        group by store_id;
    rec record;
begin
    for rec in cursor_films_per_store loop
        raise notice 'store id: %, count: %',rec.store_id, rec.film_count;
    end loop;
end;
$$
  
--Triggers
 
--1 Write a trigger that logs whenever a new customer is inserted.
create or replace function triggerfunc()
returns trigger as $$
begin
	Raise Notice 'New customer with id-% inserted',New.customer_id;
	return New;
end;
$$ language plpgsql;
 
create trigger customerinsert 
after insert on customer
for each row
execute function triggerfunc();
 
insert into customer (store_id, first_name, last_name, email, address_id, activebool, create_date, last_update, active)
values (1, 'Kane', 'Harry', 'Harrykane@tot.com', 100, TRUE, '2025-05-09', '2025-05-09', 1);
 
--2 Create a trigger that prevents inserting a payment of amount 0.
create or replace function payment_trigger_func()
returns trigger as $$
begin
	if New.amount=0.00 then 
		raise exception 'Amount cannot be 0';
	end if;
	return New;
end;
$$ language plpgsql;
 
create or replace trigger paymentinsert 
after insert on payment
for each row
execute function payment_trigger_func();
 
insert into payment (payment_id, customer_id, staff_id, rental_id, amount, payment_date)
VALUES (1, 101, 1, 202, 0.00, '2025-05-09');
--for checking fk in staff table to avoid fk_constarint error
select * from staff;
 
 
--3 Set up a trigger to automatically set last_update on the film table before update.
 
create or replace function last_update_function()
returns trigger as $$
begin
	New.last_update=NOW();
	Raise notice 'Last updated set using trigger';
	return New;
end;
$$ language plpgsql;
 
create or replace trigger on_film_update
after update on film
for each row
execute function last_update_function();
 
select * from film where film_id=2;
update film set title='Ace' where film_id=2;

--4 Create a trigger to log changes in the inventory table (insert/delete).
create or replace function inventory_log()
returns trigger as $$
begin
	if TG_OP='INSERT' then
		Raise notice 'Data inserted to inventory with id %',New.inventory_id;
		return New;
	end if;
	if TG_OP='DELETE' then 
		Raise notice 'Data with id % has been deleted',old.inventory_id;
		return New;
	end if;
	return null;
end;
$$ language plpgsql;
 
create or replace trigger inv_insert
after insert on inventory
for each row 
execute function inventory_log();
 
create or replace trigger inv_delete
after delete on inventory
for each row 
execute function inventory_log();
 
select * from inventory;
select * from rental where inventory_id=4;
delete from payment where rental_id in (10883,14624);
delete from rental where inventory_id=4;
delete from inventory where inventory_id=4;

--Transaction-Based Questions (5)
--Write a transaction that inserts a customer and an initial rental in one atomic operation.
DO $$
DECLARE new_customer_id INT;
BEGIN
    INSERT INTO public.customer (store_id, address_id, first_name, last_name, email, create_date)
    VALUES (1, 1, 'John', 'Doe', 'johndoe@example.com', NOW())
    RETURNING customer_id INTO new_customer_id;

    INSERT INTO public.rental (rental_date, inventory_id, customer_id, staff_id, return_date)
    VALUES (NOW(), 1, new_customer_id, 1, NULL);
END;
$$;

--Simulate a failure in a multi-step transaction (update film + insert into inventory) and roll back.
DO $$
BEGIN
    UPDATE public.film 
    SET rental_rate = 5.99
    WHERE film_id = 1;

    INSERT INTO public.inventory (film_id, store_id) 
    VALUES (99999, 1);

EXCEPTION
    WHEN others THEN
        ROLLBACK;
        RAISE NOTICE 'Transaction rolled back due to error.';
END;
$$;

--Create a transaction that transfers an inventory item from one store to another.
DO $$
DECLARE v_inventory_id INT := 100;
DECLARE v_from_store_id INT := 1;
DECLARE v_to_store_id INT := 2;
BEGIN
    UPDATE public.inventory
    SET store_id = v_to_store_id
    WHERE inventory_id = v_inventory_id
    AND store_id = v_from_store_id;

    IF NOT FOUND THEN
        RAISE EXCEPTION 'Inventory item % not found in store %', v_inventory_id, v_from_store_id;
    END IF;

    RAISE NOTICE 'Inventory item % moved from store % to store %', v_inventory_id, v_from_store_id, v_to_store_id;
END;
$$;

--Demonstrate SAVEPOINT and ROLLBACK TO SAVEPOINT by updating payment amounts, then undoing one.
BEGIN;

UPDATE public.payment
SET amount = amount + 5
WHERE payment_id = 1;

SAVEPOINT payment_update;

UPDATE public.payment
SET amount = amount + 10
WHERE payment_id = 2;

ROLLBACK TO SAVEPOINT payment_update;

COMMIT;

--Write a transaction that deletes a customer and all associated rentals and payments, ensuring atomicity.
--Procedure: get_overdue_rentals() that selects relevant columns.
DO $$
DECLARE v_customer_id INT := 100;
BEGIN
    DELETE FROM public.payment
    WHERE customer_id = v_customer_id;

    DELETE FROM public.rental
    WHERE customer_id = v_customer_id;

    DELETE FROM public.customer
    WHERE customer_id = v_customer_id;
END;
$$;
