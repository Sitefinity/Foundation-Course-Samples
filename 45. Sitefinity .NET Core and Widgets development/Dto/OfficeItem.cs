using Progress.Sitefinity.RestSdk.Dto;

namespace Renderer.Dto
{
    public class OfficeItem : ISdkItem
    {
        public string Id { get; set; }

        public string Provider { get; set; }

        public string Title { get; set; }

        public string Info { get; set; }

        public Image[] Picture { get; set; }
    }

    public class Image
    {
        public string ThumbnailUrl { get; set; }

        public string AlternativeText { get; set; }
    }
}
