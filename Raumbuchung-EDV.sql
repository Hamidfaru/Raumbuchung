-- Tabelle für Räume erstellen
CREATE TABLE Raume (
    RaumId NUMBER PRIMARY KEY,
    RaumName VARCHAR2(100) NOT NULL UNIQUE,
    Kapazitaet NUMBER NOT NULL,
    Ausstattung VARCHAR2(500),
    Etage NUMBER,
    Gebaeude VARCHAR2(100),
    Aktiv NUMBER(1) DEFAULT 1,
    ErstellungsDatum DATE DEFAULT SYSDATE
);

-- Kommentare für die Tabelle und Spalten
COMMENT ON TABLE Raume IS 'Tabelle zur Speicherung von Rauminformationen';
COMMENT ON COLUMN Raume.RaumId IS 'Eindeutige ID des Raums';
COMMENT ON COLUMN Raume.RaumName IS 'Name/Nummer des Raums (eindeutig)';
COMMENT ON COLUMN Raume.Kapazitaet IS 'Maximale Personenzahl';
COMMENT ON COLUMN Raume.Ausstattung IS 'Ausstattungsmerkmale (Beamer, Whiteboard, etc.)';
COMMENT ON COLUMN Raume.Aktiv IS '1 = aktiv, 0 = inaktiv';






-- Tabelle für Buchungen erstellen
CREATE TABLE Buchungen (
    BuchungId NUMBER PRIMARY KEY,
    RaumId NUMBER NOT NULL,
    BenutzerName VARCHAR2(100) NOT NULL,
    BenutzerEmail VARCHAR2(150) NOT NULL,
    StartZeit TIMESTAMP NOT NULL,
    EndZeit TIMESTAMP NOT NULL,
    BuchungsZweck VARCHAR2(300) NOT NULL,
    TeilnehmerAnzahl NUMBER,
    BuchungsDatum DATE DEFAULT SYSDATE,
    Status VARCHAR2(20) DEFAULT 'bestätigt',
    Bemerkungen VARCHAR2(500),
    
    -- Fremdschlüssel
    FOREIGN KEY (RaumId) REFERENCES Raume(RaumId),
    
    -- Constraints für gültige Zeiten
    CONSTRAINT CHK_Zeit CHECK (EndZeit > StartZeit),
    CONSTRAINT CHK_Status CHECK (Status IN ('bestätigt', 'storniert', 'wartend'))
);

-- Kommentare für die Buchungen-Tabelle
COMMENT ON TABLE Buchungen IS 'Tabelle zur Speicherung von Raumbuchungen';
COMMENT ON COLUMN Buchungen.BuchungId IS 'Eindeutige ID der Buchung';
COMMENT ON COLUMN Buchungen.BenutzerName IS 'Name des buchenden Benutzers';
COMMENT ON COLUMN Buchungen.BenutzerEmail IS 'E-Mail für Benachrichtigungen';
COMMENT ON COLUMN Buchungen.StartZeit IS 'Startzeit der Buchung';
COMMENT ON COLUMN Buchungen.EndZeit IS 'Endzeit der Buchung';
COMMENT ON COLUMN Buchungen.Status IS 'Status der Buchung';









-- Sequenz für Raum-IDs
CREATE SEQUENCE SEQ_Raume 
    START WITH 1 
    INCREMENT BY 1 
    NOCACHE 
    NOCYCLE;

-- Sequenz für Buchungs-IDs
CREATE SEQUENCE SEQ_Buchungen 
    START WITH 1 
    INCREMENT BY 1 
    NOCACHE 
    NOCYCLE;
    
    
    
    
    
    
    -- Index für schnelle Raumsuche
CREATE INDEX IDX_Raume_Name ON Raume(RaumName);
CREATE INDEX IDX_Raume_Aktiv ON Raume(Aktiv);

-- Index für schnelle Buchungsabfragen
CREATE INDEX IDX_Buchungen_RaumZeit ON Buchungen(RaumId, StartZeit, EndZeit);
CREATE INDEX IDX_Buchungen_Benutzer ON Buchungen(BenutzerEmail);
CREATE INDEX IDX_Buchungen_Status ON Buchungen(Status);
CREATE INDEX IDX_Buchungen_Zeitraum ON Buchungen(StartZeit, EndZeit);










-- Testräume einfügen
INSERT INTO Raume (RaumId, RaumName, Kapazitaet, Ausstattung, Etage, Gebaeude) 
VALUES (SEQ_Raume.NEXTVAL, 'Konferenzraum A', 12, 'Beamer, Whiteboard, Telefon', 1, 'Hauptgebäude');

INSERT INTO Raume (RaumId, RaumName, Kapazitaet, Ausstattung, Etage, Gebaeude) 
VALUES (SEQ_Raume.NEXTVAL, 'Meetingraum B', 6, 'Whiteboard, Monitor', 2, 'Hauptgebäude');

INSERT INTO Raume (RaumId, RaumName, Kapazitaet, Ausstattung, Etage, Gebaeude) 
VALUES (SEQ_Raume.NEXTVAL, 'Schulungsraum C', 20, 'Beamer, 20 Laptops, Whiteboard', 1, 'Nebengebäude');

INSERT INTO Raume (RaumId, RaumName, Kapazitaet, Ausstattung, Etage, Gebaeude) 
VALUES (SEQ_Raume.NEXTVAL, 'Kreativraum D', 8, 'Whiteboard, Flipchart, Moderationskoffer', 3, 'Hauptgebäude');

-- Testbuchungen einfügen
INSERT INTO Buchungen (BuchungId, RaumId, BenutzerName, BenutzerEmail, StartZeit, EndZeit, BuchungsZweck, TeilnehmerAnzahl)
VALUES (SEQ_Buchungen.NEXTVAL, 1, 'Max Mustermann', 'max.mustermann@firma.de', 
        TIMESTAMP '2024-01-20 09:00:00', TIMESTAMP '2024-01-20 11:00:00', 
        'Projektbesprechung XYZ', 8);

INSERT INTO Buchungen (BuchungId, RaumId, BenutzerName, BenutzerEmail, StartZeit, EndZeit, BuchungsZweck, TeilnehmerAnzahl)
VALUES (SEQ_Buchungen.NEXTVAL, 2, 'Anna Schmidt', 'anna.schmidt@firma.de', 
        TIMESTAMP '2024-01-20 14:00:00', TIMESTAMP '2024-01-20 15:30:00', 
        'Team-Meeting Entwicklung', 5);

-- Änderungen speichern
COMMIT;






-- Tabellenstruktur anzeigen
DESC Raume;
DESC Buchungen;

-- Testdaten anzeigen
SELECT * FROM Raume;
SELECT * FROM Buchungen;

-- Prüfe, ob die Sequenzen funktionieren
SELECT SEQ_Raume.NEXTVAL FROM DUAL;
SELECT SEQ_Buchungen.NEXTVAL FROM DUAL;