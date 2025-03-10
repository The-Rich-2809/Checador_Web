const MySQL = require('mysql2');

// Crear y exportar la conexión a la base de datos
const Conexion = MySQL.createConnection({
    host: 'localhost',
    user: 'root',
    password: 'sqlex090', 
    database: 'checador'
});

// Función para conectar
const conectarDB = () => {
    return new Promise((resolve, reject) => {
        Conexion.connect((error) => {
            if (error) {
                reject(error);
            } else {
                resolve('Conexión exitosa');
            }
        });
    });
};

// Función para realizar consultas
const hacerConsulta = (consulta) => {
    return new Promise((resolve, reject) => {
        Conexion.query(consulta, (err, resultados) => {
            if (err) {
                reject(err);
            } else {
                resolve(resultados);
            }
        });
    });
};

// Cerrar la conexión
const cerrarConexion = () => {
    Conexion.end();
};

module.exports = {
    conectarDB,
    hacerConsulta,
    cerrarConexion
};
