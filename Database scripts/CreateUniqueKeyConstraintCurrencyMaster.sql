USE [ExchangeRates]
GO

/****** Object:  Index [ix_tblCurrencyMaster_applicableOn]    Script Date: 2/15/2021 5:06:10 PM ******/
CREATE UNIQUE NONCLUSTERED INDEX [ix_tblCurrencyMaster_applicableOn] ON [dbo].[CurrencyMaster]
(
	[ApplicableOn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO


