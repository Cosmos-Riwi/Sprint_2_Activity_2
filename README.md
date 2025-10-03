# 🍽️ El Rincón del Sabor - Sistema de Gestión de Restaurante

Sistema completo de gestión para restaurantes desarrollado en ASP.NET Core MVC con PostgreSQL.

## 📋 Funcionalidades

### 🏢 Gestión de Entidades
- **👥 Clientes** - Gestión de información personal de clientes
- **👨‍🍳 Meseros** - Administración del personal de servicio
- **🍽️ Platos** - Catálogo de menú con precios y categorías
- **📋 Pedidos** - Sistema de órdenes con estados
- **📅 Reservas** - Gestión de reservaciones de mesas

### 🔗 Relaciones
- **Cliente → Pedidos** (1:N) - Un cliente puede tener múltiples pedidos
- **Cliente → Reservas** (1:N) - Un cliente puede hacer múltiples reservas

### ✅ Validaciones
- Precio de platos > 0
- Estados de pedidos válidos (pendiente, servido, cancelado)
- Número de personas en reservas > 0
- Validaciones de email y campos requeridos

## 🚀 Despliegue

### Prerrequisitos
- .NET 8.0 SDK
- PostgreSQL 12+
- Visual Studio Code o Visual Studio

### Instalación

1. **Clonar el repositorio**
```bash
git clone <repository-url>
cd GestionRestaurante
```

2. **Restaurar dependencias**
```bash
dotnet restore
```

3. **Configurar base de datos**
```bash
# Crear migración inicial
dotnet ef migrations add InitialCreate

# Aplicar migraciones a la base de datos
dotnet ef database update
```

4. **Ejecutar la aplicación**
```bash
dotnet run
```

5. **Acceder a la aplicación**
```
https://localhost:5001
```

## 🗄️ Base de Datos

### Credenciales PostgreSQL
```
Host: 168.119.183.3
Puerto: 5432
Base de datos: Restaurant_JuanManuel
Usuario: root
Contraseña: s7cq453mt2jnicTaQXKT
```

### Conexión
La cadena de conexión está configurada en `appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=168.119.183.3;Port=5432;Database=Restaurant_JuanManuel;Username=root;Password=s7cq453mt2jnicTaQXKT"
  }
}
```

## 📁 Estructura del Proyecto

```
GestionRestaurante/
├── 📁 Controllers/           # Controladores MVC
│   ├── ClientesController.cs
│   ├── MeserosController.cs
│   ├── PlatosController.cs
│   ├── PedidosController.cs
│   └── ReservasController.cs
├── 📁 Data/                 # Contexto de base de datos
│   └── ApplicationDbContext.cs
├── 📁 Models/               # Modelos de datos
│   ├── Cliente.cs
│   ├── Mesero.cs
│   ├── Plato.cs
│   ├── Pedido.cs
│   └── Reserva.cs
├── 📁 Views/                # Vistas Razor mejoradas
│   ├── 📁 Clientes/         # CRUD completo con diseño moderno
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   ├── Details.cshtml
│   │   └── Delete.cshtml
│   ├── 📁 Meseros/          # CRUD completo con diseño moderno
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   ├── Details.cshtml
│   │   └── Delete.cshtml
│   ├── 📁 Platos/           # CRUD completo con diseño moderno
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   ├── Details.cshtml
│   │   └── Delete.cshtml
│   ├── 📁 Pedidos/          # CRUD completo con diseño moderno
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   ├── Details.cshtml
│   │   └── Delete.cshtml
│   ├── 📁 Reservas/         # CRUD completo con diseño moderno
│   │   ├── Index.cshtml
│   │   ├── Create.cshtml
│   │   ├── Edit.cshtml
│   │   ├── Details.cshtml
│   │   └── Delete.cshtml
│   ├── 📁 Home/             # Página de inicio personalizada
│   │   └── Index.cshtml
│   └── 📁 Shared/           # Layout y componentes compartidos
│       ├── _Layout.cshtml   # Layout principal con toast dinámico
│       ├── _ViewImports.cshtml
│       ├── _ViewStart.cshtml
│       └── _ValidationScriptsPartial.cshtml
├── 📁 Helpers/              # Clases auxiliares
│   ├── PriceHelper.cs       # Formateo de precios
│   └── DecimalConverter.cs  # Conversión de decimales
├── 📁 ModelBinders/         # Model binders personalizados
│   ├── DecimalModelBinder.cs
│   └── DecimalModelBinderProvider.cs
├── 📁 wwwroot/              # Archivos estáticos
│   ├── 📁 css/
│   ├── 📁 js/
│   └── 📁 lib/
├── 📄 Program.cs            # Configuración de la aplicación
├── 📄 appsettings.json      # Configuración y cadenas de conexión
├── 📄 GestionRestaurante.csproj  # Dependencias del proyecto
├── 📄 .gitignore            # Archivos ignorados por Git
└── 📄 README.md            
```

