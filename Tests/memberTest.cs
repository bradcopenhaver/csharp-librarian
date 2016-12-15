using Xunit;
using System;
using System.Collections.Generic;
using Librarian.Objects;

namespace Librarian
{
  public class MemberTest : IDisposable
  {
    public MemberTest()
    {
      DBConfiguration.ConnectionString = "Data Source=(localdb)\\mssqllocaldb;Initial Catalog=librarian_test;Integrated Security=SSPI;";
    }
    public void Dispose()
    {
      Member.DeleteAll();
      Author.DeleteAll();
      Book.DeleteAll();
    }
    [Fact]
    public void GetAll_DatabaseEmpty_true()
    {
      //Arrange
      //Act
      int result = Member.GetAll().Count;
      //Assert
      Assert.Equal(0, result);
    }
    [Fact]
    public void Test_EqualOverride_True()
    {
      //Arrange and Act
      Member firstMember = new Member("Steve McQueen");
      Member secondMember = new Member("Steve McQueen");
      //Assert
      Assert.Equal(firstMember,secondMember);
    }
    [Fact]
    public void Save_SavesMemberToDatabase_true()
    {
      //Arrange
      Member newMember = new Member("Steve McQueen");
      //Act
      newMember.Save();
      List<Member> allMembers = Member.GetAll();
      //Assert
      Assert.Equal(newMember, allMembers[0]);
    }
    [Fact]
    public void Find_FindsMemberById_true()
    {
      //Arrange
      Member newMember = new Member("Francis Bacon");
      newMember.Save();
      //Act
      Member foundMember = Member.Find(newMember.id);
      //Assert
      Assert.Equal(newMember, foundMember);
    }
    [Fact]
    public void Update_ChangesNameInDb_true()
    {
      //Arrange
      Member newMember = new Member("Steve McQueen");
      newMember.Save();
      //Act
      newMember.Update("Steven MacWeeney");
      Member foundMember = Member.Find(newMember.id);
      Member expectedResult = new Member("Steven MacWeeney", newMember.id);

      //Assert
      Assert.Equal(expectedResult, foundMember);
    }
    [Fact]
    public void Delete_DeletesMemberFromDB()
    {
      //Arrange
      Member newMember1 = new Member("Steve McQueen");
      Member newMember2 = new Member("Francis Bacon");
      newMember1.Save();
      newMember2.Save();
      //Act
      newMember1.Delete();
      List<Member> result = Member.GetAll();
      List<Member> expectedResult = new List<Member> { newMember2 };
      //Assert
      Assert.Equal(expectedResult, result);
    }
  }
}
