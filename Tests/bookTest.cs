using Xunit;
using System;
using System.Collections.Generic;
using Librarian.Objects;

namespace  Librarian
{
  public class BookTest : IDisposable
  {
    public void Dispose()
    {
      Author.DeleteAll();
      Book.DeleteAll();
    }
    public BookTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=librarian_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void GetAll_DatabaseEmpty_true()
    {
      //Arrange
      //Act
      int result = Book.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_EqualOverride_True()
    {
      //Arrange and Act
      Book firstBook = new Book("Moby Dick", 1);
      Book secondBook = new Book("Moby Dick", 1);
      //Assert
      Assert.Equal(firstBook,secondBook);
    }
    [Fact]
    public void Save_SavesBookToDatabase_true()
    {
      //Arrange
      Book newBook = new Book("Moby Dick", 1);
      //Act
      newBook.Save();
      List<Book> allBooks = Book.GetAll();
      //Assert
      Assert.Equal(newBook, allBooks[0]);
    }
    [Fact]
    public void Find_FindsBookById_true()
    {
      //Arrange
      Book newBook = new Book("A Tale of Two Cities", 2);
      newBook.Save();
      //Act
      Book foundBook = Book.Find(newBook.id);
      //Assert
      Assert.Equal(newBook, foundBook);
    }
    [Fact]
    public void UpdateProperty_UpdatesValues_true()
    {
      //Arrange
      Book newBook = new Book("book1", 2);
      newBook.Save();
      //Act
      newBook.UpdateProperty("title", "book3");
      Book foundBook = Book.Find(newBook.id);
      Book expectedResult = new Book(newBook.title, newBook.copies, newBook.checkedIn, newBook.checkedOut, newBook.id);

      //Assert
      Assert.Equal(expectedResult, foundBook);
    }
    [Fact]
    public void UpdateAll_UpdatesValues_true()
    {
      //Arrange
      Book newBook = new Book("The Hobbit", 3);
      newBook.Save();
      Book expectedResult = new Book("The Habit", 3, 3, 0, newBook.id);
      //Act
      newBook.UpdateAll("The Habit", "3", "3", "0");
      Book result = Book.Find(newBook.id);
      //Assert
      Assert.Equal(expectedResult, result);

    }
    [Fact]
    public void Delete_DeletesClienFromDB()
    {
      //Arrange
      Book newBook1 = new Book("name1", 1);
      Book newBook2 = new Book("name2", 2);
      newBook1.Save();
      newBook2.Save();
      //Act
      newBook1.Delete();
      List<Book> result = Book.GetAll();
      List<Book> expectedResult = new List<Book> {newBook2};
      //Assert
      Assert.Equal(expectedResult, result);
    }
    [Fact]
    public void AddAuthor_AddsRelationshipToJoinTable_true()
    {
      //Arrange
      Book newBook = new Book("Of Mice and Men", 1);
      newBook.Save();
      Author newAuthor = new Author("John Steinbeck");
      newAuthor.Save();
      List<Author> expectedResult = new List<Author> {newAuthor};
      //Act
      newBook.AddAuthor(newAuthor.id);
      List<Author> result = newBook.GetAuthors();
      //Assert
      Assert.Equal(expectedResult, result);
    }
  }
}
