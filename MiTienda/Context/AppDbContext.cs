using Microsoft.EntityFrameworkCore;
using MiTienda.CapaEntidades;

namespace MiTienda.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        // Registramos las tablas (DbSets)
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<DetalleVenta> DetalleVentas { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Provincia> Provincias { get; set; }
        public DbSet<Distrito> Distritos { get; set; }
        public DbSet<Carrito> Carritos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configuraciones adicionales (si es necesario)
            // Por ejemplo, relaciones entre tablas, restricciones, etc.

            modelBuilder.Entity<Categoria>(e =>
            {
                //Tabla real en la base de datos
                e.ToTable("categoria");
                e.HasKey(c => c.IdCategoria);
                e.Property(c => c.IdCategoria).HasColumnName("idCategoria").ValueGeneratedOnAdd();
                e.Property(c => c.Descripcion).HasColumnName("descripcion").IsRequired().HasMaxLength(100);
                e.HasIndex(c => c.Descripcion).IsUnique();
                e.Property(c => c.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);
                // FECHA REGISTRO: Le decimos que use la función de SQL Server
                e.Property(c => c.FechaRegistro).HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("sysutcdatetime()") // <--- Usa el motor de SQL para la fecha
                    .ValueGeneratedOnAdd(); // <--- Solo se genera al crear el registro
                e.HasData(
                    new Categoria { IdCategoria = 1, Descripcion = "Electrónica" },
                    new Categoria { IdCategoria = 2, Descripcion = "Dormitorio" },
                    new Categoria { IdCategoria = 3, Descripcion = "Tecnologia" }
                );
            });

            modelBuilder.Entity<Marca>(e =>
            {
                e.ToTable("marca");
                e.HasKey(m => m.IdMarca);
                e.Property(m => m.IdMarca).HasColumnName("idMarca").ValueGeneratedOnAdd();
                e.Property(m => m.Descripcion).HasColumnName("descripcion").IsRequired().HasMaxLength(100);
                e.HasIndex(m => m.Descripcion).IsUnique();
                e.Property(m => m.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);
                e.Property(m => m.FechaRegistro).HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("sysutcdatetime()")
                    .ValueGeneratedOnAdd();
                e.HasData(
                    new Marca { IdMarca = 1, Descripcion = "Samsung" },
                    new Marca { IdMarca = 2, Descripcion = "LG" },
                    new Marca { IdMarca = 3, Descripcion = "Sony" }
                );
            });

            modelBuilder.Entity<Producto>(e =>
            {
                e.ToTable("producto");

                e.HasKey(p => p.IdProducto);
                e.Property(p => p.IdProducto).HasColumnName("idProducto").ValueGeneratedOnAdd();
                e.Property(p => p.Nombre).HasColumnName("nombre").IsRequired().HasMaxLength(100);
                e.Property(p => p.Descripcion).HasColumnName("descripcion").HasMaxLength(255);
                e.Property(p => p.Precio).HasColumnName("precio").IsRequired()
                    .HasColumnType("decimal(10,2)")
                    .HasDefaultValue(0);
                e.Property(p => p.Stock).HasColumnName("stock").IsRequired()
                    .HasDefaultValue(0);
                e.Property(p => p.RutaImagen).HasColumnName("rutaImagen").HasMaxLength(500);
                e.Property(p => p.NombreImagen).HasColumnName("nombreImagen").HasMaxLength(255);
                e.Property(p => p.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);
                e.Property(p => p.FechaRegistro).HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("sysutcdatetime()")
                    .ValueGeneratedOnAdd();
                e.Property(p => p.IdMarca).HasColumnName("idMarca").IsRequired();
                e.Property(p => p.IdCategoria).HasColumnName("idCategoria").IsRequired();
                // Relaciones
                e.HasOne(p => p.oCategoria)
                    .WithMany()
                    .HasForeignKey(p => p.IdCategoria)
                    .OnDelete(DeleteBehavior.Restrict);
                e.HasOne(p => p.oMarca)
                    .WithMany()
                    .HasForeignKey(p => p.IdMarca)
                    .OnDelete(DeleteBehavior.Restrict);
                //Agregamos las restricciones de checks (Check Constraints)
                e.ToTable(t => t.HasCheckConstraint("CK_Producto_Precio", "precio >= 0"));
                e.ToTable(t => t.HasCheckConstraint("CK_Producto_Stock", "stock >= 0"));
                // Datos de prueba
                e.HasData(
                    new Producto { IdProducto = 1, Nombre = "Smart TV Samsung 55\"", Descripcion = "Televisor inteligente de 55 pulgadas", Precio = 799.99m, Stock = 10, IdCategoria = 1, IdMarca = 1 },
                    new Producto { IdProducto = 2, Nombre = "Refrigeradora LG", Descripcion = "Refrigeradora de alta eficiencia", Precio = 1199.99m, Stock = 5, IdCategoria = 1, IdMarca = 2 },
                    new Producto { IdProducto = 3, Nombre = "Laptop Sony", Descripcion = "Laptop para uso diario", Precio = 499.99m, Stock = 15, IdCategoria = 3, IdMarca = 3 }
                );
            });

            modelBuilder.Entity<Usuario>(e =>
            {
                e.ToTable("usuario");
                e.HasKey(u => u.IdUsuario);
                e.Property(u => u.IdUsuario).HasColumnName("idUsuario").ValueGeneratedOnAdd();
                e.Property(u => u.Nombres).HasColumnName("nombres").IsRequired().HasMaxLength(100);
                e.Property(u => u.Apellidos).HasColumnName("apellidos").IsRequired().HasMaxLength(100);
                e.Property(u => u.Correo).HasColumnName("correo").IsRequired().HasMaxLength(100);
                e.HasIndex(u => u.Correo).IsUnique();
                e.Property(u => u.Clave).HasColumnName("clave").IsRequired().HasMaxLength(150);
                e.Property(u => u.Reestablecer).HasColumnName("reestablecer").IsRequired().HasDefaultValue(false);
                e.Property(u => u.Activo).HasColumnName("activo").IsRequired().HasDefaultValue(true);
                e.Property(u => u.FechaRegistro).HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("sysutcdatetime()")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Cliente>(e =>
            {
                e.ToTable("cliente");
                e.HasKey(c => c.IdCliente);
                e.Property(c => c.IdCliente).HasColumnName("idCliente").ValueGeneratedOnAdd();
                e.Property(c => c.Nombres).HasColumnName("nombres").IsRequired().HasMaxLength(100);
                e.Property(c => c.Apellidos).HasColumnName("apellidos").IsRequired().HasMaxLength(100);
                e.Property(c => c.Correo).HasColumnName("correo").IsRequired().HasMaxLength(100);
                e.HasIndex(c => c.Correo).IsUnique();
                e.Property(c => c.Clave).HasColumnName("clave").IsRequired().HasMaxLength(150);
                e.Property(c => c.Reestablecer).HasColumnName("reestablecer").IsRequired().HasDefaultValue(false);
                e.Property(c => c.FechaRegistro).HasColumnName("fechaRegistro")
                    .HasDefaultValueSql("sysutcdatetime()")
                    .ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<Carrito>(e =>
            {
                e.ToTable("carrito");
                e.HasKey(c => c.IdCarrito);
                e.Property(c => c.IdCarrito).HasColumnName("idCarrito").ValueGeneratedOnAdd();
                e.Property(c => c.IdCliente).HasColumnName("idCliente").IsRequired();
                e.Property(c => c.IdProducto).HasColumnName("idProducto").IsRequired();
                e.Property(c => c.Cantidad).HasColumnName("cantidad").IsRequired();
                // Relaciones
                e.HasOne(c => c.oCliente)
                    .WithMany()
                    .HasForeignKey(c => c.IdCliente)
                    .OnDelete(DeleteBehavior.Cascade);
                e.HasOne(c => c.oProducto)
                    .WithMany()
                    .HasForeignKey(c => c.IdProducto)
                    .OnDelete(DeleteBehavior.Cascade);
                //Agregamos las restricciones de checks (Check Constraints)
                e.ToTable(t => t.HasCheckConstraint("CK_Carrito_Cantidad", "cantidad > 0"));
            });

            //Ubigeo: Departamento, Provincia, Distrito
            // 1. Mapeo de Departamento
            modelBuilder.Entity<Departamento>(e =>
            {
                e.ToTable("departamento");
                e.HasKey(d => d.IdDepartamento);

                // El ID es un texto de longitud fija (ej: "01")
                e.Property(d => d.IdDepartamento)
                    .HasColumnName("idDepartamento")
                    .HasMaxLength(2)
                    .IsFixedLength();

                e.Property(d => d.Descripcion)
                    .HasColumnName("descripcion")
                    .IsRequired()
                    .HasMaxLength(50);
            });

            // 2. Mapeo de Provincia
            modelBuilder.Entity<Provincia>(e =>
            {
                e.ToTable("provincia");
                e.HasKey(p => p.IdProvincia);

                e.Property(p => p.IdProvincia)
                    .HasColumnName("idProvincia")
                    .HasMaxLength(4)
                    .IsFixedLength();

                e.Property(p => p.Descripcion)
                    .HasColumnName("descripcion")
                    .IsRequired()
                    .HasMaxLength(50);

                e.Property(p => p.IdDepartamento)
                    .HasColumnName("idDepartamento")
                    .HasMaxLength(2)
                    .IsFixedLength()
                    .IsRequired();

                // Relación: Una Provincia pertenece a un Departamento
                e.HasOne(p => p.oDepartamento)
                    .WithMany() // No necesitamos la lista de provincias en Departamento por ahora
                    .HasForeignKey(p => p.IdDepartamento)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            // 3. Mapeo de Distrito
            modelBuilder.Entity<Distrito>(e =>
            {
                e.ToTable("distrito");
                e.HasKey(d => d.IdDistrito);

                e.Property(d => d.IdDistrito)
                    .HasColumnName("idDistrito")
                    .HasMaxLength(6)
                    .IsFixedLength();

                e.Property(d => d.Descripcion)
                    .HasColumnName("descripcion")
                    .IsRequired()
                    .HasMaxLength(50);

                e.Property(d => d.IdProvincia)
                    .HasColumnName("idProvincia")
                    .HasMaxLength(4)
                    .IsFixedLength()
                    .IsRequired();

                e.Property(d => d.IdDepartamento)
                    .HasColumnName("idDepartamento")
                    .HasMaxLength(2)
                    .IsFixedLength()
                    .IsRequired();
                // Relación: Un Distrito pertenece a un Departamento (aunque ya lo tenemos en Provincia, es bueno tenerlo para consultas directas)
                e.HasOne(d => d.oDepartamento)
                    .WithMany()
                    .HasForeignKey(d => d.IdDepartamento)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relación: Un Distrito pertenece a una Provincia
                e.HasOne(d => d.oProvincia)
                    .WithMany()
                    .HasForeignKey(d => d.IdProvincia)
                    .OnDelete(DeleteBehavior.Restrict);

                // ⬇️ VALIDACIÓN: Asegura que el IdDepartamento del distrito coincida con el de su provincia
                e.ToTable(t => t.HasCheckConstraint(
                    "CK_Distrito_Departamento_Consistente", "idDepartamento = LEFT(idProvincia, 2)"
                ));
            });

            modelBuilder.Entity<Venta>(e =>
            {
                e.ToTable("venta");
                e.HasKey(v => v.IdVenta);
                e.Property(v => v.IdVenta).HasColumnName("idVenta").ValueGeneratedOnAdd();

                e.Property(v => v.TotalProducto).HasColumnName("totalProducto").IsRequired();
                e.Property(v => v.MontoTotal).HasColumnName("montoTotal").HasColumnType("decimal(10,2)").IsRequired();

                e.Property(v => v.Contacto).HasColumnName("contacto").HasMaxLength(100).IsRequired();
                e.Property(v => v.Telefono).HasColumnName("telefono").HasMaxLength(20).IsRequired();
                e.Property(v => v.Direccion).HasColumnName("direccion").HasMaxLength(500).IsRequired();
                e.Property(v => v.IdTransaccion).HasColumnName("idTransaccion").HasMaxLength(100);

                e.Property(v => v.FechaVenta).HasColumnName("fechaVenta")
                    .HasDefaultValueSql("sysutcdatetime()")
                    .ValueGeneratedOnAdd();

                // Relación con Cliente
                e.HasOne(v => v.oCliente)
                    .WithMany()
                    .HasForeignKey(v => v.IdCliente)
                    .OnDelete(DeleteBehavior.Restrict);

                // Relación con Distrito (Ubigeo)
                e.Property(v => v.IdDistrito).HasColumnName("idDistrito").HasMaxLength(6).IsFixedLength();
                // Si tienes el objeto oDistrito en tu clase Venta:
                e.HasOne(v => v.oDistrito)
                    .WithMany()
                    .HasForeignKey(v => v.IdDistrito);
            });

            modelBuilder.Entity<DetalleVenta>(e =>
            {
                e.ToTable("detalleVenta");
                e.HasKey(dv => dv.IdDetalleVenta);
                e.Property(dv => dv.IdDetalleVenta).HasColumnName("idDetalleVenta").ValueGeneratedOnAdd();

                e.Property(dv => dv.Cantidad).HasColumnName("cantidad").IsRequired();
                e.Property(dv => dv.Total).HasColumnName("total").HasColumnType("decimal(10,2)").IsRequired();

                // Relación con la Venta Padre
                e.HasOne(dv => dv.oVenta)
                    .WithMany(v => v.oDetalleVenta) // O .WithMany(v => v.oDetalleVenta) si tienes la lista en Venta
                    .HasForeignKey(dv => dv.IdVenta)
                    .OnDelete(DeleteBehavior.Cascade); // Si se borra la venta, se borra su detalle

                // Relación con el Producto comprado
                e.HasOne(dv => dv.oProducto)
                    .WithMany()
                    .HasForeignKey(dv => dv.IdProducto)
                    .OnDelete(DeleteBehavior.Restrict);

                e.ToTable(t => t.HasCheckConstraint("CK_DetalleVenta_Cantidad", "cantidad > 0"));
                e.ToTable(t => t.HasCheckConstraint("CK_DetalleVenta_Total", "total >= 0"));
            });
        }
    }
}
