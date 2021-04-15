using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_Project.Models;
using System.Configuration;
using System.Data.SqlClient;

namespace MVC_Project.Controllers
{
    public class publishersController : Controller
    {
        private pubsEntities db = new pubsEntities();

        // GET: publishers
        public ActionResult Index()
        {
            var publishers = db.publishers.Include(p => p.pub_info);
            return View(publishers.ToList());
        }

        // GET: publishers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            publishers publishers = db.publishers.Find(id);
            if (publishers == null)
            {
                return HttpNotFound();
            }
            return View(publishers);
        }

        // GET: publishers/Create
        public ActionResult Create()
        {
            ViewBag.pub_id = new SelectList(db.pub_info, "pub_id", "pr_info");
            return View();
        }

        // POST: publishers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "pub_id,pub_name,city,state,country")] publishers publishers)
        {
            if (ModelState.IsValid)
            {
                db.publishers.Add(publishers);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.pub_id = new SelectList(db.pub_info, "pub_id", "pr_info", publishers.pub_id);
            return View(publishers);
        }

        // GET: publishers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            publishers publishers = db.publishers.Find(id);
            if (publishers == null)
            {
                return HttpNotFound();
            }
            ViewBag.pub_id = new SelectList(db.pub_info, "pub_id", "pr_info", publishers.pub_id);
            return View(publishers);
        }

        // POST: publishers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "pub_id,pub_name,city,state,country")] publishers publishers)
        {
            if (ModelState.IsValid)
            {
                db.Entry(publishers).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.pub_id = new SelectList(db.pub_info, "pub_id", "pr_info", publishers.pub_id);
            return View(publishers);
        }

        // GET: publishers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            publishers publishers = db.publishers.Find(id);
            if (publishers == null)
            {
                return HttpNotFound();
            }
            return View(publishers);
        }

        // POST: publishers/Delete/5
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
            string query = "SELECT COUNT(*) FROM employee";
            var cmd = new SqlCommand(query, con);
            int rowsAmount = (int)cmd.ExecuteScalar();
            employee employee;
            for (int i = 0; i < rowsAmount; i++)
            {
                employee = db.employee.FirstOrDefault(m => m.pub_id== id);
                if (employee == null)
                {

                }
                else
                {
                    db.employee.Remove(employee);
                    db.SaveChanges();
                }
            }
            query = "SELECT COUNT(*) FROM pub_info";
            cmd = new SqlCommand(query, con);
            rowsAmount = (int)cmd.ExecuteScalar();
            pub_info pub_Info;
            for (int i = 0; i < rowsAmount; i++)
            {
                pub_Info = db.pub_info.FirstOrDefault(m => m.pub_id == id);
                if (pub_Info == null)
                {

                }
                else
                {
                    db.pub_info.Remove(pub_Info);
                    db.SaveChanges();
                }
            }
            query = "SELECT COUNT(*) FROM titles";
            cmd = new SqlCommand(query, con);
            rowsAmount = (int)cmd.ExecuteScalar();
            titles titles;
            for (int i = 0; i < rowsAmount; i++)
            {
                titles = db.titles.FirstOrDefault(m => m.pub_id == id);
                if (titles == null)
                {

                }
                else
                {
                    db.titles.Remove(titles);
                    db.SaveChanges();
                }
            }
            publishers publishers = db.publishers.Find(id);
            db.publishers.Remove(publishers);
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
