using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using LibraryManagement.Helpers;
using LibraryManagement.Models;

namespace LibraryManagement.ViewModels;

public class BookDialogViewModel : INotifyPropertyChanged
{
    private string _title = string.Empty;
    private int _publishYear = DateTime.Now.Year;
    private string _isbn = string.Empty;
    private int _quantityInStock;
    private Author? _selectedAuthor;
    private Genre? _selectedGenre;
    private string _windowTitle = "Добавить книгу";

    public BookDialogViewModel()
    {
        SaveCommand = new RelayCommand(_ => Save(), _ => CanSave());
        CancelCommand = new RelayCommand(_ => Cancel());
    }

    // ========== Свойства ==========

    public string WindowTitle
    {
        get => _windowTitle;
        set { _windowTitle = value; OnPropertyChanged(); }
    }

    public string Title
    {
        get => _title;
        set { _title = value; OnPropertyChanged(); }
    }

    public int PublishYear
    {
        get => _publishYear;
        set { _publishYear = value; OnPropertyChanged(); }
    }

    public string ISBN
    {
        get => _isbn;
        set { _isbn = value; OnPropertyChanged(); }
    }

    public int QuantityInStock
    {
        get => _quantityInStock;
        set { _quantityInStock = value; OnPropertyChanged(); }
    }

    public Author? SelectedAuthor
    {
        get => _selectedAuthor;
        set { _selectedAuthor = value; OnPropertyChanged(); }
    }

    public Genre? SelectedGenre
    {
        get => _selectedGenre;
        set { _selectedGenre = value; OnPropertyChanged(); }
    }

    public ObservableCollection<Author> Authors { get; set; } = new();
    public ObservableCollection<Genre> Genres { get; set; } = new();

    // ========== Команды ==========

    public ICommand SaveCommand { get; }
    public ICommand CancelCommand { get; }

    // Результат диалога
    public bool? DialogResult { get; private set; }
    public event Action<bool?>? RequestClose;

    // ========== Методы ==========

    private bool CanSave()
    {
        return !string.IsNullOrWhiteSpace(Title)
            && !string.IsNullOrWhiteSpace(ISBN)
            && SelectedAuthor != null
            && SelectedGenre != null
            && PublishYear > 0
            && QuantityInStock >= 0;
    }

    private void Save()
    {
        DialogResult = true;
        RequestClose?.Invoke(true);
    }

    private void Cancel()
    {
        DialogResult = false;
        RequestClose?.Invoke(false);
    }

    /// <summary>
    /// Заполняет ViewModel данными существующей книги для редактирования
    /// </summary>
    public void LoadFromBook(Book book)
    {
        WindowTitle = "Редактировать книгу";
        Title = book.Title;
        PublishYear = book.PublishYear;
        ISBN = book.ISBN;
        QuantityInStock = book.QuantityInStock;
        SelectedAuthor = Authors.FirstOrDefault(a => a.Id == book.AuthorId);
        SelectedGenre = Genres.FirstOrDefault(g => g.Id == book.GenreId);
    }

    /// <summary>
    /// Применяет данные из ViewModel к объекту Book
    /// </summary>
    public void ApplyToBook(Book book)
    {
        book.Title = Title;
        book.PublishYear = PublishYear;
        book.ISBN = ISBN;
        book.QuantityInStock = QuantityInStock;
        book.AuthorId = SelectedAuthor!.Id;
        book.GenreId = SelectedGenre!.Id;
    }

    // ========== INotifyPropertyChanged ==========

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
