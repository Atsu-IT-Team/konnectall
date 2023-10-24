MERGE INTO [dbo].[LocaleStringResource] AS target
USING (
    VALUES 
        (1, 'Admin.Catalog.Categories.Fields.ShowAsBanner', 'Show as banner'),
		(1, 'Admin.Catalog.Categories.Fields.ShowAsBanner.Hint', 'Check if you want to show a category as banner on shop page.'),
		(1, 'Shop.PopularCategory.Product.Count', 'products'),
        (1, 'Admin.Catalog.Categories.Fields.IconId', 'Icon'),
		(1, 'Admin.Catalog.Categories.Fields.IconId.Hint', 'Upload category icon.')
) AS source ([LanguageId], [ResourceName], [ResourceValue])
ON target.[LanguageId] = source.[LanguageId] AND target.[ResourceName] = source.[ResourceName]
WHEN MATCHED THEN
    UPDATE SET target.[ResourceValue] = source.[ResourceValue]
WHEN NOT MATCHED THEN
    INSERT ([LanguageId], [ResourceName], [ResourceValue])
    VALUES (source.[LanguageId], source.[ResourceName], source.[ResourceValue]);
