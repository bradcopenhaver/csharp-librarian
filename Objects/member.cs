using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Librarian.Objects
{
  public class Member
  {
    public int id {get; set;}
    public string name {get; set;}

    public Member(string inputName, int inputId=0)
    {
      name = inputName;
      id = inputId;
    }


    public override bool Equals(System.Object otherMember)
    {
      if (!(otherMember is Member))
      {
        return false;
      }
      else
      {
        Member newMember = (Member) otherMember;
        bool idEquality = this.id == newMember.id;
        bool nameEquality = this.name == newMember.name;
        return (idEquality && nameEquality);
      }
    }

    public static void DeleteAll()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM checkouts; DELETE FROM members;", conn);
      cmd.ExecuteNonQuery();
      if(conn != null) conn.Close();
    }

    public void Save()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO members (name) OUTPUT INSERTED.id VALUES (@name);", conn);
      cmd.Parameters.AddWithValue("@name", this.name);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        this.id = rdr.GetInt32(0);
      }
      if(rdr!=null) rdr.Close();
      if(conn!=null) conn.Close();
    }

    public static List<Member> GetAll()
    {
      List<Member> allMembers = new List<Member>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM members ORDER BY name ASC;", conn);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int idNew = rdr.GetInt32(0);
        string nameNew = rdr.GetString(1);

        allMembers.Add(new Member(nameNew, idNew));
      }
      if(rdr!=null) rdr.Close();
      if(conn!=null) conn.Close();
      return allMembers;
    }

    public static Member Find(int id)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM members WHERE id = @memberId;", conn);
      cmd.Parameters.AddWithValue("@memberId", id);
      SqlDataReader rdr = cmd.ExecuteReader();
      int idNew = 0;
      string nameNew = "";
      while(rdr.Read())
      {
        idNew = rdr.GetInt32(0);
        nameNew = rdr.GetString(1);
      }
      Member foundMember = new Member(nameNew, idNew);
      if(rdr!=null) rdr.Close();
      if(conn!=null) conn.Close();
      return foundMember;
    }

    public void Delete()
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("DELETE FROM checkouts WHERE member_id = @memberId; DELETE FROM members WHERE id = @memberId;", conn);
      cmd.Parameters.AddWithValue("@memberId", this.id);

      cmd.ExecuteNonQuery();
      if(conn!=null) conn.Close();
    }

    public void CheckoutBook(int bookId)
    {
      Book targetBook = Book.Find(bookId);

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("INSERT INTO checkouts (member_id, book_id, due_date) VALUES (@memberId, @bookId, @dueDate); UPDATE books SET checked_in = @checkedIn, checked_out = @checkedOut WHERE id = @bookId;", conn);
      cmd.Parameters.AddWithValue("@memberId", this.id);
      cmd.Parameters.AddWithValue("@bookId", bookId);
      cmd.Parameters.AddWithValue("@dueDate", DateTime.Today.AddDays(14));
      cmd.Parameters.AddWithValue("@checkedIn", targetBook.checkedIn - 1);
      cmd.Parameters.AddWithValue("@checkedOut", targetBook.checkedOut + 1);

      cmd.ExecuteNonQuery();
      if (conn != null) conn.Close();
    }

    public void ReturnBook(int checkoutId)
    {
      Checkout targetCheckout = Checkout.Find(checkoutId);
      Book targetBook = Book.Find(targetCheckout.bookId);

      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE checkouts SET returned = 1 WHERE id = @checkoutId; UPDATE books SET checked_in = @checkedIn, checked_out = @checkedOut WHERE id = @bookId;", conn);
      cmd.Parameters.AddWithValue("@checkoutId", targetCheckout.id);
      cmd.Parameters.AddWithValue("@bookId", targetCheckout.bookId);
      cmd.Parameters.AddWithValue("@checkedIn", targetBook.checkedIn + 1);
      cmd.Parameters.AddWithValue("@checkedOut", targetBook.checkedOut - 1);

      cmd.ExecuteNonQuery();
      if (conn != null) conn.Close();
    }

    public void Update(string newName)
    {
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("UPDATE members SET name = @newName WHERE id = @memberId;", conn);
      cmd.Parameters.AddWithValue("@newName", newName);
      cmd.Parameters.AddWithValue("@memberId", this.id);

      cmd.ExecuteNonQuery();
      if (conn != null) conn.Close();
    }

    public List<Checkout> GetCheckouts()
    {
      List<Checkout> allCheckouts = new List<Checkout>{};
      SqlConnection conn = DB.Connection();
      conn.Open();

      SqlCommand cmd = new SqlCommand("SELECT * FROM checkouts WHERE member_id = @memberId ORDER BY returned, due_date DESC;", conn);
      cmd.Parameters.AddWithValue("@memberId", this.id);
      SqlDataReader rdr = cmd.ExecuteReader();
      while(rdr.Read())
      {
        int checkoutId = rdr.GetInt32(0);
        int checkoutBookId = rdr.GetInt32(1);
        int checkoutMemberId = rdr.GetInt32(2);
        DateTime checkoutDueDate = rdr.GetDateTime(3);
        bool checkoutReturned = rdr.GetBoolean(4);

        allCheckouts.Add(new Checkout(checkoutBookId, checkoutMemberId, checkoutDueDate, checkoutReturned, checkoutId));
      }
      if (rdr != null) rdr.Close();
      if (conn != null) conn.Close();
      return allCheckouts;
    }
  }
}
