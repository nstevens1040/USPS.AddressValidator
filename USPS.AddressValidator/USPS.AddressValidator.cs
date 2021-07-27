namespace Usps
{
    using System;
    using System.Xml;
    using System.Web;
    using System.Net;
    using System.Reflection;
    using System.IO;
    using System.Text;
    public class ValidAddress
    {
        public string Address2
        {
            get;
            set;
        }
        public string Address1
        {
            get;
            set;
        }
        public string City
        {
            get;
            set;
        }
        public string State
        {
            get;
            set;
        }
        public string FiveDigitZip
        {
            get;
            set;
        }
        public string FourDigitZip
        {
            get;
            set;
        }
    }
    public class ReturnObject
    {
        public string ValidatedAddress
        {
            get;
            set;
        }
        public XmlDocument ResponseXml
        {
            get;
            set;
        }
        public ValidAddress ValidAddressObject
        {
            get;
            set;
        }
        public string XmlResponseString
        {
            get;
            set;
        }
    }
    public class LookupAddress
    {
        public string Address2
        {
            get;
            set;
        }
        public string Address1
        {
            get;
            set;
        }
        public string City
        {
            get;
            set;
        }
        public string StateCode
        {
            get;
            set;
        }
        public string Zip5
        {
            get;
            set;
        }
        public string Zip4
        {
            get;
            set;
        }
        public ReturnObject Results
        {
            get;
            set;
        }
        public void Validate()
        {
            if(String.IsNullOrEmpty(this.Address2) | String.IsNullOrEmpty(this.City) | String.IsNullOrEmpty(this.StateCode) | String.IsNullOrEmpty(this.Zip5))
            {
                throw (new ArgumentNullException());
            } else
            {
                this.Results = ValidationRequest.Send(this);
            }
        }
    }
    public class Address
    {
        private XmlDocument response
        {
            get;
            set;
        }
        private XmlElement xml
        {
            get;
            set;
        }
        private string AddressLine2
        {
            get;
            set;
        }
        private string AddressLine1
        {
            get;
            set;
        }
        private string City
        {
            get;
            set;
        }
        private string StateCode
        {
            get;
            set;
        }
        private string Zip5
        {
            get;
            set;
        }
        private string Zip4
        {
            get;
            set;
        }
        private string ValidatedAddress
        {
            get;
            set;
        }
        public ValidAddress validAddress
        {
            get;
            set;
        }
        public ReturnObject ReturnObject
        {
            get;
            set;
        }
        private string xmlBody
        {
            get;
            set;
        }
        private XmlWriterSettings settings
        {
            get;
            set;
        }
        private StringWriter stringWriter
        {
            get;
            set;
        }
        private string XmlResponseString
        {
            get;
            set;
        }
        public void GetValidAddress(LookupAddress lookupAddress)
        {
            this.xmlBody = "<AddressValidateRequest USERID=\"" + Environment.GetEnvironmentVariable("USPS_USERID") + "\"><Revision>1</Revision><Address><Address1>" + lookupAddress.Address1 + "</Address1><Address2>" + lookupAddress.Address2 + "</Address2><City>" + lookupAddress.City + "</City><State>" + lookupAddress.StateCode + "</State><Zip5>" + lookupAddress.Zip5 + "</Zip5><Zip4>" + lookupAddress.Zip4 + "</Zip4></Address></AddressValidateRequest>";
            this.response = new XmlDocument();
            this.XmlResponseString = new WebClient().DownloadString("https://secure.shippingapis.com/ShippingAPI.dll?API=Verify&XML=" + HttpUtility.UrlEncode(this.xmlBody));
            this.response.LoadXml(this.XmlResponseString);
            this.xml = response["AddressValidateResponse"]["Address"];
            this.AddressLine2 = this.xml["Address2"].InnerText;
            this.City = this.xml["City"].InnerText;
            this.StateCode = this.xml["State"].InnerText;
            this.Zip5 = this.xml["Zip5"].InnerText;
            if (this.xml["Address1"] != null)
            {
                this.AddressLine1 = this.xml["Address1"].InnerText;
                if (this.xml["Zip4"] != null)
                {
                    this.Zip4 = this.xml["Zip4"].InnerText;
                    this.ValidatedAddress = this.AddressLine2 + (Char)10 + this.AddressLine1 + (Char)10 + this.City + ", " + this.StateCode + " " + this.Zip5 + "-" + this.Zip4;
                    this.validAddress = new ValidAddress()
                    {
                        Address2 = this.AddressLine2,
                        Address1 = this.AddressLine1,
                        City = this.City,
                        State = this.StateCode,
                        FiveDigitZip = this.Zip5,
                        FourDigitZip = this.Zip4
                    };
                } else
                {
                    this.ValidatedAddress = this.AddressLine2 + (Char)10 + this.AddressLine1 + (Char)10 + this.City + ", " + this.StateCode + " " + this.Zip5;
                    this.validAddress = new ValidAddress()
                    {
                        Address2 = this.AddressLine2,
                        Address1 = this.AddressLine1,
                        City = this.City,
                        State = this.StateCode,
                        FiveDigitZip = this.Zip5
                    };
                }
            } else
            {
                if (this.xml["Zip4"] != null)
                {
                    this.Zip4 = this.xml["Zip4"].InnerText;
                    this.ValidatedAddress = this.AddressLine2 + (Char)10 + this.City + ", " + this.StateCode + " " + this.Zip5 + "-" + this.Zip4;
                    this.validAddress = new ValidAddress()
                    {
                        Address2 = this.AddressLine2,
                        City = this.City,
                        State = this.StateCode,
                        FiveDigitZip = this.Zip5,
                        FourDigitZip = this.Zip4
                    };
                }
                else
                {
                    this.ValidatedAddress = this.AddressLine2 + (Char)10 + this.City + ", " + this.StateCode + " " + this.Zip5;
                    this.validAddress = new ValidAddress()
                    {
                        Address2 = this.AddressLine2,
                        City = this.City,
                        State = this.StateCode,
                        FiveDigitZip = this.Zip5
                    };
                }
            }
            this.settings = new XmlWriterSettings()
            {
                Indent = true,
                IndentChars = "    ",
                Encoding = Encoding.UTF8
            };
            using (this.stringWriter = new StringWriter())
            {
                using(var xmlTextWriter = XmlWriter.Create(this.stringWriter, this.settings))
                {
                    this.response.WriteTo(xmlTextWriter);
                    xmlTextWriter.Flush();
                    this.XmlResponseString = this.stringWriter.GetStringBuilder().ToString();
                }
            }
            this.ReturnObject = new ReturnObject()
            {
                ValidAddressObject = this.validAddress,
                ValidatedAddress = this.ValidatedAddress,
                ResponseXml = this.response,
                XmlResponseString = this.XmlResponseString
            };
        }
        public Address()
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                string resourceName = new AssemblyName(args.Name).Name + ".dll";
                string resource = Array.Find(this.GetType().Assembly.GetManifestResourceNames(), element => element.EndsWith(resourceName));
                using (var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                {
                    Byte[] assemblyData = new Byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
        }
    }
    public class ValidationRequest
    {
        public static ReturnObject Send(LookupAddress lookupAddress)
        {
            Address a = new Address();
            a.GetValidAddress(lookupAddress);
            return a.ReturnObject;
        }
    }
}