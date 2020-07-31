using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cloud.Core.Extensions;
using Cloud.Core.Testing;
using FluentAssertions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Xunit;

namespace Cloud.Core.Tests
{
    [IsUnit]
    public class StringExtensionsTest
    {
        /// <summary>Verify content between placeholders is found as expected.</summary>
        [Theory]
        [InlineData("{{", "}}", "OBJECT1,OBJECT2,OBJECT3")]
        [InlineData("<<", ">>", "OBJECT4")]
        [InlineData("mollit", "est", " anim id ")]
        [InlineData("[[", ">>", "OBJECT5")]
        public void Test_String_FindBetweenDelimiters(string startDelimiter, string endDelimiter, string expected)
        {
            // Arrange
            var expectedResult = expected.Split(",").ToHashSet();
            var searchString = "Lorem ipsum dolor sit amet, {{OBJECT1}} adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, " +
                "quis nostrud exercitation {{OBJECT2}} laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore " +
                "eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa {{OBJECT3}} officia deserunt mollit anim id est <<OBJECT4>>. Duis aute irure dolor in reprehenderit in voluptate [[OBJECT5>>";

            // Act
            var results = searchString.FindBetweenDelimiters(startDelimiter, endDelimiter);

            // Assert
            results.Should().BeEquivalentTo(expectedResult);
            results.Count.Should().Be(expectedResult.Count);
        }

        /// <summary>Verify content between placeholders is found and substituted using a model expected.</summary>
        [Fact]
        public void Test_String_SubstitutePlaceholders_AnonymousObject()
        {
            // Arrange
            var searchString = "Lorem ipsum dolor sit amet, {{OBJECT1}} adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, " +
                               "quis nostrud exercitation {{OBJECT2}} laboris nisi ut aliquip ex ea commodo, sunt in culpa {{OBJECT3}} officia deserunt mollit anim id est <<OBJECT4>>. " +
                               "Duis aute irure dolor in reprehenderit in voluptate [[OBJECT5>>";

            var expectedResult = "Lorem ipsum dolor sit amet, ROB adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, " +
                                 "quis nostrud exercitation ROB laboris nisi ut aliquip ex ea commodo, sunt in culpa {{OBJECT3}} officia deserunt mollit anim id est <<OBJECT4>>. " +
                                 "Duis aute irure dolor in reprehenderit in voluptate [[OBJECT5>>";

            // Act
            var dictionary = new Dictionary<string, object>();
            dictionary.Add("Object1", "ROB");
            dictionary.Add("OBJECT2", "ROB");

            var result = searchString.SubstitutePlaceholders(dictionary);
            var keys = string.Join(",", result.PlaceholderKeys);

            // Assert
            result.SubstitutedContent.Should().BeEquivalentTo(expectedResult);
            result.ModelKeyValues.Should().BeEquivalentTo(new Dictionary<string, string>
            {
                { "object1", "ROB" },
                { "object2", "ROB" }
            });
            result.SubstitutedValueCount.Should().Be(2);
            keys.Should().BeEquivalentTo("OBJECT1,OBJECT2,OBJECT3");
        }

        /// <summary>Verify content between placeholders is found and substituted using a model expected.</summary>
        [Fact]
        public void Test_String_SubstitutePlaceholders_JObject()
        {
            // Arrange
            var searchString = "Lorem ipsum dolor sit amet, {{OBJECT1}} adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, " +
                               "quis nostrud exercitation {{OBJECT2}} laboris nisi ut aliquip ex ea commodo, sunt in culpa {{OBJECT3}} officia deserunt mollit anim id est <<OBJECT4>>. " +
                               "Duis aute irure dolor in reprehenderit in voluptate [[OBJECT5>>";

            var expectedResult = "Lorem ipsum dolor sit amet, ROB adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, " +
                                 "quis nostrud exercitation ROB laboris nisi ut aliquip ex ea commodo, sunt in culpa {{OBJECT3}} officia deserunt mollit anim id est <<OBJECT4>>. " +
                                 "Duis aute irure dolor in reprehenderit in voluptate [[OBJECT5>>";

            // Act
            string json = "{ \"Object1\":\"ROB\", \"OBJECT2\":\"ROB\" }";
            JToken outer = JToken.Parse(json);

            var result = searchString.SubstitutePlaceholders(outer);
            var keys = string.Join(",", result.PlaceholderKeys);

            // Assert
            result.SubstitutedContent.Should().BeEquivalentTo(expectedResult);
            result.ModelKeyValues.Should().BeEquivalentTo(new Dictionary<string, string>
            {
                { "object1", "ROB" },
                { "object2", "ROB" }
            });
            result.SubstitutedValueCount.Should().Be(2);
            keys.Should().BeEquivalentTo("OBJECT1,OBJECT2,OBJECT3");
        }

