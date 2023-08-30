DROP DATABASE IF EXISTS fleurs ;
CREATE DATABASE fleurs ;
USE fleurs ;

-- -----------------------------------------------------
-- Table `customer`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `customer` (
  `id` INT NOT NULL AUTO_INCREMENT UNIQUE,
  `name` VARCHAR(128) NOT NULL,
  `firstname` VARCHAR(128) NOT NULL,
  `phone` VARCHAR(32) NOT NULL,
  `email` VARCHAR(256) NOT NULL UNIQUE,
  `password` VARBINARY(64) NOT NULL,
  `salt` VARBINARY(64) NOT NULL,
  PRIMARY KEY (`id`));

-- -----------------------------------------------------
-- Table `address`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `address` (
  `id` INT NOT NULL AUTO_INCREMENT UNIQUE,
  `idCustomer` INT NOT NULL,
  `name` VARCHAR(128) NOT NULL,
  `country` VARCHAR(64) NOT NULL,
  `zip` INT NOT NULL,
  `city` VARCHAR(128) NOT NULL,
  `street` VARCHAR(256) NOT NULL,
  `number` INT NOT NULL,
  `comment` VARCHAR(64) NULL DEFAULT NULL,
  `hint` VARCHAR(512) NULL DEFAULT NULL,
  FOREIGN KEY (`idCustomer`) REFERENCES `customer` (`id`),
  PRIMARY KEY (`id`));

-- -----------------------------------------------------
-- Table `colors`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `colors` (
  `id` INT NOT NULL AUTO_INCREMENT UNIQUE,
  `name` VARCHAR(32) NOT NULL UNIQUE,
  PRIMARY KEY (`id`));

-- -----------------------------------------------------
-- Table `store`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `store` (
  `id` INT NOT NULL AUTO_INCREMENT UNIQUE,
  `name` VARCHAR(128) NOT NULL UNIQUE,
  `city` VARCHAR(64) NOT NULL,
  PRIMARY KEY (`id`));

-- -----------------------------------------------------
-- Table `order`
-- -----------------country------------------------------------
CREATE TABLE IF NOT EXISTS `order` (
  `id` INT NOT NULL AUTO_INCREMENT UNIQUE,
  `idCustomer` INT NOT NULL,
  `idAddress` INT NOT NULL,
  `idStore` INT NOT NULL,
  `idFacturationAddress` INT NOT NULL,
  `creditCard` VARCHAR(64) NOT NULL,
  `date` DATETIME NOT NULL,
  `deliveryDate` DATETIME NOT NULL,
  `message` VARCHAR(128) NULL DEFAULT NULL,
  `fullPrice` DOUBLE NOT NULL,
  `finalPrice` DOUBLE NOT NULL,
  PRIMARY KEY (`id`),
  FOREIGN KEY (`idAddress`) REFERENCES `address` (`id`),
  FOREIGN KEY (`idCustomer`) REFERENCES `customer` (`id`),
  FOREIGN KEY (`idStore`) REFERENCES `store` (`id`),
  FOREIGN KEY (`idFacturationAddress`) REFERENCES `address` (`id`));

-- -----------------------------------------------------
-- Table `item`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `item` (
  `id` INT NOT NULL AUTO_INCREMENT UNIQUE,
  `name` VARCHAR(128) NOT NULL UNIQUE,
  PRIMARY KEY (`id`));

-- -----------------------------------------------------
-- Table `product`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `product` (
  `id` INT NOT NULL AUTO_INCREMENT UNIQUE,
  `name` VARCHAR(128) NOT NULL UNIQUE,
  `description` VARCHAR(4096) NULL DEFAULT NULL,
  `profilePicture` VARCHAR(512) NULL DEFAULT NULL,
  `price` DOUBLE NOT NULL,
  `visible` BOOLEAN NOT NULL,
  PRIMARY KEY (`id`));

