
// ========= Add to Home Screen =========
var deferredPrompt = null;

// Android / Chrome
$(window).on("beforeinstallprompt", function (e) {
    e.preventDefault();
    deferredPrompt = e.originalEvent;
    $("#installAppBtn").show();
});

// Button click handler
$("#installAppBtn").on("click", function () {
    if (deferredPrompt) {
        deferredPrompt.prompt();
        deferredPrompt.userChoice.then(function (choiceResult) {
            if (choiceResult.outcome === "accepted") {
                console.log("✅ App installed");
            } else {
                console.log("❌ App dismissed");
            }
            deferredPrompt = null;
        });
        return;
    }

    // iOS Safari
    if (/iPhone|iPad|iPod/i.test(navigator.userAgent)) {
        $("#iosHelper").fadeIn();
        return;
    }

    alert("⚠️ Your browser does not support Add to Home Screen.");
});

// iOS Safari fallback: always show button
if (/iPhone|iPad|iPod/i.test(navigator.userAgent)) {
    $("#installAppBtn").show();
}