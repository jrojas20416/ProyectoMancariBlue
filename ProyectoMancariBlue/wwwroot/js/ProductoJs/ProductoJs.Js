function NewProducto() {
    showSpinner();
    setTimeout(function () {
        var Codigo = $("#txtCodigo").val();
        var Nombre = $("#txtProductoNombre").val();
        var Descripcion = $("#txtDescripcion").val();
        var IdTipoProducto = GetDropDownValueSelected('ddlAddTipoProductoModal', 'AddTipoProductolList');
        var IdCategoriaProducto = GetDropDownValueSelected('ddlAddProductoCategoriaModal', 'AddProductoCategoriaList');
        var Identification = GetDropDownValueSelected('ddlAddProductoProveedorModal', 'AddProductoProveedorList');
        var Stock = $("#txtCantidad").val();
        var Estado = $("#chkEstado").is(':checked');

        if (Validate('txtCodigo', 'txtProductoNombre  ', 'txtDescripcion', 'ddlAddTipoProductoModal', 'AddTipoProductolList', 'ddlAddProductoCategoriaModal', 'AddProductoCategoriaList', 'ddlAddProductoProveedorModal',
            'AddProductoProveedorList', 'txtCantidad', false)) {
            var producto = { Codigo: Codigo, Nombre: Nombre, Descripcion: Descripcion, IdTipoProducto: IdTipoProducto, IdCategoriaProducto: IdCategoriaProducto, Identification: Identification, Stock: Stock, Estado: Estado };
            $.ajax({
                url: '/Inventario/Create',
                type: 'POST',
                data: producto,
            }).done(function (response) {

                if (response.includes("éxito")) {
                    Swal.fire({
                        text: response,
                        icon: 'success',
                        didClose: () => {
                            window.location.href = '/Inventario/Index';
                        }
                    });
                }
                else {
                    Swal.fire('', response, 'warning');
                }
            });
        }
        hideSpinner();
    }, 2000);
}

function ModifyProducto() {
    showSpinner();
    setTimeout(function () {
        var Codigo = $("#txtModifyCodigo").val();
        var Nombre = $("#txtModifyProductoNombre").val();
        var Descripcion = $("#txtModifyDescripcion").val();
        var IdTipoProducto = GetDropDownValueSelected('ddlModifyTipoProductoModal', 'ModifyTipoProductolList');
        var IdCategoriaProducto = GetDropDownValueSelected('ddlModifyProductoModal', 'ModifyProductoCategoriaList');
        var Identification = GetDropDownValueSelected('ddlModifyProductoProveedorModal', 'ModifyProductoProveedorList');
        var Stock = $("#txtModifyCantidad").val();
        var Estado = $("#chkModifyEstado").is(':checked');
        var Id = $("#hdfIdModify").val();

        if (Validate('txtModifyCodigo', 'txtModifyProductoNombre', 'txtModifyDescripcion', 'ddlModifyTipoProductoModal', 'ModifyTipoProductolList', 'ddlModifyProductoModal', 'ModifyProductoCategoriaList',
            'ddlModifyProductoProveedorModal', 'ModifyProductoProveedorList', 'txtModifyCantidad', true)) {
            var producto = { Id: Id, Codigo: Codigo, Nombre: Nombre, Descripcion: Descripcion, IdTipoProducto: IdTipoProducto, IdCategoriaProducto: IdCategoriaProducto, Identification: Identification, Stock: Stock, Estado: Estado };
            $.ajax({
                url: '/Inventario/Edit',
                type: 'POST',
                data: producto,
            }).done(function (response) {

                if (response.includes("éxito")) {
                    Swal.fire({
                        text: response,
                        icon: 'success',
                        didClose: () => {
                            window.location.href = '/Inventario/Index';
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

function ModifyStock() {
  
    var Stock = $("#txtCantidadStock").val();
    var Id = $("#hdfIdStock").val();
    if (Stock === '') {
        Swal.fire('', 'Debe digitar la cantidad a sumar o restar al producto.', 'error');
        return;
    }
    showSpinner();
    setTimeout(function () {
        
            var producto = { Id: Id, Stock: Stock};
            $.ajax({
                url: '/Inventario/ModifyStock',
                type: 'POST',
                data: producto,
            }).done(function (response) {

                if (response.includes("éxito")) {
                    Swal.fire({
                        text: response,
                        icon: 'success',
                        didClose: () => {
                            window.location.href = '/Inventario/Index';
                        }
                    });
                }
                else {
                    Swal.fire('', response, 'warning');
                }
            });
       

        hideSpinner();
    }, 2000);


}

function Validate(txtCodigo, txtProductoNombre, txtDescripcion, ddlAddTipoProductoModal, AddTipoProductolList, ddlAddProductoCategoriaModal, AddProductoCategoriaList,
    ddlAddProductoProveedorModal, AddProductoProveedorList, txtCantidad, IsModify) {
    if ($("#" + txtCodigo).val() === '') {
        Swal.fire('', 'Debe digitar el código del producto.', 'error');
        return false;
    }
    else {
        if (!IsModify) {
            var productFound = SeachProductByCode($("#" + txtCodigo).val());
            if (productFound) {
                Swal.fire('', 'Ya existe un producto registrado con el código: .' + $("#" + txtCodigo).val(), 'error');
                return false;
            }
        }

    }
    if ($("#" + txtProductoNombre).val() === '') {
        Swal.fire('', 'Debe digitar el nombre del producto.', 'error');
        return false;
    }
    if ($("#" + txtDescripcion).val() === '') {
        Swal.fire('', 'Debe digitar la descripción del producto.', 'error');
        return false;
    }

    if (GetDropDownValueSelected(ddlAddTipoProductoModal, AddTipoProductolList) === null) {
        Swal.fire('', 'Debe seleccionar el tipo de producto.', 'error');
        return false;
    }

    if (GetDropDownValueSelected(ddlAddProductoCategoriaModal, AddProductoCategoriaList) === null) {
        Swal.fire('', 'Debe seleccionar la categoría de producto.', 'error');
        return false;
    }

    if (GetDropDownValueSelected(ddlAddProductoProveedorModal, AddProductoProveedorList) === null) {
        Swal.fire('', 'Debe seleccionar el proveedor del producto.', 'error');
        return false;
    }

    if ($("#" + txtCantidad).val() === '') {
        Swal.fire('', 'Debe digitar la cantidad inicial disponible.', 'error');
        return false;
    }

    return true;
}