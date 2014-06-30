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
    public class DomainController : Controller
    {
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
            ViewBag.Types = unitOfWork.Get<Types>().ToList<Types>();
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
                var typeIds = new List<int>();
                // Get selected types
                string selectedIds = Request["types[]"];
                if (selectedIds != null)
                {
                    string[] checkedIds = selectedIds.Split(',');
                    typeIds.AddRange(checkedIds.Select(id => Convert.ToInt32(id)));
                }
                // Add Types to this Domain
                domain.Types = new Collection<Types>();
                foreach (int id in typeIds)
                {
                    Types type = unitOfWork.GetById<Types>(id);
                    domain.Types.Add(type);
                }

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

            var selectedTypes = domain.Types;
            var selectableTypes = unitOfWork.Get<Types>().ToList<Types>();
            foreach (var type in selectedTypes)
            {
                selectableTypes.Remove(type);
            }
            ViewBag.SelectedTypes = selectedTypes;
            ViewBag.SelectableTypes = selectableTypes;
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
                var typeIds = new List<int>();
                // Get selected types
                string selectedIds = Request["types[]"];
                if (selectedIds != null)
                {
                    string[] checkedIds = selectedIds.Split(',');
                    typeIds.AddRange(checkedIds.Select(id => Convert.ToInt32(id)));
                }
                // Add Types to this Domain
                int domainId = domain.ID;
                // load domain with types from the database
                var domainItem = unitOfWork.getContext().Domain.Include(r => r.Types).Single(r => r.ID == domainId);
                // apply the values that have changed
                unitOfWork.getContext().Entry(domainItem).CurrentValues.SetValues(domain);
                // clear the types to let the framework know they have to be processed
                domainItem.Types.Clear();
                // now reload the types again, but from the list of selected ones provided by the view
                foreach (int typeId in typeIds)
                {
                    domainItem.Types.Add(unitOfWork.GetById<Types>(typeId));
                }
                //finally, save changes as usual
                unitOfWork.getContext().SaveChanges();
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
                unitOfWork.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
