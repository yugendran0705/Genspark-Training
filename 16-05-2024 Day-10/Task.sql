--phase1
 
--creating tables
CREATE TABLE students (
    student_id SERIAL PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    email VARCHAR(100) UNIQUE NOT NULL,
    phone VARCHAR(15)
);
 
CREATE TABLE courses (
    course_id SERIAL PRIMARY KEY,
    course_name VARCHAR(100) NOT NULL,
    category VARCHAR(50),
    duration_days INTEGER
);
 
CREATE TABLE trainers (
    trainer_id SERIAL PRIMARY KEY,
    trainer_name VARCHAR(100) NOT NULL,
    expertise VARCHAR(100)
);
 
CREATE TABLE enrollments (
    enrollment_id SERIAL PRIMARY KEY,
    student_id INTEGER NOT NULL REFERENCES students(student_id) ON DELETE CASCADE,
    course_id INTEGER NOT NULL REFERENCES courses(course_id) ON DELETE CASCADE,
    enroll_date DATE DEFAULT CURRENT_DATE
);
 
CREATE TABLE certificates (
    certificate_id SERIAL PRIMARY KEY,
    enrollment_id INTEGER NOT NULL REFERENCES enrollments(enrollment_id) ON DELETE CASCADE,
    issue_date DATE DEFAULT CURRENT_DATE,
    serial_no VARCHAR(50) 
);
 
CREATE TABLE course_trainers (
    course_id INTEGER NOT NULL REFERENCES courses(course_id) ON DELETE CASCADE,
    trainer_id INTEGER NOT NULL REFERENCES trainers(trainer_id) ON DELETE CASCADE,
    PRIMARY KEY (course_id, trainer_id)
);


--phase 2
 
--creating indexes
CREATE INDEX indx_student_id ON students(student_id);
CREATE INDEX indx_email ON students(email);
CREATE INDEX indx_course_id ON courses(course_id);
 
--inserting sample data
 
INSERT INTO students (name, email, phone) VALUES
('Alice Johnson', 'alice.johnson@example.com', '1234567890'),
('Bob Smith', 'bob.smith@example.com', '2345678901'),
('Charlie Lee', 'charlie.lee@example.com', '3456789012');
 
INSERT INTO courses (course_name, category, duration_days) VALUES
('Python Programming', 'Programming', 30),
('Data Science Basics', 'Data Science', 45),
('Web Development', 'Web', 40);
 
INSERT INTO trainers (trainer_name, expertise) VALUES
('Dr. Jane Doe', 'Python, AI'),
('Mr. John Davis', 'Web Development'),
('Ms. Emily Clark', 'Data Science');
 
INSERT INTO course_trainers (course_id, trainer_id) VALUES
(1, 1),  
(2, 3), 
(3, 2);  
 
INSERT INTO enrollments (student_id, course_id, enroll_date) VALUES
(1, 1, '2025-01-10'),
(2, 2, '2025-02-15'),
(3, 3, '2025-03-20'),
(1, 3, '2025-04-05');  
 
INSERT INTO certificates (enrollment_id, issue_date, serial_no) VALUES
(1, '2025-02-10', 'CERT-PY-001'),
(2, '2025-03-20', 'CERT-DS-002'),
(3, '2025-04-30', 'CERT-WD-003');
 
--phase 3
 
--1. List students and the courses they enrolled in
 
select e.enroll_date,s.name,c.course_name,c.category from enrollments e join students s on e.student_id=s.student_id join courses c on e.course_id=c.course_id;
 
--2. Show students who received certificates with trainer names
--certifate->enrollment->student,course->course_trainers->trainers
select c.serial_no,s.name,t.trainer_name,e.course_id from certificates c join enrollments e on c.enrollment_id=e.enrollment_id join students s on e.student_id=s.student_id join course_trainers ct on ct.course_id=e.course_id join trainers t on t.trainer_id=ct.trainer_id;
select * from courses;
 
--3. Count number of students per course
 
