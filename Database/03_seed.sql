USE LearningKidsDB;
GO
BEGIN TRANSACTION;
BEGIN TRY
    INSERT INTO Usuarios (nombre, username, password, idRol) VALUES
    ('Admin','admin','123',1),('Docente1','doc1','123',2),('Docente2','doc2','123',2),('Alumno1','alum1','123',3);
    INSERT INTO CamposFormativos (nombre) VALUES ('Lenguajes'),('Saberes y Pensamiento Científico');
    INSERT INTO Proyectos (nombre, descripcion, grado, idCampo, creadoPor) VALUES ('Cuerpo Humano','Sistemas del cuerpo',6,2,2);
    DECLARE @proy1 INT = SCOPE_IDENTITY();
    INSERT INTO Proyectos (nombre, descripcion, grado, idCampo, creadoPor) VALUES ('Lectura Básica','Comprensión lectora',5,1,3);
    DECLARE @proy2 INT = SCOPE_IDENTITY();
    INSERT INTO Temas (nombre, descripcion, idProyecto) VALUES
    ('Sistema inmunológico','Defensas del cuerpo',@proy1),('Sistema respiratorio','Respiración',@proy1),('Oraciones','Lenguaje básico',@proy2);
    DECLARE @tema1 INT = (SELECT MIN(idTema) FROM Temas WHERE idProyecto = @proy1);
    DECLARE @tema2 INT = (SELECT MAX(idTema) FROM Temas WHERE idProyecto = @proy1);
    DECLARE @tema3 INT = (SELECT MAX(idTema) FROM Temas WHERE idProyecto = @proy2);
    INSERT INTO Pruebas (titulo, idTema, creadoPor) VALUES ('Prueba Inmunológico',@tema1,2),('Prueba Respiratorio',@tema2,2),('Prueba Oraciones',@tema3,3);
    DECLARE @prueba1 INT = (SELECT MIN(idPrueba) FROM Pruebas WHERE idTema = @tema1);
    DECLARE @prueba2 INT = (SELECT MIN(idPrueba) FROM Pruebas WHERE idTema = @tema2);
    DECLARE @prueba3 INT = (SELECT MIN(idPrueba) FROM Pruebas WHERE idTema = @tema3);
    INSERT INTO Preguntas (texto, idPrueba) VALUES ('¿Cómo cuidar el sistema inmunológico?', @prueba1);
    DECLARE @p1 INT = SCOPE_IDENTITY();
    INSERT INTO Respuestas (texto, esCorrecta, idPregunta) VALUES ('Buena alimentación y higiene',1,@p1),('Comer chatarra',0,@p1),('No dormir',0,@p1),('No hacer ejercicio',0,@p1);
    INSERT INTO Preguntas (texto, idPrueba) VALUES ('¿Cómo cuidar los pulmones?', @prueba2);
    DECLARE @p2 INT = SCOPE_IDENTITY();
    INSERT INTO Respuestas (texto, esCorrecta, idPregunta) VALUES ('Evitar humo',1,@p2),('Fumar',0,@p2),('No respirar',0,@p2),('Comer dulces',0,@p2);
    INSERT INTO Preguntas (texto, idPrueba) VALUES ('¿Qué es una oración?', @prueba3);
    DECLARE @p3 INT = SCOPE_IDENTITY();
    INSERT INTO Respuestas (texto, esCorrecta, idPregunta) VALUES ('Conjunto de palabras con sentido',1,@p3),('Un número',0,@p3),('Un color',0,@p3),('Un objeto',0,@p3);
    INSERT INTO Alumnos (idAlumno, idTutor, grado) VALUES (4, NULL, 6);
    INSERT INTO Resultados (idAlumno, idPrueba, calificacion) VALUES (4,@prueba1,9.0),(4,@prueba2,8.5),(4,@prueba3,10.0);
    COMMIT TRANSACTION;
    PRINT 'OK seed';
END TRY
BEGIN CATCH
    ROLLBACK TRANSACTION;
    PRINT 'Error: ' + ERROR_MESSAGE();
    THROW;
END CATCH;
GO
