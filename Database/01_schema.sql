CREATE DATABASE LearningKidsDB;
GO
USE LearningKidsDB;
GO
CREATE TABLE Roles (idRol INT PRIMARY KEY IDENTITY, nombre VARCHAR(50) NOT NULL);
INSERT INTO Roles (nombre) VALUES ('ADMIN'),('DOCENTE'),('ALUMNO');
CREATE TABLE Usuarios (idUsuario INT PRIMARY KEY IDENTITY, nombre VARCHAR(100), username VARCHAR(50) UNIQUE, password VARCHAR(255), idRol INT, FOREIGN KEY (idRol) REFERENCES Roles(idRol));
CREATE TABLE Tutores (idTutor INT PRIMARY KEY IDENTITY, nombre VARCHAR(100), correo VARCHAR(100) UNIQUE);
CREATE TABLE Alumnos (idAlumno INT PRIMARY KEY, idTutor INT, grado INT, FOREIGN KEY (idAlumno) REFERENCES Usuarios(idUsuario), FOREIGN KEY (idTutor) REFERENCES Tutores(idTutor));
CREATE TABLE DocenteAlumno (id INT PRIMARY KEY IDENTITY, idDocente INT, idAlumno INT, FOREIGN KEY (idDocente) REFERENCES Usuarios(idUsuario), FOREIGN KEY (idAlumno) REFERENCES Alumnos(idAlumno));
CREATE TABLE CamposFormativos (idCampo INT PRIMARY KEY IDENTITY, nombre VARCHAR(100));
CREATE TABLE Proyectos (idProyecto INT PRIMARY KEY IDENTITY, nombre VARCHAR(100), descripcion VARCHAR(255), grado INT, idCampo INT, creadoPor INT, FOREIGN KEY (idCampo) REFERENCES CamposFormativos(idCampo), FOREIGN KEY (creadoPor) REFERENCES Usuarios(idUsuario));
CREATE TABLE Temas (idTema INT PRIMARY KEY IDENTITY, nombre VARCHAR(100), descripcion VARCHAR(255), idProyecto INT, FOREIGN KEY (idProyecto) REFERENCES Proyectos(idProyecto));
CREATE TABLE Pruebas (idPrueba INT PRIMARY KEY IDENTITY, titulo VARCHAR(100), idTema INT, creadoPor INT, FOREIGN KEY (idTema) REFERENCES Temas(idTema), FOREIGN KEY (creadoPor) REFERENCES Usuarios(idUsuario));
CREATE TABLE Preguntas (idPregunta INT PRIMARY KEY IDENTITY, texto VARCHAR(255), idPrueba INT, FOREIGN KEY (idPrueba) REFERENCES Pruebas(idPrueba));
CREATE TABLE Respuestas (idRespuesta INT PRIMARY KEY IDENTITY, texto VARCHAR(255), esCorrecta BIT, idPregunta INT, FOREIGN KEY (idPregunta) REFERENCES Preguntas(idPregunta));
CREATE TABLE Resultados (idResultado INT PRIMARY KEY IDENTITY, idAlumno INT, idPrueba INT, calificacion DECIMAL(5,2), fecha DATETIME DEFAULT GETDATE(), FOREIGN KEY (idAlumno) REFERENCES Alumnos(idAlumno), FOREIGN KEY (idPrueba) REFERENCES Pruebas(idPrueba));
CREATE TABLE ChatHistorial (idChat INT PRIMARY KEY IDENTITY, idAlumno INT, mensaje TEXT, respuesta TEXT, fecha DATETIME DEFAULT GETDATE(), FOREIGN KEY (idAlumno) REFERENCES Alumnos(idAlumno));
GO
