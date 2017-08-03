CREATE TABLE [dbo].[Actor] (
    [ActID] INT           IDENTITY (100, 1) NOT NULL,
    [Name]  VARCHAR (30)  NULL,
    [Sex]   TINYINT       NULL,
    [DOB]   DATE          NULL,
    [Bio]   VARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([ActID] ASC)
);


CREATE TABLE [dbo].[Producer] (
    [ProID] INT           IDENTITY (100, 1) NOT NULL,
    [Name]  VARCHAR (30)  NULL,
    [Sex]   TINYINT       NULL,
    [DOB]   DATE          NULL,
    [Bio]   VARCHAR (200) NULL,
    PRIMARY KEY CLUSTERED ([ProID] ASC)
);

CREATE TABLE [dbo].[Movie] (
    [MovID]       INT           IDENTITY (100, 1) NOT NULL,
    [Name]        VARCHAR (30)  NULL,
    [ReleaseDate] DATE          NULL,
    [Plot]        VARCHAR (200) NULL,
    [Poster]      IMAGE         NULL,
    [ProID]       INT           NULL,
    PRIMARY KEY CLUSTERED ([MovID] ASC),
    FOREIGN KEY ([ProID]) REFERENCES [dbo].[Producer] ([ProID])
);



CREATE TABLE [dbo].[MovieActor] (
    [RelID] INT IDENTITY (100, 1) NOT NULL,
    [ActID] INT NOT NULL,
    [MovID] INT NOT NULL,
    PRIMARY KEY CLUSTERED ([RelID] ASC),
    UNIQUE NONCLUSTERED ([ActID] ASC, [MovID] ASC),
    FOREIGN KEY ([ActID]) REFERENCES [dbo].[Actor] ([ActID]),
    FOREIGN KEY ([MovID]) REFERENCES [dbo].[Movie] ([MovID])
);



