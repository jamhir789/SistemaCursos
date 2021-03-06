﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using SistemaAC.Data;
using SistemaAC.Models;

namespace SistemaAC.ModelsClass
{
    public class CursoModels
    {

        private ApplicationDbContext context;
        private string code = "", des = "";
        private List<IdentityError> errorList = new List<IdentityError>();
        private Boolean estados;

        public CursoModels(ApplicationDbContext context)
        {
            this.context = context;
        }

        internal List<Categoria> getCategorias()
        {
            return context.Categoria.Where(c => c.Estado == true).ToList();
        }

        public List<Categoria> getCategoria(int id)
        {
            return context.Categoria.Where(c => c.CategoriaID == id).ToList();
        }

        public List<object[]> filtrarCursos(int numPagina, string valor, string order)
        {
            int  cant, numRegistros = 0, inicio = 0, reg_por_pagina = 3;
            int can_paginas, pagina;
            string dataFielter = "", paginador = "", Estado = null;
            List<object[]> data = new List<object[]>();
            IEnumerable<Curso> query;

            List<Curso> categorias = null;
            switch (order)
            {
                case "nombre":
                    categorias = context.Curso.OrderBy(c => c.Nombre).ToList();
                    break;
                case "des":
                    categorias = context.Curso.OrderBy(c => c.Descripcion).ToList();

                    break;
                case "horas":
                    categorias = context.Curso.OrderByDescending(c => c.Horas).ToList();
                    break;
                case "creditos":
                    categorias = context.Curso.OrderByDescending(c => c.Creditos).ToList();
                    break;
                case "costo":
                    categorias = context.Curso.OrderByDescending(c => c.Costo).ToList();
                    break;
                case "estado":
                    categorias = context.Curso.OrderByDescending(c => c.Estado).ToList();
                    break;
                case "categoria":
                    categorias = context.Curso.OrderByDescending(c => c.Categoria).ToList();
                    break;
            }
            numRegistros = categorias.Count;
            if ((numRegistros % reg_por_pagina) > 0)
            {
                numRegistros += 1;
            }
            inicio = (numPagina - 1) * reg_por_pagina;
            can_paginas = (numRegistros / reg_por_pagina);
           
            if (valor == "null")
            {
                query = categorias.Skip(inicio).Take(reg_por_pagina);
            }
            else
            {
                //el metodo start busca los nombres que coincidan con el paramatro valor, utilizando la propiedad nombre
                query = categorias.Where(c => c.Nombre.StartsWith(valor) || c.Descripcion.StartsWith(valor) ).Skip(inicio).Take(reg_por_pagina);
            }
            //ahora tenemos la cantidad de la consulta de los datos que coinciden con nuestra busqueda
            cant = query.Count();
            foreach (var item in query)
            {
                var categoria = getCategoria(item.CategoriaID);
                if (item.Estado == true)
                {
                    Estado = "<a  data-toggle='modal' data-target='#modalEstadoCurso' onclick='editarEstadoCurso(" + item.CursoID + ',' + 0 + ")'  class='btn btn-success' >Activo</a>";
                }
                else
                {
                    Estado = "<a data-toggle='modal' data-target='#modalEstadoCurso' onclick='editarEstadoCurso(" + item.CursoID + ',' + 0 + ")' class='btn btn-danger' >NO Activo</a>";
                }
                dataFielter += "<tr>" +
                    "<td>" + item.Nombre + "</td>" +
                    "<td>" + item.Descripcion + "</td>" +
                    "<td>" + item.Creditos + "</td>" +
                    "<td>" + item.Horas + "</td>" +
                    "<td>" + item.Costo + "</td>" +
                    "<td>" + Estado + "</td>" +
                    "<td>" + categoria[0].Nombre + "</td>" +

                    "<td>" +
                    //mandamos a llamar al modal que utilizamos para crear una nueva categoria
                    "<a data-toggle='modal' data-target='#modalCS' class='btn btn-success' onclick='editarEstadoCurso(" + item.CursoID + ',' + 1 + ")'>Edit</a>" +
                     
                     "&nbsp;&nbsp;&nbsp;" +
                     "<a data-toggle='modal' data-target='#myModal3'   class='btn btn-danger'>Delete</a>" +
                     "</td>" +
                     "</tr>";

            }
            if (valor == "null")
            {

                if (numPagina > 1)
                {
                    pagina = numPagina - 1;
                    paginador += "<a class='btn btn-default' onclick='filtrarCursos(" + 1 + ',' + '"' + order + '"' + ")'><<</a>" +
                        "<a class='btn btn-default' onclick='filtrarCursos(" + pagina + ',' + '"' + order + '"' + ")'> <</a>";
                }
                if (1 < can_paginas)
                {
                    paginador += "<strong class='btn btn-success'>" + numPagina + ".de." + can_paginas + "</strong>";
                }

                //para navegar entre los registros hacia adelante- primer b
                if (numPagina < can_paginas)
                {
                    pagina = numPagina + 1;
                    paginador += "<a class='btn btn-default' onclick='filtrarCursos(" + pagina + ',' + '"' + order + '"' + ")'> > </a>" +
                        "<a class='btn btn-default' onclick='filtrarCursos(" + can_paginas + ',' + '"' + order + '"' + ")'> >> </a>";
                }
            }





            cant = query.Count();
            object[] dataObj = { dataFielter, paginador };
            data.Add(dataObj);
            return data;
        }




        public List<Curso> getCursos(int id)
        {
            return context.Curso.Where(c => c.CursoID == id).ToList();
        }


        public List<IdentityError> agregarCurso(int id, string nombre, string descripcion, Boolean estado,
            byte creditos, byte horas, decimal costo, int categoriaID, string funcion)
        {
            var errorList = new List<IdentityError>();
            var curso = new Curso
            {
                Nombre = nombre,
                Descripcion = descripcion,
                //asiganamos un convert ala propiedad estado para convertir el dato de tipo string a un tipo boolean
                Estado = estado,
                Creditos= creditos,
                Horas = horas,
                Costo =  costo,
                CategoriaID = categoriaID,
               

            };
            try
            {
                context.Add(curso);
                context.SaveChanges();
                code = "Save";
                des = "Save";

            }
            catch(Exception e)
            {
                code = "error";
                des = e.Message;
            }
            
            errorList.Add(new IdentityError
            {
                Code = code,
                Description = des
            });
            return errorList;
        }


        public List<IdentityError> editarCurso(int id, string nombre, string descripcion, bool estado, byte creditos,
            byte horas, decimal costo, int categoriaID, int funcion)
        {
            switch(funcion)
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

            //var errorList = new List<IdentityError>();
            var curso = new Curso
            {
                CursoID= id,
                Nombre = nombre,
                Descripcion = descripcion,
                //asiganamos un convert ala propiedad estado para convertir el dato de tipo string a un tipo boolean
                Creditos = creditos,
                Horas = horas,
                Costo = costo,
                Estado = estados,
                CategoriaID = categoriaID,


            };
            try
            {
                context.Update(curso);
                context.SaveChanges();
                code = "Save";
                des = "Save";

            }
            catch (Exception ex)
            {
                code = "error";
                des = ex.Message;
            }

            errorList.Add(new IdentityError
            {
                Code = code,
                Description = des
            });
            return errorList;

        }
      
    } 
}
