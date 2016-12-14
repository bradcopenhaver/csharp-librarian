using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Librarian.Objects
{
  public class Book
  {
    private int _id;
    private string _title;
    private int _copies;
    private int _checkedOut;
    private int _checkedIn;

    //Initial constructor
    public Book(string title, int copies)
    {
      _title = title;
      _copies = copies;
      _checkedOut = 0;
      _checkedIn = copies;
      _id = 0;
    }

    //Re-constructor
    public Book(string title, int copies, int checkedOut, int checkedIn, int id)
    {
      _title = title;
      _copies = copies;
      _checkedOut = checkedOut;
      _checkedIn = checkedIn;
      _id = id;
    }


  }
}
