using LibraryManagement.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.Data;

public static class DbInitializer
{
    public static void Initialize(LibraryContext context)
    {
        // Создаём базу данных, если она не существует
        context.Database.EnsureCreated();

        // Если данные уже есть — выходим
        if (context.Authors.Any())
            return;

        // ========== Авторы ==========
        var authors = new Author[]
        {
            new() { FirstName = "Лев", LastName = "Толстой", BirthDate = new DateTime(1828, 9, 9), Country = "Россия" },
            new() { FirstName = "Фёдор", LastName = "Достоевский", BirthDate = new DateTime(1821, 11, 11), Country = "Россия" },
            new() { FirstName = "Александр", LastName = "Пушкин", BirthDate = new DateTime(1799, 6, 6), Country = "Россия" },
            new() { FirstName = "Джордж", LastName = "Оруэлл", BirthDate = new DateTime(1903, 6, 25), Country = "Великобритания" },
            new() { FirstName = "Габриэль", LastName = "Гарсиа Маркес", BirthDate = new DateTime(1927, 3, 6), Country = "Колумбия" },
        };
        context.Authors.AddRange(authors);
        context.SaveChanges();

        // ========== Жанры ==========
        var genres = new Genre[]
        {
            new() { Name = "Роман", Description = "Крупная форма эпической прозы" },
            new() { Name = "Поэзия", Description = "Художественная литература в стихотворной форме" },
            new() { Name = "Антиутопия", Description = "Жанр, описывающий тоталитарное общество будущего" },
            new() { Name = "Магический реализм", Description = "Жанр, сочетающий реалистичное повествование с магическими элементами" },
            new() { Name = "Философский роман", Description = "Роман, основная проблематика которого — философские вопросы" },
        };
        context.Genres.AddRange(genres);
        context.SaveChanges();

        // ========== Книги ==========
        var books = new Book[]
        {
            new() { Title = "Война и мир", AuthorId = authors[0].Id, GenreId = genres[0].Id, PublishYear = 1869, ISBN = "978-5-17-084879-3", QuantityInStock = 5 },
            new() { Title = "Анна Каренина", AuthorId = authors[0].Id, GenreId = genres[0].Id, PublishYear = 1877, ISBN = "978-5-17-090012-5", QuantityInStock = 3 },
            new() { Title = "Преступление и наказание", AuthorId = authors[1].Id, GenreId = genres[4].Id, PublishYear = 1866, ISBN = "978-5-17-097420-1", QuantityInStock = 4 },
            new() { Title = "Братья Карамазовы", AuthorId = authors[1].Id, GenreId = genres[4].Id, PublishYear = 1880, ISBN = "978-5-17-090555-7", QuantityInStock = 2 },
            new() { Title = "Евгений Онегин", AuthorId = authors[2].Id, GenreId = genres[1].Id, PublishYear = 1833, ISBN = "978-5-17-082345-5", QuantityInStock = 6 },
            new() { Title = "1984", AuthorId = authors[3].Id, GenreId = genres[2].Id, PublishYear = 1949, ISBN = "978-5-17-080214-6", QuantityInStock = 7 },
            new() { Title = "Скотный двор", AuthorId = authors[3].Id, GenreId = genres[2].Id, PublishYear = 1945, ISBN = "978-5-17-093177-7", QuantityInStock = 4 },
            new() { Title = "Сто лет одиночества", AuthorId = authors[4].Id, GenreId = genres[3].Id, PublishYear = 1967, ISBN = "978-5-17-058700-5", QuantityInStock = 3 },
        };
        context.Books.AddRange(books);
        context.SaveChanges();
    }
}
