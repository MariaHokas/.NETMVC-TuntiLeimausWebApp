        function päivitäAsiakaslistaus() {
            $.getJSON("/TuntiLeimaus/getlist", null, function (json) {
                var leimaus = JSON.parse(json);
                var html = "";
                for (var index = 0; index < leimaus.length; index++) {
                    html += "<tr>"
                        +
                        "<td>" + leimaus[index].EmployeeName + "</td>" +
                        "<td>" + leimaus[index].Sisaan + "</td>" +
                        "<td>" + leimaus[index].Ulos + "</td>" +
                        "<td>" + leimaus[index].LuokkahuoneID + "</td>" +
                        "</tr>\r\n";
                }
                $(".leimausList tbody").html(html);
                //tästä lisätty
            });
        }
        // sivun alustaminen
            $(function () {
            päivitäAsiakaslistaus();
        $("#SaveButton").click(function () {
                    // luetaan käyttäjän syöttämät kentät
                    var details = {
                        OppilasID: $("#leiModal_Oid").val(),
                        LuokkahuoneID: $("#leiModal_Lid").val(),
                        Sisään: $("#leiModal_Sid").val(),
    };
                    $.post("/TuntiLeimaus/Sisaan/", details, function (status) {
                        if (status == true) {
            alert("Sisään kirjautuminen tallennettu!");
        päivitäAsiakaslistaus();
    }
                        else {
            alert("Tallennus ei onnistunut, yritä uudelleen!");
        }
    });
});
console.log("Alustus valmis!");
});

// sivun alustaminen
            $(function () {
            päivitäAsiakaslistaus();
        $("#UlosButton").click(function () {
                    // luetaan käyttäjän syöttämät kentät
                    var details = {
            UserID: $("#leiModal_Oid").val(),
        LuokkahuoneID: $("#leiModal_Lid").val(),
        Ulos: $("#leiModal_Uid").val(),
    };
                    $.post("/TuntiLeimaus/ulos/", details, function (status) {
                        if (status == true) {
            alert("Ulos leimaus tallennettu!");
        päivitäAsiakaslistaus();
    }
                        else {
            alert("Tallennus ei onnistunut, yritä uudelleen!");
        }
    });
});
console.log("Alustus valmis!");
});
    // sivun alustaminen
    

