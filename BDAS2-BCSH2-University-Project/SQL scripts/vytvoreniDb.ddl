-- Generated by Oracle SQL Developer Data Modeler 22.2.0.165.1149
--   at:        2022-12-28 19:28:32 CET
--   site:      Oracle Database 12c
--   type:      Oracle Database 12c



-- predefined type, no DDL - MDSYS.SDO_GEOMETRY

-- predefined type, no DDL - XMLTYPE

CREATE TABLE adresy (
    idadresy                  NUMBER NOT NULL,
    mesto                     VARCHAR2(32) NOT NULL,
    ulice                     VARCHAR2(32),
    shops_idshops       NUMBER,
    zamestnan�i_idzamestnance NUMBER
);

ALTER TABLE adresy
    ADD CONSTRAINT fkarc_3 CHECK ( ( ( zamestnan�i_idzamestnance IS NOT NULL )
                                     AND ( shops_idshops IS NULL ) )
                                   OR ( ( shops_idshops IS NOT NULL )
                                        AND ( zamestnan�i_idzamestnance IS NULL ) )
                                   OR ( ( zamestnan�i_idzamestnance IS NULL )
                                        AND ( shops_idshops IS NULL ) ) );

ALTER TABLE adresy ADD CONSTRAINT adresa_pk PRIMARY KEY ( idadresy );

CREATE TABLE hotove (
    idplatby NUMBER NOT NULL,
    vraceno  NUMBER NOT NULL
);

ALTER TABLE hotove ADD CONSTRAINT hotove_pk PRIMARY KEY ( idplatby );

CREATE TABLE karty (
    idplatby       NUMBER NOT NULL,
    cislokarty     NUMBER NOT NULL,
    autorizacnikod NUMBER NOT NULL
);

ALTER TABLE karty ADD CONSTRAINT karta_pk PRIMARY KEY ( idplatby );

CREATE TABLE kategorije (
    idkategorije NUMBER NOT NULL,
    nazev        VARCHAR2(32) NOT NULL
);

ALTER TABLE kategorije ADD CONSTRAINT kategorije_pk PRIMARY KEY ( idkategorije );

CREATE TABLE kupony (
    idplatby NUMBER NOT NULL,
    cislo    NUMBER NOT NULL
);

ALTER TABLE kupony ADD CONSTRAINT kupon_pk PRIMARY KEY ( idplatby );

CREATE TABLE platby (
    idplatby    NUMBER NOT NULL,
    jeclubcarta NUMBER(1) NOT NULL,
    platba_type VARCHAR2(6) NOT NULL
);

ALTER TABLE platby
    ADD CHECK ( jeclubcarta IN ( 0, 1 ) );

ALTER TABLE platby
    ADD CONSTRAINT ch_inh_platba CHECK ( platba_type IN ( 'HOTOVE', 'KARTA', 'KUPON', 'PLATBA' ) );

ALTER TABLE platby ADD CONSTRAINT platby_pk PRIMARY KEY ( idplatby );

CREATE TABLE pokladny (
    idpokladny          NUMBER NOT NULL,
    shops_idshops NUMBER,
    cislo               NUMBER NOT NULL,
    jesamoobsluzna      NUMBER(1) NOT NULL
);

ALTER TABLE pokladny
    ADD CHECK ( jesamoobsluzna IN ( 0, 1 ) );

ALTER TABLE pokladny ADD CONSTRAINT pokladny_pk PRIMARY KEY ( idpokladny );

CREATE TABLE pozice (
    idpozice NUMBER NOT NULL,
    nazev    VARCHAR2(32) NOT NULL,
    mzda     NUMBER NOT NULL
);

ALTER TABLE pozice ADD CONSTRAINT pozice_pk PRIMARY KEY ( idpozice );

CREATE TABLE prodane_zbozi (
    pocetzbozi       NUMBER NOT NULL,
    prodej_idprodeje NUMBER NOT NULL,
    zbozi_idzbozi    NUMBER NOT NULL,
    prodejnicena     NUMBER NOT NULL
);

ALTER TABLE prodane_zbozi ADD CONSTRAINT prodane_zbozi_pk PRIMARY KEY ( prodej_idprodeje,
                                                                        zbozi_idzbozi );

