using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.Models
{
    /// <summary>
    /// Representa el modelo utilizado para describir un estado
    /// </summary>
    public class Estado
    {
        /// <summary>
        /// Identificador del estado
        /// </summary>
        public long? id_estado { get; set; }
        /// <summary>
        /// Descripción del estado, ej: "Activo", "Inactivo"
        /// </summary>
        public string descripcion { get; set; } = "";
        /// <summary>
        /// Lista de estados predeterminados 
        /// </summary>

        public static IEnumerable<Estado> estados = new List<Estado>() {
            new Estado() {
                id_estado = 1, descripcion = "Activo"
            },
            new Estado() {
                id_estado = 2, descripcion = "Inactivo"
            }
        };
    }
}
