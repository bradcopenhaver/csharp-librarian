# SQL Library Manager

#### Create and manage library inventory and users. {December 2016}

#### By **Brad Copenhaver and Erik Killops**

## Description


### Specifications

| BEHAVIOR                          | INPUT                                       | OUTPUT                   |
|-----------------------------------|---------------------------------------------|--------------------------|
| Add and hold books in inventory   | Add "Moby Dick"                             | Added to inventory       |
| Remove books from inventory       | Remove "Moby Dick"                          | Removed from inventory   |
| Update book info                  | Add author "Herman Melville" to "Moby Dick" | Author added             |
| Manage multiple copies of books   | Add additional copy of "Moby Dick"          | 2 copies in stock        |
| Allow users to become members     | Create account for "John Smith"             | Account created          |
| Allow members to check out a book | Loan "Moby Dick" to "John Smith"            | "Moby Dick" checked out. |
|                                   |                                             |                          |

## Setup/Installation Requirements

1. Clone this GitHub repository.
2. From the command prompt, run '>SqlLocalDb.exe c MSSQLLocalDB -s' to create an instance of LocalDB.
3. Run the command '>sqlcmd -S "(localdb)\\MSSQLLocalDB"' and run the following SQL commands to create the local database and tables:

        >CREATE DATABASE librarian
        >GO
        >USE librarian
        >GO
        >
        >CREATE TABLE books (
        >id int IDENTITY (1, 1) PRIMARY KEY,
        >title varchar(255),
        >copies int,
        >checked_in int,
        >checked_out int
        >)
        >GO
        >
        >CREATE TABLE authors (
        >id int IDENTITY (1, 1) PRIMARY KEY,
        >name varchar(255)
        >)
        >GO
        >
        >CREATE TABLE authors_books (
        >id int IDENTITY (1, 1) PRIMARY KEY,
        >author_id int,
        >book_id int
        >)
        >GO
        >
        >CREATE TABLE members (
        >id int IDENTITY (1, 1) PRIMARY KEY,
        >name varchar(255)
        >)
        >GO
        >
        >CREATE TABLE checkouts (
        >id int IDENTITY (1, 1) PRIMARY KEY,
        >book_id int,
        >member_id int,
        >due_date datetime,
        >returned bit
        >)
        >GO

4. Navigate to the repository in terminal and run the command >dnu restore
5. In the same location, create a local server by running the command >dnx kestrel
6. Open a web browser and navigate to localhost:5004 to view the app.

## Known Bugs

None yet.


## Support and contact details

If you have questions or comments, contact the authors at bradcopenhaver@gmail.com or erik.killops@gmail.com

## Technologies Used

* C#
* SQL
* Nancy framework
* Razor view engine
* html/css
* Bootstrap

### License

This project is licensed under the MIT license.

Copyright (c) 2016 **Brad Copenhaver, Erik Killops**