        /// <summary>Verify content between placeholders is found and substituted using a model expected.</summary>
        [Fact]
        public void Test_String_SubstitutePlaceholders_Dictionary()
        {
            // Arrange
            var searchString = "Lorem ipsum dolor sit amet, {{OBJECT1}} adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, " +
                               "quis nostrud exercitation {{OBJECT2}} laboris nisi ut aliquip ex ea commodo, sunt in culpa {{OBJECT3}} officia deserunt mollit anim id est <<OBJECT4>>. " +
                               "Duis aute irure dolor in reprehenderit in voluptate [[OBJECT5>>";

            var expectedResult = "Lorem ipsum dolor sit amet, ROB adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, " +
                                 "quis nostrud exercitation ROB laboris nisi ut aliquip ex ea commodo, sunt in culpa {{OBJECT3}} officia deserunt mollit anim id est <<OBJECT4>>. " +
                                 "Duis aute irure dolor in reprehenderit in voluptate [[OBJECT5>>";

            // Act
            var model = new { Object1 = "ROB", Object2 = "ROB" };
            var result = searchString.SubstitutePlaceholders(model, "{{", "}}");
            var keys = string.Join(",", result.PlaceholderKeys);

            // Assert
            result.SubstitutedContent.Should().BeEquivalentTo(expectedResult);
            result.ModelKeyValues.Should().BeEquivalentTo(new Dictionary<string, string>
            {
                { "object1", "ROB" },
                { "object2", "ROB" }
            });
            result.SubstitutedValueCount.Should().Be(2);
            keys.Should().BeEquivalentTo("OBJECT1,OBJECT2,OBJECT3");
        }

