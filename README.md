# 📚 LibraryManagement — Управление библиотекой книг

WPF-приложение для управления небольшой библиотекой с использованием **Entity Framework Core** и паттерна **MVVM**.

---

## 🎯 Возможности

- Просмотр каталога книг в табличном виде (DataGrid)
- Добавление, редактирование и удаление книг
- Фильтрация по автору и жанру
- Поиск по названию и ISBN
- Автоматическое создание SQLite-базы с тестовыми данными при первом запуске

---

## 🏗️ Архитектура проекта

```
LibraryManagement/
├── LibraryManagement.sln
├── .gitignore
└── LibraryManagement/
    ├── App.xaml / App.xaml.cs          — Точка входа, инициализация БД
    ├── Models/
    │   ├── Author.cs                   — Сущность "Автор"
    │   ├── Genre.cs                    — Сущность "Жанр"
    │   └── Book.cs                     — Сущность "Книга"
    ├── Data/
    │   ├── LibraryContext.cs            — DbContext с Fluent API
    │   └── DbInitializer.cs            — Заполнение начальными данными
    ├── ViewModels/
    │   ├── MainViewModel.cs            — Логика главного окна
    │   └── BookDialogViewModel.cs      — Логика диалога добавления/редактирования
    ├── Views/
    │   ├── MainWindow.xaml / .xaml.cs   — Главное окно
    │   └── BookDialogView.xaml / .xaml.cs — Диалоговое окно книги
    ├── Helpers/
    │   └── RelayCommand.cs             — Реализация ICommand для MVVM
    └── LibraryManagement.csproj
```

---

## 🔧 Требования

| Компонент | Версия |
|-----------|--------|
| .NET SDK | 8.0+ |
| ОС | Windows 10/11 |
| IDE | Visual Studio 2022 / Rider / VS Code |

---

## 🚀 Способы сборки и запуска

### Способ 1 — Visual Studio 2022 (рекомендуемый)

1. Откройте `LibraryManagement.sln` в Visual Studio
2. Дождитесь восстановления NuGet-пакетов (происходит автоматически)
3. Нажмите **F5** или кнопку **▶ Запуск**

### Способ 2 — .NET CLI (без Visual Studio)

Установите [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0), затем:

```bash
cd LibraryManagement
dotnet restore
dotnet build
dotnet run --project LibraryManagement
```

### Способ 3 — JetBrains Rider

1. Откройте папку проекта или `.sln`-файл
2. Rider автоматически восстановит пакеты
3. Нажмите **Shift+F10** для запуска

### Способ 4 — VS Code

1. Установите расширение [C# Dev Kit](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csdevkit)
2. Откройте папку проекта
3. Запустите через терминал: `dotnet run --project LibraryManagement`

---

## 🌐 Запуск онлайн (без локальной установки)

> **Важно:** WPF — это десктопный Windows-фреймворк, поэтому полноценных онлайн-сред для его запуска не существует. Однако есть несколько рабочих вариантов:

### GitHub Codespaces — сборка и просмотр кода

Можно собрать проект и убедиться, что он компилируется, но GUI не запустится (нет Windows GUI в контейнере):

```
https://github.com/codespaces
```

1. Загрузите проект в GitHub-репозиторий
2. Создайте Codespace
3. Выполните `dotnet build` — проверит компиляцию

### Microsoft Dev Box — полноценный запуск

[Microsoft Dev Box](https://azure.microsoft.com/products/dev-box/) предоставляет облачную Windows-машину с Visual Studio:

1. Настройте Dev Box через Azure Portal
2. Подключитесь через браузер или RDP
3. Клонируйте репозиторий и запустите как обычно

### GitHub + Windows VM

Бесплатный вариант для демонстрации:

1. Создайте GitHub Actions workflow с `windows-latest`
2. Соберите проект в CI — это подтверждает работоспособность кода
3. Артефакты сборки можно скачать

Пример `.github/workflows/build.yml`:

```yaml
name: Build

on: [push]

jobs:
  build:
    runs-on: windows-latest
    steps:
      - uses: actions/checkout@v4
      - uses: actions/setup-dotnet@v4
        with:
          dotnet-version: '8.0.x'
      - run: dotnet restore
      - run: dotnet build --configuration Release
      - run: dotnet publish -c Release -o ./publish
      - uses: actions/upload-artifact@v4
        with:
          name: LibraryManagement
          path: ./publish/
```

---

## 📊 Модель данных

```
┌──────────────┐       ┌──────────────┐       ┌──────────────┐
│    Author     │       │     Book     │       │    Genre     │
├──────────────┤       ├──────────────┤       ├──────────────┤
│ Id           │──┐    │ Id           │    ┌──│ Id           │
│ FirstName    │  │    │ Title        │    │  │ Name         │
│ LastName     │  └───▶│ AuthorId (FK)│    │  │ Description  │
│ BirthDate    │       │ GenreId  (FK)│◀───┘  └──────────────┘
│ Country      │       │ PublishYear   │
└──────────────┘       │ ISBN          │
                       │ QuantityInStock│
                       └──────────────┘

        1 : N                    N : 1
   Author ──── Book ──── Genre
```

**Связи:** один автор → много книг, один жанр → много книг. Каскадное удаление настроено через Fluent API.

---

## 📦 Используемые NuGet-пакеты

| Пакет | Назначение |
|-------|-----------|
| `Microsoft.EntityFrameworkCore` 8.0 | ORM-фреймворк |
| `Microsoft.EntityFrameworkCore.Sqlite` 8.0 | Провайдер SQLite |
| `Microsoft.EntityFrameworkCore.Tools` 8.0 | Инструменты миграций |

---

## 📝 Примечания

- База данных `library.db` создаётся автоматически в директории запуска при первом старте
- Тестовые данные включают 5 авторов, 5 жанров и 8 книг русской и мировой классики
- Для сброса базы данных удалите файл `library.db` и перезапустите приложение
