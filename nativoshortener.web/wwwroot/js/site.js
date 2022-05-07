// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
addEvents();

function addEvents() {
    const btnShortenUrl = document.querySelector("#btnShortenUrl");

    btnShortenUrl.addEventListener('click', ShortenUrl);
}

function ShortenUrl() {
    const url = document.querySelector("#txtUrl").value;

    fetchShortenUrl(url)
        .then(s => document.querySelector('#txtShortedUrl').value = s)
        .catch(ex => alert(ex));
}

async function fetchShortenUrl(url) {
    const response = await fetch("/Index?handler=ShortenUrl", {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': document.querySelector("input[name='__RequestVerificationToken']").value
        },
        body: JSON.stringify(url)
    });

    if (!response.ok)
        throw new Error(`😢 something went wrong...${response.status}`);

    return await response.json();
}
