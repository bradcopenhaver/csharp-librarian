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
    public int checkedIn {get; set;}
    public int checkedOut {get; set;}

    //Initial constructor
    public Book(string newTitle, int newCopies)
    {
      title = newTitle;
      copies = newCopies;
      checkedIn = newCopies;
      checkedOut = 0;
      id = 0;
    }

    //Re-constructor
    public Book(string newTitle, int newCopies, int newCheckedIn, int newCheckedOut, int newId)
    {
      title = newTitle;
      copies = newCopies;
      checkedIn = newCheckedIn;
      checkedOut = newCheckedOut;
      id = newId;
    }

    public override bool Equals(System.Object otherBook)
    {
      if (!(otherBook is Book))
      {
        return false;
      }
      else
      {
        Book newBook = (Book) otherBook;
        bool idEquality = this.id == newBook.id;
        bool titleEquality = this.title == newBook.title;
        bool copiesEquality = this.copies == newBook.copies;
        bool checkedInEquality = this.checkedIn == newBook.checkedIn;
        bool checkedOutEquality = this.checkedOut == newBook.checkedOut;
        return (idEquality && titleEquality && copiesEquality && checkedOutEquality && checkedInEquality);
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM authors_books; DELETE FROM books;", conn);
      cmd.ExecuteNonQuery();
      if(conn != null) conn.Close();
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO books (title, copies, checked_in, checked_out) OUTPUT INSERTED.id VALUES (@title, @copies, @checked_in, @checked_out);", conn);
      cmd.Parameters.AddWithValue("@title", this.title);
      cmd.Parameters.AddWithValue("@copies", this.copies);
      cmd.Parameters.AddWithValue("@checked_in", this.checkedIn);
      cmd.Parameters.AddWithValue("@checked_out", this.checkedOut);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        this.id = rdr.GetInt32(0);
      }
      if(rdr!=null) rdr.Close();
      if(conn!=null) conn.Close();
    }

    public static List<Book> GetAll()
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
        int checkedInNew = rdr.GetInt32(3);
        int checkedOutNew = rdr.GetInt32(4);

        allBooks.Add(new Book(titleNew, copiesNew, checkedInNew, checkedOutNew, idNew));
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
      int checkedInNew = 0;
      int checkedOutNew = 0;
      while(rdr.Read())
      {
        idNew = rdr.GetInt32(0);
        titleNew = rdr.GetString(1);
        copiesNew = rdr.GetInt32(2);
        checkedInNew = rdr.GetInt32(3);
        checkedOutNew = rdr.GetInt32(4);
      }
      Book foundBook = new Book(titleNew, copiesNew, checkedInNew, checkedOutNew, idNew);
      if(rdr!=null) rdr.Close();
      if(conn!=null) conn.Close();
      return foundBook;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM authors_books WHERE book_id = @bookId; DELETE FROM books WHERE id = @bookId;", conn);
      cmd.Parameters.AddWithValue("@bookId", this.id);

      cmd.ExecuteNonQuery();
      if(conn!=null) conn.Close();
    }

    public void UpdateProperty(string propertyToChange, string changeValue)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand();
      cmd.Connection = conn;

      switch (propertyToChange)
      {
        case "title":
            cmd.CommandText = "UPDATE books SET title = @newValue WHERE id = @id";
            this.title = changeValue;
            break;
        case "copies":
            cmd.CommandText = "UPDATE books SET copies = @newValue WHERE id = @id";
            this.copies = int.Parse(changeValue);
            break;
        case "checked_out":
            cmd.CommandText = "UPDATE books SET checked_out = @newValue WHERE id = @id";
            this.checkedOut = int.Parse(changeValue);
            break;
        case "checked_in":
            cmd.CommandText = "UPDATE books SET checked_in = @newValue WHERE id = @id";
            this.checkedIn = int.Parse(changeValue);
            break;
        default:
            break;
      }

      cmd.Parameters.AddWithValue("@newValue", changeValue);
      cmd.Parameters.AddWithValue("@id", this.id);

      cmd.ExecuteNonQuery();
      if(conn!=null) conn.Close();
    }

    public void UpdateAll(string inputTitle, string inputCopies, string inputCheckedIn, string inputCheckedOut)
    {
      string newTitle = (inputTitle != "") ? inputTitle : this.title;
      int newCopies = (inputCopies != "") ? int.Parse(inputCopies) : this.copies;
      int newCheckedIn = (inputCheckedIn != "") ? int.Parse(inputCheckedIn) : this.checkedIn;
      int newCheckedOut = (inputCheckedOut != "") ? int.Parse(inputCheckedOut) : this.checkedOut;

      this.title = newTitle;
      this.copies = newCopies;
      this.checkedIn = newCheckedIn;
      this.checkedOut = newCheckedOut;

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE books SET title = @newTitle, copies = @newCopies, checked_in = @newCheckedIn, checked_out = @newCheckedOut WHERE id = @id", conn);
      cmd.Parameters.AddWithValue("@newTitle", newTitle);
      cmd.Parameters.AddWithValue("@newCopies", newCopies);
      cmd.Parameters.AddWithValue("@newCheckedIn", newCheckedIn);
      cmd.Parameters.AddWithValue("@newCheckedOut", newCheckedOut);
      cmd.Parameters.AddWithValue("@id", this.id);

      cmd.ExecuteNonQuery();
      if(conn != null) conn.Close();
    }

    public void AddAuthor(int authorId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO authors_books (author_id, book_id) VALUES (@authorId, @bookId);", conn);
      cmd.Parameters.AddWithValue("@authorId", authorId);
      cmd.Parameters.AddWithValue("@bookId", this.id);

      cmd.ExecuteNonQuery();
      if (conn != null) conn.Close();
    }

    public void RemoveAuthor(int authorId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM authors_books WHERE author_id = @authorId AND book_id = @bookId;", conn);
      cmd.Parameters.AddWithValue("@authorId", authorId);
      cmd.Parameters.AddWithValue("@bookId", this.id);

      cmd.ExecuteNonQuery();
      if (conn != null) conn.Close();
    }


    public List<Author> GetAuthors()
    {
      List<Author> bookAuthors = new List<Author> {};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT authors.* FROM books JOIN authors_books ON (books.id = authors_books.book_id) JOIN authors ON (authors_books.author_id = authors.id) WHERE books.id = @bookId;", conn);
      cmd.Parameters.AddWithValue("@bookId", this.id);

      SqlDataReader rdr = cmd.ExecuteReader();

      while(rdr.Read())
      {
        int authorId = rdr.GetInt32(0);
        string authorName = rdr.GetString(1);
        bookAuthors.Add(new Author(authorName, authorId));
      }
      if (rdr != null) rdr.Close();
      if(conn != null) conn.Close();

      return bookAuthors;
    }

  }
}
