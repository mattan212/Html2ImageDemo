$(document).ready(() => {    
    $("#btn-snap").click(() => {
        html2canvas(document.body, {
            background: undefined,
            onrendered: function (canvas) {
                var image = canvas.toDataURL("image/png");
                var url = "api/ImageBankApi";
                var payload = {
                    Data: image
                };
                $.ajax({
                    url: url,
                    type: "POST",
                    data: JSON.stringify(payload),
                    contentType: "application/json",
                    success: showSuccessfulSnap,
                    error: function (err) {
                        console.log(JSON.stringify(err));
                    }
                });
            }
        });
    });

    $("#btn-download").click(() => {
        downloadURI("https://localhost:44374/images/image.png", "image.png");
    })

    $.getJSON("https://localhost:44374/images/image.png",
        () => $("#btn-download").removeClass("disabled"))    
    .fail(() => $("#btn-download").removeClass("disabled"));
});

function showSuccessfulSnap() {    $("#snap-success").show();    
    setTimeout(() => $("#snap-success").fadeOut(1000), 2000);    
}

function getClock() {
    var d = new Date();
    var hours = d.getHours().toString().padStart(2, '0');
    var minutes = d.getMinutes().toString().padStart(2, '0');
    var seconds = d.getSeconds().toString().padStart(2, '0');
    return `${hours}:${minutes}:${seconds}`;
}

setInterval(() => {
    $("#clock").text(getClock());
}, 1000);
$("#clock").text(getClock());

function downloadURI(uri, name) {
    var link = document.createElement("a");
    link.download = name;
    link.href = uri;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    delete link;
}