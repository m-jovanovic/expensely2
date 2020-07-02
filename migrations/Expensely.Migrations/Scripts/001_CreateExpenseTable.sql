CREATE TABLE [dbo].[Expense] (
	[Id] [uniqueidentifier] NOT NULL,
	[Amount] [decimal](19, 4) NOT NULL,
	[CurrencyId] [int] NOT NULL,
	[CurrencyCode] [varchar](3) NOT NULL,
	[CurrencySign] [varchar](5) NOT NULL,
	[CreatedOnUtc] [datetime2](7) NOT NULL,
	[ModifiedOnUtc] [datetime2](7) NULL,
	[Deleted] [bit] NOT NULL,
	CONSTRAINT [PK_Expense] PRIMARY KEY CLUSTERED 
	(
		[Id] ASC
	)
	WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON)ON [PRIMARY]
) ON [PRIMARY]

ALTER TABLE [dbo].[Expense] ADD  CONSTRAINT [DF_Expense_Deleted]  DEFAULT ((0)) FOR [Deleted]