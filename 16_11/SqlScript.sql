CREATE DATABASE SneakerShopDB;
GO

USE SneakerShopDB;
GO

-- Таблица брендов
CREATE TABLE Brands (
    BrandId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Country NVARCHAR(50),
    Description NVARCHAR(500)
);

-- Таблица категорий
CREATE TABLE Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(50) NOT NULL,
    Description NVARCHAR(500)
);

-- Таблица кроссовок
CREATE TABLE Sneakers (
    SneakerId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Price DECIMAL(10,2) NOT NULL,
    BrandId INT NOT NULL,
    CategoryId INT NOT NULL,
    Size INT NOT NULL,
    Color NVARCHAR(50) NOT NULL,
    StockQuantity INT NOT NULL DEFAULT 0,
    ImageUrl NVARCHAR(500),
    IsAvailable BIT NOT NULL DEFAULT 1,
    CreatedDate DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (BrandId) REFERENCES Brands(BrandId),
    FOREIGN KEY (CategoryId) REFERENCES Categories(CategoryId)
);

-- Таблица клиентов
CREATE TABLE Customers (
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Address NVARCHAR(200),
    RegistrationDate DATETIME DEFAULT GETDATE()
);

-- Вставка тестовых данных
INSERT INTO Brands (Name, Country, Description) VALUES
('Nike', 'USA', 'Just Do It'),
('Adidas', 'Germany', 'Impossible is Nothing'),
('New Balance', 'USA', 'Fearlessly Independent'),
('Puma', 'Germany', 'Forever Faster'),
('Reebok', 'USA', 'Be More Human');

INSERT INTO Categories (Name, Description) VALUES
('Беговые', 'Кроссовки для бега и тренировок'),
('Баскетбольные', 'Кроссовки для баскетбола'),
('Повседневные', 'Повседневные кроссовки для улицы'),
('Лимитированные', 'Лимитированные серии и коллаборации'),
('Кроссовки для зала', 'Для фитнеса и тренировок в зале');

INSERT INTO Sneakers (Name, Description, Price, BrandId, CategoryId, Size, Color, StockQuantity, ImageUrl) VALUES
('Nike Air Max 270', 'Комфортные кроссовки с технологией Air Max', 12999.99, 1, 3, 42, 'Black/White', 15, 'nike_air_max_270.jpg'),
('Adidas Ultraboost 22', 'Беговые кроссовки с технологией Boost', 14999.99, 2, 1, 43, 'Blue', 10, 'adidas_ultraboost.jpg'),
('New Balance 574', 'Классические повседневные кроссовки', 8999.99, 3, 3, 41, 'Grey', 20, 'new_balance_574.jpg'),
('Puma RS-X', 'Стильные кроссовки в ретро-стиле', 10999.99, 4, 3, 44, 'Red/Blue', 8, 'puma_rs_x.jpg'),
('Nike Jordan 1', 'Легендарные баскетбольные кроссовки', 19999.99, 1, 2, 42, 'Black/Red', 5, 'jordan_1.jpg');

INSERT INTO Customers (FirstName, LastName, Email, Phone, Address) VALUES
('Иван', 'Иванов', 'ivan@mail.ru', '+79991234567', 'Москва, ул. Ленина, 1'),
('Петр', 'Петров', 'petr@mail.ru', '+79997654321', 'Санкт-Петербург, ул. Пушкина, 10'),
('Мария', 'Сидорова', 'maria@mail.ru', '+79995554433', 'Казань, ул. Баумана, 25');