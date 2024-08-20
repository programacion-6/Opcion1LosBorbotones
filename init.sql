CREATE TABLE BookGenre (
   id SERIAL PRIMARY KEY,
   name VARCHAR(50) NOT NULL
);

INSERT INTO BookGenre (name) VALUES
 ('Fantasy'),
 ('Thriller'),
 ('Mistery'),
 ('Horror'),
 ('Young Adult'),
 ('Romance');

CREATE TABLE Patron (
    id UUID PRIMARY KEY,
    name VARCHAR(100) NOT NULL,
    membershipNumber INT NOT NULL UNIQUE,
    contactDetails INT NOT NULL
);

CREATE TABLE Book (
  id UUID PRIMARY KEY,
  title VARCHAR(255) NOT NULL,
  author VARCHAR(100) NOT NULL,
  ISBN BIGINT NOT NULL UNIQUE,
  genre INT REFERENCES BookGenre(id),
  publicationYear DATE NOT NULL
);

CREATE TABLE BorrowStatus (
  id SERIAL PRIMARY KEY,
  name VARCHAR(50) NOT NULL
);

INSERT INTO BorrowStatus (name) VALUES
    ('Borrowed'),
    ('Overdue'),
    ('Returned'),
    ('Reserved');

CREATE TABLE Borrow (
    id UUID PRIMARY KEY,
    patron UUID REFERENCES Patron(id),
    book UUID REFERENCES Book(id),
    borrowStatus INT REFERENCES BorrowStatus(id),
    dueDate DATE NOT NULL,
    borrowDate DATE NOT NULL
);

INSERT INTO Book (id, title, author, ISBN, genre, publicationYear)
SELECT
    gen_random_uuid(),
    (ARRAY[
     'The Great Gatsby',
     'To Kill a Mockingbird',
     '1984',
     'Pride and Prejudice',
     'The Catcher in the Rye',
     'Moby-Dick',
     'War and Peace',
     'The Odyssey',
     'The Hobbit',
     'The Lord of the Rings',
     'Jane Eyre',
     'The Picture of Dorian Gray',
     'Crime and Punishment',
     'Brave New World',
     'The Kite Runner',
     'The Road',
     'Fahrenheit 451',
     'The Grapes of Wrath',
     'Wuthering Heights',
     'Catch-22'
    ])[floor(random() * 20 + 1)],
    (ARRAY[
     'George Orwell',
     'Harper Lee',
     'F. Scott Fitzgerald',
     'Jane Austen',
     'J.D. Salinger',
     'Herman Melville',
     'Leo Tolstoy',
     'Homer',
     'J.R.R. Tolkien',
     'Charlotte Brontë',
     'Oscar Wilde',
     'Fyodor Dostoevsky',
     'Aldous Huxley',
     'Khaled Hosseini',
     'Cormac McCarthy',
     'Ray Bradbury',
     'John Steinbeck',
     'Emily Brontë',
     'Joseph Heller'
    ])[floor(random() * 19 + 1)],
    floor(random() * 9000000000000 + 1000000000000)::bigint,
    floor(random() * 6 + 1)::int,
    timestamp '2020-01-01' + random() * (timestamp '2023-12-31' - timestamp '2020-01-01')
FROM generate_series(1, 20);

INSERT INTO Patron (id, name, membershipNumber, contactDetails)
SELECT
    gen_random_uuid(),
    (ARRAY[
     'John Smith',
     'Jane Doe',
     'Emily Johnson',
     'Michael Brown',
     'Jessica Williams',
     'David Jones',
     'Sarah Davis',
     'Daniel Garcia',
     'Laura Miller',
     'James Wilson',
     'Sophia Moore',
     'Matthew Taylor',
     'Olivia Anderson',
     'Ethan Thomas',
     'Ava Martinez',
     'William Hernandez',
     'Mia Robinson',
     'Benjamin Clark',
     'Isabella Lewis',
     'Lucas Walker'
    ])[floor(random() * 20 + 1)],
    trunc(random() * 100000)::int,
    trunc(random() * 100000)::int
FROM generate_series(1, 20);

INSERT INTO Borrow (id, patron, book, borrowStatus, dueDate, borrowDate)
SELECT
    gen_random_uuid(),
    (SELECT id FROM Patron ORDER BY random() LIMIT 1),
    (SELECT id FROM Book ORDER BY random() LIMIT 1),
    floor(random() * 4 + 1)::int,
    timestamp '2023-01-01' + random() * (timestamp '2023-12-31' - timestamp '2023-01-01'),
    timestamp '2022-01-01' + random() * (timestamp '2023-01-01' - timestamp '2022-01-01')
FROM generate_series(1, 10);
