const uri = "api/job";
let todos = null;

//hämtar information via interna api om antal objekt som finns med i databasen
function getCount(data) {
    const el = $("#counter");
    let name = "aktiva annonser";

    //kontrollerar antalet annonser och skriver ut rätt ord / även om det inte finns några annonser
    if (data) {
        if (data <= 1) {
            name = "aktiv annons";
        }
        el.text("Just nu finns det " + data + " " + name + " i Hudiksvalls kommun.");
    } else {
        el.text("Inga " + name);
    }
}

//När sidan är färdigladdad så kör den funktionen getData()
$(document).ready(function () {

    
    $(".sBtn").on("click", function () {
        getData();
        $(".start").css({ display: "none" });
        $(".startList").css({ display: "block" });
    });

    $(".closeBtn").on("click", function () {
        closeView();
    });

    $("#overlay").on("click", function () {
        closeView();
    });
});

//hämtar information via interna apiet och skriver ut det i en lista
function getData() {
    $.ajax({
        type: "GET",
        url: uri,
        cache: false,
        success: function (data) {
            const tBody = $("#list");

            $(tBody).empty();

            getCount(data.length);

            //loopar igenom varje object
            $.each(data, function (key, item) {
                const tr = $("<tr></tr>")

                    //lägga till de fieldsen från classen i c#
                    //.append($("<td></td>").text(item.id))
                    .append($("<td></td>").text(item.title))
                    .append($("<td></td>").text(item.dateEnd));

                tr.appendTo(tBody).on("click", function () {
                    viewItem(item.id);
                });
            });

            todos = data;
        }
    });

}

//visar ytterligare information om annonsen
function viewItem(id) {
    $.each(todos, function (key, item) {
        if (item.id === id) {
            //$("#view-id").text(id);
            $("#view-title").text(item.title);
            $("#view-place").text(item.place);
            $("#view-dateEnd").text(item.dateEnd);
            $("#view-text").text(item.text);
        }
    });

    $("#overlay").css({ display: "block" });
    $("#view").css({ display: "block" });
    $("body").css({ overflow: "hidden" });
}

function closeView() {
    $("#overlay").css({ display: "none" });
    $("#view").css({ display: "none" });
    $("body").css({ overflow: "auto" });
}