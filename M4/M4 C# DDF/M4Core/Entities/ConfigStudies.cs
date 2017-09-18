using System.Drawing;

namespace M4Core.Entities
{
    public class ConfigStudies
    {
        public decimal LineThickness { get; set; }

        public Color Color { get; set; }

        public string[,] Retracements { get; set; }

        public string[,] Projections { get; set; }
    }
}