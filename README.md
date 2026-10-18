# E-CommerceProyect

Panel administrativo de comercio electrónico desarrollado con **ASP.NET Core MVC**, **C#**, **Entity Framework Core** y **SQL Server**. Permite gestionar clientes, categorías, productos y pedidos, y consultar estadísticas de ventas a partir de los datos registrados.

El proyecto está organizado en tres capas y utiliza vistas Razor, modales de Bootstrap y tablas interactivas para facilitar las tareas de administración.

## Funcionalidades

### Panel principal

* Contadores de productos, clientes y pedidos.
* Ventas totales calculadas con los pedidos completados.
* Gráfico de ventas de los últimos seis meses.
* Gráfico de pedidos agrupados por estado.
* Importes presentados en pesos dominicanos (RD$).

### Usuarios y clientes

* Registro, inicio y cierre de sesión con ASP.NET Core Identity.
* Asociación de cada usuario registrado con un cliente.
* Consulta de información, edición de datos y eliminación de clientes mediante modales.
* Confirmación antes de eliminar y regreso al detalle al cancelar.
* Validación para impedir la eliminación del propio usuario durante su sesión.

### Categorías y productos

* Creación, consulta, edición y eliminación mediante modales.
* Organización de productos por categoría.
* Registro de nombre, descripción, precio y existencias.
* Búsqueda, ordenación y paginación con DataTables.
* Desactivación del botón de creación de productos y mensaje **“Guardando...”** al enviar el formulario.

### Pedidos

* Creación de pedidos con un cliente y varios productos.
* Selección de cantidades y cálculo de subtotales en el formulario.
* Comprobación de cantidades contra las existencias disponibles y cálculo de importes en el servidor.
* Registro del nombre del producto, precio unitario y cantidad en cada detalle del pedido.
* Consulta de productos, cliente, fecha, estado e importes en un modal de detalle.
* Cambio de estado y eliminación con confirmación.

Los estados disponibles son **Pending**, **Processing**, **Shipped**, **Completed** y **Cancelled**. La edición de un pedido permite cambiar su estado; los productos se consultan desde el detalle.

## Tecnologías

|Tecnología|Uso|
|-|-|
|.NET 10 y ASP.NET Core MVC|Aplicación web y controladores|
|C#|Lógica de la aplicación|
|Razor y ViewModels|Presentación de datos y formularios|
|Entity Framework Core 10|Consultas, persistencia y migraciones|
|SQL Server|Base de datos relacional|
|ASP.NET Core Identity|Gestión de usuarios y autenticación|
|Bootstrap 5|Diseño, formularios y modales|
|JavaScript y jQuery|Interacción con la interfaz|
|DataTables|Tablas con búsqueda, ordenación y paginación|
|Chart.js|Gráficos del panel principal|

## Organización del proyecto

|Proyecto|Responsabilidad|
|-|-|
|`E-CommerceSite`|Capa de presentación: controladores MVC, vistas Razor, ViewModels y recursos de la interfaz|
|`E-CommerceSite.Negocio`|Capa de negocio: servicios, interfaces y registro de dependencias|
|`E-CommerceSite.Datos`|Capa de datos: entidades, `ApplicationDbContext` y migraciones de Entity Framework Core|

La solución se encuentra en `E-CommerceProyect.slnx`. El proyecto web recibe los datos a través de los servicios registrados mediante inyección de dependencias.

## Modelo de datos

* Un usuario de Identity está asociado a un cliente.
* Una categoría agrupa varios productos.
* Un cliente puede tener varios pedidos.
* Un pedido contiene varios detalles (`OrderItem`).
* Cada detalle referencia un producto y conserva el nombre, el precio unitario y la cantidad utilizados en el pedido.

## Requisitos

* SDK de **.NET 10**.
* Una instancia de **SQL Server**; para desarrollo en Windows también puede utilizarse **SQL Server Express LocalDB**.
* Git.
* Visual Studio compatible con .NET 10 o un editor con acceso a la CLI de .NET.
* Acceso a Internet para restaurar paquetes y cargar los recursos de DataTables y Chart.js utilizados desde CDN.

## Instalación y ejecución

### 1\. Clonar el repositorio

```bash
git clone https://github.com/leandro1623/E-CommerceProyect.git
cd E-CommerceProyect
```

### 2\. Configurar la conexión

Agrega o ajusta la sección `ConnectionStrings` en `E-CommerceSite/appsettings.Development.json`, conservando las demás opciones del archivo.

Ejemplo para LocalDB en Windows:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\\\MSSQLLocalDB;Database=ECommerceProyect;Trusted\_Connection=True;TrustServerCertificate=True;"
  }
}
```

Si utilizas otra instancia de SQL Server, adapta el servidor, la base de datos y el método de autenticación a tu entorno. La aplicación busca la cadena con el nombre **`DefaultConnection`**.

### 3\. Restaurar los paquetes

Ejecuta los siguientes comandos desde la raíz del repositorio:

```bash
dotnet restore E-CommerceProyect.slnx
```

### 4\. Aplicar las migraciones

Instala la herramienta de Entity Framework Core si todavía no la tienes. La versión indicada coincide con los paquetes actuales del proyecto:

```bash
dotnet tool install --global dotnet-ef --version 10.0.11
```

Si ya tienes la herramienta instalada, comprueba su versión con `dotnet ef --version` y utiliza una versión compatible con EF Core 10.

Aplica las migraciones de la capa de datos utilizando el proyecto web como proyecto de inicio:

```bash
dotnet ef database update --project E-CommerceSite.Datos --startup-project E-CommerceSite --context ApplicationDbContext -- --environment Development
```

Las migraciones utilizadas por la aplicación están en **`E-CommerceSite.Datos/Migrations`**.

### 5\. Ejecutar la aplicación

En Windows, confía en el certificado HTTPS de desarrollo:

```bash
dotnet dev-certs https --trust
```

Inicia la aplicación:

```bash
dotnet run --project E-CommerceSite --launch-profile https
```

Con la configuración actual, abre **https://localhost:7243**. También puedes abrir la solución en Visual Studio, seleccionar `E-CommerceSite` como proyecto de inicio y ejecutar el perfil `https`.

### 6\. Registrar una cuenta y agregar datos

Visita `/Account/Register`, registra una cuenta e inicia sesión. Después crea categorías y productos desde sus respectivas secciones. Para crear un pedido necesitas al menos un cliente y un producto con existencias disponibles.

Los datos que registres se reflejarán en las listas y en el panel principal.

## Comportamiento actual

* El acceso a las secciones de administración requiere autenticación.
* Los modales permiten consultar datos y cancelar operaciones sin navegar a otra página. Los formularios envían las operaciones al servidor mediante POST.
* Los pedidos utilizan un impuesto del **18 %** y un descuento de **0** en la lógica actual de creación.
* Las ventas del panel consideran los pedidos con estado **Completed**.
* Las existencias se administran desde Productos. La creación de pedidos comprueba las cantidades disponibles, pero la actualización automática del stock está pendiente.

El proyecto se encuentra en desarrollo y su alcance actual es la administración del catálogo, clientes y pedidos.

## Autor

**Leandro Brito Contreras** — [leandro1623](https://github.com/leandro1623)

## Referencias

* [Documentación de las herramientas de Entity Framework Core](https://learn.microsoft.com/es-es/ef/core/cli/dotnet)
