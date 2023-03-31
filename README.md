# Sistema Control de Calidad de Calzados

## Descripción del Sistema

- Aplicación Web.
- El Sistema está desarrollado con Blazor server side .NET 6, código c# de base. 
- Se utilizó MudBlazor como framework de frontend.
- Y base de datos SqlServer 2017 para la persistencia. Manejado con EntityFramework.

## Integrantes Grupo 23
- **Christian Nicolas Soria**
- **Rafael Eduardo Montero**

##Docentes
- **Docente de Teoría: Nieto Peñalver, Luis Eduardo.**
- **Docente de Prática: Vicente, Francisco.**

##Explicación

El sistema permite el control de defectos y pares a primera de una orden de producción de calzados. Con el se puede realizar logueo de empleados como Supervisor de Línea y Supervisor de calidad(quién es el que controla los pares de calzados).
![](https://i.ibb.co/vHG151H/login.png)
Una vez logueado el supervisor de línea debe crear la OP.

![](https://i.ibb.co/WxKYRv4/CrearOP.png)

Una vez creada, tiene las opciones de administración de OP.

![](https://i.ibb.co/K6YwCMN/OPCreada.png)


Creada la OP al loguearse el supervisor de calidad se les muestra las op disponibles para control.

![](https://i.ibb.co/Jp0KwkW/OPDisponibles.png)

El Supervisor de calidad se vincula a la OP. Y tiene las opciones de control.

![](https://i.ibb.co/vjRxwW5/Control-OP.png)

Si presiona el botón monitor abre una nueva pantalla que se muestra la cantidad de defectos que se van cargando.

![](https://i.ibb.co/YcWf076/Monitor-Sin-Defectos.png)

Al empezar a controlar se actualiza el monitor en tiempo real. Y dependiendo los límites establecidos el semáforo va cambiando de color.

![](https://i.ibb.co/r0ZfW42/Control-OP1.png)   

![](https://i.ibb.co/FgD7sJW/Monitor-Con-Defectos.png)
![](https://i.ibb.co/qrP12PW/Monitor-Con-Defectos1.png) ![](https://i.ibb.co/ftrLcpq/Monitor-Con-Defectos2.png) ![](https://i.ibb.co/YcWf076/Monitor-Sin-Defectos.png)

Para probarlo es necesario clonar el proyecto y restaurar la BD(.bkp) que dejo en la carpeta llamada DBakup.