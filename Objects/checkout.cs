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

    public static Checkout Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM checkouts WHERE id = @checkoutId;", conn);
      cmd.Parameters.AddWithValue("@checkoutId", id);
      SqlDataReader rdr = cmd.ExecuteReader();
      int idNew = 0;
      int bookIdNew = 0;
      int memberIdNew = 0;
      DateTime dueDateNew = DateTime.Today;
      bool returnedNew = false;
      while(rdr.Read())
      {
        idNew = rdr.GetInt32(0);
        bookIdNew = rdr.GetInt32(1);
        memberIdNew = rdr.GetInt32(2);
        dueDateNew = rdr.GetDateTime(3);
        returnedNew = rdr.GetBoolean(4);
      }
      Checkout foundCheckout = new Checkout(bookIdNew, memberIdNew, dueDateNew, returnedNew, idNew);
      if(rdr!=null) rdr.Close();
      if(conn!=null) conn.Close();
      return foundCheckout;
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

    public string GetMemberName()
    {
      string memberName = "";
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT name FROM members WHERE id = @memberId;", conn);
      cmd.Parameters.AddWithValue("@memberId", this.memberId);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        memberName = rdr.GetString(0);
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return memberName;
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
