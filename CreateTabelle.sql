CREATE TABLE Cliente (
    IdCliente INT PRIMARY KEY IDENTITY,
    AziendaPrivato BIT NOT NULL,
    Nome NVARCHAR(50) NOT NULL,
    Cognome NVARCHAR(50),
    CodiceFiscale NVARCHAR(16),
    PartitaIva NVARCHAR(16),
    Indirizzo NVARCHAR(100) NOT NULL,
    Citta NVARCHAR(50) NOT NULL
);

CREATE TABLE Spedizione (
    IdSpedizione INT PRIMARY KEY IDENTITY,
    IdCliente INT FOREIGN KEY REFERENCES Cliente(IdCliente),
    DataSpedizione DATE NOT NULL,
    Peso DECIMAL(10,2) NOT NULL,
    CittaDestinazione NVARCHAR(50) NOT NULL,
    IndirizzoDestinazione NVARCHAR(100) NOT NULL,
    NominativoDestinatario NVARCHAR(100) NOT NULL,
    SpeseSpedizione MONEY NOT NULL,
    DataConsegnaPrevista DATE NOT NULL
);