CREATE TABLE prodeje (
    idprodeje       NUMBER NOT NULL,
    datumprodeje    DATE NOT NULL,
    celkovacena     NUMBER NOT NULL,
    platba_idplatby NUMBER NOT NULL
);

CREATE UNIQUE INDEX prodej__idxv1 ON
    prodeje (
        platba_idplatby
    ASC );

ALTER TABLE prodeje ADD CONSTRAINT prodej_pk PRIMARY KEY ( idprodeje );

CREATE TABLE shops (
    idshops     NUMBER NOT NULL,
    kontaktnicislo VARCHAR2(32),
    plocha         VARCHAR2(32) NOT NULL
);

ALTER TABLE shops ADD CONSTRAINT Shop_pk PRIMARY KEY ( idshops );

CREATE TABLE pulty (
    idpultu             NUMBER NOT NULL,
    shops_idshops NUMBER,
    cislo               NUMBER NOT NULL,
    pocetpolicek        NUMBER NOT NULL
);

ALTER TABLE pulty ADD CONSTRAINT pulty_pk PRIMARY KEY ( idpultu );

CREATE TABLE sklady (
    idskladu     NUMBER NOT NULL,
    pocetpolicek NUMBER NOT NULL
);

ALTER TABLE sklady ADD CONSTRAINT sklady_pk PRIMARY KEY ( idskladu );

CREATE TABLE zamestnanci (
    idzamestnance       NUMBER NOT NULL,
    pozice_idpozice     NUMBER NOT NULL,
    jmeno               VARCHAR2(32) NOT NULL,
    prijmeni            VARCHAR2(32),
    rodnecislo          VARCHAR2(32) NOT NULL,
    telefonnicislo      VARCHAR2(32) NOT NULL,
    shops_idshops NUMBER NOT NULL
);

ALTER TABLE zamestnanci ADD CONSTRAINT zamestnan�i_pk PRIMARY KEY ( idzamestnance );

CREATE TABLE zbozi (
    idzbozi                 NUMBER NOT NULL,
    nazev                   VARCHAR2(32) NOT NULL,
    aktualnicena            NUMBER NOT NULL,
    cenazeclubcartou        NUMBER,
    kategorije_idkategorije NUMBER NOT NULL,
    hmotnost                NUMBER NOT NULL
);

ALTER TABLE zbozi ADD CONSTRAINT zbozi_pk PRIMARY KEY ( idzbozi );

CREATE TABLE zbozi_na_pulte (
    pulty_idpultu NUMBER NOT NULL,
    zbozi_idzbozi NUMBER NOT NULL,
    pocetzbozi    NUMBER NOT NULL
);

ALTER TABLE zbozi_na_pulte ADD CONSTRAINT zbozi_na_pulte_pk PRIMARY KEY ( pulty_idpultu,
                                                                          zbozi_idzbozi );

CREATE TABLE zbozi_na_sklade (
    pocetzbozi      NUMBER NOT NULL,
    sklady_idskladu NUMBER NOT NULL,
    zbozi_idzbozi   NUMBER NOT NULL
);

ALTER TABLE zbozi_na_sklade ADD CONSTRAINT zbozi_na_sklade_pk PRIMARY KEY ( zbozi_idzbozi,
                                                                            sklady_idskladu );

ALTER TABLE adresy
    ADD CONSTRAINT adresy_shops_fk FOREIGN KEY ( shops_idshops )
        REFERENCES shops ( idshops );

ALTER TABLE adresy
    ADD CONSTRAINT adresy_zamestnan�i_fk FOREIGN KEY ( zamestnan�i_idzamestnance )
        REFERENCES zamestnanci ( idzamestnance );

ALTER TABLE hotove
    ADD CONSTRAINT hotove_platba_fk FOREIGN KEY ( idplatby )
        REFERENCES platby ( idplatby );

ALTER TABLE karty
    ADD CONSTRAINT karta_platba_fk FOREIGN KEY ( idplatby )
        REFERENCES platby ( idplatby );

ALTER TABLE kupony
    ADD CONSTRAINT kupon_platba_fk FOREIGN KEY ( idplatby )
        REFERENCES platby ( idplatby );

ALTER TABLE pokladny
    ADD CONSTRAINT pokladny_shops_fk FOREIGN KEY ( shops_idshops )
        REFERENCES shops ( idshops );

