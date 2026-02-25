using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using LibraryManagement.Data;
using LibraryManagement.Helpers;
using LibraryManagement.Models;
using LibraryManagement.Views;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagement.ViewModels;

public class MainViewModel : INotifyPropertyChanged
{
    private Book? _selectedBook;
    private Author? _selectedFilterAuthor;
    private Genre? _selectedFilterGenre;
    private string _searchText = string.Empty;

    public MainViewModel()
    {
        // Инициализация команд
        AddBookCommand = new RelayCommand(_ => AddBook());
        EditBookCommand = new RelayCommand(_ => EditBook(), _ => SelectedBook != null);
        DeleteBookCommand = new RelayCommand(_ => DeleteBook(), _ => SelectedBook != null);
        ResetFiltersCommand = new RelayCommand(_ => ResetFilters());

        // Загрузка данных
        LoadData();
    }

    // ========== Коллекции ==========

    public ObservableCollection<Book> Books { get; set; } = new();
    public ObservableCollection<Author> Authors { get; set; } = new();
    public ObservableCollection<Genre> Genres { get; set; } = new();

    // ========== Свойства ==========

    public Book? SelectedBook
    {
        get => _selectedBook;
        set { _selectedBook = value; OnPropertyChanged(); }
    }

    public Author? SelectedFilterAuthor
    {
        get => _selectedFilterAuthor;
        set { _selectedFilterAuthor = value; OnPropertyChanged(); ApplyFilters(); }
    }

    public Genre? SelectedFilterGenre
    {
        get => _selectedFilterGenre;
        set { _selectedFilterGenre = value; OnPropertyChanged(); ApplyFilters(); }
    }

    public string SearchText
    {
        get => _searchText;
        set { _searchText = value; OnPropertyChanged(); ApplyFilters(); }
    }

    // ========== Команды ==========

    public ICommand AddBookCommand { get; }
    public ICommand EditBookCommand { get; }
    public ICommand DeleteBookCommand { get; }
    public ICommand ResetFiltersCommand { get; }

    // ========== Загрузка данных ==========

    private void LoadData()
    {
        using var context = new LibraryContext();

        Authors.Clear();
        foreach (var author in context.Authors.OrderBy(a => a.LastName).ToList())
            Authors.Add(author);

        Genres.Clear();
        foreach (var genre in context.Genres.OrderBy(g => g.Name).ToList())
            Genres.Add(genre);

        LoadBooks();
    }

    private void LoadBooks()
    {
        using var context = new LibraryContext();

        IQueryable<Book> query = context.Books
            .Include(b => b.Author)
            .Include(b => b.Genre);

        // Применяем фильтры
        if (SelectedFilterAuthor != null)
            query = query.Where(b => b.AuthorId == SelectedFilterAuthor.Id);

        if (SelectedFilterGenre != null)
            query = query.Where(b => b.GenreId == SelectedFilterGenre.Id);

        if (!string.IsNullOrWhiteSpace(SearchText))
            query = query.Where(b => b.Title.Contains(SearchText) || b.ISBN.Contains(SearchText));

        Books.Clear();
        foreach (var book in query.OrderBy(b => b.Title).ToList())
            Books.Add(book);
    }

    private void ApplyFilters()
    {
        LoadBooks();
    }

    // ========== CRUD-операции ==========

    private void AddBook()
    {
        var viewModel = CreateBookDialogViewModel();
        var dialog = new BookDialogView { DataContext = viewModel };

        viewModel.RequestClose += result => dialog.DialogResult = result;

        if (dialog.ShowDialog() == true)
        {
            using var context = new LibraryContext();
            var book = new Book();
            viewModel.ApplyToBook(book);
            context.Books.Add(book);
            context.SaveChanges();
            LoadBooks();
        }
    }

    private void EditBook()
    {
        if (SelectedBook == null) return;

        var viewModel = CreateBookDialogViewModel();
        viewModel.LoadFromBook(SelectedBook);

        var dialog = new BookDialogView { DataContext = viewModel };
        viewModel.RequestClose += result => dialog.DialogResult = result;

        if (dialog.ShowDialog() == true)
        {
            using var context = new LibraryContext();
            var book = context.Books.Find(SelectedBook.Id);
            if (book != null)
            {
                viewModel.ApplyToBook(book);
                context.SaveChanges();
                LoadBooks();
            }
        }
    }

    private void DeleteBook()
    {
        if (SelectedBook == null) return;

        var result = MessageBox.Show(
            $"Вы уверены, что хотите удалить книгу \"{SelectedBook.Title}\"?",
            "Подтверждение удаления",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            using var context = new LibraryContext();
            var book = context.Books.Find(SelectedBook.Id);
            if (book != null)
            {
                context.Books.Remove(book);
                context.SaveChanges();
                LoadBooks();
            }
        }
    }

    private void ResetFilters()
    {
        _selectedFilterAuthor = null;
        _selectedFilterGenre = null;
        _searchText = string.Empty;

        OnPropertyChanged(nameof(SelectedFilterAuthor));
        OnPropertyChanged(nameof(SelectedFilterGenre));
        OnPropertyChanged(nameof(SearchText));

        LoadBooks();
    }

    private BookDialogViewModel CreateBookDialogViewModel()
    {
        var vm = new BookDialogViewModel();
        foreach (var author in Authors) vm.Authors.Add(author);
        foreach (var genre in Genres) vm.Genres.Add(genre);
        return vm;
    }

    // ========== INotifyPropertyChanged ==========

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
