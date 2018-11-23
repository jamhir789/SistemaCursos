//modal de usuarios
$('#modalEditar').on('shown.bs.modal', function () {
    $('#myInput').focus()
})
//modal de Categorias
$('#moAgregar').on('shown.bs.modal', function () {
    $('#Nombre').focus()
})
//modal de Cursos
$('#modalCS').on('chown.bs.modal', () => {
    $('#Nombre').focus();
});


//funcion que se ejecuta automaticamente cada vez que se carga la vista index esto trae la tabla de los registros a visualizarse
$().ready(() => {
    document.getElementById("filtrar").focus();
    filtrarDatos(1, "nombre");
    filtrarCursos(1,"nombre");
    getCategorias();
});



//toma la funcion mostrar usuario, al hacer click en el boton editar se ejecuta esta funcion
function getUsuario(id,action)
{
    $.ajax({
        type: "POST",
        url: action,
        data: {id},
        success: function (response) {
            mostrarUsuario(response);
        }

    });

}

//la variaable items obtiene todos los datos de abajo
var items;
var j = 0;
///variables globales por cada propiedad del usuario
var id;
var userName;
var email;
var phoneNumber;

var role;
var selectRole;
//otras variables donde almacenaremos los datos de registro, pero estos datos no seran modificados
var accessFailedCount;
var concurrencyStamp;
var emailConfirmed ;
var lockoutEnabled;
var lockoutEnd;
var normalizedUserName;
var normalizedEmail;
var passwordHash;
var phoneNumberConfirmed;
var securityStamp;
var twoFactorEnabled;



/* 
 CODIGO PERTENECIANTE A USUARIOS/////////////////////////////////////////////////
 */
//funcion para mostrar loos datos
function mostrarUsuario(response)
{
    items = response;
    j = 0;
    for (var i; i < 3;i++)
    {
        var x = document.getElementById('Select');
        x.remove(i);
    }

    $.each(items, function (index, val) {
        $('input[name=Id]').val(val.id);
        $('input[name=UserName]').val(val.userName);
        $('input[name=Email]').val(val.email);
        $('input[name=PhoneNumber]').val(val.phoneNumber);
        document.getElementById('Select').options[0] = new Option(val.role, val.roleId);

        //mostrar los detalles del usuario
        $("#dEmail").text(val.email);
        $("#dUserName").text(val.userName);
        $("#dPhoneNumber").text(val.phoneNumber);
        $("#dRole").text(val.role);


        //Mostrar los datos del usuario que deseo eliminar
        $("#eUsuario").text(val.userName);
        $('input[name=EIdUsuario]').val(val.id);

    });
}
function getRoles(action)
{
    $.ajax({
        type: "POST",
        url: action,
        data: {},
        success: function (response) {
            if (j==0)
            {
                for (var i = 0; i < response.length;i++)
                {
                    document.getElementById('Select').options[i] = new Option(response[i].text, response[i].value);
                    document.getElementById('SelectNuevo').options[i] = new Option(response[i].text, response[i].value);
                }
                j = 1;
            }
        }

    })
}


function editarUsuario(action)
{
    id = $('input[name=Id]')[0].value;
    email = $('input[name=Email]')[0].value;
    phoneNumber = $('input[name=PhoneNumber]')[0].value;
    role = document.getElementById('Select');
    selectRole = role.options[role.selectedIndex].text;


    $.each(items, function (index, val) {
        accessFailedCount = val.accessFailedCount;
        concurrencyStamp = val.concurrencyStamp;
        emailConfirmed = val.emailConfirmed;
        lockoutEnabled = val.lockoutEnabled;
        lockoutEnd = val.lockoutEnd;
        userName = val.userName;
        normalizedUserName = val.normalizedUserName;
        normalizedEmail = val.normalizedEmail;
        passwordHash = val.passwordHash;
        phoneNumberConfirmed = val.phoneNumberConfirmed;
        securityStamp = val.securityStamp;
        twoFactorEnabled = val.twoFactorEnabled;
    });
    
    $.ajax({
        type:"POST",
        url: action,
        data: {id,userName, email, phoneNumber, accessFailedCount, concurrencyStamp, emailConfirmed,
            lockoutEnabled, lockoutEnd,  normalizedEmail,normalizedUserName, 
            passwordHash, phoneNumberConfirmed, securityStamp, twoFactorEnabled, selectRole
        },
        success: function (response)
        {
           
            if (response == "Save") {
                window.location.href = "Usuarios";
            }
            else
            {
                alert("No se puede editar los datos del usuario");
            }
        }


    });

}

function ocultarDetalleUsuario()
{
    $("#modalDetalle").modal("hide");
}

