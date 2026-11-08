using System;
using System.Collections.Generic;

// ===== Книга =====
class Book
{
    // Інкапсуляція: поля приховані
    private int id;
    private string title;
    private string author;
    private bool isAvailable;

    public Book(int id, string title, string author)
    {
        this.id = id;
        this.title = title;
        this.author = author;
        this.isAvailable = true; // нова книга одразу доступна
    }

    // Тільки читання
    public int Id { get { return id; } }
    public string Title { get { return title; } }
    public string Author { get { return author; } }
    public bool IsAvailable { get { return isAvailable; } }

    // Зміна стану книги тільки через методи
    public void Take()
    {
        isAvailable = false;
    }

    public void Return()
    {
        isAvailable = true;
    }

    public void ShowInfo()
    {
        string status = isAvailable ? "доступна" : "в оренді";
        Console.WriteLine("  [" + id + "] \"" + title + "\" - " + author + " (" + status + ")");
    }
}

// ===== Користувач =====
class User
{
    private int id;
    private string name;
    private List<Book> borrowedBooks = new List<Book>();

    public User(int id, string name)
    {
        this.id = id;
        this.name = name;
    }

    public int Id { get { return id; } }
    public string Name { get { return name; } }
    public int BorrowedCount { get { return borrowedBooks.Count; } }

    // Чи є ця книга у користувача
    public bool HasBook(Book book)
    {
        return borrowedBooks.Contains(book);
    }

    public void AddBook(Book book)
    {
        borrowedBooks.Add(book);
    }

    public void RemoveBook(Book book)
    {
        borrowedBooks.Remove(book);
    }

    public void ShowInfo()
    {
        Console.WriteLine("  Користувач [" + id + "] " + name + ", книг в оренді: " + borrowedBooks.Count);
        foreach (Book book in borrowedBooks)
        {
            Console.WriteLine("      - \"" + book.Title + "\" (" + book.Author + ")");
        }
    }
}

// ===== Бібліотека =====
class Library
{
    private List<Book> books = new List<Book>();
    private List<User> users = new List<User>();

    // Додавання книги (ID має бути унікальним)
    public void AddBook(Book book)
    {
        if (FindBook(book.Id) != null)
        {
            Console.WriteLine("Помилка: книга з ID " + book.Id + " вже існує.");
            return;
        }
        books.Add(book);
    }

    // Додавання користувача (ID має бути унікальним)
    public void AddUser(User user)
    {
        if (FindUser(user.Id) != null)
        {
            Console.WriteLine("Помилка: користувач з ID " + user.Id + " вже існує.");
            return;
        }
        users.Add(user);
    }

    // Пошук за ID (повертає null, якщо не знайдено)
    private Book FindBook(int id)
    {
        foreach (Book book in books)
        {
            if (book.Id == id)
            {
                return book;
            }
        }
        return null;
    }

    private User FindUser(int id)
    {
        foreach (User user in users)
        {
            if (user.Id == id)
            {
                return user;
            }
        }
        return null;
    }

    // Видача книги
    public void BorrowBook(int userId, int bookId)
    {
        User user = FindUser(userId);
        if (user == null)
        {
            Console.WriteLine("Помилка: користувача з ID " + userId + " не існує.");
            return;
        }

        Book book = FindBook(bookId);
        if (book == null)
        {
            Console.WriteLine("Помилка: книги з ID " + bookId + " не існує.");
            return;
        }

        if (!book.IsAvailable)
        {
            Console.WriteLine("Помилка: книга \"" + book.Title + "\" вже в оренді.");
            return;
        }

        book.Take();
        user.AddBook(book);
        Console.WriteLine("Успіх: " + user.Name + " взяв(ла) книгу \"" + book.Title + "\".");
    }

    // Повернення книги
    public void ReturnBook(int userId, int bookId)
    {
        User user = FindUser(userId);
        if (user == null)
        {
            Console.WriteLine("Помилка: користувача з ID " + userId + " не існує.");
            return;
        }

        Book book = FindBook(bookId);
        if (book == null)
        {
            Console.WriteLine("Помилка: книги з ID " + bookId + " не існує.");
            return;
        }

        if (!user.HasBook(book))
        {
            Console.WriteLine("Помилка: " + user.Name + " не орендував(ла) книгу \"" + book.Title + "\".");
            return;
        }

        book.Return();
        user.RemoveBook(book);
        Console.WriteLine("Успіх: " + user.Name + " повернув(ла) книгу \"" + book.Title + "\".");
    }

    // ----- Перегляд інформації -----
    public void ShowAllBooks()
    {
        Console.WriteLine("Усі книги:");
        foreach (Book book in books)
        {
            book.ShowInfo();
        }
    }

    public void ShowAvailableBooks()
    {
        Console.WriteLine("Доступні книги:");
        int count = 0;
        foreach (Book book in books)
        {
            if (book.IsAvailable)
            {
                book.ShowInfo();
                count++;
            }
        }
        if (count == 0)
        {
            Console.WriteLine("  (немає доступних книг)");
        }
    }

    public void ShowBorrowedBooks()
    {
        Console.WriteLine("Книги в оренді:");
        int count = 0;
        foreach (User user in users)
        {
            if (user.BorrowedCount > 0)
            {
                user.ShowInfo();
                count++;
            }
        }
        if (count == 0)
        {
            Console.WriteLine("  (жодна книга не в оренді)");
        }
    }

    public void ShowUsers()
    {
        Console.WriteLine("Користувачі:");
        foreach (User user in users)
        {
            user.ShowInfo();
        }
    }
}

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Library library = new Library();

        // Початкове наповнення: книги
        library.AddBook(new Book(1, "Кобзар", "Тарас Шевченко"));
        library.AddBook(new Book(2, "Лісова пісня", "Леся Українка"));
        library.AddBook(new Book(3, "Захар Беркут", "Іван Франко"));
        library.AddBook(new Book(4, "Тіні забутих предків", "Михайло Коцюбинський"));

        // Початкове наповнення: користувачі
        library.AddUser(new User(1, "Олена"));
        library.AddUser(new User(2, "Андрій"));
        library.AddUser(new User(3, "Марія"));

        Console.WriteLine("=== 1. Початковий стан ===");
        library.ShowAllBooks();
        Console.WriteLine();
        library.ShowUsers();

        Console.WriteLine("\n=== 2. Перегляд доступних книг ===");
        library.ShowAvailableBooks();

        Console.WriteLine("\n=== 3. Видача книги користувачу ===");
        library.BorrowBook(1, 1); // Олена бере "Кобзар"

        Console.WriteLine("\n=== 4. Спроба повторно видати вже орендовану книгу ===");
        library.BorrowBook(2, 1); // Андрій намагається взяти ту саму книгу

        Console.WriteLine("\n=== 5. Книга, якої не існує ===");
        library.BorrowBook(2, 99);

        Console.WriteLine("\n=== 6. Стан після видачі ===");
        library.ShowAvailableBooks();
        Console.WriteLine();
        library.ShowBorrowedBooks();

        Console.WriteLine("\n=== 7. Спроба повернути книгу, яку користувач не брав ===");
        library.ReturnBook(2, 1); // Андрій не брав "Кобзар"

        Console.WriteLine("\n=== 8. Повернення книги ===");
        library.ReturnBook(1, 1); // Олена повертає "Кобзар"

        Console.WriteLine("\n=== 9. Повторна перевірка доступності ===");
        library.ShowAvailableBooks();
        Console.WriteLine();
        library.ShowBorrowedBooks();
    }
}
