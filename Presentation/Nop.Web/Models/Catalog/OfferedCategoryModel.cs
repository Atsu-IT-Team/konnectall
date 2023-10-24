namespace Nop.Web.Models.Catalog
{
    public partial record OfferedCategoryModel
    {
        public int CategoryId { get; set; }
        public string Name { get; set; }
        public string SeName { get; set; }
        public string ImageUrl { get; set; }
    }
}