ALTER TABLE prodane_zbozi
    ADD CONSTRAINT prodane_zbozi_prodej_fk FOREIGN KEY ( prodej_idprodeje )
        REFERENCES prodeje ( idprodeje );

ALTER TABLE prodane_zbozi
    ADD CONSTRAINT prodane_zbozi_zbozi_fk FOREIGN KEY ( zbozi_idzbozi )
        REFERENCES zbozi ( idzbozi );

ALTER TABLE prodeje
    ADD CONSTRAINT prodej_platba_fk FOREIGN KEY ( platba_idplatby )
        REFERENCES platby ( idplatby );

ALTER TABLE pulty
    ADD CONSTRAINT pulty_shops_fk FOREIGN KEY ( shops_idshops )
        REFERENCES shops ( idshops );

ALTER TABLE zamestnanci
    ADD CONSTRAINT zamestnan�i_pozice_fk FOREIGN KEY ( pozice_idpozice )
        REFERENCES pozice ( idpozice );

ALTER TABLE zamestnanci
    ADD CONSTRAINT zamestnan�i_shops_fk FOREIGN KEY ( shops_idshops )
        REFERENCES shops ( idshops );

ALTER TABLE zbozi
    ADD CONSTRAINT zbozi_kategorije_fk FOREIGN KEY ( kategorije_idkategorije )
        REFERENCES kategorije ( idkategorije );

ALTER TABLE zbozi_na_pulte
    ADD CONSTRAINT zbozi_na_pulte_pulty_fk FOREIGN KEY ( pulty_idpultu )
        REFERENCES pulty ( idpultu );

ALTER TABLE zbozi_na_pulte
    ADD CONSTRAINT zbozi_na_pulte_zbozi_fk FOREIGN KEY ( zbozi_idzbozi )
        REFERENCES zbozi ( idzbozi );

ALTER TABLE zbozi_na_sklade
    ADD CONSTRAINT zbozi_na_sklade_sklady_fk FOREIGN KEY ( sklady_idskladu )
        REFERENCES sklady ( idskladu );

ALTER TABLE zbozi_na_sklade
    ADD CONSTRAINT zbozi_na_sklade_zbozi_fk FOREIGN KEY ( zbozi_idzbozi )
        REFERENCES zbozi ( idzbozi );

CREATE OR REPLACE TRIGGER arc_fkarc_2_kupony BEFORE
    INSERT OR UPDATE OF idplatby ON kupony
    FOR EACH ROW
DECLARE
    d VARCHAR2(6);
BEGIN
    SELECT
        a.platba_type
    INTO d
    FROM
        platby a
    WHERE
        a.idplatby = :new.idplatby;

    IF ( d IS NULL OR d <> 'KUPON' ) THEN
        raise_application_error(-20223, 'FK Kupon_PLATBA_FK in Table KUPONY violates Arc constraint on Table PLATBY - discriminator column PLATBA_TYPE doesn''t have value ''KUPON'''
        );
    END IF;

EXCEPTION
    WHEN no_data_found THEN
        NULL;
    WHEN OTHERS THEN
        RAISE;
END;
/

CREATE OR REPLACE TRIGGER arc_fkarc_2_hotove BEFORE
    INSERT OR UPDATE OF idplatby ON hotove
    FOR EACH ROW
DECLARE
    d VARCHAR2(6);
BEGIN
    SELECT
        a.platba_type
    INTO d
    FROM
        platby a
    WHERE
        a.idplatby = :new.idplatby;

    IF ( d IS NULL OR d <> 'HOTOVE' ) THEN
        raise_application_error(-20223, 'FK HOTOVE_PLATBA_FK in Table HOTOVE violates Arc constraint on Table PLATBY - discriminator column PLATBA_TYPE doesn''t have value ''HOTOVE'''
        );
    END IF;

EXCEPTION
    WHEN no_data_found THEN
        NULL;
    WHEN OTHERS THEN
        RAISE;
END;
/

CREATE OR REPLACE TRIGGER arc_fkarc_2_karty BEFORE
    INSERT OR UPDATE OF idplatby ON karty
    FOR EACH ROW
DECLARE
    d VARCHAR2(6);
