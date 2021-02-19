CREATE SCHEMA santa_shop;

CREATE TABLE santa_shop.criancas (
crianca_id int UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
nome varchar(120) NOT NULL ,
idade int UNSIGNED NOT NULL ,
presente_id int UNSIGNED ,
comportamento_id int UNSIGNED ,
CONSTRAINT unq_criancas_presente_id UNIQUE ( presente_id ) ,
CONSTRAINT unq_criancas_comportamento_id UNIQUE ( comportamento_id )
) engine=InnoDB;

CREATE TABLE santa_shop.presentes (
presente_id int UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
nome varchar(120) NOT NULL ,
quantidade int UNSIGNED NOT NULL
) engine=InnoDB;

CREATE TABLE santa_shop.comportamento (
comportamento_id int UNSIGNED NOT NULL AUTO_INCREMENT PRIMARY KEY,
descricao varchar(120) NOT NULL ,
condicao bit
) engine=InnoDB;

ALTER TABLE santa_shop.comportamento ADD CONSTRAINT fk_comportamento_criancas FOREIGN KEY ( comportamento_id ) REFERENCES santa_shop.criancas( comportamento_id ) ON DELETE NO ACTION ON UPDATE NO ACTION;

ALTER TABLE santa_shop.presentes ADD CONSTRAINT fk_presentes_criancas FOREIGN KEY ( presente_id ) REFERENCES santa_shop.criancas( presente_id ) ON DELETE NO ACTION ON UPDATE NO ACTION;