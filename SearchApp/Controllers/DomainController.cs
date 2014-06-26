using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using SearchApp.Models;
using SearchApp.DataAccess.Interfaces;
using SearchApp.DataAccess.Implementation;

namespace SearchApp.Controllers
{
    [Authorize]
    public class DomainController : Controller
    {
        private DataContext db = new DataContext();
        private IUnitOfWork unitOfWork;

        public DomainController()
        {
            unitOfWork = new UnitOfWork<DataContext>();
        }

        // GET: /Domain/
        public ActionResult Index()
        {
            return View(unitOfWork.Get<Domains>(orderBy: q => q.OrderBy(u => u.Name)).ToList());
        }

        // GET: /Domain/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Domains domain = unitOfWork.GetById<Domains>(id);
            if (domain == null)
            {
                return HttpNotFound();
            }
            return View(domain);
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
        public ActionResult Create([Bind(Include="ID,Name,FreebaseName")] Domains domain)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Insert<Domains>(domain);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(domain);
        }

        // GET: /Domain/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Domains domain = unitOfWork.GetById<Domains>(id);
            if (domain == null)
            {
                return HttpNotFound();
            }
            return View(domain);
        }

        // POST: /Domain/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,FreebaseName")] Domains domain)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Update<Domains>(domain);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(domain);
        }

        // GET: /Domain/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Domains domain = unitOfWork.GetById<Domains>(id);
            if (domain == null)
            {
                return HttpNotFound();
            }
            return View(domain);
        }

        // POST: /Domain/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            unitOfWork.DeleteById<Domains>(id);
            unitOfWork.SaveChanges();
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
