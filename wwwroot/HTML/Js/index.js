const {conectarDB, hacerConsulta, cerrarConexion} = require('./mysql');

conectarDB().then((mensaje) => {
    console.log(mensaje);

    // Realizar una consulta
    hacerConsulta('SELECT * FROM Empleados').then((resultados) => {
        console.log(resultados);
        
        // Cerrar la conexión después de la consulta
        cerrarConexion();
    }).catch((err) => {
        console.error('Error al hacer la consulta:', err);
    });

}).catch((err) => {
    console.error('Error al conectar a la base de datos:', err);
});