        /// <summary>Verify content between placeholders is found and substituted using a jtoken model and object model.</summary>
        [Fact]
        public void Test_String_SubstitutePlaceholders_DictionaryComplex()
        {
            // Arrange
            var searchString = "<!doctype html> <html lang='en'> <head> <meta charset='utf-8'> <meta charset='utf-8'> <meta http-equiv='X-UA-Compatible' content='IE=edge'> <title>Invoice</title> <style> html { font-family: sans-serif; -ms-text-size-adjust: 100%; -webkit-text-size-adjust: 100%; }  body { margin: 0; }  strong { font-weight: 700; }  h1 { font-size: 2em; margin: 0.67em 0; }  sup { font-size: 75%; line-height: 0; position: relative; vertical-align: baseline; top: -0.5em; }  img { border: 0; }  table { border-collapse: collapse; border-spacing: 0; }  td, th { padding: 0; }  html { font-size: 10px; -webkit-tap-highlight-color: #fff; }  body { font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif; font-size: 14px; line-height: 1.42857143; color: #333; background-color: #fff; }  img { vertical-align: middle; }  h1, h2, h3 { font-family: inherit; font-weight: 500; line-height: 1.1; color: inherit; }  h1, h2, h3 { margin-top: 20px; margin-bottom: 10px; }  h1 { font-size: 36px; }  h2 { font-size: 30px; }  h3 { font-size: 24px; }  p { margin: 0 0 5px; }  .text-right { text-align: right; }  .row { margin-right: -15px; margin-left: -15px; }  .col-xs-2, .col-xs-4, .col-xs-8, .col-xs-10, .col-xs-12 { position: relative; min-height: 1px; padding-right: 15px; padding-left: 15px; float: left; }  .col-xs-12 { width: 100%; }  .col-xs-10 { width: 83.33333333%; }  .col-xs-8 { width: 66.66666667%; }  .col-xs-4 { width: 33.33333333%; }  .col-xs-2 { width: 16.66666667%; }  table { background-color: transparent; }  th { text-align: left; }  .table { width: 100%; max-width: 100%; margin-bottom: 20px; }  .table > thead > tr > th, .table > tbody > tr > th, .table > tfoot > tr > th, .table > thead > tr > td, .table > tbody > tr > td, .table > tfoot > tr > td { padding: 8px; line-height: 1.42857143; vertical-align: top; border-top: 1px solid #ddd; }  .table > thead > tr > th { vertical-align: bottom; border-bottom: 2px solid #ddd; }  .table > caption + thead > tr:first-child > th, .table > colgroup + thead > tr:first-child > th, .table > thead:first-child > tr:first-child > th, .table > caption + thead > tr:first-child > td, .table > colgroup + thead > tr:first-child > td, .table > thead:first-child > tr:first-child > td { border-top: 0; }  .table > tbody + tbody { border-top: 2px solid #ddd; }  .row:before, .row:after { display: table; content: ' '; }  .row:after { clear: both; }  body { width: 100%; height: 100%; margin: 0; padding: 0; font-family: 'Helvetica Neue', Helvetica, Arial, 'Lucida Grande', sans-serif; font-weight: 400; color: #000; font-size: 8pt; line-height: 1.5; }  * { box-sizing: border-box; -moz-box-sizing: border-box; }  .page { width: 210mm; min-height: 297mm; padding: 10mm 20mm; margin: 10mm auto; position: relative; background: #fff; box-shadow: 0 0 5px #d2d2d2; }  h1 { font-size: 18pt; font-weight: 500; margin: 30px 0 20px 0; }  h2 { font-size: 12pt; font-weight: 700; text-transform: uppercase; }  h3 { font-size: 8pt; font-weight: 700; text-transform: uppercase; margin: 0 0 10px 0; }  .logo { position: absolute; left: 20mm; width: 50mm; height: 20mm; }  .addresses { display: -webkit-box; display: -moz-box; display: -webkit-flex; display: -ms-flexbox; display: flex; }  .addresses .address-col { line-height: 1.5; padding: 0; min-width: 28%; max-width: 35%; display: inline-block; -ms-flex: 1 1 auto; flex: 1 1 auto; }  .addresses .address-divider { border-left: 1px solid #b0b0b0; display: block; padding: 0 4mm; }  .date-info { text-align: right; }  .date-info p { font-size: 9pt; line-height: 1.2; margin: 0 0 0.9rem 0; }  .registration-footer { border-top: 1px solid #b0b0b0; display: block; width: 100%; padding: 15px 15px 0 15px; }  .reason-count { margin: 0 5px; }  .table-invoice { margin-top: 10px; border-bottom: 1px solid #000; }  .table-invoice thead tr th { -webkit-print-color-adjust: exact !important; background-color: #000; color: #fff; border-bottom: 0 none !important; vertical-align: middle !important; padding: 10px 15px; }  .table-invoice thead tr th:nth-child(6) { width: 12%; padding: 0; padding-right: 10px; }  .table-invoice div.incl-vat-text { font-size: 10px; }  .table-invoice tbody tr td { padding: 5px 15px; border-color: #b0b0b0; }  .table-invoice tbody tr:first-of-type td { border-top: 0 none; }  .table-invoice.side-borders { border-bottom: 0 none; }  .table-invoice.side-borders thead tr th { border: 1px solid #ccc; }  .table-invoice.side-borders tbody tr td { border: 1px solid #ccc; }  .table-totals tbody tr td { padding: 0 0 10px 0; border: 0 none; }  .text-watermark { -webkit-print-color-adjust: exact !important; position: absolute; top: 40%; left: 0; width: 210mm; text-align: center; font-size: 110pt; color: #e6e6e6 !important; -ms-transform: rotate(-48deg); -webkit-transform: rotate(-48deg); transform: rotate(-48deg); }  @@media print { html, body { width: 210mm; height: auto; }  .page { margin: 0; border: initial; border-radius: initial; width: initial; min-height: initial; box-shadow: initial; background: initial; page-break-after: always; }  .table-invoice { border-bottom: 1px solid #000; }  .table-invoice thead tr th { -webkit-print-color-adjust: exact !important; background-color: #000 !important; color: #fff !important; }  .text-watermark { -webkit-print-color-adjust: exact !important; } }  .pr-0, .px-0 { padding-right: 0 !important; }  .pl-0, .px-0 { padding-left: 0 !important; }  .text-right { text-align: right !important; }  .m-0 { margin: 0 !important; }  .mb-0, .my-0 { margin-bottom: 0 !important; }  .mb-1, .my-1 { margin-bottom: 0.25rem !important; }  .mb-2, .my-2 { margin-bottom: 0.5rem !important; }  .mt-3, .my-3 { margin-top: 1rem !important; }  .mb-3, .my-3 { margin-bottom: 1rem !important; } </style> </head> <body> <div id='invoice' class='page'>  <img src='https://eswlogisticsprod.blob.core.windows.net/images/levis.svg' class='logo' alt='Levis' />  <div class='row px-0'> <div class='col-xs-12 text-right text-light'> Levi's<sup>&reg;</sup> Customer Services<br /> P: 00800 53847 501<br /> https://www.levi.com/GB/en_GB/contact<br /> eShop: levi.com </div> </div>  <div class='row px-0'> <div class='col-xs-12 text-right'> <h1>Invoice</h1> </div> </div>  <div class='row px-0'> <div class='col-xs-10'> <div class='addresses'> <div class='address-col'> <h3><strong>BILL TO</strong></h3> <p> {{BillingAddress:FirstName}}&nbsp;{{BillingAddress:LastName}}<br /> {{BillingAddress:Address1}}<br /> {{BillingAddress:Address2}}<br /> {{BillingAddress:Address3}}<br /> {{BillingAddress:City}}<br /> {{BillingAddress:PostalCode}}<br /> {{BillingAddress:Country}} </p> <p class='mb-0'><strong>P:</strong> {{BillingAddress:Telephone}}</p> </div>  <div class='address-divider'></div>  <div class='address-col'> <h3><strong>SHIP TO</strong></h3> <p> {{ShippingAddress:FirstName}}&nbsp;{{ShippingAddress:LastName}}<br /> {{ShippingAddress:Address1}}<br /> {{ShippingAddress:Address2}}<br /> {{ShippingAddress:Address3}}<br /> {{ShippingAddress:City}}<br /> {{ShippingAddress:PostalCode}}<br /> {{ShippingAddress:Country}} </p> <p class='mb-0'><strong>P:</strong> {{ShippingAddress:Telephone}}</p> </div>  <div class='address-divider'></div>  <div class='address-col'> <h3><strong>SOLD BY</strong></h3> <p> Levi Strauss &amp; Co Europe<br /> Airport Plaza - Rio Building<br /> Leonardo Da Vincilaan 19<br /> 1831 Diegem<br /> Belguim </p> </div> </div> </div> <div class='col-xs-2 date-info'> <p> <strong>Order No.</strong><br /> {{ORDERNUMBER}} </p> <p> <strong>Order Date</strong><br /> {{ORDERDATE}} </p> <p> <strong>Invoice No.</strong><br /> {{INVOICENUMBER}} </p> <p> <strong>Issue Date</strong><br /> {{ORDERDATE}} </p> </div> </div>  <div class='row px-0'> <div class='col-xs-10'> <p class='mt-3'><strong>Dear Customer,</strong></p> <p>Thank you for shopping on levi.com. </p> <p>If your order comes in multiple shipments, each invoice will show the initial shipping charge but you will only be charged once for your order.</p> <p>If you have any questions relating to your purchase, please visit us at levi.com. Please check our Help Centre section for further guidance.</p> <p>In case of returns, please note that you have 28 days to return any item.</p> <p>We look forward to seeing you next time.</p> <p class='mb-0'>Levi's<sup>&reg;</sup> Customer Service</p> </div> </div>  <div class='row px-0'> <div class='col-xs-12 px-0'> <p class='text-right'><strong>VAT amount and price are in: GBP</strong></p> <table class='table table-invoice'> <thead> <tr> <th>Product Code</th> <th>Description</th> <th>Size</th> <th>Qty</th> <th>VAT%</th> <th class='text-right'> Line Total <div class='incl-vat-text'>(Inc VAT)</div> </th> </tr> </thead> <tbody> <tr> <td>ABC123</td> <td> Umbrella1 </td> <td>100cm x 100cm</td> <td>1</td> <td class='text-right'>19.99</td> <td class='text-right'>60.00</td> </tr> <tr> <td>ZXY987</td> <td> Umbrella2 </td> <td>100cm x 100cm</td> <td>1</td> <td class='text-right'>19.99</td> <td class='text-right'>60.00</td> </tr> </tbody> </table> </div> </div>  <div class='row px-0'> <div class='col-xs-8'> <h3 class='mb-3'>Payment Info</h3> <p class='mb-1'><strong>Customer No.</strong> {{CUSTOMERNUMBER}}</p> <p class='mb-1'><strong>Payment Method.</strong> {{PAYMENTMETHOD}}</p> </div> <div class='col-xs-4'> <table class='table table-totals'> <tbody> <tr> <td><h3 class='m-0'>SUBTOTAL</h3></td> <td class='text-right'>{{CURRENCYSYMBOL}}{{SUBTOTAL}}</td> </tr> <tr> <td><strong>SHIPPING</strong></td> <td class='text-right'>{{CURRENCYSYMBOL}}{{SHIPPINGTOTAL}}</td> </tr> <tr> <td><strong>VAT</strong></td> <td class='text-right'>{{CURRENCYSYMBOL}}{{VAT}}</td> </tr> <tr> <td><h2 class='m-0'>TOTAL</h2></td> <td class='text-right'><h2 class='m-0'>{{CURRENCYSYMBOL}} {{TOTAL}}</h2></td> </tr> </tbody> </table> </div> </div>  <div class='row px-0'> <div class='col-xs-12 px-0'> <div class='registration-footer'> <p class='mb-2'> <strong>Vat Registration Number:</strong> GB 869 111 411 </p> <p class='mb-2'><strong>Company Registration Number:</strong> 0424 656 991</p> <p class='mb-2'><strong>Place Of Registration:</strong> 1831 Diegem, Belguim</p> <p class='mb-2'><strong>Registered office address:</strong>  Leonardo Da Vincilaan 19, 1831 Diegem</p> </div> </div> </div> </div> </body> </html>";            
            string json = "{ \"OrderNumber\": \"Order12345\", \"OrderDate\": \"31-07-2020\", \"InvoiceNumber\": \"Inv12345\", \"BillingAddress\":{ \"FirstName\": \"Robert\", \"LastName\": \"McCabe\", \"Address1\": \"24 Someroad\", \"Address2\": \"Somelane\", \"Address3\": \"SomePlace\", \"City\": \"Belfast\", \"PostalCode\": \"BT456YT\", \"Country\": \"N.Ireland\", \"Telephone\": \"0712345678\" }, \"ShippingAddress\":{ \"FirstName\": \"Robert\", \"LastName\": \"McCabe\", \"Address1\": \"24 Someroad\", \"Address2\": \"Somelane\", \"Address3\": \"SomePlace\", \"City\": \"Belfast\", \"PostalCode\": \"BT456YT\", \"Country\": \"N.Ireland\", \"Telephone\": \"0712345678\" }, \"CustomerNumber\":\"Cust12345\", \"PaymentMethod\":\"Visa Debit\", \"CurrencySymbol\": \"£\", \"Subtotal\": \"60.00\", \"ShippingTotal\":\"20.00\", \"Vat\":\"20.00\", \"Total\": \"100.00\" } ";//"{ \"title\":\"test\", \"other\":true, \"arr\":[1,2,3,4,5] }";

            // Act
            JToken outer = JToken.Parse(json.ToString());
            var model = JsonConvert.DeserializeObject<dynamic>(json);
            var jTokenResult = searchString.SubstitutePlaceholders(outer);
            var objectResult = searchString.SubstitutePlaceholders((object)model);

            // Assert
            jTokenResult.Should().NotBeNull();
            jTokenResult.ModelKeyValues.Count.Should().Be(28);
            jTokenResult.PlaceholderKeys.Count.Should().Be(32);
            jTokenResult.SubstitutedValueCount.Should().Be(32);
            objectResult.Should().NotBeNull();
            objectResult.ModelKeyValues.Count.Should().Be(28);
            objectResult.PlaceholderKeys.Count.Should().Be(32);
            objectResult.SubstitutedValueCount.Should().Be(32);
        }

