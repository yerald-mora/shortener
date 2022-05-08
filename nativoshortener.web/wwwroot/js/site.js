// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
addEvents();

function addEvents() {
    const btnShortenUrl = document.querySelector("#btnShortenUrl");
    const btnTop20MostVisited = document.querySelector("#btnTop20MostVisited");

    btnShortenUrl.addEventListener('click', ShortenUrl);
    btnTop20MostVisited.addEventListener('click', GetTop20MostVisited);
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

function GetTop20MostVisited() {
    fetch("/Index?handler=Top20MostVisited")
        .then(response => response.json())
        .then(data => drawTable(data));
}

function drawTable(json) {
    const table = document.querySelector("#results");
    const result = JSON.parse(json);

    table.querySelectorAll(".row-link").forEach(r => r.remove());

    result.forEach(element => {
        const row = table.insertRow();
        const url = row.insertCell(0)
        const visits = row.insertCell(1)
        const shortcode = row.insertCell(2)

        row.classList += "row-link";
        url.innerHTML = element.url;
        visits.innerHTML = element.visits;
        shortcode.innerHTML = element.shortCode;
    });
}