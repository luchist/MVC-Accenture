using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_accenture.Models;
using System.Data.Entity;
using System.Web.Helpers;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Net;

namespace MVC_accenture.Controllers
{
    public class GridAutoresController : Controller
    {
        BibliotecaOKEntities db = new BibliotecaOKEntities();
        
        // GET: GridAutores
        //public ActionResult Index()
        //{
        //    var autores = db.Autores.ToList();
        //    return View(autores);
        //}

        public ViewResult Index(string buscar)
        {
            var autores = db.Autores.Include(a => a.Pais);
            if (buscar != "")
            {
                if (!String.IsNullOrEmpty(buscar))
                {
                    //Contiene las letras
                    //autores = autores.Where(p => p.Apellido.Contains(buscar));
                    //Comienza con la letra
                    autores = autores.Where(p => p.Apellido.StartsWith(buscar));
                }
            }
            return View(autores.ToList());
        }


        public FileStreamResult ExportarPDF()
        {
            var autores = db.Autores.Include(a => a.Pais);
            WebGrid grid = new WebGrid(source: autores, canPage: false, canSort: false);
            string gridHtml = grid.GetHtml(
                    columns: grid.Columns
                            (
                             grid.Column("ID", header: "Codigo"),
                             grid.Column("Apellido", header: "Apellido"),
                             grid.Column("Nombre", header: "Nombre"),
                             grid.Column("Nacionalidad", header: "CodPais"),
                             grid.Column("Pais.Descripcion", header: "Pais"),
                             grid.Column("FechaNacimiento", header: "Fecha Nacimiento", format: (item) => string.Format("{0:dd/MM/yyyy}", item.FechaNacimiento))
                            )
                    ).ToString();
            string exportData = String.Format("<html><head>{0}</head><body>{1}</body></html>", "<p>Lista de Autores</p> <style>table{ border-spacing: 10px; border-collapse: separate; }</style>", gridHtml);
            var bytes = System.Text.Encoding.UTF8.GetBytes(exportData);
            using (var input = new MemoryStream(bytes))
            {
                var output = new MemoryStream();
                var document = new iTextSharp.text.Document(PageSize.A4, 50, 50, 50, 50);
                var writer = PdfWriter.GetInstance(document, output);
                writer.CloseStream = false;
                document.Open();
                var xmlWorker = iTextSharp.tool.xml.XMLWorkerHelper.GetInstance();
                xmlWorker.ParseXHtml(writer, document, input, System.Text.Encoding.UTF8);
                document.Close();
                output.Position = 0;
                return new FileStreamResult(output, "application/pdf");
            }
        }

        public void ExportarEXCEL()
        {
            var grid = new GridView
            {
                DataSource = db.Autores.ToList()
            };
            grid.DataBind();
            Response.ClearContent();
            Response.AddHeader("content-disposition", "inline; filename=Autores.xls");
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }

        public ActionResult GetPDF(int? id)
        {
            string filename = @"~/Content/Borges.pdf";
            return File(filename, "application/pdf",
                Server.HtmlEncode(filename));
        }

        [HttpGet]
        public ActionResult Agregar()
        {
            ViewBag.Nacionalidad = new SelectList(db.Paises, "ID", "Descripcion");
            Autore autor = new Autore();
            return View(autor);
        }

        [HttpPost]
        public ActionResult Agregar(Autore autor)
        {
            if (ModelState.IsValid)
            {
                db.Autores.Add(autor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(autor);
        }

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

        [HttpPost]
        public ActionResult Edit([Bind(Include = "ID,Apellido,Nombre,FechaNacimiento,Nacionalidad")] Autore autore)
        {
            if (ModelState.IsValid)
            {
                //USAR STORED PROCEDURE
                //db.Entry(autore).State = EntityState.Modified;
                //db.SaveChanges();
                BibliotecaOKEntities db = new BibliotecaOKEntities();
                db.ProcModificaAutor(autore.ID, autore.Apellido, autore.Nombre, autore.FechaNacimiento, autore.Nacionalidad);
                return RedirectToAction("Index");
            }
            ViewBag.Nacionalidad = new SelectList(db.Paises, "ID", "Descripcion", autore.Nacionalidad);
            return View(autore);
        }



    }
}