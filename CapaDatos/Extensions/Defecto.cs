using System.ComponentModel.DataAnnotations.Schema;

namespace CapaDatos.Models
{
    public partial class Defecto
    {
        [NotMapped]
        public int CantidadMarcada { get; set; }

        public Defecto Clone()
        {
            return (Defecto)this.MemberwiseClone();
        }
    }
}
