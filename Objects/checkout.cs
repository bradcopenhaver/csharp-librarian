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

    public Checkout(int bookIdInput, int memberIdInput, DateTime dueDateInput, bool returnedInput = false, int idInput = 0)
    {
      bookId = bookIdInput;
      memberId = memberIdInput;
      dueDate  = dueDateInput;
      returned = returnedInput;
      id = idInput;
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

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO checkouts (book_id, member_id, due_date, returned) OUTPUT INSERTED.id VALUES (@bookId, @memberId, @dueDate, @returned);", conn);
      cmd.Parameters.AddWithValue("@bookId", this.bookId);
      cmd.Parameters.AddWithValue("@memberId", this.memberId);
      cmd.Parameters.AddWithValue("@dueDate", this.dueDate);
      cmd.Parameters.AddWithValue("@returned", this.returned);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        this.id = rdr.GetInt32(0);
      }
      if(rdr!=null) rdr.Close();
      if(conn!=null) conn.Close();
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

    public static List<Checkout> GetOverdue()
    {
      List<Checkout> overdues = new List<Checkout>{};
      int foundId = 0;
      int foundBookId = 0;
      int foundMemberId = 0;
      DateTime foundDueDate = DateTime.Today;
      bool foundReturned = false;
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM checkouts WHERE due_date < @today AND returned = 0;", conn);
      cmd.Parameters.AddWithValue("@today", DateTime.Today);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        foundId = rdr.GetInt32(0);
        foundBookId = rdr.GetInt32(1);
        foundMemberId = rdr.GetInt32(2);
        foundDueDate = rdr.GetDateTime(3);
        foundReturned = rdr.GetBoolean(4);
        overdues.Add(new Checkout(foundBookId, foundMemberId, foundDueDate, foundReturned, foundId));
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return overdues;
    }
  }
}