-- -----------------------------------------------------
-- Table `product_content`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `product_content` (
  `idItem` INT NULL DEFAULT NULL,
  `idProduct` INT NULL DEFAULT NULL,
  `quantity` INT NOT NULL,
  UNIQUE KEY (`idItem`,`idProduct`),
  FOREIGN KEY (`idItem`) REFERENCES `item` (`id`),
  FOREIGN KEY (`idProduct`) REFERENCES `product` (`id`));

-- -----------------------------------------------------
-- Table `order_content`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `order_content` (
  `idOrder` INT NULL DEFAULT NULL,
  `idProduct` INT NULL DEFAULT NULL,
  `quantity` INT NOT NULL,
  UNIQUE KEY (`idOrder`,`idProduct`),
  FOREIGN KEY (`idOrder`) REFERENCES `order` (`id`),
  FOREIGN KEY (`idProduct`) REFERENCES `product` (`id`));

-- -----------------------------------------------------
-- Table `events`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `events` (
  `id` INT NOT NULL AUTO_INCREMENT UNIQUE,
  `name` VARCHAR(32) NOT NULL UNIQUE,
  PRIMARY KEY (`id`));

-- -----------------------------------------------------
-- Table `pictures`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `pictures` (
  `id` INT NOT NULL AUTO_INCREMENT UNIQUE,
  `name` VARCHAR(64) NOT NULL UNIQUE,
  `url` VARCHAR(512) NOT NULL UNIQUE,
  PRIMARY KEY (`id`));

-- -----------------------------------------------------
-- Table `item_color`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `item_color` (
  `idItem` INT NULL DEFAULT NULL,
  `idColor` INT NULL DEFAULT NULL,
  UNIQUE KEY (`idColor`,`idItem`),
  FOREIGN KEY (`idItem`) REFERENCES `item` (`id`),
  FOREIGN KEY (`idColor`) REFERENCES `colors` (`id`));
    
-- -----------------------------------------------------
-- Table `product_event`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `product_event` (
  `idEvent` INT NULL DEFAULT NULL,
  `idProduct` INT NULL DEFAULT NULL,
  UNIQUE KEY (`idEvent`,`idProduct`),
  FOREIGN KEY (`idEvent`) REFERENCES `events` (`id`),
  FOREIGN KEY (`idProduct`) REFERENCES `product` (`id`));

-- -----------------------------------------------------
-- Table `product_picture`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `product_picture` (
  `idPicture` INT NULL DEFAULT NULL,
  `idProduct` INT NULL DEFAULT NULL,
  UNIQUE KEY (`idPicture`,`idProduct`),
  FOREIGN KEY (`idPicture`) REFERENCES `pictures` (`id`),
  FOREIGN KEY (`idProduct`) REFERENCES `product` (`id`));

-- -----------------------------------------------------
-- Table `tags`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `tags` (
  `id` INT NOT NULL AUTO_INCREMENT UNIQUE,
  `name` VARCHAR(32) NOT NULL UNIQUE,
  PRIMARY KEY (`id`));

-- -----------------------------------------------------
-- Table `product_tag`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `product_tag` (
  `idTag` INT NULL DEFAULT NULL,
  `idProduct` INT NULL DEFAULT NULL,
  UNIQUE KEY (`idTag`,`idProduct`),
  FOREIGN KEY (`idTag`) REFERENCES `tags` (`id`),
  FOREIGN KEY (`idProduct`) REFERENCES `product` (`id`));

-- -----------------------------------------------------
-- Table `stock`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `stock` (
  `idItem` INT NULL DEFAULT NULL,
  `idStore` INT NULL DEFAULT NULL,
  `quantity` INT NULL DEFAULT NULL,
  UNIQUE KEY (`idItem`,`idStore`),
  FOREIGN KEY (`idItem`) REFERENCES `item` (`id`),
  FOREIGN KEY (`idStore`) REFERENCES `store` (`id`));
