
//llamamos a localStorage para almacenar los datos en el navegador
var localStorage = window.localStorage;

class Categorias {
    constructor(nombre,descripcion,estado,action)
    {
        this.nombre = nombre;
        this.descripcion = descripcion;
        this.estado = estado;
        this.action = action;
    }
   //creo un metodo
    agregarCategoria(id,funcion)
    {
        if (this.nombre == "")
        {
            document.getElementById("Nombre").focus();
        }
        else
        {
            if (this.descripcion == "")
            {
                document.getElementById("Descripcion").focus();
            }
            else
            {
                if (this.estado == "0")
                {
                    document.getElementById("mensaje").innerHTML="Seleccione un estado";
                }
                else
                {
                    //alert(this.nombre);
                    var nombre = this.nombre;
                    var descripcion = this.descripcion;
                    var estado = this.estado;
                    var action = this.action;
                    var mensaje ='';
                    $.ajax({
                        type: "POST",
                        url: action,
                        data: {id,nombre, descripcion, estado,funcion },
                        success: (response) => {
                            $.each(response, (index, val) => {
                                mensaje=val.code;

                            });
                            console.log(response);
                            if (mensaje == "Save") {
                                this.restablecer();
                            }
                            else
                            {
                                document.getElementById("mensaje").innerHTML("Nose puede gurardar la categoria. <br>verifique que ha llenado todos los campos correctamente")
                            }
                           
                        }
                        
                    });

                }
            }
        }
    }
    //
    filtrarDatos(numPagina,order)
    {
        var valor = this.nombre;
        var action = this.action;
        if (valor == "")
        {
            valor = "null";
        }
        $.ajax({
            type: "POST",
            url: action,
            data: { valor, numPagina , order},
            success: (response) => {
                console.log(response);
                $.each(response, (index, val) => {

                    $("#resultSearch").html(val[0]);
                    $("#paginado").html(val[1]);

                });
            }

        });
    }
    //recibe la direccion del controlador
    getCategoria(id,funcion)
    {
        var action = this.action;

        $.ajax({
            type: "POST",
            url: action,
            data: { id },
            success: (response) => {
                console.log(response);
                if (funcion == 0) {

                    if (response[0].estado) {
                        //obtenemos el id de el h4 y le asiganmos un texto ademas de concatenar la propiedad nombre del objeto
                        document.getElementById("titleCategoria").innerHTML = "Estas seguro de desactivar la categoria " + response[0].nombre
                    }
                    else {
                        document.getElementById("titleCategoria").innerHTML = "Estas seguro de habilitar esta categoria " + response[0].nombre
                    }
                }

                else
                {
                    document.getElementById("Nombre").value = response[0].nombre;
                    document.getElementById("Descripcion").value = response[0].descripcion;
                    if (response[0].estado) {
                        document.getElementById("Estado").selectedIndex = 1;
                    }
                    else
                    {
                        document.getElementById("Estado").selectedIndex = 2;
                    }
                }

                //invocamos al metodo - como los datos almacenados por el servidor son de tipo string
                // llamamos la funcion stringfy para poder pasarselos al response
                localStorage.setItem("categoria", JSON.stringify(response));
            }

        })

    }
    //Editar categoria
    editarCategorias(id,funcion)
    {
      
        var action = this.action;
        var response = JSON.parse(localStorage.getItem("categoria"));
             //se coloca el valor 0 al objeto response para poder obtener la informacion en la memoria local del navegador
        var  nombre = response[0].nombre;
        var descripcion = response[0].descripcion;
        var estado = response[0].estado;
                //caudno ya tenemos la informacion eliminamos esa informacion para que nose llene el espacio de memoria que es de 5mb
                localStorage.removeItem("categoria");
                // le pasamos la informacion del metodo editarCategoria al metodo editar
 
           //enviamos por medio de ajax la informacion al controlador
        $.ajax({
            type: "POST",
            url: action,
            data: { id, nombre, descripcion, estado, funcion },
            success: (response) => {
                console.log(response);
                this.restablecer();
            }
        });
          
    }

    restablecer() {
        document.getElementById("Nombre").value = "";
        document.getElementById("Descripcion").value = "";
        document.getElementById("mensaje").innerHtml = "";
        document.getElementById("Estado").selectedIndex = 0;
        $('#moAgregar').modal('hide');
        $('#moEstado').modal('hide');
        //ejecutamos filtrar datos para actualizar los datos despues de cambiar el estado  de una categoria
        filtrarDatos(1, "nombre");
    }

    }



    //
  



