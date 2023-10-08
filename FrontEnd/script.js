async function fetchDataImg() {
    try {
        const id = 1
        const ruta = "http://localhost:5151/api/FileUpload/GetFileById/"
        const response = await fetch(URL(ruta,id));
        console.log(response);
        const data = await response.json();
        console.log(data);
        return data;
    } catch (error) {
        throw 'Error al obtener los datos';
    }
}