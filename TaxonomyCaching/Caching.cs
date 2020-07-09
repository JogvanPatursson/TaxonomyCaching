﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;
using JeffFerguson.Gepsio;
using System.Linq;
using Newtonsoft.Json;
using System.Xml;
using System.Xml.Linq;
using System.IO;
using System.Threading;
using System.Web;
using System.Linq.Expressions;
using Newtonsoft.Json.Linq;

namespace TaxonomyCaching
{
    class Caching
    {
        //Class members
        //Local path to store cached data
        public string folderPath { get; set; }
        //File name to be created in local path
        public string fileName { get; set; }
        //Local path to which file is stored
        public string filePath { get; set; }
        //Address found in xml instance
        public string taxonomyAddress { get; set; }
        //Address trimmed to taxonomy folder
        public string taxonomyAddressTrimmed { get; set; }
        //Combined string of trimmed address and xlink:href
        public string combinedLink { get; set; }
        //
        public List<XAttribute> listXlink = new List<XAttribute>();
        
        //Run function
        public void run()
        {
            //Set file path for cached files
            folderPath = @"C:\Users\jogva\Desktop\cache";

            //Set file name and directory
            fileName = @"\xlink_href.xml";
            filePath = folderPath + fileName;


            //Call function to create directory
            createDirectory(folderPath);

            //Create xDocument variable
            XDocument xDoc;
            xDoc = XDocument.Load("5694_1_2019.xml");
            //xDoc = XDocument.Load("1453_1_2019.xml");
            //xDoc = XDocument.Load("2874_1_2019.xml");



            //Call function to get xlink:href from xDocument
            string xlinkString = getXSDFileAddressString(xDoc);
            readXlinkAndWriteFile(xlinkString, filePath);
            //
            getXMLFileAddressString(filePath);
            //
            getXMLFileAddressString();
            //
            trimXLinkAddress();
            //
            combineAddress();
            //Deletes directory
            deleteDirectory();
            Console.ReadKey();
        }

      //Checking if directory path already exists
        public Boolean isPath(string folderPath)
        {
            bool path;

            if(Directory.Exists(folderPath))
            {
                path = true;
            }
            else
            {
                path = false;
            }
            return path;
        }

        //Create empty directory
        public void createEmptyDirectory()
        {

        }

