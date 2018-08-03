using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVC_accenture.Models;

namespace MVC_accenture.Controllers
{
    public class AutoresController : Controller
    {
        private BibliotecaOKEntities db = new BibliotecaOKEntities();

        // GET: Autores
        public ActionResult Index()
        {
            var autores = db.Autores.Include(a => a.Pais);
            return View(autores.ToList());
        }

        // GET: Autores/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autore autore = db.Autores.Find(id);
            if (autore == null)
            {
                return HttpNotFound();
            }
            return View(autore);
        }

        // GET: Autores/Create
        public ActionResult Create()
        {
            ViewBag.Nacionalidad = new SelectList(db.Paises, "ID", "Descripcion");
            return View();
        }

        // POST: Autores/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Apellido,Nombre,FechaNacimiento,Nacionalidad")] Autore autore)
        {
            if (ModelState.IsValid)
            {
                db.Autores.Add(autore);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Nacionalidad = new SelectList(db.Paises, "ID", "Descripcion", autore.Nacionalidad);
            return View(autore);
        }

        // GET: Autores/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autore autore = db.Autores.Find(id);
            if (autore == null)
            {
                return HttpNotFound();
            }
            ViewBag.Nacionalidad = new SelectList(db.Paises, "ID", "Descripcion", autore.Nacionalidad);
            return View(autore);
        }

        // POST: Autores/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Apellido,Nombre,FechaNacimiento,Nacionalidad")] Autore autore)
        {
            if (ModelState.IsValid)
            {
                db.Entry(autore).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.Nacionalidad = new SelectList(db.Paises, "ID", "Descripcion", autore.Nacionalidad);
            return View(autore);
        }

        // GET: Autores/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Autore autore = db.Autores.Find(id);
            if (autore == null)
            {
                return HttpNotFound();
            }
            return View(autore);
        }

        // POST: Autores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Autore autore = db.Autores.Find(id);
            db.Autores.Remove(autore);
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
