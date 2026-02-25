namespace LibraryManagement.Models;

public class Author
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime? BirthDate { get; set; }
    public string Country { get; set; } = string.Empty;

    // Навигационное свойство
    public ICollection<Book> Books { get; set; } = new List<Book>();

    // Вычисляемое свойство для удобного отображения
    public string FullName => $"{FirstName} {LastName}";

    public override string ToString() => FullName;
}
