using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegistroAsistenciasSMART.Model.DTO
{
	/// <summary>
	/// Modelo que describe la información de una dirección IP
	/// </summary>
	public class IpInfo
	{
		/// <summary>
		/// Dirección IP
		/// </summary>
		public string ip_address { get; set; } = "";
		/// <summary>
		/// Descripción de la dirección IP
		/// </summary>
		public string descripcion { get; set; } = "";
	}
}