        /// <summary>Ensure non-alphanumeric characters are removed from the given string.</summary>
        [Fact]
        public void Test_String_CleanContent()
        {
            // Arrange
            var source = $"This\n is  {Environment.NewLine}   a   cleaned string ! - £$$%(*71 ";

            // Act
            var replaced = source.RemoveNonAlphanumericCharacters();

            // Assert
            replaced.Should().Be("This is a cleaned string 71 ");
        }

        /// <summary>Ensure multiple matches of a specified string are removed as expected.</summary>
        [Fact]
        public void Test_String_RemoveMultiple()
        {
            // Arrange
            var source = "my test string is here";

            // Act
            var replaced = source.RemoveMultiple("is", "test");

            // Assert
            replaced.Should().Be("my  string  here");
        }

        /// <summary>Ensure multiple matches of a specified string are removed as expected and non matches are ignored.</summary>
        [Fact]
        public void Test_String_ReplaceMultiple()
        {
            // Arrange
            var source = "my test string is here";

            // Act
            var replaced = source.ReplaceMultiple("text", "is", "test");

            // Assert
            replaced.Should().Be("my text string text here");
        }

        /// <summary>Ensure default if null or emtype sets the default value as expected.</summary>
        [Fact]
        public void Test_String_DefaultIfNullOrEmtpy()
        {
            // Arrange
            var original = "";

            // Act
            original = original.DefaultIfNullOrEmtpy("default");

            // Assert
            original.Should().Be("default");
        }

