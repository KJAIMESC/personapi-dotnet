-- Verificar si la base de datos persona_db no existe y crearla
IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'persona_db')
BEGIN
    CREATE DATABASE persona_db;
END
GO

-- Usar la base de datos persona_db
USE persona_db;
GO

-- Crear tabla profesion
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='profesion' AND xtype='U')
BEGIN
    CREATE TABLE profesion (
        id INT PRIMARY KEY,
        nom VARCHAR(90),
        des TEXT
    );
END
GO

-- Crear tabla persona
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='persona' AND xtype='U')
BEGIN
    CREATE TABLE persona (
        cc BIGINT PRIMARY KEY,
        nombre VARCHAR(45),
        apellido VARCHAR(45),
        genero CHAR(1) CHECK (genero IN ('M', 'F')),
        edad INT
    );
END
GO

-- Crear tabla estudios
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='estudios' AND xtype='U')
BEGIN
    CREATE TABLE estudios (
        id_prof INT,
        cc_per BIGINT,
        fecha DATE,
        univer VARCHAR(50),
        PRIMARY KEY (id_prof, cc_per),
        FOREIGN KEY (id_prof) REFERENCES profesion(id),
        FOREIGN KEY (cc_per) REFERENCES persona(cc)
    );
END
GO

-- Crear tabla telefono
IF NOT EXISTS (SELECT * FROM sysobjects WHERE name='telefono' AND xtype='U')
BEGIN
    CREATE TABLE telefono (
        num VARCHAR(15) PRIMARY KEY,
        oper VARCHAR(45),
        dueno BIGINT,
        FOREIGN KEY (dueno) REFERENCES persona(cc)
    );
END
GO

-- Insertar datos ficticios en la tabla profesion
INSERT INTO profesion (id, nom, des) VALUES
(1, 'Ingeniero', 'Encargado de diseñar y gestionar proyectos de ingeniería.'),
(2, 'Doctor', 'Profesional de la medicina.'),
(3, 'Abogado', 'Especialista en leyes y asesoramiento legal.');

-- Insertar datos ficticios en la tabla persona
INSERT INTO persona (cc, nombre, apellido, genero, edad) VALUES
(123456789012345, 'Carlos', 'Gonzalez', 'M', 30),
(234567890123456, 'Ana', 'Martinez', 'F', 25),
(345678901234567, 'Luis', 'Perez', 'M', 40);

-- Insertar datos ficticios en la tabla estudios
INSERT INTO estudios (id_prof, cc_per, fecha, univer) VALUES
(1, 123456789012345, '2010-06-15', 'Universidad Nacional'),
(2, 234567890123456, '2015-09-10', 'Universidad de los Andes'),
(3, 345678901234567, '2005-03-20', 'Universidad Javeriana');

-- Insertar datos ficticios en la tabla telefono
INSERT INTO telefono (num, oper, dueno) VALUES
('3001234567', 'Claro', 123456789012345),
('3109876543', 'Movistar', 234567890123456),
('3155678901', 'Tigo', 345678901234567);
GO
