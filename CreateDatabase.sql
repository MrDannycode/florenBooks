-- ============================================================
-- florenBooks - Library Management System
-- Script creare baza de date SQL Server (LocalDB)
-- ============================================================
-- UTILIZARE:
--   1. Deschide SQL Server Management Studio (SSMS) sau
--      Visual Studio → View → SQL Server Object Explorer
--   2. Conecteaza-te la: (LocalDB)\MSSQLLocalDB
--   3. Ruleaza acest script
--   4. Copiaza fisierele library.mdf si library_log.ldf
--      in folderul bin\Debug\net8.0-windows\ al proiectului
-- ============================================================

-- Creaza baza de date (schimba calea daca este necesar)
CREATE DATABASE library
ON PRIMARY (
    NAME = library,
    FILENAME = 'C:\Users\user\Downloads\dan\cti\florenBooks\library.mdf',
    SIZE = 8MB,
    MAXSIZE = UNLIMITED,
    FILEGROWTH = 64MB
)
LOG ON (
    NAME = library_log,
    FILENAME = 'C:\Users\user\Downloads\dan\cti\florenBooks\library_log.ldf',
    SIZE = 8MB,
    MAXSIZE = 2GB,
    FILEGROWTH = 64MB
);
GO

USE library;
GO

-- ============================================================
-- TABELA: book - Catalogul cartilor
-- ============================================================
CREATE TABLE book (
    id        INT           IDENTITY(1,1) PRIMARY KEY,
    title     NVARCHAR(200) NOT NULL,
    author    NVARCHAR(150) NOT NULL,
    publisher NVARCHAR(150),
    year      NVARCHAR(10),
    isbn      NVARCHAR(20),
    category  NVARCHAR(100),
    quantity  INT           NOT NULL DEFAULT 1,
    price     DECIMAL(10,2) DEFAULT 0.00,
    shelf     NVARCHAR(20)
);
GO

-- ============================================================
-- TABELA: member - Membrii bibliotecii
-- ============================================================
CREATE TABLE member (
    id          INT           IDENTITY(1,1) PRIMARY KEY,
    name        NVARCHAR(150) NOT NULL,
    gender      NVARCHAR(20),
    phone       NVARCHAR(20),
    email       NVARCHAR(150),
    address     NVARCHAR(300),
    date_joined NVARCHAR(20),
    member_type NVARCHAR(50),   -- Student / Profesor / Public
    max_books   INT           DEFAULT 3
);
GO

-- ============================================================
-- TABELA: book_issue - Imprumuturi
-- ============================================================
CREATE TABLE book_issue (
    id           INT            IDENTITY(1,1) PRIMARY KEY,
    member_id    INT            NOT NULL,
    member_name  NVARCHAR(150),
    book_id      INT            NOT NULL,
    book_title   NVARCHAR(200),
    book_author  NVARCHAR(150),
    issue_date   NVARCHAR(20),
    due_date     NVARCHAR(20),
    return_date  NVARCHAR(20),
    fine_per_day DECIMAL(10,2)  DEFAULT 1.00,
    total_fine   DECIMAL(10,2)  DEFAULT 0.00,
    status       NVARCHAR(30)   DEFAULT 'Imprumutat'   -- Imprumutat / Returnat
);
GO

-- ============================================================
-- DATE DE TEST - Carti
-- ============================================================
INSERT INTO book (title, author, publisher, year, isbn, category, quantity, price, shelf)
VALUES
    (N'Ion',                         N'Liviu Rebreanu',      N'Minerva',        '1920', '978-973-21-0001-1', N'Roman',      5,  25.00, 'A1'),
    (N'Miorita',                     N'Folclor',             N'Humanitas',      '2005', '978-973-50-0002-2', N'Poezie',     3,  15.00, 'A2'),
    (N'Enigma Otiliei',              N'George Calinescu',    N'Minerva',        '1938', '978-973-21-0003-3', N'Roman',      4,  30.00, 'A3'),
    (N'Morometii',                   N'Marin Preda',         N'Cartea Romaneasca','1955','978-973-23-0004-4',N'Roman',      6,  28.00, 'B1'),
    (N'Harap-Alb',                   N'Ion Creanga',         N'Polirom',        '1877', '978-973-46-0005-5', N'Povesti',    8,  12.00, 'B2');
GO

-- ============================================================
-- DATE DE TEST - Membri
-- ============================================================
INSERT INTO member (name, gender, phone, email, address, date_joined, member_type, max_books)
VALUES
    (N'Maria Popescu',   N'Feminin',  '0721000001', 'maria.p@email.ro', N'Str. Florilor 1, Cluj',    '01/01/2025', N'Student',  5),
    (N'Ion Ionescu',     N'Masculin', '0731000002', 'ion.i@email.ro',   N'Bd. Unirii 10, Bucuresti', '15/02/2025', N'Profesor', 10),
    (N'Ana Constantin',  N'Feminin',  '0741000003', 'ana.c@email.ro',   N'Calea Victoriei 5, Iasi',  '01/03/2025', N'Public',   3);
GO

PRINT 'Baza de date florenBooks creata cu succes!';
GO
