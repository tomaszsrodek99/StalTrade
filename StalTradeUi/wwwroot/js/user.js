function scheduleTokenExpiration(token) {
    const tokenData = parseJwt(token);
    if (tokenData && tokenData.exp) {
        const tokenExpiration = new Date(tokenData.exp * 1000); //Przekszta³æ `exp` w tokenie na datê

        const countdownInterval = 1000; //Interwa³ odœwie¿ania

        const countdown = setInterval(function () {
            const remainingTime = tokenExpiration - new Date();

            if (remainingTime <= 0) {
                clearInterval(countdown); //Wy³¹cz odliczanie po wygaœniêciu tokenu
                handleTokenExpiration();
            }
        }, countdownInterval);
    }

}

function parseJwt(token) {
    try {
        const base64Url = token.split('.')[1];
        const base64 = base64Url.replace('-', '+').replace('_', '/');
        return JSON.parse(atob(base64));
    } catch (e) {
        return null;
    }
}

function handleTokenExpiration() {
    $('#blurOverlay').show();
    $('body').addClass('blur-overlay-visible');

    const tokenExpirationMessage = document.getElementById("tokenExpirationMessage");
    tokenExpirationMessage.style.display = "block";

    const acceptButton = document.getElementById("acceptButton");
    acceptButton.addEventListener("click", function () {
        $('#blurOverlay').hide();
        $('body').removeClass('blur-overlay-visible');
        window.location.href = "/UserUI/LoginView";
    });
}

function updateClock() {
    const clockElement = document.getElementById("clock");
    const currentTime = new Date();
    const hours = currentTime.getHours().toString().padStart(2, "0");
    const minutes = currentTime.getMinutes().toString().padStart(2, "0");
    const seconds = currentTime.getSeconds().toString().padStart(2, "0");
    const timeString = `${hours}:${minutes}:${seconds}`;
    clockElement.textContent = timeString;
}
