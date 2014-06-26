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
    public class TypeController : Controller
    {
        private IUnitOfWork unitOfWork;

        public TypeController()
        {
            unitOfWork = new UnitOfWork<DataContext>();
        }

        // GET: /Type/
        public ActionResult Index()
        {
            return View(unitOfWork.Get<Types>(orderBy: q => q.OrderBy(u => u.Name)).ToList());
        }

        // GET: /Type/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Types type = unitOfWork.GetById<Types>(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        // GET: /Type/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Type/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include="ID,Name,FreebaseName")] Types type)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Insert<Types>(type);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(type);
        }

        // GET: /Type/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Types type = unitOfWork.GetById<Types>(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        // POST: /Type/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,FreebaseName")] Types type)
        {
            if (ModelState.IsValid)
            {
                unitOfWork.Update<Types>(type);
                unitOfWork.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(type);
        }

        // GET: /Type/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Types type = unitOfWork.GetById<Types>(id);
            if (type == null)
            {
                return HttpNotFound();
            }
            return View(type);
        }

        // POST: /Type/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            unitOfWork.DeleteById<Types>(id);
            unitOfWork.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
