using System.Collections.Generic;

namespace Podedex.Models.Pokemon
{
    public class Language
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class Version
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class FlavorTextEntry
    {
        public string flavor_text { get; set; }
        public Language language { get; set; }
        public Version version { get; set; }
    }

    public class Habitat
    {
        public string name { get; set; }
        public string url { get; set; }
    }

    public class PokemonResponse
    {
        public List<FlavorTextEntry> flavor_text_entries { get; set; }
        public Habitat habitat { get; set; }
        public bool is_legendary { get; set; }
        public string name { get; set; }
    }
}
