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
    // [Fact]
    // public void Find_FindsBookById_true()
    // {
    //   //Arrange
    //   Book newBook = new Book("Betty", 2);
    //   newBook.Save();
    //   //Act
    //   Book foundBook = Book.Find(newBook.GetId());
    //   //Assert
    //   Assert.Equal(newBook, foundBook);
    // }
    // [Fact]
    // public void Edit_UpdatesValues_true()
    // {
    //   //Arrange
    //   Book newBook = new Book("book1", 2);
    //   newBook.Save();
    //   //Act
    //   newBook.Edit("Cliff", 1);
    //   Book foundBook = Book.Find(newBook.GetId());
    //   Book expectedResult = new Book(newBook.GetName(), newBook.GetStylistId(), newBook.GetId());
    //
    //   //Assert
    //   Assert.Equal(expectedResult, foundBook);
    // }
    // [Fact]
    // public void Delete_DeletesClienFromDB()
    // {
    //   //Arrange
    //   Book newBook1 = new Book("name1", 1);
    //   Book newBook2 = new Book("name2", 2);
    //   newBook1.Save();
    //   newBook2.Save();
    //   //Act
    //   newBook1.Delete();
    //   List<Book> result = Book.GetAll();
    //   List<Book> expectedResult = new List<Book> {newBook2};
    //   //Assert
    //   Assert.Equal(expectedResult, result);
    // }
    // [Fact]
    // public void GetStylistName_ReturnsStylistName_true()
    // {
    //   //Arrange
    //   Stylist newStylist = new Stylist("Frankie");
    //   newStylist.Save();
    //   Book newBook = new Book("Frances", newStylist.GetId());
    //   newBook.Save();
    //   string expectedResult = newStylist.GetName();
    //   //Act
    //   string result = newBook.GetStylistName();
    //   //Assert
    //   Assert.Equal(expectedResult, result);
    // }
  }
}