        //Function for creating folder directory
        public void createDirectory(string fp)
        {
            string folderPath = fp;

            //Try to create directory
            try
            {
                //Check if directory already exists
                if (isPath(folderPath) == true)
                {
                    Console.WriteLine("Path already exists");
                    return;
                }

                //Create directory
                DirectoryInfo dirInfo = Directory.CreateDirectory(folderPath);
                Console.WriteLine("Directory created at: ", Directory.GetDirectoryRoot(folderPath));

            }
            //If directory creation fails
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

        //Function to delete directory after use
        public void deleteDirectory()
        {
            //If the path is there
            if (isPath(folderPath))
            {
                System.IO.DirectoryInfo di = new DirectoryInfo(folderPath);

                foreach (FileInfo file in di.GetFiles())
                {
                    file.Delete();
                }
                foreach (DirectoryInfo dir in di.GetDirectories())
                {
                    dir.Delete(true);
                }
                Directory.Delete(folderPath, true);
                
                
                Console.WriteLine("Directory deleted");
            }

        }

        //Function to get address of xsd file from xml instance
        public string getXSDFileAddressString(XDocument xDocument)
        {
            XDocument xDoc;
            xDoc = xDocument;

            //Make xml file xDoc into string
            string xml = xDoc.ToString();

            //Parse xml string and get first descendant
            XElement element = XElement.Parse(xml).Descendants().First();
            string elementString = element.ToString();

            //Console.WriteLine(elementString);

            //Subtrings
            //string schemaStr = "<link:schemaRef";
            string hrefStr = "xlink:href=";
            

            //if (elementString.StartsWith(schemaStr))
            //{
                //Console.WriteLine("<link:schemaRef is true");

                if (elementString.Contains(hrefStr))
                {
                    //Console.WriteLine("xlink:href= is true");

                    int lastIndex = (elementString.IndexOf(hrefStr) + hrefStr.Length + 1);
                    //Console.WriteLine(lastIndex);

                    string xlinkStr = elementString.Substring(lastIndex, (elementString.Length - lastIndex));

                    //Console.WriteLine(xlinkStr);

                    taxonomyAddress = xlinkStr.Split('"')[0];

                    //Console.WriteLine(originString);
                }
            //}
            return taxonomyAddress;
        }

        //Function to get xml files from xsd file
        public void getXMLFileAddressString()
        {
            foreach(var xLink in listXlink)
            {
                //Console.WriteLine(xLink.Value);
            }
        }

        //Function to trim taxonomy address found in xml instance
        public string trimXLinkAddress()
        {
            taxonomyAddress = "https://www.vinnugluggin.fo/taxonomy/20180301/entryFODanishGAAPBalanceSheetAccountFormIncomeStatementByNatureIncludingManagementsReview20161001.xsd";
            //Reverse taxonomyAddress string


            //Split taxonomyAddress string at first '/'
            string[] split = taxonomyAddress.Split('/');
            string last = split.Last();

            taxonomyAddress = taxonomyAddress.Replace(last, "");

            Console.WriteLine(taxonomyAddress);

            //Console.WriteLine(value.Length);

            return taxonomyAddressTrimmed;
        }

        //Function to combine taxonomy address and xlink:href
        public void combineAddress()
        {
            foreach(var address in listXlink)
            {
                string adr = address.Value.ToString();
                combinedLink = taxonomyAddressTrimmed + adr;
                //Console.WriteLine(combinedLink);
            }
            
        }

        //Function to load XDocument and write file locally
        public /*string*/void readXlinkAndWriteFile(string xlink, string fp)
        {
            string originString = xlink;
            string filePath = fp;

            //Reading from xlinkHref link
            XDocument xlinkHref;
            xlinkHref = XDocument.Load(originString);

            string xlinkHrefString = xlinkHref.ToString();

            //Creating file with input from xlinkHref link
            try
            {
                using (FileStream fs = File.Create(filePath))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(xlinkHrefString);
                    fs.Write(info, 0, info.Length);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Process failed. ", e.ToString());
            }
        }

        public void xlinkSearch(string fp)
        {
            /*
            string filePath = fp;
            //string link = "";
            //string keyString = "";
            
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(filePath);

            //XDocument xDoc;
            //xDoc = XDocument.Load(filePath);

            foreach(XmlNode node in xDoc.DocumentElement)
            {
                string nodeStr = node.Name;
                Console.WriteLine(nodeStr);

                if(nodeStr == "xsd:appinfo")
                {

                }
            }
            */
        }

        //Checks if xAttribute contains 
        public bool containsXlinkHref(XAttribute xAttribute)
        {
            bool contains = false;
            string str;
            str = xAttribute.Value.ToString();
            

            //Console.WriteLine(str);

            //Substrings
            string subStr = ".xml";
            //string originStr = "";

            if(str.Contains(subStr))
            {
                //getXmlFile(str);
                //Console.WriteLine(str);
                contains = true;
            }
            else
            {
                
            }
            return contains;
        }

        // Function to find XML file address links from XSD file
        public void getXMLFileAddressString(string fp)
        {
            //Variables
            string strXmlLocation = fp;
            //string toString = "";
            var doc = XDocument.Load(strXmlLocation);
            XNamespace link = "http://www.xbrl.org/2003/linkbase";
            
            // Number of elements with the name label in the document
            var numberOfLabelElements = (int)doc.Descendants(link + "label").Count();
            //Creating lists for storing label and ids
            
            List<XAttribute> listAttributes = new List<XAttribute>();

            listAttributes = doc.Descendants().Attributes().ToList();

            //Looping through the list of ids and printing them out
            foreach (var xAttribute in listAttributes)
            {
                if (containsXlinkHref(xAttribute))
                {
                    //toString = xAttribute.Value.ToString();
                    //Console.WriteLine(toString);
                    listXlink.Add(xAttribute);
                    //Console.WriteLine(xAttribute.Value.ToString());
                }
                //Console.WriteLine(xAttribute.Value.ToString());
            }
        }

        public void getXmlFile(string link, string fp)
        {
            string filePath = fp;
            string lastPartLink = link;
            //string firstPartLink = "";
        }

        /*-----------------------------------------------------*/
        /*-------------------------FACTS-----------------------*/
        /*-----------------------------------------------------*/

        private static void ShowFactsInDocument(XbrlDocument doc)
        {
            foreach (var currentFragment in doc.XbrlFragments)
            {
                ShowFactsInFragment(currentFragment);
            }
        }

        private static void FindFactInDocument(XbrlDocument doc, string factName)
        {
            foreach (var currentFragment in doc.XbrlFragments)
            {
                FindFactInFragment(currentFragment, factName);
            }
        }

        private static void ShowFactsInFragment(XbrlFragment currentFragment)
        {
            foreach (var currentFact in currentFragment.Facts)
            {
                ShowFact(currentFact);
            }
        }

        private static void FindFactInFragment(XbrlFragment currentFragment, string factName)
        {
            var factFound = currentFragment.Facts.GetFactByName(factName);
            if (factFound != null)
                ShowFact(factFound);
        }

        private static void ShowFact(Fact fact)
        {
            Console.WriteLine($"FACT {fact.Name}");
            if (fact is Item)
            {
                ShowItem(fact as Item);
            }
            else if (fact is JeffFerguson.Gepsio.Tuple)
            {
                ShowTuple(fact as JeffFerguson.Gepsio.Tuple);
            }
        }

        private static void ShowItem(Item item)
        {
            Console.WriteLine("\tType     : Item");
            Console.WriteLine($"\tNamespace: {item.Namespace}");
            Console.WriteLine($"\tValue    : {item.Value}");
        }

        private static void ShowTuple(JeffFerguson.Gepsio.Tuple tuple)
        {
            Console.WriteLine("\tType     : Tuple");
            foreach (var currentFact in tuple.Facts)
            {
                ShowFact(currentFact);
            }
        }


        /*-----------------------------------------------------*/
        /*---------------------CONTEXT-------------------------*/
        /*-----------------------------------------------------*/

        private static void ShowContextsInDocument(XbrlDocument doc)
        {
            foreach (var currentFragment in doc.XbrlFragments)
            {
                ShowContextsInFragment(currentFragment);
            }
        }

        private static void ShowContextsInFragment(XbrlFragment currentFragment)
        {
            foreach (var currentContext in currentFragment.Contexts)
            {
                ShowContext(currentContext);
            }
        }

        private static void ShowContext(Context currentContext)
        {
            Console.WriteLine("CONTEXT");
            Console.WriteLine($"\tID          : {currentContext.Id}");
            Console.Write($"\tPeriod Type : ");
            if (currentContext.InstantPeriod)
            {
                Console.WriteLine("instant");
                Console.WriteLine($"\tInstant Date: {currentContext.InstantDate}");
            }
            else if (currentContext.DurationPeriod)
            {
                Console.WriteLine("period");
                Console.WriteLine($"\tPeriod Date : from {currentContext.PeriodStartDate} to {currentContext.PeriodEndDate}");
            }
            else if (currentContext.ForeverPeriod)
            {
                Console.WriteLine("forever");
            }
        }
    }
}
