USE [ExchangeRates]
GO

/****** Object:  Table [dbo].[ExchangeRatesData]    Script Date: 2/15/2021 5:06:57 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[ExchangeRatesData](
	[Id] [bigint] IDENTITY(1,1) NOT NULL,
	[CurrencyMasterID] [bigint] NOT NULL,
	[BaseCurrency] [varchar](3) NOT NULL,
	[ConvertedCurrency] [varchar](3) NOT NULL,
	[ExchangeRate] [float] NOT NULL,
	[UpdatedDate] [datetime] NOT NULL,
 CONSTRAINT [PK_tblExchangeRates] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

ALTER TABLE [dbo].[ExchangeRatesData]  WITH NOCHECK ADD  CONSTRAINT [FK_tblExchangeRates_tblCurrencyMaster] FOREIGN KEY([CurrencyMasterID])
REFERENCES [dbo].[CurrencyMaster] ([Id])
GO

ALTER TABLE [dbo].[ExchangeRatesData] CHECK CONSTRAINT [FK_tblExchangeRates_tblCurrencyMaster]
GO


