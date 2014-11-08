/// <reference path="c:\projects\test\app2\app2\scripts\jquery-2.1.1.js" />
/// <reference path="c:\projects\test\app2\app2\scripts\jquery.signalr-2.1.2.js" />
$(function () {

    var conn = $.hubConnection();
    var hub = conn.createHubProxy("MyHub");
    hub.on("ReceivedMessage", function (message) {
        var msgElem = $('<p>');
        msgElem.text(message);
        $('#messages').append(msgElem);
    });
    conn.start();


    alert('loaded.');


    $('#btn-send').click(function () {
        var text = $('#text').val();
        $('#text').val('');
        hub.invoke('SendMessage', text);

    });
});