        /// <summary>Ensure multiple lines are created as expected.</summary>
        [Fact]
        public void Test_String_MultiLine()
        {
            // Arrange
            var multi = StringExtensions.MultiLine("lineone","linetwo");

            // Act
            int numLines = multi.Split('\n').Length;

            // Assert
            numLines.Should().Be(2);
        }

        /// <summary>Ensure substring by removing start and end results in expected.</summary>
        [Fact]
        public void Test_String_SubstringRemveStartAndEnd()
        {
            // Arrange
            var test = "MyTest,String.IsThis";

            // Act
            var result = test.Substring("MyTest,", ".IsThis");

            // Assert
            result.Should().Be("String");
            result = test.Substring("Test,", ".Is");
            result.Should().Be("String");
        }

        /// <summary>Ensure SetStringIfNullOrEmpty results in result expected.</summary>
        [Fact]
        public void Test_String_SetDefaultIfNullOrEmpty()
        {
            // Assert does not set default.
            "start".SetDefaultIfNullOrEmpty("default").Should().Be("start");

            // Assert null string defaults.
            ((string)null).SetDefaultIfNullOrEmpty("default").Should().Be("default");

            // Assert empty string defaults.
            "".SetDefaultIfNullOrEmpty("default").Should().Be("default");
        }

