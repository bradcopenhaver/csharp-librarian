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
      Get["/member"] = _ => {
        Member currentMember = Member.Find(Request.Query["memberId"]);
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Checkout> checkouts = currentMember.GetCheckouts();
        List<Book> allBooks = Book.GetAll();
        model.Add("books", allBooks);
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
        newMember.Save();
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Checkout> checkouts = newMember.GetCheckouts();
        List<Book> allBooks = Book.GetAll();
        model.Add("books", allBooks);
        model.Add("member", newMember);
        model.Add("checkouts", checkouts);
        model.Add("today", DateTime.Today);
        return View["member-portal.cshtml", model];
      };
      Get["/librarian"] = _ => {
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Member> allMembers = Member.GetAll();
        List<Checkout> allCheckouts = Checkout.GetAll();
        List<Checkout> overdueCheckouts = Checkout.GetOverdue();
        List<Book> allBooks = Book.GetAll();
        List<Author> allAuthors = Author.GetAll();
        model.Add("members", allMembers);
        model.Add("checkouts", allCheckouts);
        model.Add("overdues", overdueCheckouts);
        model.Add("books", allBooks);
        model.Add("authors", allAuthors);
        return View["librarian-portal.cshtml", model];
      };
      Get["/book/new/step1"] = _ => {
        List<Author> allAuthors = Author.GetAll();
        return View["new-book-form1.cshtml", allAuthors];
      };
      Get["/book/new/step2"] = _ => {
        int authorId = Request.Query["authorId"];
        return View["new-book-form2", authorId];
      };
      Post["/book/new/step2"] = _ => {
        Book newBook = new Book(Request.Form["bookTitle"], int.Parse(Request.Form["bookCopies"]));
        newBook.Save();
        newBook.AddAuthor(Request.Form["authorId"]);
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Member> allMembers = Member.GetAll();
        List<Checkout> allCheckouts = Checkout.GetAll();
        List<Checkout> overdueCheckouts = Checkout.GetOverdue();
        List<Book> allBooks = Book.GetAll();
        List<Author> allAuthors = Author.GetAll();
        model.Add("members", allMembers);
        model.Add("checkouts", allCheckouts);
        model.Add("overdues", overdueCheckouts);
        model.Add("books", allBooks);
        model.Add("authors", allAuthors);
        return View["librarian-portal.cshtml", model];
      };
      Get["/author/new"] = _ => {
        return View["new-author-form.cshtml"];
      };
      Post["/author/new"] = _ => {
        Author newAuthor = new Author(Request.Form["authorName"]);
        newAuthor.Save();
        List<Author> allAuthors = Author.GetAll();
        return View["new-book-form1.cshtml", allAuthors];
      };
      Get["/book"] = _ => {
        Book currentBook = Book.Find(Request.Query["bookId"]);
        return View["book.cshtml", currentBook];
      };
      Post["/book/checkout"] = _ => {
        Member currentMember = Member.Find(Request.Form["memberId"]);
        currentMember.CheckoutBook(Request.Form["bookId"]);
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Checkout> checkouts = currentMember.GetCheckouts();
        List<Book> allBooks = Book.GetAll();
        model.Add("books", allBooks);
        model.Add("member", currentMember);
        model.Add("checkouts", checkouts);
        model.Add("today", DateTime.Today);
        return View["member-portal.cshtml", model];
      };
      Patch["/book/return"] = _ => {
        Member currentMember = Member.Find(Request.Form["memberId"]);
        currentMember.ReturnBook(int.Parse(Request.Form["checkoutId"]));
        Dictionary<string, object> model = new Dictionary<string, object>();
        List<Checkout> checkouts = currentMember.GetCheckouts();
        List<Book> allBooks = Book.GetAll();
        model.Add("books", allBooks);
        model.Add("member", currentMember);
        model.Add("checkouts", checkouts);
        model.Add("today", DateTime.Today);
        return View["member-portal.cshtml", model];
      };
    }
  }
}
