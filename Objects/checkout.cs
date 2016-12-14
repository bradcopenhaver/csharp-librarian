using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Librarian.Objects
{
  public class Checkout
  {
    private int _id;
    private int _bookId;
    private int _memberId;
    private DateTime _dueDate;
    private bool _returned;

    public Checkout(int bookId, int memberId, DateTime dueDate, bool returned = false, int id = 0)
    {
      _bookId = bookId;
      _memberId = memberId;
      _dueDate  = dueDate;
      _returned = returned;
      _id = id;
    }
  }
}
