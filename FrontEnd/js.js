

function OnButtonUpload() {
    const file = document.getElementById("file");
    const buttonUpload = document.getElementById("buttonUpload");

    file.addEventListener("change", (event) => {
        if (file.value) {
            buttonUpload.disabled = false;
            console.log("Se ha seleccionado un archivo.");
            document.getElementById("buttonUpload").addEventListener("click", (event) => {
                UploadFile(file);
            });
        } else {
            buttonUpload.disabled = true;
            console.log("El input de tipo archivo está vacío.");
        }
    });
}

async function UploadFile(fileInput) {
    console.log(fileInput.files);
    const file = fileInput.files[0]; 

    if (file) {
        const apiUrl = "http://localhost:5151/api/FileUpload"; 
        const formData = new FormData();
        formData.append("file", file);

        try {
            const response = await fetch(apiUrl, {
                method: "POST",
                body: formData
            });

            if (response.ok) {
                console.log("La imagen se ha cargado correctamente en la API.");
            } else {
                console.error("Error al cargar la imagen en la API: " + response.status);
            }
        } catch (error) {
            console.error("Error al cargar la imagen en la API: " + error);
        }
    } else {
        console.error("No se ha seleccionado ningún archivo para cargar.");
    }
}


async function fetchDocumentsName() {
    try {
        const response = await fetch("http://localhost:5151/api/FileUpload/dataDocs");

        if (response.ok) {
            const tbody = document.getElementById("tbody");
            const data = await response.json();
            var docs =``;
            data.forEach(dataDoc => {
                const div = document.createElement("div"); div.setAttribute("class", "card align-items-center col-xxl-3 col-xl-4 col-lg-4 col-md-4 col-md-6 col-sm-6 col-xs-12");
                console.log(dataDoc);
                docs += `
                    <tr class="">
                        <td scope="row">${dataDoc.name}</td>
                        <td>
                            <a name="" id="${dataDoc.id}" class="btn btn-success select-button-tbody"  role="button">Download</a>
                           
                        </td>
                    </tr>`
                    /*  <a name="" id="${dataDoc.id}" class="btn btn-danger select-button-tbody" role="button">Delete</a> */
                tbody.appendChild(div);

            });
            tbody.innerHTML = docs;

            tbody.addEventListener("click", (event) => {
                if (event.target.classList.contains("select-button-tbody")) {
                    const buttonId = event.target.id;
                    console.log(event.target.classList.value);
                    console.log("Botón con ID en el  tbody " + buttonId + " clicado.");
                    if(event.target.classList.value.includes("success"))
                    {
                        DownloadDocument(parseInt(buttonId));
                        //console.log("le di al success");
                    }else if(event.target.classList.value.includes("danger")) {
                        //console.log("le di al danger");
                    }
                }
            });
        } else {
            console.error("Error al cargar la imagen: " + response.status);
        }
    } catch (error) {
        console.error("Error al cargar la imagen: " + error);
    }
}

async function DownloadDocument(documentId) {
    console.log(typeof(documentId));
    const apiUrl = `http://localhost:5151/api/FileUpload/file/${parseInt(documentId)}`;
    try {
        const response = await fetch(apiUrl);

        if (response.ok) {
            // Extraer el nombre del archivo de la respuesta (puedes hacerlo en función de cómo esté diseñada tu API)
            console.log(response.headers);
            /* response.headers.forEach(element => {
                console.log("header " + element);
            }); */

            const contentDisposition = response.headers.get('content-disposition');
            console.log(contentDisposition);
            const filenameMatch = contentDisposition.match(/filename="(.+)"/);
            const fileName = filenameMatch ? filenameMatch[1] : 'document';

            // Crear un objeto Blob con los datos de la respuesta
            const blob = await response.blob();

            // Crear un enlace para descargar el archivo
            const downloadLink = document.createElement('a');
            downloadLink.href = window.URL.createObjectURL(blob);
            downloadLink.download = fileName;

            // Hacer clic en el enlace para iniciar la descarga
            downloadLink.click();
        } else {
            console.error("Error al descargar el documento: " + response.status);
        }
    } catch (error) {
        console.error("1 Error al descargar el documento: " + error);
    }
}

async function fetch2() {
    try {
        const response = await fetch("http://localhost:5151/api/FileUpload/Img");

        if (response.ok) {
            const pics = document.getElementById("pics");
            const data = await response.json();
            data.forEach(imageDataUrl => {
                const div = document.createElement("div"); div.setAttribute("class", "card align-items-center col-xxl-3 col-xl-4 col-lg-4 col-md-4 col-md-6 col-sm-6 col-xs-12");
                console.log(imageDataUrl);
                div.innerHTML = `
                    <img class="card-img-top" src="${imageDataUrl.image}"
                        alt="Card image cap" />
                    <div class="card-body">
                        <a name="" id="${imageDataUrl.id}" class="btn btn-primary select-button" href="#" role="button">Seleccionar</a>
                    </div>`
                pics.appendChild(div);

            });

            pics.addEventListener("click", (event) => {
                if (event.target.classList.contains("select-button")) {
                    const buttonId = event.target.id;
                    console.log("Botón con ID " + buttonId + " clicado.");
                    showImage(buttonId);
                }
            });
        } else {
            console.error("Error al cargar la imagen: " + response.status);
        }
    } catch (error) {
        console.error("Error al cargar la imagen: " + error);
    }
}

async function showImage(id) {
    try {
        const response = await fetch("http://localhost:5151/api/FileUpload/file/" + id);
        // ImgShowing
        if (response.ok) {
            const data = await response.json();

            const buttonsOptions = document.getElementById("buttonsOptions");
            const OptionsImg = document.getElementById("OptionsImg");
            option = document.getElementById("option");

            option.setAttribute("src", data.image);

            var buttons = document.getElementsByClassName("btn btn-success select-button-options");

            if (buttons.length > 0) {
                buttons[0].parentNode.removeChild(buttons[0]);
            }

            const buttonDown = document.createElement("a"); buttonDown.setAttribute("type", "button");
            buttonDown.setAttribute("class", "btn btn-success select-button-options");
            buttonDown.innerText = "Download"; buttonDown.setAttribute("id", data.id);
            buttonDown.href = data.image; 
            buttonDown.download = data.name; 
            buttonsOptions.appendChild(buttonDown);

            OptionsImg.addEventListener("click", (event) => {
                if (event.target.classList.contains("select-button-options")) {
                    const buttonId = event.target.id;
                    console.log("Botón con ID " + buttonId + " clicado.");

                }
            });

        } else {
            console.error("Error al cargar la imagen: " + response.status);
        }
    } catch (error) {
        console.error("Error al cargar la imagen: " + error);
    }
}




fetch2();
OnButtonUpload();
fetchDocumentsName();