select c.course_name,count(*) as students_per_course from enrollments e join students s on e.student_id=s.student_id join courses c on e.course_id=c.course_id group by course_name;
 
 
--phase 4
--Create `get_certified_students(course_id INT)`
-- → Returns a list of students who completed the given course and received certificates.
 
create or replace function get_certified_students(cid INT)
returns Table(
	student_id int,
	student_name varchar,
	serial_no varchar
)
as $$
begin 
	return query select s.student_id,s.name,c.serial_no from certificates c
	join enrollments e on c.enrollment_id=e.enrollment_id join students s on s.student_id=e.student_id where e.course_id=cid;
end;
$$ language plpgsql;
 
select * from get_certified_students(3);

--Create `sp_enroll_student(p_student_id, p_course_id)`
-- → Inserts into `enrollments` and conditionally adds a certificate if completed (simulate with status flag).
 
create or replace procedure sp_enroll_student(in p_student_id int,in p_course_id int,in status boolean)
language plpgsql as $$
declare eid int;
begin 
	insert into enrollments(student_id,course_id, enroll_date)
	values(p_student_id,p_course_id,current_date) returning enrollment_id into eid;
	if status then 
		insert into certificates (enrollment_id, issue_date, serial_no)
		values(eid,current_date,'ab123');
	end if;
end;
$$;
 
call sp_enroll_student(3,1,false);
select * from enrollments;
select * from certificates;
 
--phase 5
 
--Use a cursor to:
--* Loop through all students in a course
--* Print name and email of those who do not yet have certificates
select * from students;
select s.name,s.email from enrollments e join students s on e.student_id=s.student_id where e.enrollment_id not in (select enrollment_id from certificates)
 
--cursor
 
do $$
declare 
	cr cursor for select s.name,s.email from enrollments e join students s on e.student_id=s.student_id where e.enrollment_id not in (select enrollment_id from certificates);
	rec record;
begin 
	for rec in cr loop
		raise notice 'Name - % , Email - %',rec.name,rec.email;
	end loop;
end;
$$;

--phase 6
 
CREATE USER readonly_test_user WITH PASSWORD 'password';
CREATE USER data_entry_test_user WITH PASSWORD 'password';
 
CREATE ROLE readonly_user;
GRANT SELECT ON students, courses, certificates TO readonly_user;
 
CREATE ROLE data_entry_user;
GRANT INSERT ON students, enrollments TO data_entry_user;
GRANT USAGE, SELECT ON SEQUENCE students_student_id_seq TO data_entry_user;
GRANT USAGE, SELECT ON SEQUENCE enrollments_enrollment_id_seq TO data_entry_user;
 
GRANT readonly_user TO readonly_test_user;
GRANT data_entry_user TO data_entry_test_user;
 
-- Try running the following insert with readonly_test_user 
-- can switch user with psql -U readonly_test_user -d StudentManagement -W
INSERT INTO students (name, email, phone) VALUES ('Han Lue', 'hanlue@gmail.com', '1234567893');
 
-- Now try with data_entry_user
INSERT INTO students (name, email, phone) VALUES ('Han Lue', 'hanlue@gmail.com', '1234567893');
SELECT * FROM students;
 
-- phase 7

--Write a transaction block that:

-- Enrolls a student
-- Issues a certificate
-- Fails if certificate generation fails (rollback)
create or replace procedure sp_enroll_student2(in p_student_id int,in p_course_id int,in status boolean)
language plpgsql as $$
declare eid int;
begin 
	insert into enrollments(student_id,course_id, enroll_date)
	values(p_student_id,p_course_id,current_date) returning enrollment_id into eid;
	if status then 
		insert into certificates (enrollment_id, issue_date, serial_no)
		values(eid,current_date,CONCAT('CERT-', p_course_id, '-',eid));
	end if;
	exception
	when others then
		rollback;
	commit;
end;
$$;
call sp_enroll_student2(3,3,true);
select * from enrollments;
select * from certificates;