function eliminarUsuario(action)
{
    //con jquery leeo el valor que esta dentro del input
    var id = $('input[name=EIdUsuario]')[0].value;
    $.ajax({
        type: "POST",
        url: action,
        data: { id },
        success: function (response) {
            if (response === "Delete")
            {
                window.location.href = "Usuarios";
            }
            else
            {
                alert("No se puede eliminar el registro");
            }
        }
    });
}

function crearUsuario(action)
{
    //obtener los datos ingresados en los input
    email = $('input[name=EmailNuevo]')[0].value;
    phoneNumber = $('input[name=PhoneNumberNuevo]')[0].value;
    passwordHash = $('input[name=PasswordHashNuevo]')[0].value;
    role = document.getElementById('SelectNuevo');
    selectRole = role.options[role.selectedIndex].text;

    //vamos a validar que los datos no esten vacios
    if (email == "")
    {
        $('#EmailNuevo').focus();
        alert("Ingrese el email del usuario");
    }
    else
    {
        if (passwordHash == "") {
            $('#Password').focus();
            alert("Ingrese el password del usuario");
        }
        else
        {
            $.ajax({
                type: "POST",
                url: action,
                data: { email, phoneNumber, passwordHash, selectRole },
                success: function (response) {
                    if (response == "Save") {
                        window.location.href = "Usuarios";
                    }
                    else
                    {
                        $('#mensajenuevo').html("Nose puede gurardar el usuario. <br>Seleccione un rol.</br> Ingrese un email correcto <br/> el password debe tener de 6-100 caracteres,al menos un caracter especial, una letra mayuscula y un numero")
                    }
                }

                });
        }
        

    }
}

////////////////////////////////////////////////////////////////////////////////////





/////////////////////////codigo perteneciente a categorias

//objetos globales
var idCategoria;
var funcion = 0; 

var agregarCategoria = () => {
    var nombre = document.getElementById("Nombre").value;
    var descripcion = document.getElementById("Descripcion").value;
    var estados = document.getElementById('Estado');
    //creamos una variable estado para obtener el id y despues pasarlo a estado donde obtenemos el dato seleccionado 
    var estado = estados.options[estados.selectedIndex].value
    if (funcion == 0) {
        var action = 'Categorias/CreateCategoria';
    }
    else {
        var action = 'Categorias/editarCategorias';
    }
   
    //mandamos a llamar a la clases categorias para despues ejecutarlo
    var categoria = new Categorias(nombre, descripcion, estado, action);
    categoria.agregarCategoria(idCategoria, funcion);
    //ahora que ya tenemos las invocaciones no ahi que olvidar que debemos pasar el archivo categorias.js al layout por que lo estara buscando
}

var filtrarDatos = (numPagina,order) =>
{
    var valor = document.getElementById("filtrar").value;
    var action = 'Categorias/filtrarDatos';
    var categoria = new Categorias(valor, "", "", action);
    categoria.filtrarDatos(numPagina,order);
}

var editarEstado = (id, fun) =>
{
    idCategoria = id;
    funcion = fun;
    var action = 'Categorias/getCategorias';
    var categoria = new Categorias("", "", "", action);
    categoria.getCategoria(id, funcion);
}

var editarCategorias = () =>
{
    var action = 'Categorias/editarCategorias';
    var categoria = new Categorias("", "", "", action);
    categoria.editarCategorias(idCategoria,funcion);
}

//////////////////////////////////


///////////////codigo perteneciente a cursos


//para poder obtener las categorias activas
var getCategorias = () => {
    var action = 'Cursoes/getCategorias';
    var cursos = new Cursos("", "", "", "", "", "", "", action);
    cursos.getCategorias();
}

var agregarCurso = () => {
    var action = 'Cursoes/agregarCurso'
    var nombre = document.getElementById("Nombre").value;
    var descripcion = document.getElementById("Descripcion").value;
    var creditos = document.getElementById("Creditos").value;
    var horas = document.getElementById("Horas").value;
    var costo = document.getElementById("Costo").value;
    var estado = document.getElementById("Estado").checked;
    var categorias = document.getElementById('CategoriaCursos');
    var categoria = categorias.options[categorias.selectedIndex].value;
    
   
    var cursos = new Cursos(nombre, descripcion, creditos,horas,costo,estado,categoria, action);
    cursos.agregarCurso("","");
}

var filtrarCursos = (numPagina, order) => {
    var valor = document.getElementById("filtrar").value;
    var action = 'Cursoes/filtrarCursos';
    var curso = new Cursos(valor, "", "", "", "", "", "",  action);
    curso.filtrarCursos(numPagina, order);
}