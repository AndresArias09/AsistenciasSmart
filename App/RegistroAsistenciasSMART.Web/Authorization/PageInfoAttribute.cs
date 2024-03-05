namespace RegistroAsistenciasSMART.Web.Authorization
{
    /// <summary>
    /// Representa un atributo de página mediante el cual se puede indicar el módulo al cual pertenece cada página
    /// </summary>
    public class PageInfoAttribute : Attribute
    {
        /// <summary>
        /// Identificador del módulo asociado a la página
        /// </summary>
        public string id_modulo = "";
        /// <summary>
        /// Determina si la página es suceptible a perfilamiento o no. Por defecto su valor es <see langword="true" />
        /// </summary>
        public bool perfilable = true;

        public PageInfoAttribute(string id_modulo = "", bool perfilable = true)
        {
            this.id_modulo = id_modulo;
            this.perfilable = perfilable;
        }
    }
}
