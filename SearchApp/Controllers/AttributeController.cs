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
    public class AttributeController : Controller
    {
        private DataContext db = new DataContext();
        private IUnitOfWork unitOfWork;

        public AttributeController()
        {
            unitOfWork = new UnitOfWork<DataContext>();
        }

        // GET: /Attribute/
        public ActionResult Index()
        {
            
            return View(db.Attribute.ToList());
        }

        // GET: /Attribute/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attributes attribute = unitOfWork.GetById<Attributes>(id);
            if (attribute == null)
            {
                return HttpNotFound();
            }
            return View(attribute);
        }

        // GET: /Attribute/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Attribute/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,FreebaseName")] Attributes attribute)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Insert<Attributes>(attribute);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(attribute);
        }

        // GET: /Attribute/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attributes attribute = unitOfWork.GetById<Attributes>(id);
            if (attribute == null)
            {
                return HttpNotFound();
            }
            return View(attribute);
        }

        // POST: /Attribute/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include="ID,Name,FreebaseName")] Attributes attribute)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Update<Attributes>(attribute);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(attribute);
        }

        // GET: /Attribute/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Attributes attribute = unitOfWork.GetById<Attributes>(id);
            if (attribute == null)
            {
                return HttpNotFound();
            }
            return View(attribute);
        }

        // POST: /Attribute/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            unitOfWork.DeleteById<Attributes>(id);
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
