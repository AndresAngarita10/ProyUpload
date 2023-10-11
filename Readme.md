# PROYUPLOAD

## STAFF:
`<Colaborador>` : <https://github.com/AndresAngarita10>
`<Colaborador>` : <https://github.com/DJangoo00>

## Material AudioVisual:
- 
+ Instructivo:
    + [Documento](https://docs.google.com/document/d/19J7EKegzOwjvSMPM6iHk4Y_K0dxpPlbM/edit?usp=sharing&ouid=109141888634537685486&rtpof=true&sd=true)
+ Video:
    + [YouTube](https://www.youtube.com/watch?v=EOYqG8yGNp0)

**Table of Contents**

[TOC]

## Lenguajes:
- HTML
- CSS
- JavaScript
- C#

## Tecnologias:
- [Dotnet 7.0.4](https://dotnet.microsoft.com/en-us/download/dotnet/7.0/)

## Proyecto:
### Objetivo:
Generar una API que pueda consumir archivos.
### Descripcion
API que contiene una entidad UploadFile, la cual se encarga de recibir un archivo, sus controller estan diseñados para soportar imagenes o documentos respectivamente, incluyen restricciones segun corresponde.
Adicionalmente incluyen un ejemplo de pagina en HTML para consumir la API

## Base de datos:
1. Tecnologia:
- SQL
2. Tablas:
- UploadFile:

    	| id  |   name  | extension |  size  |  route  |
    	| --- | ------- | --------- | ------ | ------- |
    	| int | varchar |  varchar  | double | varchar |

  - TypeFile:

    	| id  | description |
    	| --- | ----------- |
    	| int |   varchar   |

## Dependencias Importantes: 

Ubicacion: Application
- System.Drawing.Common
- System.IO.Compression


