using Microsoft.AspNetCore.Identity;
using SistemaAC.Data;
using SistemaAC.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SistemaAC.ModelsClass
{
    //aqui escribiremos los metodos para poder insertar en la tabla categorias para no sobrecargar el CategoriasController
    public class CategoriaModels
    {
        private ApplicationDbContext context;
        public CategoriaModels(ApplicationDbContext context)
        {
            this.context = context;
            filtrarDatos(1 , "mexico");
        }

        //metodo para insertar los datos en la tabbla categoria
        public List<IdentityError> CreateCategoria(string nombre, string descripcion, string estado)
        {
            var errorList = new List<IdentityError>();
            var categoria = new Categoria
            {
                Nombre = nombre,
                Descripcion = descripcion,
                //asiganamos un convert ala propiedad estado para convertir el dato de tipo string a un tipo boolean
                Estado = Convert.ToBoolean(estado)

            };
            //invocamos a context,para poder agregar nuestro objeto categoria que ya viene con los datos que almacenaremos
            context.Add(categoria);
            context.SaveChangesAsync();
            errorList.Add(new IdentityError
            {
                Code = "Save",
                Description = "Save"
            });
            return errorList;
        }

        //metodo del paginador y filtro
        public List<object[]> filtrarDatos(int numPagina, string valor)
        {
            int count = 0, cant, numRegistros = 0, inicio = 0, reg_por_pagina = 1;
            int can_paginas, pagina;
            string dataFielter = "", paginador = "", Estado = null;
            List<object[]> data = new List<object[]>();
            IEnumerable<Categoria> query;
            var categorias = context.Categoria.OrderBy(c => c.Nombre).ToList();
            numRegistros = categorias.Count;
            inicio = (numPagina - 1) * reg_por_pagina;
            can_paginas=(numRegistros /reg_por_pagina);
            if (valor== "null")
            {
                query = categorias.Skip(inicio).Take(reg_por_pagina);
            }
            else
            {
                //el metodo start busca los nombres que coincidan con el paramatro valor, utilizando la propiedad nombre
                query = categorias.Where(c => c.Nombre.StartsWith(valor)||c.Descripcion.StartsWith(valor)).Skip(inicio).Take(reg_por_pagina);
            }
            cant = query.Count();
           
            return data;


        }

    }
}
