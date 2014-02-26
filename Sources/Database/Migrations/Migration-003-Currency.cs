using FluentMigrator;

namespace NDepth.Database.Migrations
{
    [Migration(3)]
    public class Migration003Currency : Migration
    {
        public override void Up()
        {
            Create.Table("Currency")
                .WithColumn("Id").AsInt64().NotNullable().PrimaryKey()
                .WithColumn("Version").AsInt32().NotNullable()
                .WithColumn("Name").AsString().NotNullable()
                .WithColumn("Description").AsString().NotNullable();

            Create.Index("UC_Name").OnTable("Currency")
                .OnColumn("Name").Ascending().WithOptions().Unique();

            // Insert new key with initial value to HiLoTable table.
            Insert.IntoTable("HiLoTable").Row(new { TableName = "Currency", NextHi = 1 });

            // Fill table with data.
            if (!this.IsSchemaOnly())
            {
                Insert.IntoTable("Currency").Row(new { Id =   1, Version = 1, Name = "AED", Description = "United Arab Emirates Dirham" });
                Insert.IntoTable("Currency").Row(new { Id =   2, Version = 1, Name = "AFN", Description = "Afghan Afghani" });
                Insert.IntoTable("Currency").Row(new { Id =   3, Version = 1, Name = "ALL", Description = "Albanian Lek" });
                Insert.IntoTable("Currency").Row(new { Id =   4, Version = 1, Name = "AMD", Description = "Armenian Dram" });
                Insert.IntoTable("Currency").Row(new { Id =   5, Version = 1, Name = "ANG", Description = "Netherlands Antillean Guilder" });
                Insert.IntoTable("Currency").Row(new { Id =   6, Version = 1, Name = "AOA", Description = "Angolan Kwanza" });
                Insert.IntoTable("Currency").Row(new { Id =   7, Version = 1, Name = "ARS", Description = "Argentine Peso" });
                Insert.IntoTable("Currency").Row(new { Id =   8, Version = 1, Name = "AUD", Description = "Australian Dollar" });
                Insert.IntoTable("Currency").Row(new { Id =   9, Version = 1, Name = "AWG", Description = "Aruban Florin" });
                Insert.IntoTable("Currency").Row(new { Id =  10, Version = 1, Name = "AZN", Description = "Azerbaijani Manat" });
                Insert.IntoTable("Currency").Row(new { Id =  11, Version = 1, Name = "BAM", Description = "Bosnia-Herzegovina Convertible Mark" });
                Insert.IntoTable("Currency").Row(new { Id =  12, Version = 1, Name = "BBD", Description = "Barbadian Dollar" });
                Insert.IntoTable("Currency").Row(new { Id =  13, Version = 1, Name = "BDT", Description = "Bangladeshi Taka" });
                Insert.IntoTable("Currency").Row(new { Id =  14, Version = 1, Name = "BGN", Description = "Bulgarian Lev" });
                Insert.IntoTable("Currency").Row(new { Id =  15, Version = 1, Name = "BHD", Description = "Bahraini Dinar" });
                Insert.IntoTable("Currency").Row(new { Id =  16, Version = 1, Name = "BIF", Description = "Burundian Franc" });
                Insert.IntoTable("Currency").Row(new { Id =  17, Version = 1, Name = "BMD", Description = "Bermudan Dollar" });
                Insert.IntoTable("Currency").Row(new { Id =  18, Version = 1, Name = "BND", Description = "Brunei Dollar" });
                Insert.IntoTable("Currency").Row(new { Id =  19, Version = 1, Name = "BOB", Description = "Bolivian Boliviano" });
                Insert.IntoTable("Currency").Row(new { Id =  30, Version = 1, Name = "BRL", Description = "Brazilian Real" });
                Insert.IntoTable("Currency").Row(new { Id =  31, Version = 1, Name = "BSD", Description = "Bahamian Dollar" });
                Insert.IntoTable("Currency").Row(new { Id =  32, Version = 1, Name = "BTC", Description = "Bitcoin" });
                Insert.IntoTable("Currency").Row(new { Id =  33, Version = 1, Name = "BTN", Description = "Bhutanese Ngultrum" });
                Insert.IntoTable("Currency").Row(new { Id =  34, Version = 1, Name = "BWP", Description = "Botswanan Pula" });
                Insert.IntoTable("Currency").Row(new { Id =  35, Version = 1, Name = "BYR", Description = "Belarusian Ruble" });
                Insert.IntoTable("Currency").Row(new { Id =  36, Version = 1, Name = "BZD", Description = "Belize Dollar" });
                Insert.IntoTable("Currency").Row(new { Id =  37, Version = 1, Name = "CAD", Description = "Canadian Dollar" });
                Insert.IntoTable("Currency").Row(new { Id =  38, Version = 1, Name = "CDF", Description = "Congolese Franc" });
                Insert.IntoTable("Currency").Row(new { Id =  39, Version = 1, Name = "CHF", Description = "Swiss Franc" });
                Insert.IntoTable("Currency").Row(new { Id =  40, Version = 1, Name = "CLF", Description = "Chilean Unit of Account (UF)" });
                Insert.IntoTable("Currency").Row(new { Id =  41, Version = 1, Name = "CLP", Description = "Chilean Peso" });
                Insert.IntoTable("Currency").Row(new { Id =  42, Version = 1, Name = "CNY", Description = "Chinese Yuan" });
                Insert.IntoTable("Currency").Row(new { Id =  43, Version = 1, Name = "COP", Description = "Colombian Peso" });
                Insert.IntoTable("Currency").Row(new { Id =  44, Version = 1, Name = "CRC", Description = "Costa Rican Colón" });
                Insert.IntoTable("Currency").Row(new { Id =  45, Version = 1, Name = "CUP", Description = "Cuban Peso" });
                Insert.IntoTable("Currency").Row(new { Id =  46, Version = 1, Name = "CVE", Description = "Cape Verdean Escudo" });
                Insert.IntoTable("Currency").Row(new { Id =  47, Version = 1, Name = "CZK", Description = "Czech Republic Koruna" });
                Insert.IntoTable("Currency").Row(new { Id =  48, Version = 1, Name = "DJF", Description = "Djiboutian Franc" });
                Insert.IntoTable("Currency").Row(new { Id =  49, Version = 1, Name = "DKK", Description = "Danish Krone" });
                Insert.IntoTable("Currency").Row(new { Id =  50, Version = 1, Name = "DOP", Description = "Dominican Peso" });
                Insert.IntoTable("Currency").Row(new { Id =  51, Version = 1, Name = "DZD", Description = "Algerian Dinar" });
                Insert.IntoTable("Currency").Row(new { Id =  52, Version = 1, Name = "EEK", Description = "Estonian Kroon" });
                Insert.IntoTable("Currency").Row(new { Id =  53, Version = 1, Name = "EGP", Description = "Egyptian Pound" });
                Insert.IntoTable("Currency").Row(new { Id =  54, Version = 1, Name = "ETB", Description = "Ethiopian Birr" });
                Insert.IntoTable("Currency").Row(new { Id =  55, Version = 1, Name = "EUR", Description = "Euro" });
                Insert.IntoTable("Currency").Row(new { Id =  56, Version = 1, Name = "FJD", Description = "Fijian Dollar" });
                Insert.IntoTable("Currency").Row(new { Id =  57, Version = 1, Name = "FKP", Description = "Falkland Islands Pound" });
                Insert.IntoTable("Currency").Row(new { Id =  58, Version = 1, Name = "GBP", Description = "British Pound Sterling" });
                Insert.IntoTable("Currency").Row(new { Id =  59, Version = 1, Name = "GEL", Description = "Georgian Lari" });
                Insert.IntoTable("Currency").Row(new { Id =  60, Version = 1, Name = "GHS", Description = "Ghanaian Cedi" });
                Insert.IntoTable("Currency").Row(new { Id =  61, Version = 1, Name = "GIP", Description = "Gibraltar Pound" });
                Insert.IntoTable("Currency").Row(new { Id =  62, Version = 1, Name = "GMD", Description = "Gambian Dalasi" });
                Insert.IntoTable("Currency").Row(new { Id =  63, Version = 1, Name = "GNF", Description = "Guinean Franc" });
                Insert.IntoTable("Currency").Row(new { Id =  64, Version = 1, Name = "GTQ", Description = "Guatemalan Quetzal" });
                Insert.IntoTable("Currency").Row(new { Id =  65, Version = 1, Name = "GYD", Description = "Guyanaese Dollar" });
                Insert.IntoTable("Currency").Row(new { Id =  66, Version = 1, Name = "HKD", Description = "Hong Kong Dollar" });
                Insert.IntoTable("Currency").Row(new { Id =  67, Version = 1, Name = "HNL", Description = "Honduran Lempira" });
                Insert.IntoTable("Currency").Row(new { Id =  68, Version = 1, Name = "HRK", Description = "Croatian Kuna" });
                Insert.IntoTable("Currency").Row(new { Id =  69, Version = 1, Name = "HTG", Description = "Haitian Gourde" });
                Insert.IntoTable("Currency").Row(new { Id =  70, Version = 1, Name = "HUF", Description = "Hungarian Forint" });
                Insert.IntoTable("Currency").Row(new { Id =  71, Version = 1, Name = "IDR", Description = "Indonesian Rupiah" });
                Insert.IntoTable("Currency").Row(new { Id =  72, Version = 1, Name = "ILS", Description = "Israeli New Sheqel" });
                Insert.IntoTable("Currency").Row(new { Id =  73, Version = 1, Name = "INR", Description = "Indian Rupee" });
                Insert.IntoTable("Currency").Row(new { Id =  74, Version = 1, Name = "IQD", Description = "Iraqi Dinar" });
                Insert.IntoTable("Currency").Row(new { Id =  75, Version = 1, Name = "IRR", Description = "Iranian Rial" });
                Insert.IntoTable("Currency").Row(new { Id =  76, Version = 1, Name = "ISK", Description = "Icelandic Króna" });
                Insert.IntoTable("Currency").Row(new { Id =  77, Version = 1, Name = "JEP", Description = "Jersey Pound" });
                Insert.IntoTable("Currency").Row(new { Id =  78, Version = 1, Name = "JMD", Description = "Jamaican Dollar" });
                Insert.IntoTable("Currency").Row(new { Id =  79, Version = 1, Name = "JOD", Description = "Jordanian Dinar" });
                Insert.IntoTable("Currency").Row(new { Id =  80, Version = 1, Name = "JPY", Description = "Japanese Yen" });
                Insert.IntoTable("Currency").Row(new { Id =  81, Version = 1, Name = "KES", Description = "Kenyan Shilling" });
                Insert.IntoTable("Currency").Row(new { Id =  82, Version = 1, Name = "KGS", Description = "Kyrgystani Som" });
                Insert.IntoTable("Currency").Row(new { Id =  83, Version = 1, Name = "KHR", Description = "Cambodian Riel" });
                Insert.IntoTable("Currency").Row(new { Id =  84, Version = 1, Name = "KMF", Description = "Comorian Franc" });
                Insert.IntoTable("Currency").Row(new { Id =  85, Version = 1, Name = "KPW", Description = "North Korean Won" });
                Insert.IntoTable("Currency").Row(new { Id =  86, Version = 1, Name = "KRW", Description = "South Korean Won" });
                Insert.IntoTable("Currency").Row(new { Id =  87, Version = 1, Name = "KWD", Description = "Kuwaiti Dinar" });
                Insert.IntoTable("Currency").Row(new { Id =  88, Version = 1, Name = "KYD", Description = "Cayman Islands Dollar" });
                Insert.IntoTable("Currency").Row(new { Id =  89, Version = 1, Name = "KZT", Description = "Kazakhstani Tenge" });
                Insert.IntoTable("Currency").Row(new { Id =  90, Version = 1, Name = "LAK", Description = "Laotian Kip" });
                Insert.IntoTable("Currency").Row(new { Id =  91, Version = 1, Name = "LBP", Description = "Lebanese Pound" });
                Insert.IntoTable("Currency").Row(new { Id =  92, Version = 1, Name = "LKR", Description = "Sri Lankan Rupee" });
                Insert.IntoTable("Currency").Row(new { Id =  93, Version = 1, Name = "LRD", Description = "Liberian Dollar" });
                Insert.IntoTable("Currency").Row(new { Id =  94, Version = 1, Name = "LSL", Description = "Lesotho Loti" });
                Insert.IntoTable("Currency").Row(new { Id =  95, Version = 1, Name = "LTL", Description = "Lithuanian Litas" });
                Insert.IntoTable("Currency").Row(new { Id =  96, Version = 1, Name = "LVL", Description = "Latvian Lats" });
                Insert.IntoTable("Currency").Row(new { Id =  97, Version = 1, Name = "LYD", Description = "Libyan Dinar" });
                Insert.IntoTable("Currency").Row(new { Id =  98, Version = 1, Name = "MAD", Description = "Moroccan Dirham" });
                Insert.IntoTable("Currency").Row(new { Id =  99, Version = 1, Name = "MDL", Description = "Moldovan Leu" });
                Insert.IntoTable("Currency").Row(new { Id = 100, Version = 1, Name = "MGA", Description = "Malagasy Ariary" });
                Insert.IntoTable("Currency").Row(new { Id = 101, Version = 1, Name = "MKD", Description = "Macedonian Denar" });
                Insert.IntoTable("Currency").Row(new { Id = 102, Version = 1, Name = "MMK", Description = "Myanma Kyat" });
                Insert.IntoTable("Currency").Row(new { Id = 103, Version = 1, Name = "MNT", Description = "Mongolian Tugrik" });
                Insert.IntoTable("Currency").Row(new { Id = 104, Version = 1, Name = "MOP", Description = "Macanese Pataca" });
                Insert.IntoTable("Currency").Row(new { Id = 105, Version = 1, Name = "MRO", Description = "Mauritanian Ouguiya" });
                Insert.IntoTable("Currency").Row(new { Id = 106, Version = 1, Name = "MTL", Description = "Maltese Lira" });
                Insert.IntoTable("Currency").Row(new { Id = 107, Version = 1, Name = "MUR", Description = "Mauritian Rupee" });
                Insert.IntoTable("Currency").Row(new { Id = 108, Version = 1, Name = "MVR", Description = "Maldivian Rufiyaa" });
                Insert.IntoTable("Currency").Row(new { Id = 109, Version = 1, Name = "MWK", Description = "Malawian Kwacha" });
                Insert.IntoTable("Currency").Row(new { Id = 110, Version = 1, Name = "MXN", Description = "Mexican Peso" });
                Insert.IntoTable("Currency").Row(new { Id = 111, Version = 1, Name = "MYR", Description = "Malaysian Ringgit" });
                Insert.IntoTable("Currency").Row(new { Id = 112, Version = 1, Name = "MZN", Description = "Mozambican Metical" });
                Insert.IntoTable("Currency").Row(new { Id = 113, Version = 1, Name = "NAD", Description = "Namibian Dollar" });
                Insert.IntoTable("Currency").Row(new { Id = 114, Version = 1, Name = "NGN", Description = "Nigerian Naira" });
                Insert.IntoTable("Currency").Row(new { Id = 115, Version = 1, Name = "NIO", Description = "Nicaraguan Córdoba" });
                Insert.IntoTable("Currency").Row(new { Id = 116, Version = 1, Name = "NOK", Description = "Norwegian Krone" });
                Insert.IntoTable("Currency").Row(new { Id = 117, Version = 1, Name = "NPR", Description = "Nepalese Rupee" });
                Insert.IntoTable("Currency").Row(new { Id = 118, Version = 1, Name = "NZD", Description = "New Zealand Dollar" });
                Insert.IntoTable("Currency").Row(new { Id = 119, Version = 1, Name = "OMR", Description = "Omani Rial" });
                Insert.IntoTable("Currency").Row(new { Id = 130, Version = 1, Name = "PAB", Description = "Panamanian Balboa" });
                Insert.IntoTable("Currency").Row(new { Id = 131, Version = 1, Name = "PEN", Description = "Peruvian Nuevo Sol" });
                Insert.IntoTable("Currency").Row(new { Id = 132, Version = 1, Name = "PGK", Description = "Papua New Guinean Kina" });
                Insert.IntoTable("Currency").Row(new { Id = 133, Version = 1, Name = "PHP", Description = "Philippine Peso" });
                Insert.IntoTable("Currency").Row(new { Id = 134, Version = 1, Name = "PKR", Description = "Pakistani Rupee" });
                Insert.IntoTable("Currency").Row(new { Id = 135, Version = 1, Name = "PLN", Description = "Polish Zloty" });
                Insert.IntoTable("Currency").Row(new { Id = 136, Version = 1, Name = "PYG", Description = "Paraguayan Guarani" });
                Insert.IntoTable("Currency").Row(new { Id = 137, Version = 1, Name = "QAR", Description = "Qatari Rial" });
                Insert.IntoTable("Currency").Row(new { Id = 138, Version = 1, Name = "RON", Description = "Romanian Leu" });
                Insert.IntoTable("Currency").Row(new { Id = 139, Version = 1, Name = "RSD", Description = "Serbian Dinar" });
                Insert.IntoTable("Currency").Row(new { Id = 140, Version = 1, Name = "RUB", Description = "Russian Ruble" });
                Insert.IntoTable("Currency").Row(new { Id = 141, Version = 1, Name = "RWF", Description = "Rwandan Franc" });
                Insert.IntoTable("Currency").Row(new { Id = 142, Version = 1, Name = "SAR", Description = "Saudi Riyal" });
                Insert.IntoTable("Currency").Row(new { Id = 143, Version = 1, Name = "SBD", Description = "Solomon Islands Dollar" });
                Insert.IntoTable("Currency").Row(new { Id = 144, Version = 1, Name = "SCR", Description = "Seychellois Rupee" });
                Insert.IntoTable("Currency").Row(new { Id = 145, Version = 1, Name = "SDG", Description = "Sudanese Pound" });
                Insert.IntoTable("Currency").Row(new { Id = 146, Version = 1, Name = "SEK", Description = "Swedish Krona" });
                Insert.IntoTable("Currency").Row(new { Id = 147, Version = 1, Name = "SGD", Description = "Singapore Dollar" });
                Insert.IntoTable("Currency").Row(new { Id = 148, Version = 1, Name = "SHP", Description = "Saint Helena Pound" });
                Insert.IntoTable("Currency").Row(new { Id = 149, Version = 1, Name = "SLL", Description = "Sierra Leonean Leone" });
                Insert.IntoTable("Currency").Row(new { Id = 150, Version = 1, Name = "SOS", Description = "Somali Shilling" });
                Insert.IntoTable("Currency").Row(new { Id = 151, Version = 1, Name = "SRD", Description = "Surinamese Dollar" });
                Insert.IntoTable("Currency").Row(new { Id = 152, Version = 1, Name = "STD", Description = "São Tomé and Príncipe Dobra" });
                Insert.IntoTable("Currency").Row(new { Id = 153, Version = 1, Name = "SVC", Description = "Salvadoran Colón" });
                Insert.IntoTable("Currency").Row(new { Id = 154, Version = 1, Name = "SYP", Description = "Syrian Pound" });
                Insert.IntoTable("Currency").Row(new { Id = 155, Version = 1, Name = "SZL", Description = "Swazi Lilangeni" });
                Insert.IntoTable("Currency").Row(new { Id = 156, Version = 1, Name = "THB", Description = "Thai Baht" });
                Insert.IntoTable("Currency").Row(new { Id = 157, Version = 1, Name = "TJS", Description = "Tajikistani Somoni" });
                Insert.IntoTable("Currency").Row(new { Id = 158, Version = 1, Name = "TMT", Description = "Turkmenistani Manat" });
                Insert.IntoTable("Currency").Row(new { Id = 159, Version = 1, Name = "TND", Description = "Tunisian Dinar" });
                Insert.IntoTable("Currency").Row(new { Id = 160, Version = 1, Name = "TOP", Description = "Tongan Paʻanga" });
                Insert.IntoTable("Currency").Row(new { Id = 161, Version = 1, Name = "TRY", Description = "Turkish Lira" });
                Insert.IntoTable("Currency").Row(new { Id = 162, Version = 1, Name = "TTD", Description = "Trinidad and Tobago Dollar" });
                Insert.IntoTable("Currency").Row(new { Id = 163, Version = 1, Name = "TWD", Description = "New Taiwan Dollar" });
                Insert.IntoTable("Currency").Row(new { Id = 164, Version = 1, Name = "TZS", Description = "Tanzanian Shilling" });
                Insert.IntoTable("Currency").Row(new { Id = 165, Version = 1, Name = "UAH", Description = "Ukrainian Hryvnia" });
                Insert.IntoTable("Currency").Row(new { Id = 166, Version = 1, Name = "UGX", Description = "Ugandan Shilling" });
                Insert.IntoTable("Currency").Row(new { Id = 167, Version = 1, Name = "USD", Description = "United States Dollar" });
                Insert.IntoTable("Currency").Row(new { Id = 168, Version = 1, Name = "UYU", Description = "Uruguayan Peso" });
                Insert.IntoTable("Currency").Row(new { Id = 169, Version = 1, Name = "UZS", Description = "Uzbekistan Som" });
                Insert.IntoTable("Currency").Row(new { Id = 170, Version = 1, Name = "VEF", Description = "Venezuelan Bolívar Fuerte" });
                Insert.IntoTable("Currency").Row(new { Id = 171, Version = 1, Name = "VND", Description = "Vietnamese Dong" });
                Insert.IntoTable("Currency").Row(new { Id = 172, Version = 1, Name = "VUV", Description = "Vanuatu Vatu" });
                Insert.IntoTable("Currency").Row(new { Id = 173, Version = 1, Name = "WST", Description = "Samoan Tala" });
                Insert.IntoTable("Currency").Row(new { Id = 174, Version = 1, Name = "XAF", Description = "CFA Franc BEAC" });
                Insert.IntoTable("Currency").Row(new { Id = 175, Version = 1, Name = "XAG", Description = "Silver (troy ounce)" });
                Insert.IntoTable("Currency").Row(new { Id = 176, Version = 1, Name = "XAU", Description = "Gold (troy ounce)" });
                Insert.IntoTable("Currency").Row(new { Id = 177, Version = 1, Name = "XCD", Description = "East Caribbean Dollar" });
                Insert.IntoTable("Currency").Row(new { Id = 178, Version = 1, Name = "XDR", Description = "Special Drawing Rights" });
                Insert.IntoTable("Currency").Row(new { Id = 179, Version = 1, Name = "XOF", Description = "CFA Franc BCEAO" });
                Insert.IntoTable("Currency").Row(new { Id = 180, Version = 1, Name = "XPF", Description = "CFP Franc" });
                Insert.IntoTable("Currency").Row(new { Id = 181, Version = 1, Name = "YER", Description = "Yemeni Rial" });
                Insert.IntoTable("Currency").Row(new { Id = 182, Version = 1, Name = "ZAR", Description = "South African Rand" });
                Insert.IntoTable("Currency").Row(new { Id = 183, Version = 1, Name = "ZMK", Description = "Zambian Kwacha (pre-2013)" });
                Insert.IntoTable("Currency").Row(new { Id = 184, Version = 1, Name = "ZMW", Description = "Zambian Kwacha" });
                Insert.IntoTable("Currency").Row(new { Id = 185, Version = 1, Name = "ZWL", Description = "Zimbabwean Dollar" });
            }
        }

        public override void Down()
        {
            // Remove corresponding key from HiLoTable table.
            Delete.FromTable("HiLoTable").Row(new { TableName = "Currency" });

            Delete.Table("Currency");
        }
    }
}
