jQuery.fn.serializeObject = function () {
    var arrayData, objectData;
    arrayData = this.serializeArray();
    objectData = {};
    console.log(arrayData);
    $.each(arrayData, function () {
        var value;

        if (this.value != null) {
            value = this.value;
        } else {
            value = '';
        }

        if (objectData[this.name] != null) {
            if (!objectData[this.name].push) {
                objectData[this.name] = [objectData[this.name]];
            }

            objectData[this.name].push(value);
        } else {
            objectData[this.name] = value;
        }
    });

    return objectData;
};

var Cedulas = {}

/* Calle Editor */
var CalleEditor = function (config) {
    this.createUrl = config["createUrl"];
    this.editUrl = config["editUrl"];
    this.calleNombre = config["nombre"];
    this.esEditarNombre = false;
    this.prepararFormulario();
}

CalleEditor.prototype.prepararFormulario = function () {
	$("#buttonNuevaCalle").click($.proxy(this.on_crear_click, this));
	$("#buttonNuevoNombre").click($.proxy(this.on_agregar_nombre_click, this));
	$("#EditarNombre").click($.proxy(this.on_editar_nombre_click, this));
    $("#buttonGuardarCalle").click($.proxy(this.on_editar_crear_guardar_click, this));
}

CalleEditor.prototype.on_crear_click = function () {
    $("#CalleModal").modal("show");
}

CalleEditor.prototype.on_agregar_nombre_click = function () {
    $("#inputNombre").val("");
    $("#CalleModal").modal("show");
}

CalleEditor.prototype.on_editar_nombre_click = function () {
    this.esEditarNombre = true;
    $("#inputNombre").val(this.calleNombre);
    $("#CalleModal").modal("show");
}

CalleEditor.prototype.on_editar_guardar_click = function () {
    var data = $("#CalleForm").serialize();
    this.esEditarNombre = false;
    $("#CalleModal").modal("hide");
}

CalleEditor.prototype.on_editar_crear_guardar_click = function () {
    var data = $("#CalleForm").serializeObject();
    var url = (this.createUrl) ? this.createUrl : this.editUrl;
    if (!this.esEditarNombre)
        data["nombres"] = data["nombre"];
    if (this.createUrl)
        delete data["cId"];
    console.log(data);
    var self = this;
    $.ajax({
        url: url,
        type: "POST",
        data: data,
        dataType: 'json',
        success: function (data) {
            if (data["Status"] === "ok") {
                $("#CalleModal").modal("hide");
                $("#formErrors").fadeOut(100);
                window.location = data["Url"];
            } else if (data["Status"] === "error") {
                var errorString = "";
                $(data["Errors"]).each(function (index, error) {
                    errorString = errorString + "<h4>" + error["Key"] + "</h4>";
                    errorString = errorString + error["Value"].join("<br>");
                });
                $("#formErrors").html(errorString);
                $("#formErrors").fadeIn(500);
            }
        }
    });
}


/* Altura Editor */
var AlturaEditor = function (config) {
    this.jsonUrl = config["jsonUrl"];
    this.editUrl = config["editUrl"];
    this.deleteUrl = config["deleteUrl"];
    this.dataUrl = config["dataUrl"];
    this.zonaTableRows = $("#CalleZonasRows");
    this.conectarAcciones();
    this.prepararFormulario();
}

AlturaEditor.prototype.prepararFormulario = function () {

    $("#buttonGuardarZona").click($.proxy(this.on_guardar_click, this));
    $("#optionsAlturas").change($.proxy(this.on_altura_changed, this));
    $("#optionsDomicilio").change($.proxy(this.on_domicilio_changed, this));
    $("#optionsRango").change($.proxy(this.on_rango_changed, this));
}

AlturaEditor.prototype.on_domicilio_changed = function () {
    var checked = $("#optionsDomicilio").is(':checked');
    $("#inputDesde").attr("disabled", !checked);
    $("#inputHasta").attr("disabled", checked);
    $("#inputVereda").attr("disabled", checked);
    if (checked) {
        $("#inputHasta").val("");
        $("#inputDesde").val("");
        $("#inputVereda").val(3);
    }
}

AlturaEditor.prototype.on_rango_changed = function () {
    var checked = $("#optionsRango").is(':checked');
    $("#inputDesde").attr("disabled", !checked);
    $("#inputHasta").attr("disabled", !checked);
    $("#inputVereda").attr("disabled", !checked);
    if (checked) {
        $("#inputHasta").val("");
        $("#inputDesde").val("");
        $("#inputVereda").val(3);
    }
}

AlturaEditor.prototype.on_altura_changed = function () {
    var checked = $("#optionsAlturas").is(':checked');
    $("#inputDesde").attr("disabled", checked);
    $("#inputHasta").attr("disabled", checked);
    $("#inputVereda").attr("disabled", checked);
    if (checked) {
        $("#inputHasta").val("");
        $("#inputDesde").val("");
        $("#inputVereda").val(3);
    }
}

AlturaEditor.prototype.on_crear_click = function () {
    $("#inputZID").val("");
    $("#ZonaModal").modal("show");
}

AlturaEditor.prototype.on_guardar_click = function () {
    var data = $("#AlturaForm").serializeObject();
    // jQuery no me serializa los input=radio
    if ($("#optionsAlturas").is(':checked'))
        data["optionsAltura"] = "alturas";
    else if ($("#optionsRango").is(':checked'))
        data["optionsAltura"] = "rango";
    else if ($("#optionsDomicilio").is(':checked'))
        data["optionsAltura"] = "domicilio";
    var self = this;
    $.ajax({
        url: this.editUrl,
        type: "POST",
        data: data,
        dataType: 'json',
        success: function (data) {
            if (data["Status"] === "ok") {
                $("#ZonaModal").modal("hide");
                $("#formErrors").fadeOut(100);
                self.updateZonas();
            } else if (data["Status"] === "error") {
                var errorString = "";
                $(data["Errors"]).each(function (index, error) {
                    errorString = errorString + "<h4>" + error["Key"] + "</h4>";
                    errorString = errorString + error["Value"].join("<br>");
                });
                $("#formErrors").html(errorString);
                $("#formErrors").fadeIn(500);
            }
        }
    });
}

AlturaEditor.prototype.conectarAcciones = function () {
    var self = this;
    $(".zona-edit").each(function (index, item) {
        $(item).click(function () {
            var idAltura = $(this).attr("alturaId");
            self.editarZona(idAltura);
        });
    });
    $(".zona-delete").each(function (index, item) {
        $(item).click(function () {
            var idAltura = $(this).attr("alturaId");
            self.borrarZona(idAltura);
        });
    });
    $("#buttonNuevaZona").click($.proxy(this.on_crear_click, this));
}

AlturaEditor.prototype.editarZona = function (id) {
    var self = this;
    $.ajax({
        url: this.jsonUrl,
        data: { alturaId: id },
        dataType: 'json',
        success: function (data) {
            var alturaForm = $("#AlturaForm");
            for (var i in data) {
                alturaForm.find("[name=" + i + "]").val(data[i]);
            }
            $("#ZonaModal").modal("show");
        }
    });
}

AlturaEditor.prototype.borrarZona = function (id) {
    var self = this;
    $.post(
		this.deleteUrl,
		{ alturaId: id },
        function (response) {
            self.zonaTableRows.html($(response).fadeIn(500));
            self.conectarAcciones();
        }
	);
}

AlturaEditor.prototype.updateZonas = function () {
    var self = this;
        $.get(
        this.dataUrl,
        function (response) {
            self.zonaTableRows.html($(response).fadeIn(500));
            self.conectarAcciones();
        }
	);
}
