# USPS.AddressValidator
.NET library used to confirm that a given mailing address is correct per the USPS Coding Accuracy Support System.  
## Requirements  
   - A **USPS Web Tools USERID** from USPS. Register here: [https://registration.shippingapis.com/](https://registration.shippingapis.com/)
   - The USERID needs to be stored on your computer as an environment variable with the name **USPS_USERID**
## Build  
   - Clone and build in Visual Studio, or  
   - Use the **Developer Command Prompt for VS 2019** (Requires **Git** [https://git-scm.com/book/en/v2/Getting-Started-Installing-Git](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git))  
```bat
git clone https://github.com/nstevens1040/USPS.AddressValidator.git
cd USPS.AddressValidator
msbuild
```  
## Usage  
**CSharp**  
```cs
namespace ValidateMailingAddress
{
    using Usps;
    using System;
    class Program
    {
        static void Main(string[] args)
        {
            LookupAddress a = new LookupAddress()
            {
                Address2 = args[0],     // required - street number & street name
                Address1 = args[1],     // optional - extra identifiers such as APT 123
                City = args[2],         // required - full city name
                StateCode = args[3],    // required - two character state code. ex. CA, IL, NY, FL, etc...
                Zip5 = args[4],         // required - five digit zip code
                Zip4 = args[5]          // optional - four digit zip code. ex. 60660-5121. {60660} is required 5 digit zip & {5121} is optional 4 digit zip.
            };
            a.Validate();
            Console.Write(a.Results.ValidatedAddress);
            Console.Write(a.Results.XmlResponseString);
        }
    }
}
```  
**PowerShell**  
```ps1
Add-Type -Path "C:\your\path\to\build\directory\USPS.AddressValidator\USPS.AddressValidator\bin\Debug\USPS.AddressValidator.dll"
$a = [Usps.LookupAddress]@{
    Address2="1060 W ADDISON ST";
    City="CHICAGO";
    StateCode="IL";
    Zip5="60613";
    Zip4="4566";
}
$a.Validate()

# then you can parse through the Usps.ReturnObject

$a.Results.ValidatedAddress
$a.Results.XmlResponseString
$a.Results.ResponseXml
$a.Results.ValidAddressObject
$a.Results.ValidAddressObject
```  
