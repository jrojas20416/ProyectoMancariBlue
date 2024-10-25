function getSelectedGender(rdbGender) {
    return $('input[name="' + rdbGender + '"]:checked').val();
}

function NewAnimal() {
    showSpinner();
    setTimeout(function () {
    var Padre = GetDropDownValueSelected('ddlAddAnimalModal', 'AddAnimalList')
    var Madre = GetDropDownValueSelected('ddlAddAnimalMothreModal', 'AddAnimalMotherlList')
    var Raza = GetDropDownValueSelected('ddlAddAnimalRazaModal', 'AddAnimalRazalList')
    var FechaNacimiento = $("#dtDate").val();
    var Genero = getSelectedGender("animalGender");
    var PartosRegistrados = $("#txtanimalBirths").val();
    var PesoKG = $("#txtanimalHeigth").val();
    var Estado = true;
    var Codigo = $("#txtCodigo").val();
    
    if (Validate('ddlAddAnimalModal', 'AddAnimalList', 'ddlAddAnimalMothreModal', 'AddAnimalMotherlList', 'ddlAddAnimalRazaModal', 'AddAnimalRazalList', 'txtanimalBirths', 'txtanimalHeigth','dtDate','txtCodigo',false)) {
        var animal = { Padre: Padre, Madre: Madre, Raza: Raza, Genero: Genero, PartosRegistrados: PartosRegistrados, PesoKG: PesoKG, FechaNacimiento: [FechaNacimiento], Estado: Estado, Codigo: Codigo };
        $.ajax({
            url: '/Animal/Create',
            type: 'POST',
            data: animal,
        }).done(function (response) {

            if (response.includes("éxito")) {
                Swal.fire({
                    text: response,
                    icon: 'success',
                    didClose: () => {
                        window.location.href = '/Animal/Index';
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
function ModifyAnimal() {
    showSpinner();
    setTimeout(function () {
    var Padre = GetDropDownValueSelected('ddlModifyAnimalModal', 'ModifyAnimalList')
    var Madre = GetDropDownValueSelected('ddlModifyAnimalMothreModal', 'ModifyAnimalMotherlList')
    var Raza = GetDropDownValueSelected('ddlModifyAnimalRazaModal', 'ModifyAnimalRazalList')
    var FechaNacimiento = $("#dtDateModify").val();
    var Genero = getSelectedGender("animalGenderModify");
    var PartosRegistrados =0;
    if (Genero === "H") {
         PartosRegistrados = $("#txtanimalModifyBirths").val();
    }
    var PesoKG = $("#txtanimalModifyHeigth").val();
    var Codigo = $("#txtModifyCodigo").val();
    var Id = $("#hdfIdModify").val();
    var Estado = $("#hdfEstadoModify").val();
    if (Validate('ddlModifyAnimalModal', 'ModifyAnimalList', 'ddlModifyAnimalMothreModal', 'ModifyAnimalMotherlList', 'ddlModifyAnimalRazaModal', 'ModifyAnimalRazalList', 'txtanimalModifyBirths', 'txtanimalModifyHeigth', 'dtModifyDate', 'txtModifyCodigo',true)) {
        var animal = { Id:Id, Padre: Padre, Madre: Madre, Raza: Raza, Genero: Genero, PartosRegistrados: PartosRegistrados, PesoKG: PesoKG, FechaNacimiento: [FechaNacimiento], Estado: Estado, Codigo: Codigo };
        $.ajax({
            url: '/Animal/Edit',
            type: 'POST',
            data: animal,
        }).done(function (response) {

            if (response.includes("éxito")) {
                Swal.fire({
                    text: response,
                    icon: 'success',
                    didClose: () => {
                        window.location.href = '/Animal/Index';
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
function Validate(dropdownListFather, dataSelectedFather, dropdownListMother, dataSelectedMother, dropdownListRaza, dataSelectedRaza, NumberRegisteredBirths, Heigth, dtDate, Codigo,IsModify) {
    if ($("#" + Codigo).val() === '') {
        Swal.fire('', 'Debe digitar el código del animal.', 'error');
        return false;
    }
    else {
        if (!IsModify) {
            var AniamlFound = SeachAnimalByCode($("#" + Codigo).val());
            console.log(AniamlFound);
            if (AniamlFound) {
                Swal.fire('', 'Ya existe registrado un animal con el código: ' + $("#" + Codigo).val(), 'error');
                return false;
            }
        }
    }

    if (GetDropDownValueSelected(dropdownListRaza, dataSelectedRaza) === null) {
        Swal.fire('', 'Debe seleccionar la raza del animal', 'error');
        return false;
    }
    const gender = getSelectedGender('animalGender'); // Cambia 'animalGender' por el nombre real
    if (gender === 'H') {
        const registeredBirths = $("#" + NumberRegisteredBirths).val();

        // Verificar si es nulo o vacío
        if (registeredBirths === '') {
            Swal.fire('', 'Debe digitar la cantidad de partos del animal.', 'error');
            return false;
        }

        // Convertir el valor a un número
        const birthsCount = parseInt(registeredBirths, 10);

        // Verificar si es un valor negativo
        if (birthsCount < 0) {
            Swal.fire('', 'La cantidad de partos no puede ser un valor negativo.', 'error');
            return false;
        }
    }
    if ($("#" + Heigth).val() === '') {
        Swal.fire('', 'Debe digitar el peso del animal.', 'error');
        return false;
    }
    else {
        if ($("#" + Heigth).val() <= 0) {
            Swal.fire('', 'El peso debe ser mayor a 0.', 'error');
            return false;
        }
    }
   

    if ($("#" + dtDate).val() === '') {
        Swal.fire('', 'Debe seleccionar la fecha de la primer ejecución.', 'error');
        return false;
    }
    else {
        if (new Date($("#" + dtDate).val()) > new Date()) {
            Swal.fire('', 'La fecha de nacimiento no puede ser mayor a la fecha del día', 'error');
            return false;
        }
    }

    return true;
}




