# Sistema de Inventario Ricinomex

Aplicacion de escritorio desarrollada en C# con Windows Forms para apoyar la gestion operativa de Ricinomex. El sistema centraliza procesos de compras, inventario, produccion, ventas, cotizaciones, clientes, vendedores y administracion de precios.

## Contexto del proyecto

Este proyecto fue desarrollado como parte de practicas profesionales para mejorar el control de inventario y el seguimiento de operaciones internas. La aplicacion se conecta a una base de datos MySQL y separa la logica principal en formularios, modelos, acceso a datos y recursos visuales.

## Funcionalidades principales

- Inicio de sesion por usuario y recuperacion/cambio de contrasena.
- Registro, busqueda, edicion y eliminacion de productores.
- Registro de compras de grano, semillas y costales.
- Consulta de resultados de compras y pagos.
- Control de inventario de grano, producto envasado y producto terminado.
- Registro y seguimiento de pedidos de produccion.
- Registro de clientes, cotizaciones, ventas concretadas y ventas no concretadas.
- Administracion de vendedores, facturacion y condiciones comerciales.
- Actualizacion de precios desde el modulo de administrador.

## Tecnologias

- C#
- Windows Forms
- .NET Framework 4.7.2
- MySQL
- NuGet `packages.config`
- iText / iTextSharp para documentos PDF
- Newtonsoft.Json

## Estructura del proyecto

```text
.
|-- appInventario.sln
|-- appInventario.csproj
|-- App.config
|-- Program.cs
|-- Form1.cs
|-- DAOS/
|   |-- daos.cs
|   |-- daosadministrador.cs
|   |-- daosproduccion.cs
|   `-- daosventas.cs
|-- DB/
|   `-- conexiondb.cs
|-- Forms/
|   |-- Administrador/
|   |-- Produccion/
|   |-- Ventas/
|   |-- compras.cs
|   |-- actualizar.cs
|   |-- carga.cs
|   |-- confirmacioncompra.cs
|   |-- recuperarusuario.cs
|   |-- resultadocompras.cs
|   |-- resultadosbusqueda.cs
|   `-- resultadoscompracostales.cs
|-- Icons/
|-- Models/
|-- Properties/
|-- Resources/
|-- packages.config
`-- README.md
```

## Convenciones de organizacion

- `DAOS/`: clases de acceso a datos y consultas SQL por modulo.
- `DB/`: clase de conexion a MySQL.
- `Forms/`: pantallas de la aplicacion. Los modulos principales viven en subcarpetas (`Administrador`, `Produccion`, `Ventas`).
- `Models/`: entidades y clases auxiliares usadas por formularios y DAOs.
- `Icons/`: iconos e imagenes pequenas usadas por la interfaz y el ejecutable.
- `Resources/`: imagenes institucionales o recursos graficos generales.
- `Properties/`: recursos generados por Visual Studio, configuracion y manifiesto.

## Configuracion

1. Abrir `appInventario.sln` en Visual Studio.
2. Restaurar paquetes NuGet si Visual Studio no lo hace automaticamente.
3. Configurar la conexion a MySQL en los DAOs. Actualmente la conexion principal se instancia en `DAOS/daos.cs` con valores de ejemplo:

```csharp
db = new conexiondb("IP", "usuario", "base", "password");
```

4. Verificar que la base de datos tenga las tablas usadas por los DAOs, por ejemplo `registro_usuarios`, `registro_productores`, `compra_grano`, `compra_costales`, `inventario_producto`, `registro_pedido`, `registro_clientes`, `cotizacion`, `historial_compra`, `ventas_noconcretadas`, `ventas_concretadas` y `registro_vendedor`.
5. Compilar en `Debug` o `Release`.

## Notas de mantenimiento

- El icono de aplicacion esta en `Icons/rc-removebg.ico`.
- Los recursos visuales estan referenciados desde `Properties/Resources.resx`; si se renombra una imagen, tambien debe actualizarse ahi y en `appInventario.csproj`.
- El proyecto tiene configuracion de publicacion ClickOnce y firma de manifiesto. Si los certificados `.pfx` no estan disponibles en el equipo, puede ser necesario desactivar la firma o volver a configurar la publicacion desde Visual Studio.

## Autor

Edwin Lustre
