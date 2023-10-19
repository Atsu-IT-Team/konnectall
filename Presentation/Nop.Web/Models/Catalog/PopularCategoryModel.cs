namespace Nop.Web.Models.Catalog
{
    public partial record PopularCategoryModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SeName { get; set; }
        public string ImageUrl { get; set; }
        public int ProductsCount { get; set; }
    }
}

