//alert("registration.js wurde eingebunden")

//jQuery ... JS-Bibliothek -> vereinfacht vile aufgaben und behebt Probleme bei unterschidlichen browsern
$(document).ready(() => {
    alert("registration.js wurde eingebunden");
    //Dollar entspricht document get element by id´class / tag
    //blur() ist ein Ereigniss - wird aufgerufen wenn das feld den focus verliert
    $("#Email").blur(() => {
        //alert("blur funktioniert")
        let mail = $("#Email").val()
        //AJAX ..... Asynchronous JAva script and xml (statt xml wird json verwendet)
        // über einen asynchronen aufruf wollen wir nun die eingegebene email an den server senden
        //der server sendet uns zurück ob die mail adresse existiert oda nicht
        //die methode check email muss einen parameter mit email haben
        $.ajax({
            url: "/user/CheckEmail",
            method: "GET",
            data: { email: $("#Email").val() }
        })
            //Falls der request erfolgreich war ( der Server/url ist erreichbar)
            .done((dataFromServer) => {
                if (dataFromServer === false) {
                    //Meldung anzeigen
                    $("#eMailMassage").css("visibility", "visible");
                    //Eingabefeld soll rot werden
                    $("#Email").addClass("redBorder");
                }
                else {
                    //Meldung ausblenden
                    $("#eMailMassage").css("visibility", "hidden");
                    //roten rand um das eingabefeld
                    $("#Email").removeClass("redBorder");
                }
            })
            //war nicht erfolgreich
            .fail(() => {
                alert("server/ URL nicht erreichbar");
            });
    });

    $("#Btn").click(() => {
        $("#formReg").toggle(2000);
    })

})