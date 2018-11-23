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
                console.log(response);
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
                                console.log(nombre);
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
    

