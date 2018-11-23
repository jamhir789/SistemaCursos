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
    public class CategoriasController : Controller
    {
        private readonly ApplicationDbContext _context;
        //creamos un objeto de la clase categorias model
        private CategoriaModels categoriaModels;
        public CategoriasController(ApplicationDbContext context)
        {
            _context = context;
            //
            categoriaModels = new CategoriaModels(_context);
        }

        // GET: Categorias
        public async Task<IActionResult> Index()
        {
            return View();
        }

        //metodo para obtener los datos por ajax del metodo filtrarDatos
        public List<object[]>filtrarDatos(int numPagina,string valor,string order)
        {
            return categoriaModels.filtrarDatos(numPagina, valor,order);
        }


        public List<Categoria> getCategorias(int id)
        {
            return categoriaModels.getCategorias(id) ;
        }



        //creamos el metodo que recibe los datos del ajax en Categorias.js
        // y de esta forma los retornamos al metodo de CreateCategorias dentro del archivo CategoriaModels
        public List<IdentityError> CreateCategoria(string nombre, string descripcion, string estado)
        {
            return categoriaModels.CreateCategoria(nombre,descripcion,estado);
            
        }


        //editar categoria
        public List<IdentityError>editarCategorias(int id, string nombre , string descripcion , Boolean estado, int funcion)
        {
            return categoriaModels.editarCategorias(id,nombre,descripcion,estado,funcion);
        }





       
        // GET: Categorias/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var categoria = await _context.Categoria.SingleOrDefaultAsync(m => m.CategoriaID == id);
            if (categoria == null)
            {
                return NotFound();
            }
            return View(categoria);
        }

        
    }

    


}
