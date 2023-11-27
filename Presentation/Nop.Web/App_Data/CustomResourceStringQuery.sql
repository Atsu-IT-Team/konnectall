MERGE INTO [dbo].[LocaleStringResource] AS target
USING (
    VALUES 
        (1, 'Admin.Catalog.Categories.Fields.ShowAsBanner', 'Show as banner'),
		(1, 'Admin.Catalog.Categories.Fields.ShowAsBanner.Hint', 'Check if you want to show a category as banner on shop page.'),
		(1, 'Shop.PopularCategory.Product.Count', 'products'),
        (1, 'Admin.Catalog.Categories.Fields.IconId', 'Icon'),
		(1, 'Admin.Catalog.Categories.Fields.IconId.Hint', 'Upload category icon.'),
        (1, 'Admin.Catalog.Categories.Fields.BannerId', 'Banner'),
		(1, 'Admin.Catalog.Categories.Fields.BannerId.Hint', 'Upload category banner.'),
        (1, 'Admin.Catalog.Categories.Fields.IsMostPopular', 'Is most popular'),
		(1, 'Admin.Catalog.Categories.Fields.IsMostPopular.Hint', 'Check if you want to show a category as most popular on shop page.'),
        (1, 'Admin.Vendors.Fields.Commission', 'Commission'),
		(1, 'Admin.Vendors.Fields.Commission.Hint', 'Enter the KonnectAll commission in (%).'),
        (1, 'Admin.Orders.Fields.Confirmed', 'Confirmed'),
		(1, 'Admin.Orders.Fields.Confirmed.Hint', 'Checked to confirmed the order after that you can not modify this order.'),
        (1, 'Admin.Orders.Products.Confirmed', 'Confirmed'),
) AS source ([LanguageId], [ResourceName], [ResourceValue])
ON target.[LanguageId] = source.[LanguageId] AND target.[ResourceName] = source.[ResourceName]
WHEN MATCHED THEN
    UPDATE SET target.[ResourceValue] = source.[ResourceValue]
WHEN NOT MATCHED THEN
    INSERT ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (source.[LanguageId], source.[ResourceName], source.[ResourceValue]);
