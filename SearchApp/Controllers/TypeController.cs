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
using System.Collections.ObjectModel;

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
            ViewBag.Attributes = unitOfWork.Get<Attributes>().ToList<Attributes>();
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
                var attributeIds = new List<int>();
                // Get selected types
                string selectedIds = Request["attributes[]"];
                if (selectedIds != null)
                {
                    string[] checkedIds = selectedIds.Split(',');
                    attributeIds.AddRange(checkedIds.Select(id => Convert.ToInt32(id)));
                }
                // Add Types to this Domain
                type.Attributes = new Collection<Attributes>();
                foreach (int id in attributeIds)
                {
                    Attributes attribute = unitOfWork.GetById<Attributes>(id);
                    type.Attributes.Add(attribute);
                }

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

            var selectedAttributes = type.Attributes;
            var selectableAttributes = unitOfWork.Get<Attributes>().ToList<Attributes>();
            foreach (var attribute in selectedAttributes)
            {
                selectableAttributes.Remove(attribute);
            }
            ViewBag.SelectedAttributes = selectedAttributes;
            ViewBag.SelectableAttributes = selectableAttributes;
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
                var attributeIds = new List<int>();
                // Get selected attributes
                string selectedIds = Request["attributes[]"];
                if (selectedIds != null)
                {
                    string[] checkedIds = selectedIds.Split(',');
                    attributeIds.AddRange(checkedIds.Select(id => Convert.ToInt32(id)));
                }
                // Add attributes to this Type
                int typeId = type.ID;
                // load domain with types from the database
                var typeItem = unitOfWork.getContext().Type.Include(r => r.Attributes).Single(r => r.ID == typeId);
                // apply the values that have changed
                unitOfWork.getContext().Entry(typeItem).CurrentValues.SetValues(type);
                // clear the types to let the framework know they have to be processed
                typeItem.Attributes.Clear();
                // now reload the types again, but from the list of selected ones provided by the view
                foreach (int attributeId in attributeIds)
                {
                    typeItem.Attributes.Add(unitOfWork.GetById<Attributes>(attributeId));
                }
                //finally, save changes as usual
                unitOfWork.getContext().SaveChanges();
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
