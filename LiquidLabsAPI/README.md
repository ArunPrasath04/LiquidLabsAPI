# Liquid Labs - Assignment API 

	#Libraries Used
		** Microsoft.Data.Client -> Provider library for .NET to connect with SQL Server
		** Microsoft.Extensions.Http -> Provides the HttpClientFactory which is used to sending request to the third party API
		** System.Net.Http.Json -> JSON sterialization/desterialization

	#Third Party API Used
		** https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd
		** Data on cryptocoins

	#Two endpoints
		** GET /api/CryptoCoin
		** GET /api/CryptoCoin/{id}

	#Instructions
		** Set the connection string 
		** If u have already created a database and set it to the connection string the API is designed to create the table if it doesnt exist already
		** You can also run the Schema.sql in Database folder to manually create a database called "CryptoCoinsDB" and the table - "Coins"

	#Build Project
		** navigate to root folder
		** dotnet build

	#Run Project
		** dotnet run --project LiquidLabsAPI