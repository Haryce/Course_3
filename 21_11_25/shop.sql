CREATE DATABASE ShopDB;
GO

USE ShopDB;
GO

-- Таблица категорий
CREATE TABLE Categories (
    CategoryId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500)
);

-- Таблица продуктов
CREATE TABLE Products (
    ProductId INT IDENTITY(1,1) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    Description NVARCHAR(500),
    Price DECIMAL(10,2) NOT NULL,
    StockQuantity INT NOT NULL DEFAULT 0,
    CategoryId INT FOREIGN KEY REFERENCES Categories(CategoryId),
    CreatedDate DATETIME DEFAULT GETDATE()
);

-- Таблица клиентов
CREATE TABLE Customers (
    CustomerId INT IDENTITY(1,1) PRIMARY KEY,
    FirstName NVARCHAR(50) NOT NULL,
    LastName NVARCHAR(50) NOT NULL,
    Email NVARCHAR(100),
    Phone NVARCHAR(20),
    Address NVARCHAR(200)
);

-- Таблица заказов
CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) PRIMARY KEY,
    CustomerId INT FOREIGN KEY REFERENCES Customers(CustomerId),
    OrderDate DATETIME DEFAULT GETDATE(),
    TotalAmount DECIMAL(10,2) NOT NULL,
    Status NVARCHAR(20) DEFAULT 'Pending'
);

-- Таблица деталей заказа
CREATE TABLE OrderDetails (
    OrderDetailId INT IDENTITY(1,1) PRIMARY KEY,
    OrderId INT FOREIGN KEY REFERENCES Orders(OrderId),
    ProductId INT FOREIGN KEY REFERENCES Products(ProductId),
    Quantity INT NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL
);
INSERT INTO Categories (Name, Description) VALUES
('Электроника', 'Электронные устройства и гаджеты'),
('Одежда', 'Одежда и аксессуары'),
('Книги', 'Книги и литература');

INSERT INTO Products (Name, Description, Price, StockQuantity, CategoryId) VALUES
('Смартфон', 'Современный смартфон', 29999.99, 50, 1),
('Ноутбук', 'Мощный ноутбук', 59999.99, 25, 1),
('Футболка', 'Хлопковая футболка', 1999.99, 100, 2),
('Роман', 'Интересный роман', 599.99, 200, 3);

INSERT INTO Customers (FirstName, LastName, Email, Phone, Address) VALUES
('Иван', 'Иванов', 'ivan@mail.ru', '+79991234567', 'Москва, ул. Ленина, 1'),
('Петр', 'Петров', 'petr@mail.ru', '+79997654321', 'Санкт-Петербург, ул. Пушкина, 10');