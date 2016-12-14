using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Librarian.Objects
{
  public class Member
  {
    private int _id;
    private string _name;

    public Member(string name, int id=0)
    {
      _name = name;
      _id = id;
    }
  }
}
