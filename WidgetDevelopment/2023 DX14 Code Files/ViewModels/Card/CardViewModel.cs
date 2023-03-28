using Progress.Sitefinity.RestSdk.Dto;

namespace Renderer.ViewModels.Card
{
    public class CardViewModel
    {
        public string CardTitle { get; set; }
        public string CardText { get; set; }
        public string ButtonUrl { get; set; }
        public string ButtonText { get; set; }
        public string ButtonStyle { get; set; }
        public ImageDto Image { get; set; }
        public string TextAlign { get; set; }
        public string SectionCss { get; set; }
    }
}