## 🛠️ Tecnologías Utilizadas

### 🖥️ **Backend**
- **ASP.NET Core 8.0 MVC** - Framework principal
- **Entity Framework Core** - ORM para base de datos
- **Npgsql.EntityFrameworkCore.PostgreSQL** - Proveedor PostgreSQL
- **Data Annotations** - Validaciones de modelos

### 🎨 **Frontend**
- **Bootstrap 5** - Framework CSS responsive
- **Font Awesome** - Iconografía completa
- **CSS3 Animations** - Animaciones dinámicas
- **JavaScript ES6** - Interactividad avanzada

### 🗄️ **Base de Datos**
- **PostgreSQL 12+** - Base de datos principal
- **Migraciones EF Core** - Control de versiones de BD
- **Relaciones 1:N** - Cliente → Pedidos/Reservas

### 🔧 **Herramientas de Desarrollo**
- **Model Binders personalizados** - Manejo de decimales con comas
- **Helpers personalizados** - Formateo de precios
- **Type Converters** - Conversión de datos
- **Toast notifications** - Sistema de notificaciones dinámico

## 📊 Estados de Pedidos

- **🟡 Pendiente** - Pedido recibido, en preparación
- **🟢 Servido** - Pedido entregado al cliente
- **🔴 Cancelado** - Pedido cancelado

## 🏷️ Categorías de Platos

- **🥗 Entrada** - Aperitivos y entradas
- **🍖 Plato Fuerte** - Platos principales
- **🍰 Postre** - Dulces y postres
- **🥤 Bebida** - Bebidas y refrescos

## 🔧 Comandos Útiles

```bash
# Compilar proyecto
dotnet build

# Ejecutar en modo desarrollo
dotnet run

# Crear nueva migración
dotnet ef migrations add <NombreMigracion>

# Actualizar base de datos
dotnet ef database update

# Ver estado de migraciones
dotnet ef migrations list
```

## 📝 Notas de Desarrollo

### 🗄️ **Base de Datos**
- El sistema está configurado para **PostgreSQL**
- Todas las tablas usan **nombres en minúsculas** (estándar PostgreSQL)
- Las relaciones están configuradas con **eliminación en cascada**
- **Enums** convertidos a strings para compatibilidad con PostgreSQL
- **Value Converters** personalizados para manejo de tipos

### 🎨 **Interfaz de Usuario**
- **Diseño completamente responsive** para todos los dispositivos
- **Toast notifications dinámicos** con animaciones avanzadas
- **Formularios mejorados** con input groups y validaciones visuales
- **Headers con gradientes** distintivos para cada acción
- **Iconos Font Awesome** en todos los elementos

### 🔄 **Funcionalidades Avanzadas**
- **Conteo automático** de pedidos y reservas por cliente
- **Estados dinámicos** con badges de colores
- **Validaciones en tiempo real** con mensajes descriptivos
- **Navegación intuitiva** con breadcrumbs visuales
- **Efectos hover** y transiciones suaves

### 🔧 **Mejoras Técnicas**
- **Model Binders personalizados** para manejo de decimales
- **Conteo automático** de pedidos y reservas por cliente
- **Helpers personalizados** para formateo de precios
- **Type Converters** para entrada con comas
- **Value Converters** para compatibilidad con PostgreSQL


## 👨‍💻 Autor

- **Juan Manuel Arango Arana**

