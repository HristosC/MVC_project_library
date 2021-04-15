using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_Project.Models;
using System.Configuration;



namespace MVC_Project.Controllers
{
    public class titlesController : Controller
    {
        private pubsEntities db = new pubsEntities();

        // GET: titles
        public ActionResult Index()
        {
            var titles = db.titles.Include(t => t.publishers).Include(t => t.roysched);
            return View(titles.ToList());
        }

        // GET: titles/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            titles titles = db.titles.Find(id);
            if (titles == null)
            {
                return HttpNotFound();
            }
            return View(titles);
        }

        // GET: titles/Create
        public ActionResult Create()
        {
            ViewBag.pub_id = new SelectList(db.publishers, "pub_id", "pub_name");
            ViewBag.title_id = new SelectList(db.roysched, "title_id", "title_id");
            return View();
        }

        // POST: titles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "title_id,title,type,pub_id,price,advance,royalty,ytd_sales,notes,pubdate")] titles titles)
        {
            if (ModelState.IsValid)
            {
                db.titles.Add(titles);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.pub_id = new SelectList(db.publishers, "pub_id", "pub_name", titles.pub_id);
            ViewBag.title_id = new SelectList(db.roysched, "title_id", "title_id", titles.title_id);
            return View(titles);
        }

        // GET: titles/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            titles titles = db.titles.Find(id);
            if (titles == null)
            {
                return HttpNotFound();
            }
            ViewBag.pub_id = new SelectList(db.publishers, "pub_id", "pub_name", titles.pub_id);
            ViewBag.title_id = new SelectList(db.roysched, "title_id", "title_id", titles.title_id);
            return View(titles);
        }

        // POST: titles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "title_id,title,type,pub_id,price,advance,royalty,ytd_sales,notes,pubdate")] titles titles)
        {
            if (ModelState.IsValid)
            {
                db.Entry(titles).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.pub_id = new SelectList(db.publishers, "pub_id", "pub_name", titles.pub_id);
            ViewBag.title_id = new SelectList(db.roysched, "title_id", "title_id", titles.title_id);
            return View(titles);
        }

        // GET: titles/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            titles titles = db.titles.Find(id);
            if (titles == null)
            {
                return HttpNotFound();
            }
            return View(titles);
        }

        // POST: titles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            var constring = System.Configuration.ConfigurationManager.ConnectionStrings["pubsEntities"].ConnectionString;
            if (constring.ToLower().StartsWith("metadata="))
            {
                System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder efBuilder = new System.Data.Entity.Core.EntityClient.EntityConnectionStringBuilder(constring);
                constring = efBuilder.ProviderConnectionString;
            }
            var con = new SqlConnection(constring);
            con.Open();
            string query = "SELECT COUNT(*) FROM titleauthor";
            var cmd = new SqlCommand(query, con);
            int rowsAmount = (int)cmd.ExecuteScalar();
            titleauthor titleauthor;
            for (int i = 0; i < rowsAmount; i++)
            {
                titleauthor = db.titleauthor.FirstOrDefault(x => x.title_id == id);
                if (titleauthor == null)
                {

                }
                else
                {
                    db.titleauthor.Remove(titleauthor);
                    db.SaveChanges();
                }

            }
            query = "SELECT COUNT(*) FROM roysched";
            cmd = new SqlCommand(query, con);
            rowsAmount = (int)cmd.ExecuteScalar();
            roysched roysched;
            for (int i = 0; i < rowsAmount; i++)
            {
                roysched = db.roysched.FirstOrDefault(x => x.title_id == id);
                if (roysched == null)
                {

                }
                else
                {
                    db.roysched.Remove(roysched);
                    db.SaveChanges();
                }

            }
            query = "SELECT COUNT(*) FROM sales";
            cmd = new SqlCommand(query, con);
            rowsAmount = (int)cmd.ExecuteScalar();
            sales sales;
            for (int i = 0; i < rowsAmount; i++)
            {
                sales = db.sales.FirstOrDefault(x => x.title_id == id);
                if (sales == null)
                {

                }
                else
                {
                    db.sales.Remove(sales);
                    db.SaveChanges();
                }

            }
            titles titles = db.titles.Find(id);
            db.titles.Remove(titles);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