BEGIN
    SELECT
        a.platba_type
    INTO d
    FROM
        platby a
    WHERE
        a.idplatby = :new.idplatby;

    IF ( d IS NULL OR d <> 'KARTA' ) THEN
        raise_application_error(-20223, 'FK KARTA_PLATBA_FK in Table KARTY violates Arc constraint on Table PLATBY - discriminator column PLATBA_TYPE doesn''t have value ''KARTA'''
        );
    END IF;

EXCEPTION
    WHEN no_data_found THEN
        NULL;
    WHEN OTHERS THEN
        RAISE;
END;
/

CREATE SEQUENCE adresy_idadresy_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER adresy_idadresy_trg BEFORE
    INSERT ON adresy
    FOR EACH ROW
    WHEN ( new.idadresy IS NULL )
BEGIN
    :new.idadresy := adresy_idadresy_seq.nextval;
END;
/

CREATE SEQUENCE karty_idplatby_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER karty_idplatby_trg BEFORE
    INSERT ON karty
    FOR EACH ROW
    WHEN ( new.idplatby IS NULL )
BEGIN
    :new.idplatby := karty_idplatby_seq.nextval;
END;
/

CREATE SEQUENCE kategorije_idkategorije_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER kategorije_idkategorije_trg BEFORE
    INSERT ON kategorije
    FOR EACH ROW
    WHEN ( new.idkategorije IS NULL )
BEGIN
    :new.idkategorije := kategorije_idkategorije_seq.nextval;
END;
/

CREATE SEQUENCE platby_idplatby_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER platby_idplatby_trg BEFORE
    INSERT ON platby
    FOR EACH ROW
    WHEN ( new.idplatby IS NULL )
BEGIN
    :new.idplatby := platby_idplatby_seq.nextval;
END;
/

CREATE SEQUENCE pokladny_idpokladny_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER pokladny_idpokladny_trg BEFORE
    INSERT ON pokladny
    FOR EACH ROW
    WHEN ( new.idpokladny IS NULL )
BEGIN
    :new.idpokladny := pokladny_idpokladny_seq.nextval;
END;
/

CREATE SEQUENCE pozice_idpozice_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER pozice_idpozice_trg BEFORE
    INSERT ON pozice
    FOR EACH ROW
    WHEN ( new.idpozice IS NULL )
BEGIN
    :new.idpozice := pozice_idpozice_seq.nextval;
END;
/

CREATE SEQUENCE prodeje_idprodeje_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER prodeje_idprodeje_trg BEFORE
    INSERT ON prodeje
    FOR EACH ROW
    WHEN ( new.idprodeje IS NULL )
BEGIN
    :new.idprodeje := prodeje_idprodeje_seq.nextval;
END;
/

CREATE SEQUENCE shops_idshops_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER shops_idshops_trg BEFORE
    INSERT ON shops
    FOR EACH ROW
    WHEN ( new.idshops IS NULL )
BEGIN
    :new.idshops := shops_idshops_seq.nextval;
END;
/

CREATE SEQUENCE pulty_idpultu_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER pulty_idpultu_trg BEFORE
    INSERT ON pulty
    FOR EACH ROW
    WHEN ( new.idpultu IS NULL )
BEGIN
    :new.idpultu := pulty_idpultu_seq.nextval;
END;
/

CREATE SEQUENCE sklady_idskladu_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER sklady_idskladu_trg BEFORE
    INSERT ON sklady
    FOR EACH ROW
    WHEN ( new.idskladu IS NULL )
BEGIN
    :new.idskladu := sklady_idskladu_seq.nextval;
END;
/

CREATE SEQUENCE zamestnanci_idzamestnance_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER zamestnanci_idzamestnance_trg BEFORE
    INSERT ON zamestnanci
    FOR EACH ROW
    WHEN ( new.idzamestnance IS NULL )
BEGIN
    :new.idzamestnance := zamestnanci_idzamestnance_seq.nextval;
END;
/

CREATE SEQUENCE zbozi_idzbozi_seq START WITH 1 NOCACHE ORDER;

CREATE OR REPLACE TRIGGER zbozi_idzbozi_trg BEFORE
    INSERT ON zbozi
    FOR EACH ROW
    WHEN ( new.idzbozi IS NULL )
BEGIN
    :new.idzbozi := zbozi_idzbozi_seq.nextval;
END;
/
