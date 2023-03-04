using Newtonsoft.Json.Linq;
using RogueliteSurvivor.Constants;
using System.Collections.Generic;

namespace RogueliteSurvivor.Containers
{
    public class CreditsContainer
    {
        public CreditsContainer() { }
        public List<OutsideResourceCreditContainer> OutsideResources { get; set; }
        
        public static CreditsContainer ToCreditsContainer(JToken credits)
        {
            var creditsContainer = new CreditsContainer();
            creditsContainer.OutsideResources = new List<OutsideResourceCreditContainer>();

            foreach(var outsideResource in credits["outsideResources"])
            {
                creditsContainer.OutsideResources.Add(OutsideResourceCreditContainer.ToOutsideResourceCreditContainer(outsideResource));
            }

            return creditsContainer;
        }
    }

    public class OutsideResourceCreditContainer
    {
        public OutsideResourceCreditContainer() { }

        public string Author { get; set; }
        public List<string> Packages { get; set; }

        public static OutsideResourceCreditContainer ToOutsideResourceCreditContainer(JToken outsideResource)
        {
            var outsideResourceContainer = new OutsideResourceCreditContainer()
            {
                Author = (string)outsideResource["author"],
                Packages = new List<string>()
            };

            foreach(var package in outsideResource["packages"])
            {
                outsideResourceContainer.Packages.Add((string)package);
            }

            return outsideResourceContainer;
        }
    }
}
