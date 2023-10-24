IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE Name = 'ShowAsBanner' AND Object_ID = Object_ID('Category'))
BEGIN
    ALTER TABLE [Category] 
	ADD ShowAsBanner bit NOT NULL
	CONSTRAINT ShowAsBanner_Default_Standard DEFAULT 0
	WITH VALUES
END

IF NOT EXISTS(SELECT 1 FROM SYS.COLUMNS WHERE Name = 'IconId' AND Object_ID = Object_ID('Category'))
BEGIN
    ALTER TABLE [Category] 
	ADD IconId INT NOT NULL
	CONSTRAINT IconId_Default_Standard DEFAULT 0
	WITH VALUES
END