
namespace HUSI_SIISA.Utilities
{
    /// <summary>
    /// Estructura de datos para el objeto Producto
    /// </summary>
    public class Producto
    {
        /// <summary>
        /// 
        /// </summary>
        public Int32 IdProducto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IdPresentacion { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int32 Cantidad { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ValorC { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double ValCosto { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int DiaCiclo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime FechaDispensacion { get; set; }
    }
}
