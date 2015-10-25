function OnShowRetweetModal() {
    $("#retweetModal").modal();
}

function OnCreateTweetModal() {
    $("#createTweetModal").modal();
}

$(function() {
    var twitter = $.connection.twitterHub;

    twitter.client.receiveTweet = function (tweetId) {
        $.get("/Tweets/GetTweet/" + tweetId, function (result) {
            $("#userHomeTweetsContainer").prepend(result);
        });
    };

    twitter.client.receiveNotification = function () {
        var currentUnseenNotifications = $("#notificationsNavbar").text().split(" ");

        if (currentUnseenNotifications[0] === "") {
            currentUnseenNotifications[0] = "0";
        }

        if (currentUnseenNotifications.length > 2) {
            currentUnseenNotifications.shift();
        }

        currentUnseenNotifications[0] = parseInt(currentUnseenNotifications[0]) + 1;

        $("#notificationsNavbar").text(" " + currentUnseenNotifications[0] + " " + currentUnseenNotifications[1]);
    };

    $.connection.hub.start()
    .done(function () {
        console.log('Connection established!');
    });
});