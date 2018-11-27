//objeto de tipo promesa
var promesa = new Promise((resolve, reject) => {
});


class Cursos {
    constructor(nombre, descripcion, creditos, horas, costo, estado, categoria, action) {

        this.nombre = nombre;
        this.descripcion = descripcion;
        this.creditos = creditos;
        this.horas = horas;
        this.costo = costo;
        this.estado = estado;
        this.categoria = categoria;
        this.action = action;
    }
    getCategorias() {
        var action = this.action;
        var count = 1;
        $.ajax({
            type: "POST",
            url: action,
            data: {},
            success: (response) => {
                //console.log(response);
                if (0 < response.length) {
                    for (var i = 0; i < response.length; i++) {
                        document.getElementById('CategoriaCursos').options[count] = new Option(response[i].nombre, response[i].categoriaID);
                        count++;
                    }

                }
            }
        });


    }


    agregarCurso(id, funcion) {
        if (this.nombre == "") {
            document.getElementById("Nombre").focus();
        }
        else {
            if (this.descripcion == "") {
                document.getElementById("Descripcion").focus();
            }
            else {
                if (this.creditos == "") {
                    document.getElementById("Creditos").focus();
                }
                else {
                    if (this.horas == "") {
                        document.getElementById("Horas").focus();
                    }
                    else {
                        if (this.costo == "") {
                            document.getElementById("Costo").focus();
                        }
                        else {
                            if (this.categoria == "0") {
                                document.getElementById("mensaje").innerHTML="Seleccione un curso";
                            }
                            else {

                                var nombre = this.nombre;
                                var descripcion = this.descripcion;
                                var creditos = this.creditos;
                                var horas = this.horas;
                                var costo = this.costo;
                                var estado = this.estado;
                                var categoria = this.categoria;
                                var action = this.action;
                                //console.log(nombre);
                                $.ajax({
                                    type: "POST",
                                    url: action,
                                    data: {
                                        id,nombre,descripcion,creditos,horas,costo,estado,categoria,funcion
                                    },
                                    success: (response) => {
                                        if ("Save" == response[0].code) {
                                            this.restablecer();
                                        }
                                        else {
                                            document.getElementById("mensaje").innerHTML = "No se ha podido guardar el curso";
                                        }
                                    }
                                })
                            }
                        }
                        }
                    }
                    }
                }
                
            
    }


    getCursos(id, funcion)
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
                        document.getElementById("titleCurso").innerHTML = "Esta seguro de Desactivar el curso" + response[0].nombre;
                    }
                    else {
                        document.getElementById("titleCurso").innerHTML = "Esta seguro de Activar el curso" + response[0].nombre;
                    }
                    promesa = Promise.resolve({
                        id: response[0].cursoID,
                        nombre: response[0].nombre,
                        descripcion: response[0].descripcion,
                        creditos: response[0].creditos,
                        hora: response[0].horas,
                        costo: response[0].costo,
                        estado: response[0].estado,
                        categoriaID: response[0].categoriaID

                    });
                } else {
                }
            }

        });
    }


    restablecer() {
        document.getElementById("Nombre").value = "";
        document.getElementById("Descripcion").value = "";
        document.getElementById("Creditos").value = "";
        document.getElementById("Horas").value = "";
        document.getElementById("Costo").value = "";
        document.getElementById("Estado").checked = false;
        document.getElementById("CategoriasCursos").selectedIndex=0 ;
        document.getElementById("mensaje").innerHTML = "";
        $('#modalCS').modal('hide');
    }

    filtrarCursos(numPagina,order)
    {
        var valor = this.nombre;
        var action = this.action;
        if (valor == "") {
            valor = "null";
        }
        $.ajax({
            type: "POST",
            url: action,
            data: { valor, numPagina, order },
            success: (response) => {
                
                $("#resultSearch2").html(response[0]);
                $("#paginado").html(response[1]);

            }
        });
    }
}
    

