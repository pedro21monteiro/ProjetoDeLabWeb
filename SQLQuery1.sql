CREATE TABLE utilizador(
IdUtilizador integer not null,
UserName varchar(13) not null, 
_Password varchar(13) not null,
Email varchar(13) not null,
Bloqueado Bit not null,
Motivo varchar(100),
RegistoConfirmado Bit not null,
primary key(IdUtilizador),
)

CREATE TABLE Cliente(
IdUtilizador integer not null,
Nome varchar(13) not null, 

foreign key(IdUtilizador) References Utilizador,
primary key(IdUtilizador),
)

CREATE TABLE Restaurantes(
IdUtilizador integer not null,

Telefone varchar(9) not null, 
Foto varchar(100) not null, 
Morada varchar(100) not null, 
Gps varchar(100) not null, 
Horario_func varchar(100) not null, 
Dia_Descanco varchar(20) not null, 
TipoServico varchar(30) not null, 

foreign key(IdUtilizador) References Utilizador,
primary key(IdUtilizador),

)

CREATE TABLE Administrador(
IdUtilizador integer not null,

foreign key(IdUtilizador) References Utilizador,
primary key(IdUtilizador),
)

CREATE TABLE PratoDoDia(
IdPratoDoDia integer not null,
Descricao varchar(100) not null, 
Foto varchar(100),
DataPrato Date,
Tipo varchar(30) not null, 
primary key(IDPratoDoDia),

)

CREATE TABLE Possui(
DataPossui Date,
Preco Money,
Idutilizador integer not null,
IdPratoDoDia integer not null,

foreign key(IdUtilizador) References Restaurantes(IdUtilizador),
foreign key(IdPratoDoDia) References PratoDoDia(IdPratoDoDia),
primary key(DataPossui),
)

CREATE TABLE Preferem(

IdUtilizador integer not null,
IdPratoDoDia integer not null,

foreign key(IdUtilizador) References Cliente(Idutilizador),
foreign key(IdPratoDoDia) References PratoDoDia(IdPratoDoDia),

)
CREATE TABLE PreferemRestaurante(

IdUtilizadorCliente integer not null,
IdUtilizadorRestaurante integer not null,

foreign key(IdUtilizadorCliente) References Cliente(Idutilizador),
foreign key(IdUtilizadorRestaurante) References Restaurantes(IdUtilizador),

)
