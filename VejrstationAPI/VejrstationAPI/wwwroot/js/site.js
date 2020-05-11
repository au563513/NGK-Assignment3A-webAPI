var connection = new signalR.HubConnectionBuilder().withUrl('/vejrhub').build();

connection.start().then(function () {
    console.log("Connected");
}).catch(function (err) {
    console.error(err.toString());
});

connection.on('updateObservation', async function (objID) {
    console.log('updateObservation called with id: ' + objID);

    let oldData = document.getElementById('vejrTxtArea').value;

    document.getElementById('vejrTxtArea').value = JSON.stringify(await getVejrObservation(objID), undefined, 4);
    document.getElementById('vejrTxtArea').value += oldData;
});

async function getVejrObservation(id) {
    let response = await fetch('/api/Vejrobservationer/' + id);
    return await response.json();
}
