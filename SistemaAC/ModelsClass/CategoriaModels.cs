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

        //atributo para editar el estado
        private Boolean estados;
        public CategoriaModels(ApplicationDbContext context)
        {
            this.context = context;

            //llamando al metodo filtrar datos para una prueba 
            //filtrarDatos(1 , "mexico");
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
            context.SaveChanges();
            errorList.Add(new IdentityError
            {
                Code = "Save",
                Description = "Save"
            });
            return errorList;
        }

        //metodo del paginador y filtro
        public List<object[]> filtrarDatos(int numPagina, string valor,string order)
        {
            int count = 0, cant, numRegistros = 0, inicio = 0, reg_por_pagina = 4;
            int can_paginas, pagina;
            string dataFielter = "", paginador = "", Estado = null;
            List<object[]> data = new List<object[]>();
            IEnumerable<Categoria> query;

            List<Categoria> categorias = null;
            switch (order)
            {
                case "nombre":
                    categorias = context.Categoria.OrderBy(c => c.Nombre).ToList();
                    break;
                case "des":
                    categorias = context.Categoria.OrderBy(c => c.Descripcion).ToList();

                    break;
                case "estado":
                    categorias = context.Categoria.OrderByDescending(c => c.Estado).ToList();
                    break;
                
            }

           
            numRegistros = categorias.Count;
            if ((numRegistros % reg_por_pagina) > 0)
            {
                numRegistros += 1;
            }
            inicio = (numPagina - 1) * reg_por_pagina;
            can_paginas = (numRegistros / reg_por_pagina);
            
            if (valor== "null")
            {
                query = categorias.Skip(inicio).Take(reg_por_pagina);
            }
            else
            {
                //el metodo start busca los nombres que coincidan con el paramatro valor, utilizando la propiedad nombre
                query = categorias.Where(c => c.Nombre.StartsWith(valor)||c.Descripcion.StartsWith(valor)).Skip(inicio).Take(reg_por_pagina);
            }
            //ahora tenemos la cantidad de la consulta de los datos que coinciden con nuestra busqueda
            cant = query.Count();

            //recoremos la consulta
            foreach (var item in query)
            {
                if (item.Estado == true)
                {
                    Estado = "<a  data-toggle='modal' data-target='#moEstado' onclick='editarEstado("+item.CategoriaID+ ',' +0+")'  class='btn btn-success' >Activo</a>";
                }
                else
                {
                    Estado = "<a data-toggle='modal' data-target='#moEstado' onclick='editarEstado(" + item.CategoriaID + ','+0+")' class='btn btn-danger' >NO Activo</a>";
                }
                dataFielter += "<tr>" +
                    "<td>" + item.Nombre + "</td>" +
                    "<td>" + item.Descripcion + "</td>" +
                    "<td>" + Estado + "</td>" +
                    
                    "<td>" +
                    //mandamos a llamar al modal que utilizamos para crear una nueva categoria
                    "<a data-toggle='modal' data-target='#moAgregar' class='btn btn-success' onclick='editarEstado("+ item.CategoriaID + ',' + 1 +")'>Edit</a>" +
                     //como es una tabla maestra no se pueden eliminar solo podemos habilitar o deshabilitar las categorias
                     //"&nbsp;&nbsp;&nbsp;" +
                     // "<a data-toggle='modal' data-target='#myModal3' class='btn btn-danger'>Delete</a>" +
                     "</td>" +
                     "</tr>";

            }
            if (valor == "null")
            {
              
                if (numPagina > 1)
                {
                    pagina = numPagina - 1;
                    paginador += "<a class='btn btn-default' onclick='filtrarDatos(" + 1 + ',' + '"' + order + '"' + ")'><<</a>" +
                        "<a class='btn btn-default' onclick='filtrarDatos(" + pagina + ',' + '"' + order + '"' + ")'> <</a>";
                }
                if ( 1 < can_paginas)
                {
                    paginador += "<strong class='btn btn-success'>" + numPagina + ".de." + can_paginas + "</strong>";
                }

                //para navegar entre los registros hacia adelante- primer b
                if (numPagina < can_paginas)
                {
                    pagina = numPagina + 1;
                    paginador += "<a class='btn btn-default' onclick='filtrarDatos(" + pagina + ',' + '"' + order + '"' + ")'> > </a>" +
                        "<a class='btn btn-default' onclick='filtrarDatos(" + can_paginas + ',' + '"' + order + '"' + ")'> >> </a>";
                        }
            }


            object[] dataObj = { dataFielter,paginador};
            data.Add(dataObj);
            return data;


        }
        public List<Categoria> getCategorias(int id)
        {

            return context.Categoria.Where(c => c.CategoriaID == id).ToList();
        }
        public List<IdentityError> editarCategorias(int idCategoria, string nombre, string descripcion, Boolean estado, int funcion)
        {
            var errorList = new List<IdentityError>();
            string code="", des="";

            switch (funcion)
            {
                case 0:
                    if (estado)
                    {
                        estados = false;
                    }
                    else
                    {
                        estados = true;
                    }
                   
                    
                    break;
                case 1:
                    estados = estado;
                    break;
             
            }

            //creo un objeto para invocar ala clase categoria  y asignarle los nuevos parametro que cambia el estado
            var categoria = new Categoria()
            {
                CategoriaID = idCategoria,
                Nombre = nombre,
                Descripcion = descripcion,
                Estado = estados,
            };

            try
            {
                //creo un objeto context para poder invocar los metoso update y save para guardar os cambios
                context.Update(categoria);
                context.SaveChanges();
                code = "Save";
                des = "se ha podido actualizar el estado";
            }
            catch (Exception ex)
            {
                code = "Error";
                des = ex.Message;
                throw;
            }


            errorList.Add(new IdentityError {
                Code = code,
                Description = des
            });
            return  errorList;
        }
    }
}
