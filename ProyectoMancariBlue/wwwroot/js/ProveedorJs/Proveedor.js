function NewProveedor() {
    showSpinner();
    setTimeout(function () {
    var IdCategoriaProveedor = GetDropDownValueSelected('ddlAddProveedorCategoriaModal', 'AddProveedorCategoriaList')
    var Identificacion = $("#txtIdentificación").val();
    var Nombre = $("#txtProveedorNombre").val();
    var Telefono = $("#txtProveedorTelefono").val();
    var Correo = $("#txtProveedorCorreo").val();
 

    if (Validate('ddlAddProveedorCategoriaModal', 'AddProveedorCategoriaList', 'txtIdentificación', 'txtProveedorNombre', 'txtProveedorTelefono', 'txtProveedorCorreo',false)) {
        var proveedor = { Identificacion: Identificacion, Nombre: Nombre, Telefono: Telefono, Correo: Correo, IdCategoriaProveedor: IdCategoriaProveedor };
        $.ajax({
            url: '/Proveedor/Create',
            type: 'POST',
            data: proveedor,
        }).done(function (response) {

            if (response.includes("éxito")) {
                Swal.fire({
                    text: response,
                    icon: 'success',
                    didClose: () => {
                        window.location.href = '/Proveedor/Index';
                    }
                });
            }
            else {
                Swal.fire('', response, 'warning');
            }
        });
    } else {

        }
        hideSpinner();
    }, 2000);

}

function ModifyProveedor() {
    showSpinner();
    setTimeout(function () {
    var IdCategoriaProveedor = GetDropDownValueSelected('ddlModifyProveedorCategoriaModal', 'ModifyProveedorCategoriaList')
    var Identificacion = $("#txtIdentificacionModify").val();
    var Nombre = $("#txtProveedorNombreModify").val();
    var Telefono = $("#txtProveedorTelefonoModify").val();
    var Correo = $("#txtProveedorCorreoModify").val();
    var Id = $("#hdfIdModify").val();
    var Estado = $("#hdfEstadoModify").val();
    if (Validate('ddlModifyProveedorCategoriaModal', 'ModifyProveedorCategoriaList', 'txtIdentificacionModify', 'txtProveedorNombreModify', 'txtProveedorTelefonoModify', 'txtProveedorCorreoModify',true)) {
        var proveedor = { Identificacion: Identificacion, Nombre: Nombre, Telefono: Telefono, Correo: Correo, IdCategoriaProveedor: IdCategoriaProveedor,Id:Id,Estado:Estado };
        $.ajax({
            url: '/Proveedor/Edit',
            type: 'POST',
            data: proveedor,
        }).done(function (response) {

            if (response.includes("éxito")) {
                Swal.fire({
                    text: response,
                    icon: 'success',
                    didClose: () => {
                        window.location.href = '/Proveedor/Index';
                    }
                });
            }
            else {
                Swal.fire('', response, 'warning');
            }
        });
    } else {

        }
        hideSpinner();
    }, 2000);
}


function Validate(dropdownListCategorySupplier, dataSelectedCategorySupplier, Identificacion,ProveedorNombre,Telefono,Correo,IsModify) {
    if ($("#" + Identificacion).val() === '') {
        Swal.fire('', 'Debe digitar la identificación.', 'error');
        return false;
    }
    else {
        if (!IsModify) {
            var supplierFound = SeachSupplierById($("#" + Identificacion).val());
            if (supplierFound) {
                Swal.fire('', 'Ya existe un proveedor registrado con la identificación: .' + $("#" + Identificacion).val(), 'error');
                return false;
            }
        }
       
    }
    if (GetDropDownValueSelected(dropdownListCategorySupplier, dataSelectedCategorySupplier) === null) {
        Swal.fire('', 'Debe seleccionar la categoría del proveedor', 'error');
        return false;
    }
 
    if ($("#" + ProveedorNombre).val() === '') {
        Swal.fire('', 'Debe digitar el nombre.', 'error');
        return false;
    }
    if ($("#" + Telefono).val() === '') {
        Swal.fire('', 'Debe digitar el número de teléfono.', 'error');
        return false;
    }
    if ($("#" + Correo).val() === '') {
        Swal.fire('', 'Debe digitar el correo.', 'error');
        return false;
    }
    else {
        if (!ValidateEmail($("#" + Correo).val())) {
            Swal.fire('', 'Formato del correo inválido.', 'error');
            return false;
        }
    }

    return true;
}