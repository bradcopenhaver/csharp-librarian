using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Librarian.Objects
{
  public class Checkout
  {
    public int id {get; set;}
    public int bookId {get; set;}
    public int memberId {get; set;}
    public DateTime dueDate {get; set;}
    public bool returned {get; set;}

    public Checkout(int bookId, int memberId, DateTime dueDate, bool returned = false, int id = 0)
    {
      bookId = bookId;
      memberId = memberId;
      dueDate  = dueDate;
      returned = returned;
      id = id;
    }

    public static List<Checkout> GetAll()
    {
      List<Checkout> allCheckouts = new List<Checkout>{};
      int foundId = 0;
      int foundBookId = 0;
      int foundMemberId = 0;
      DateTime foundDueDate = DateTime.Today;
      bool foundReturned = false;

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM checkouts ORDER BY due_date ASC;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        foundBookId = rdr.GetInt32(1);
        foundMemberId = rdr.GetInt32(2);
        foundDueDate = rdr.GetDateTime(3);
        foundReturned = rdr.GetBoolean(4);
        allCheckouts.Add(new Checkout(foundBookId, foundMemberId, foundDueDate, foundReturned, foundId));
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return allCheckouts;
    }

    public string GetBookTitle()
    {
      string title = "";
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT title FROM books WHERE id = @bookId;", conn);
      cmd.Parameters.AddWithValue("@bookId", this.bookId);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        title = rdr.GetString(0);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return title;
    }
  }
}
