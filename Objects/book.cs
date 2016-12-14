using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Librarian.Objects
{
  public class Book
  {
    public int id {get; set;}
    public string title {get; set;}
    public int copies {get; set;}
    public int checkedOut {get; set;}
    public int checkedIn {get; set;}

    //Initial constructor
    public Book(string newTitle, int newCopies)
    {
      title = newTitle;
      copies = newCopies;
      checkedOut = 0;
      checkedIn = newCopies;
      id = 0;
    }

    //Re-constructor
    public Book(string newTitle, int newCopies, int newCheckedOut, int newCheckedIn, int newId)
    {
      title = newTitle;
      copies = newCopies;
      checkedOut = newCheckedOut;
      checkedIn = newCheckedIn;
      id = newId;
    }

    public override bool Equals(System.Object otherBook)
    {
      if (!(otherBook is Book))
      {
        return false;
      }
      else
      (
        Book newBook = (Book) otherBook;
        bool idEquality = this.id == newbook.id;
        bool titleEquality = this.title == newbook.title;
        bool copiesEquality = this.copies == newbook.copies;
        bool checkedOutEquality = this.checkedOut == newbook.checkedOut;
        bool checkedInEquality = this.checkedIn == newbook.checkedIn;
        return (idEquality && titleEquality && copiesEquality && checkedOutEquality && checkedInEquality);
      )
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO books (title, copies, checked_out, checked_in) OUTPUT INSERTED.id VALUES (@title, @copies, @checked_out, @checked_in);", conn);
      cmd.Parameters.AddWithValue("@title", this.title);
      cmd.Parameters.AddWithValue("@copies", this.copies);
      cmd.Parameters.AddWithValue("@checked_out", this.checkedOut);
      cmd.Parameters.AddWithValue("@checked_in", this.checkedIn);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        this.id = rdr.GetInt32(0);
      }
      if(rdr!=null) rdr.Close();
      if(conn!=null) conn.Close();
    }

    public List<Book> GetAll()
    {
      List<Book> allBooks = new List<Book>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int idNew = rdr.GetInt32(0);
        string titleNew = rdr.GetString(1);
        int copiesNew = rdr.GetInt32(2);
        int checkedOutNew = rdr.GetInt32(3);
        int checkedInNew = rdr.GetInt32(4);

        allBooks.Add(new Book(titleNew, copiesNew, checkedOutNew, checkedInNew, idNew));
      }
      if(rdr!=null) rdr.Close();
      if(conn!=null) conn.Close();
      return allBooks;
    }

    public static Book Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM books WHERE id = @bookId;", conn);
      cmd.Parameters.AddWithValue("@bookId", id);
      SqlDataReader rdr = cmd.ExecuteReader();
      int idNew = 0;
      string titleNew = "";
      int copiesNew = 0;
      int checkedOutNew = 0;
      int checkedInNew = 0;
      while(rdr.Read())
      {
        idNew = rdr.GetInt32(0);
        titleNew = rdr.GetString(1);
        copiesNew = rdr.GetInt32(2);
        checkedOutNew = rdr.GetInt32(3);
        checkedInNew = rdr.GetInt32(4);

      }
      Book foundBook = new Book(titleNew, copiesNew, checkedOutNew, checkedInNew, idNew);
      if(rdr!=null) rdr.Close();
      if(conn!=null) conn.Close();
      return foundBook;
    }
  }
}
