var Cedulas = {}

/* Calle Editor */
var CalleEditor = function(config) {
    this.createUrl = config["createUrl"];
    this.prepararFormulario();
}

CalleEditor.prototype.prepararFormulario = function () {
	$("#buttonGuardarCalle").click($.proxy(this.on_guardar_click, this));
    $("#buttonNuevaCalle").click($.proxy(this.on_crear_click, this));
}

CalleEditor.prototype.on_crear_click = function () {
    $("#CalleModal").modal("show");
}

CalleEditor.prototype.on_guardar_click = function () {
    var data = $("#CalleForm").serialize();
    var self = this;
    $.ajax({
        url: this.createUrl,
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


/* Zona Editor */
var ZonaEditor = function (config) {
    this.jsonUrl = config["jsonUrl"];
    this.editUrl = config["editUrl"];
    this.deleteUrl = config["deleteUrl"];
    this.dataUrl = config["dataUrl"];
    this.zonaTableRows = $("#CalleZonasRows");
    this.conectarAcciones();
    this.prepararFormulario();
}

ZonaEditor.prototype.prepararFormulario = function () {

    $("#inputAlturas").change($.proxy(this.on_altura_changed, this));
    $("#buttonGuardarZona").click($.proxy(this.on_guardar_click, this));
}

ZonaEditor.prototype.on_altura_changed = function () {
    var checked = $("#inputAlturas").is(':checked');
    $("#inputDesde").attr("disabled", checked);
    $("#inputHasta").attr("disabled", checked);
    $("#inputVereda").attr("disabled", checked);
    if (checked) {
        $("#inputHasta").val("");
        $("#inputDesde").val("");
        $("#inputVereda").val(3);
    }
}

ZonaEditor.prototype.on_crear_click = function () {
    $("#inputZID").val("");
    $("#ZonaModal").modal("show");
}

ZonaEditor.prototype.on_guardar_click = function () {
    var data = $("#ZonaForm").serialize();
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

ZonaEditor.prototype.conectarAcciones = function () {
    var self = this;
    $(".zona-edit").each(function (index, item) {
        $(item).click(function () {
            var idZona = $(this).attr("zonaId");
            self.editarZona(idZona);
        });
    });
    $(".zona-delete").each(function (index, item) {
        $(item).click(function () {
            var idZona = $(this).attr("zonaId");
            self.borrarZona(idZona);
        });
    });
    $("#buttonNuevaZona").click($.proxy(this.on_crear_click, this));
}

ZonaEditor.prototype.editarZona = function (id) {
    var self = this;
    $.ajax({
        url: this.jsonUrl,
        data: { zonaId: id },
        dataType: 'json',
        success: function (data) {
            var zonaForm = $("#ZonaForm");
            for (var i in data) {
                zonaForm.find("[name=" + i + "]").val(data[i]);
            }
            $("#inputAlturas").attr("checked", !data["alturas"]);
            self.on_altura_changed();
            $("#ZonaModal").modal("show");
        }
    });
}

ZonaEditor.prototype.borrarZona = function (id) {
    var self = this;
    $.post(
		this.deleteUrl,
		{ zonaId: id },
        function (response) {
            self.zonaTableRows.html($(response).fadeIn(500));
            self.conectarAcciones();
        }
	);
}

ZonaEditor.prototype.updateZonas = function () {
    var self = this;
        $.get(
        this.dataUrl,
        function (response) {
            self.zonaTableRows.html($(response).fadeIn(500));
            self.conectarAcciones();
        }
	);
}
