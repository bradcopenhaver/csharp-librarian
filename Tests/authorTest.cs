using Xunit;
using System;
using System.Collections.Generic;
using Librarian.Objects;

namespace  Librarian
{
  public class AuthorTest : IDisposable
  {
    public void Dispose()
    {
      Author.DeleteAll();
      Book.DeleteAll();
    }
    public AuthorTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=librarian_test;Integrated Security=SSPI;";
    }
    [Fact]
    public void GetAll_DatabaseEmpty_true()
    {
      //Arrange
      //Act
      int result = Author.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_EqualOverride_True()
    {
      //Arrange and Act
      Author firstAuthor = new Author("Stephen King");
      Author secondAuthor = new Author("Stephen King");
      //Assert
      Assert.Equal(firstAuthor,secondAuthor);
    }
    [Fact]
    public void Save_SavesAuthorToDatabase_true()
    {
      //Arrange
      Author newAuthor = new Author("Herman Melville");
      //Act
      newAuthor.Save();
      List<Author> allAuthors = Author.GetAll();
      //Assert
      Assert.Equal(newAuthor, allAuthors[0]);
    }
    [Fact]
    public void Find_FindsAuthorById_true()
    {
      //Arrange
      Author newAuthor = new Author("Anne Bronte");
      newAuthor.Save();
      //Act
      Author foundAuthor = Author.Find(newAuthor.id);
      //Assert
      Assert.Equal(newAuthor, foundAuthor);
    }
    // [Fact]
    // public void Edit_UpdatesValues_true()
    // {
    //   //Arrange
    //   Author newAuthor = new Author("author1", 2);
    //   newAuthor.Save();
    //   //Act
    //   newAuthor.Edit("Cliff", 1);
    //   Author foundAuthor = Author.Find(newAuthor.GetId());
    //   Author expectedResult = new Author(newAuthor.GetName(), newAuthor.GetStylistId(), newAuthor.GetId());
    //
    //   //Assert
    //   Assert.Equal(expectedResult, foundAuthor);
    // }
    [Fact]
    public void Delete_DeletesClienFromDB()
    {
      //Arrange
      Author newAuthor1 = new Author("name1");
      Author newAuthor2 = new Author("name2");
      newAuthor1.Save();
      newAuthor2.Save();
      //Act
      newAuthor1.Delete();
      List<Author> result = Author.GetAll();
      List<Author> expectedResult = new List<Author> {newAuthor2};
      //Assert
      Assert.Equal(expectedResult, result);
    }
    // [Fact]
    // public void GetStylistName_ReturnsStylistName_true()
    // {
    //   //Arrange
    //   Stylist newStylist = new Stylist("Frankie");
    //   newStylist.Save();
    //   Author newAuthor = new Author("Frances", newStylist.GetId());
    //   newAuthor.Save();
    //   string expectedResult = newStylist.GetName();
    //   //Act
    //   string result = newAuthor.GetStylistName();
    //   //Assert
    //   Assert.Equal(expectedResult, result);
    // }
  }
}
