namespace MiTienda.Models
{
    public class CatalogoVM
    {
        public IEnumerable<CategoriaVM>? Categorias { get; set; }
        public IEnumerable<MarcaVM>? Marcas { get; set; }
        public IEnumerable<ProductoVM>? Productos { get; set; }
        public string? Filtro { get; set; }
    }
}
