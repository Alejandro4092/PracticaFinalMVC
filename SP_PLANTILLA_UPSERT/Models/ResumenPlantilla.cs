namespace SP_PLANTILLA_UPSERT.Models
{
    public class ResumenPlantilla
    {
        public int Personas { get; set; }
        public int MaximoSalarial { get; set; }
        public double MediaSalarial { get; set; }
        public List<Plantilla> Plantilla { get; set; }
    }
}
