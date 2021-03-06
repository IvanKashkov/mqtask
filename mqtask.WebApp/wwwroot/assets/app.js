const apiBaseUrl = "https://localhost:5003";

function findByIp() {
    const value = document.getElementById("ip").value;

    fetch(`${apiBaseUrl}/ip/location?ip=${value}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Content-Encoding': 'gzip'
        }
    })
    .then(response => response.json())
    .then(function (data) {
        document.getElementById("by-ip-content-result").innerHTML = buildTable([data]);
    })
    .catch(error => {
        alert(error.message);
        console.log(error);
    });
}

function findByCity() {
    const value = document.getElementById("city").value;

    fetch(`${apiBaseUrl}/city/locations?city=${value}`, {
        method: 'GET',
        headers: {
            'Accept': 'application/json',
            'Content-Type': 'application/json',
            'Content-Encoding': 'gzip'
        }
    })
    .then(response => response.json())
    .then(function (data) {
        document.getElementById("by-city-content-result").innerHTML = buildTable(data);
    }).catch(error => {
        alert(error.message);
        console.log(error);
    });
}

function buildTable(locations) {
    let rows = "";

    if (locations != null) {
        for (let location of locations) {
            if (location != null) {
                rows += buildTr(location);
            }
        }
    }

    return `<table class="table">
<thead>
<tr>
<th scope="col">City</th>
<th scope="col">Latitude</th>
<th scope="col">Longitude</th>
<th scope="col">Organization</th>
<th scope="col">Postal</th>
<th scope="col">Region</th>
<th scope="col">Country</th>

</tr>
</thead>
<tbody>${rows}</tbody>
</table>`;
}

function buildTr(location) {
    return `<tr>
<td>${location.City}</td>
<td>${location.Latitude}</td>
<td>${location.Longitude}</td>
<td>${location.Organization}</td>
<td>${location.Postal}</td>
<td>${location.Region}</td>
<td>${location.Country}</td>
</tr>`;
}

function byIpNavItemOnClick() {
    document.getElementById("by-ip-content").style.display = "block";
    document.getElementById("by-city-content").style.display = "none";
}

function byCityNavItemOnClick() {
    document.getElementById("by-ip-content").style.display = "none";
    document.getElementById("by-city-content").style.display = "block";
}
