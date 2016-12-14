using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Librarian.Objects
{
  public class Author
  {
    public int id {get; set;}
    public string name {get; set;}

    public Author(string inputName, int inputId=0)
    {
      name = inputName;
      id = inputId;
    }

    public override bool Equals(System.Object otherAuthor)
    {
      if (!(otherAuthor is Author))
      {
        return false;
      }
      else
      {
        Author newAuthor = (Author) otherAuthor;
        bool idEquality = this.id == newAuthor.id;
        bool nameEquality = this.name == newAuthor.name;
        return (idEquality && nameEquality);
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM authors_books; DELETE FROM authors;", conn);
      cmd.ExecuteNonQuery();
      if(conn != null) conn.Close();
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO authors (name) OUTPUT INSERTED.id VALUES (@name);", conn);
      cmd.Parameters.AddWithValue("@name", this.name);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        this.id = rdr.GetInt32(0);
      }
      if(rdr!=null) rdr.Close();
      if(conn!=null) conn.Close();
    }

    public static List<Author> GetAll()
    {
      List<Author> allAuthors = new List<Author>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM authors;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int idNew = rdr.GetInt32(0);
        string nameNew = rdr.GetString(1);

        allAuthors.Add(new Author(nameNew, idNew));
      }
      if(rdr!=null) rdr.Close();
      if(conn!=null) conn.Close();
      return allAuthors;
    }

    public static Author Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM authors WHERE id = @authorId;", conn);
      cmd.Parameters.AddWithValue("@authorId", id);
      SqlDataReader rdr = cmd.ExecuteReader();
      int idNew = 0;
      string nameNew = "";
      while(rdr.Read())
      {
        idNew = rdr.GetInt32(0);
        nameNew = rdr.GetString(1);
      }
      Author foundAuthor = new Author(nameNew, idNew);
      if(rdr!=null) rdr.Close();
      if(conn!=null) conn.Close();
      return foundAuthor;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM authors_books WHERE author_id = @authorId; DELETE FROM authors WHERE id = @authorId;", conn);
      cmd.Parameters.AddWithValue("@authorId", this.id);

      cmd.ExecuteNonQuery();
      if(conn!=null) conn.Close();
    }

    public void AddBook(int bookId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO authors_books (author_id, book_id) VALUES (@authorId, @bookId)", conn);
      cmd.Parameters.AddWithValue("@authorId", this.id);
      cmd.Parameters.AddWithValue("@bookId", bookId);

      cmd.ExecuteNonQuery();
      if (conn != null) conn.Close();
    }

    public void RemoveBook(int bookId)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM authors_books WHERE author_id = @authorId AND book_id = @bookId)", conn);
      cmd.Parameters.AddWithValue("@authorId", this.id);
      cmd.Parameters.AddWithValue("@bookId", bookId);

      cmd.ExecuteNonQuery();
      if (conn != null) conn.Close();
    }
  }
}
