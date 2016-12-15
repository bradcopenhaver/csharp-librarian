using Nancy;
using System.Collections.Generic;
using System;
using Librarian.Objects;
using Nancy.ViewEngines.Razor;

namespace Librarian
{
	public class HomeModule : NancyModule
	{
		public HomeModule()
    {
      Get["/"] = _ => {
        List<Member> allMembers = Member.GetAll();
        return View["index.cshtml", allMembers];
      };
      Post["/member"] = _ => {
        Member currentMember = Member.Find(Request.Form["memberId"]);
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Checkout> checkouts = currentMember.GetCheckouts();
        model.Add("member", currentMember);
        model.Add("checkouts", checkouts);
        model.Add("today", DateTime.Today);
        return View["member-portal.cshtml", model];
      };
      Get["/member/new"] = _ => {
        return View["new-member-form.cshtml"];
      };
      Post["/member/new"] = _ => {
        Member newMember = new Member(Request.Form["memberName"]);
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Checkout> checkouts = newMember.GetCheckouts();
        model.Add("member", newMember);
        model.Add("checkouts", checkouts);
        model.Add("today", DateTime.Today);
        return View["member-portal.cshtml", model];
      };
      Get["/librarian"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Member> allMembers = Member.GetAll();
        List<Checkout> allCheckouts = Checkout.GetAll();
        List<Book> allBooks = Book.GetAll();
        List<Author> allAuthors = Author.GetAll();
        model.Add("members", allMembers);
        model.Add("checkouts", allCheckouts);
        model.Add("books", allBooks);
        model.Add("authors", allAuthors);
        return View["librarian-portal.cshtml", model];
      };
    }
  }
}
