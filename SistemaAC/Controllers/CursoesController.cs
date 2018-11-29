using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SistemaAC.Data;
using SistemaAC.Models;
using SistemaAC.ModelsClass;

namespace SistemaAC.Controllers
{
    public class CursoesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private CursoModels cursoModels;

        public CursoesController(ApplicationDbContext context)
        {
            _context = context;
            cursoModels = new CursoModels(context);
        }

        // GET: Cursoes
        public async Task<IActionResult> Index()
        {
            
            return View();
        }


        public List<Categoria> getCategorias()
        {
            return cursoModels.getCategorias();
        }

        public List<IdentityError> agregarCurso(int id,string nombre, string descripcion, Boolean estado,
            byte creditos, byte horas, decimal costo, int categoria, string funcion)
        {
            return cursoModels.agregarCurso(id,nombre, descripcion, estado, creditos, horas, costo,categoria,funcion);

        }

        public List<object[]> filtrarCursos(int numPagina, string valor, string order) {
            return  cursoModels.filtrarCursos(numPagina,valor,order);
        }

        public List<Curso>getCursos(int id)
        {
            return cursoModels.getCursos(id);
        }

        public List<IdentityError> editarCurso(int id, string nombre, string descripcion, Boolean estado, byte creditos,
            byte horas,decimal costo, int categoriaID, int funcion)
        {
            return cursoModels.editarCurso(id,nombre,descripcion,estado,creditos,horas,costo,categoriaID,funcion);
        }

        // GET: Cursoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var curso = await _context.Curso
                .Include(c => c.Categoria)
                .SingleOrDefaultAsync(m => m.CursoID == id);
            if (curso == null)
            {
                return NotFound();
            }

            return View(curso);
        }

        // GET: Cursoes/Create
        public IActionResult Create()
        {
            ViewData["CategoriaID"] = new SelectList(_context.Categoria, "CategoriaID", "CategoriaID");
            return View();
        }

        // POST: Cursoes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("CursoID,Nombre,Descripcion,Creditos,Horas,Costo,Estado,CategoriaID")] Curso curso)
        {
            if (ModelState.IsValid)
            {
                _context.Add(curso);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoriaID"] = new SelectList(_context.Categoria, "CategoriaID", "CategoriaID", curso.CategoriaID);
            return View(curso);
        }


        
      

       
    }
}