        /// <summary>Ensure characters are replaced as expected.</summary>
        [Theory]
        [InlineData("This is a test string", new [] { ' ' }, "Thisisateststring")]
        [InlineData("This,is;another|test string", new [] { ' ', ',', ';', '|' }, "Thisisanotherteststring")]
        public void Test_String_ReplaceChars(string value, char[] replaceChars, string expectedResult)
        {
            // Arrange and act.
            var result = value.ReplaceAll(replaceChars, string.Empty);

            // Assert.
            result.Should().Be(expectedResult);
        }

        /// <summary>Ensure size bytes results in expected.</summary>
        [Theory]
        [InlineData("This is a test string")]
        public void Test_String_GetSizeInBytes(string value)
        {
            // Arrange and act.
            var streamLen = value.ConvertToStream(Encoding.UTF8).Length;
            var size = value.GetSizeInBytes(Encoding.UTF8);

            // Assert.
            size.Should().Be(streamLen);
        }

        /// <summary>Ensure is null or empty gives the correct result as expected when null or empty is used.</summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Test_String_IsNullOrEmpty_Positive(string value)
        {
            Assert.True(value.IsNullOrEmpty());
        }

        /// <summary>Ensure is null or empty gives the correct result as expected when string is set.</summary>
        [Theory]
        [InlineData("TEST")]
        [InlineData(" ")]
        public void Test_String_IsNullOrEmpty_Negative(string value)
        {
            Assert.False(value.IsNullOrEmpty());
        }

