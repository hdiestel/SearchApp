using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SearchApp.Models;

namespace SearchApp.Controllers
{
    [Authorize]
    public class DomainController : Controller
    {
        private DataContext db = new DataContext();

        // GET: /Domain/
        public ActionResult Index()
        {
            return View(db.Domain.ToList());
        }

        // GET: /Domain/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Domains domains = db.Domain.Find(id);
            if (domains == null)
            {
                return HttpNotFound();
            }
            return View(domains);
        }

        // GET: /Domain/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Domain/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,FreebaseName")] Domains domains)
        {
            if (ModelState.IsValid)
            {
                db.Domain.Add(domains);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(domains);
        }

        // GET: /Domain/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Domains domains = db.Domain.Find(id);
            if (domains == null)
            {
                return HttpNotFound();
            }
            return View(domains);
        }

        // POST: /Domain/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,FreebaseName")] Domains domains)
        {
            if (ModelState.IsValid)
            {
                db.Entry(domains).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(domains);
        }

        // GET: /Domain/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Domains domains = db.Domain.Find(id);
            if (domains == null)
            {
                return HttpNotFound();
            }
            return View(domains);
        }

        // POST: /Domain/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Domains domains = db.Domain.Find(id);
            db.Domain.Remove(domains);
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
