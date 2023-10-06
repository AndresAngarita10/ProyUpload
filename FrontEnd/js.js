/* async function fetchData() {
    try {
        const response = await fetch(`http://localhost:5151/api/FileUpload/Imagenes2`);
        var response2 = await response.json();
        console.log(response2.length);
        if (response.ok) {
            response2.forEach(async element => { // Agregamos async aquí
                console.log(element);

                // Usamos await para obtener el contenido de la imagen como un Blob
                const imageBytes = await fetch(element.fileContents).then(response => response.blob());

                const objectURL = URL.createObjectURL(imageBytes);

                // Crea un elemento de imagen y establece la fuente
                const imgElement = document.createElement('img');
                imgElement.src = objectURL;

                // Agrega la imagen al documento
                const imageContainer = document.getElementById('imageContainer');
                imageContainer.appendChild(imgElement);
            });
        } else {
            console.error("Error en la solicitud: " + response.status);
        }
    } catch (error) {
        console.error("Error de red: ", error);
    }
} */
/* 
async function fetch2() {
    try {
        const response = await fetch("http://localhost:5151/api/FileUpload/Imagenes2");

        if (response.ok) {
            const data = await response.json();
            const imgElement = document.createElement("img");
            console.log(data.filecontent);
            imgElement.src = data.filecontent;
            document.body.appendChild(imgElement);
        } else {
            // Manejar errores si la imagen no se encuentra
            console.error("Error al cargar la imagen: " + response.status);
        }
    } catch (error) {
        console.error("Error al cargar la imagen: " + error);
    }
} */

async function fetch2() {
    try {
        const response = await fetch("http://localhost:5151/api/FileUpload/Imagenes2");

        if (response.ok) {
            const data = await response.json();
            data.forEach(imageDataUrl => {
                const imgElement = document.createElement("img");
                imgElement.src = imageDataUrl;
                document.body.appendChild(imgElement);
            });
        } else {
            // Manejar errores si la imagen no se encuentra
            console.error("Error al cargar la imagen: " + response.status);
        }
    } catch (error) {
        console.error("Error al cargar la imagen: " + error);
    }
}


// Llamar a la función fetch2
fetch2();


/* async function fetch2() {
    try {
        const response = await fetch("http://localhost:5151/api/FileUpload/Imagenes2");

        if (response.ok) {
            const blob = await response.blob();
            const objectURL = URL.createObjectURL(blob);
            const imgElement = document.createElement("img");
            imgElement.src = objectURL;
            document.body.appendChild(imgElement);
        } else {
            // Manejar errores si la imagen no se encuentra
            console.error("Error al cargar la imagen: " + response.status);
        }
    } catch (error) {
        console.error("Error al cargar la imagen: " + error);
    }
} */


/* fetchData(); */ // Llama a la función para iniciar la solicitud.
/* fetch2(); */