        /// <summary>Ensure string converts to and from stream successfully.</summary>
        [Theory]
        [InlineData("TEST")]
        [InlineData(" TEST ")]
        public void Test_String_ToFromStreamConversion(string value)
        {
            // Arrange/Act
            MemoryStream toStream = value.ConvertToStream(Encoding.UTF8);
            var fromStream = toStream.ReadContents();

            // Assert
            Assert.Equal(value, fromStream);
        }

        /// <summary>Ensure is null or white space gives expected true result.</summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_String_IsNullOrWhiteSpace_Positive(string value)
        {
            // Act/Assert
            Assert.True(value.IsNullOrWhiteSpace());
        }

        /// <summary>Ensure is null or white space gives expected false result.</summary>
        [Theory]
        [InlineData("TEST")]
        [InlineData(" TEST ")]
        public void Test_String_IsNullOrWhiteSpace_Negative(string value)
        {
            // Act/Assert
            Assert.False(value.IsNullOrWhiteSpace());
        }

        /// <summary>Ensure exception is thrown if null or whitespace.</summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_String_ThrowIfNullOrWhiteSpace(string value)
        {
            // Act/Assert
            Assert.Throws<ArgumentNullException>(() => value.ThrowIfNullOrWhiteSpace());
        }

        /// <summary>Ensure space is added before each capital letter.</summary>
        [Fact]
        public void Test_String_AddSpaceBeforeCaps()
        {
            // Arrange
            string value = "thisTestString";
            string expected = "this Test String";

            // Act/Assert
            Assert.Equal(value.AddSpaceBeforeCaps(), expected);
        }

        /// <summary>Ensure null is returned when null string requests AddSpaceBefore caps.</summary>
        [Fact]
        public void Test_AddSpaceBeforeCaps_Null()
        {
            // Arrange
            string value = null;

            // Act/Assert
            Assert.Null(value.AddSpaceBeforeCaps());
        }

        /// <summary>Ensure invalid guid strings are returned as an empty guid.</summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData("thisisnotaguid")]
        public void Test_String_ToEmptyGuid(string value)
        {
            // Arrange/Act
            var result = value.ToGuid();

            // Assert
            Assert.Equal(Guid.Empty, result);
        }

        /// <summary>Ensure valid guid strings are returned as an expected guid.</summary>
        [Theory]
        [InlineData("E540A2B7-D1B3-4770-B7FE-E435DBBC9D64")]
        public void Test_String_ToGuid(string value)
        {
            // Arrange/Act
            var result = value.ToGuid();
            var expected = new Guid(value);

            // Assert
            Assert.Equal(expected, result);
        }

        /// <summary>Ensure comparison returns true when two string objects are compared for equivilence.</summary>
        [Theory]
        [InlineData("TEST1", "TEST1")]
        [InlineData("TEST2", "TEST2")]
        public void Test_IsEquivalentTo_Positive(string value, string compare)
        {
            // Act/Assert
            Assert.True(value.IsEquivalentTo(compare));
        }

