SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0;
SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0;
SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='TRADITIONAL';

CREATE SCHEMA IF NOT EXISTS `scheduler` DEFAULT CHARACTER SET utf8 COLLATE utf8_bin ;
USE `scheduler` ;

-- -----------------------------------------------------
-- Table `scheduler`.`user_type`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `scheduler`.`user_type` (
  `iduser_type` INT NOT NULL AUTO_INCREMENT ,
  `title` VARCHAR(45) NULL ,
  `desc` TEXT NULL ,
  PRIMARY KEY (`iduser_type`) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `scheduler`.`user`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `scheduler`.`user` (
  `iduser` INT NOT NULL AUTO_INCREMENT ,
  `email` VARCHAR(45) NULL ,
  `first_name` VARCHAR(45) NULL ,
  `last_name` VARCHAR(45) NULL ,
  `password` VARCHAR(45) NULL ,
  `fkuser_type` INT NULL ,
  PRIMARY KEY (`iduser`) ,
  INDEX `fkuser_type` (`fkuser_type` ASC) ,
  CONSTRAINT `fkuser_type`
    FOREIGN KEY (`fkuser_type` )
    REFERENCES `scheduler`.`user_type` (`iduser_type` )
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `scheduler`.`task_type`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `scheduler`.`task_type` (
  `idtask_type` INT NOT NULL ,
  `type` VARCHAR(45) NULL ,
  `desc` TEXT NULL ,
  PRIMARY KEY (`idtask_type`) )
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `scheduler`.`task`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `scheduler`.`task` (
  `idtask` INT NOT NULL AUTO_INCREMENT ,
  `title` VARCHAR(45) NULL ,
  `notes` TEXT NULL ,
  `startdatetime` VARCHAR(14) NULL ,
  `enddatetime` VARCHAR(14) NULL ,
  `fkuser` INT NULL ,
  `fktask_type` INT NULL ,
  `place` VARCHAR(45) NULL ,
  PRIMARY KEY (`idtask`) ,
  INDEX `fkuser` (`fkuser` ASC) ,
  INDEX `fktask_type` (`fktask_type` ASC) ,
  CONSTRAINT `fkuser`
    FOREIGN KEY (`fkuser` )
    REFERENCES `scheduler`.`user` (`iduser` )
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fktask_type`
    FOREIGN KEY (`fktask_type` )
    REFERENCES `scheduler`.`task_type` (`idtask_type` )
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB;


-- -----------------------------------------------------
-- Table `scheduler`.`participants`
-- -----------------------------------------------------
CREATE  TABLE IF NOT EXISTS `scheduler`.`participants` (
  `idparticipants` INT NOT NULL AUTO_INCREMENT ,
  `fkuser` INT NULL DEFAULT NULL ,
  `fktask` INT NULL DEFAULT NULL ,
  PRIMARY KEY (`idparticipants`) ,
  INDEX `fk_user` (`fkuser` ASC) ,
  INDEX `fk_task` (`fktask` ASC) ,
  CONSTRAINT `fk_user`
    FOREIGN KEY (`fkuser` )
    REFERENCES `scheduler`.`user` (`iduser` )
    ON DELETE CASCADE
    ON UPDATE CASCADE,
  CONSTRAINT `fk_task`
    FOREIGN KEY (`fktask` )
    REFERENCES `scheduler`.`task` (`idtask` )
    ON DELETE CASCADE
    ON UPDATE CASCADE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = utf8
COLLATE = utf8_bin;



SET SQL_MODE=@OLD_SQL_MODE;
SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS;
SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS;

-- -----------------------------------------------------
-- Data for table `scheduler`.`user_type`
-- -----------------------------------------------------
START TRANSACTION;
USE `scheduler`;
INSERT INTO `scheduler`.`user_type` (`iduser_type`, `title`, `desc`) VALUES (1, 'admin', 'full access');
INSERT INTO `scheduler`.`user_type` (`iduser_type`, `title`, `desc`) VALUES (2, 'user', 'editing own schedule, view others');

COMMIT;

-- -----------------------------------------------------
-- Data for table `scheduler`.`task_type`
-- -----------------------------------------------------
START TRANSACTION;
USE `scheduler`;
INSERT INTO `scheduler`.`task_type` (`idtask_type`, `type`, `desc`) VALUES (1, 'public', 'visible to anyone');
INSERT INTO `scheduler`.`task_type` (`idtask_type`, `type`, `desc`) VALUES (2, 'private', 'visible to author only');

COMMIT;