        /// <summary>Ensure comparison returns false when two string objects are compared for equivilence.</summary>
        [Theory]
        [InlineData("TEST1", "TEST2")]
        [InlineData("TEST2", "TEST3")]
        public void Test_IsEquivalentTo_Negative(string value, string compare)
        {
            // Act/Assert
            Assert.False(value.IsEquivalentTo(compare));
        }

        /// <summary>Ensure string to int conversion works as expected.</summary>
        [Theory]
        [InlineData("1", 1)]
        [InlineData("2", 2)]
        public void Test_StringToInt32(string value, int expected)
        {
            // Act/Assert
            Assert.Equal(value.ToInt32(), expected);
        }

        /// <summary>Ensure string to int conversion throw exception when the string cannot be converted.</summary>
        [Theory]
        [InlineData("")]
        [InlineData("TEST")]
        public void Test_String_ToInt32WithException(string value)
        {
            // Act/Assert
            Assert.ThrowsAny<Exception>(() => value.ToInt32());
        }

        /// <summary>Ensure string to boolean conversion works as expected.</summary>
        [Theory]
        [InlineData("true", true)]
        [InlineData("false", false)]
        [InlineData("True", true)]
        [InlineData("False", false)]
        [InlineData("1", true)]
        [InlineData("0", false)]
        [InlineData("T", true)]
        [InlineData("F", false)]
        [InlineData("t", true)]
        [InlineData("f", false)]
        [InlineData("Y", true)]
        [InlineData("N", false)]
        [InlineData("anyOther", false)]
        public void Test_String_ToBoolean(string value, bool expected)
        {
            // Act/Assert
            Assert.Equal(value.ToBoolean(), expected);
        }

        /// <summary>Ensure string to boolean conversion throws exception when string is empty or null.</summary>
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_String_ToBooleanWithException(string value)
        {
            // Act/Assert
            Assert.ThrowsAny<Exception>(() => value.ToBoolean());
        }

        /// <summary>Ensure string contains equivalent returns true.</summary>
        [Theory]
        [InlineData("anyOther", "a")]
        [InlineData("anyOther", "any")]
        [InlineData("anyOther", "other")]
        [InlineData("anyOther", "Other")]
        public void Test_String_ContainsEquivalent(string value, string comparer)
        {
            // Act/Assert
            Assert.True(value.ContainsEquivalent(comparer));
        }

        /// <summary>Ensure string contains equivalent throws exception when empty is passed.</summary>
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_ContainsEquivalentWithException(string value)
        {
            // Act/Assert
            Assert.ThrowsAny<Exception>(() => value.ContainsEquivalent(null));
        }

        /// <summary>Ensure string starts with equivalent returns true when matched.</summary>
        [Theory]
        [InlineData("anyOther", "a")]
        [InlineData("anyOther", "any")]
        [InlineData("myTest", "m")]
        [InlineData("myTest", "myte")]
        public void Test_StartsWithEquivalent(string value, string comparer)
        {
            // Act/Assert
            Assert.True(value.StartsWithEquivalent(comparer));
        }

        /// <summary>Ensure string starts with equivalent throws exception when empty is passed.</summary>
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_StartsWithEquivalentWithException(string value)
        {
            // Act/Assert
            Assert.ThrowsAny<Exception>(() => value.StartsWithEquivalent(null));
        }

        /// <summary>Ensure string ends with equivalent returns true when end of string is matched.</summary>
        [Theory]
        [InlineData("anyOther", "r")]
        [InlineData("anyOther", "er")]
        [InlineData("myTest", "est")]
        [InlineData("myTest", "test")]
        public void Test_EndsWithEquivalent(string value, string comparer)
        {
            // Act/Assert
            Assert.True(value.EndsWithEquivalent(comparer));
        }

        /// <summary>Ensure string ends with equivalent throws exception when or empty is passed.</summary>
        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Test_EndsWithEquivalentWithException(string value)
        {
            // Act/Assert
            Assert.ThrowsAny<Exception>(() => value.EndsWithEquivalent(null));
        }
    }
}
