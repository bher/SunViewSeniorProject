using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Services;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

public partial class _codebehind : System.Web.UI.Page
{
    [WebMethod]
    public static string testing()
    {
        return "Success";
    }
    [WebMethod]
    public static List<String> GetAllProjects()
    {
        //CREATES XML DOC AND LIST. GETS ALL PROJECTS
        List<string> sends = new List<string>();
        XDocument doc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
        var projects = doc.Descendants("project");
        //FOR EACH PROJECT, PUTS ITS NAME INTO A LIST AND SENDS TO FRONTEND
        foreach (var project in projects)
        {
            string name = project.Attribute("name").Value.ToString();
            sends.Add(name);
        }
        return sends;
    }

    [WebMethod]
    public static List<string> GetCGID(string projects)
    {
        List<string> sends = new List<string>();
        XDocument doc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
        var project = doc.XPathSelectElement("//project[@name = '" + projects + "']");
        if (project != null)
        {
            string ID = project.Attribute("cgID").Value.ToString();
            string module = project.Attribute("module").Value.ToString();
            sends.Add(module);
            sends.Add(ID);
        }
        return sends;
    }

    [WebMethod]
    public static List<string> GetNumberMappings(string projects)
    {
        List<string> sends = new List<string>();
        XDocument doc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
        var project = doc.XPathSelectElement("//project[@name = '" + projects + "']");
        if (project != null)
        {
            var mapNumbers = project.Descendants("fields");
            foreach (var x in mapNumbers)
            {
                sends.Add(x.Attribute("map").Value.ToString());
            }
        }
        return sends;
    }
    [WebMethod]
    public static void SaveProject2(string pName, string mName, string cgID)
    {
        XDocument doc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
        var project = doc.XPathSelectElement("//project[@name = '" + pName + "']");
        project.SetAttributeValue("module", mName);
        if (project.Attribute("module").Value == "")
            project.Attribute("module").Remove();

        project.SetAttributeValue("cgID", cgID);
        if (project.Attribute("cgID").Value == "")
            project.Attribute("cgID").Remove();
        doc.Save(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");

    }
    [WebMethod]
    public static void SaveProject(string pName, string mName, string cgID, string fName, string tfsName, string cgName,
        string dir, string tfsVal, string cgVal, string type, string max)
    {

        XDocument doc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
        var project = doc.XPathSelectElement("//project[@name = '" + pName + "']");
        XElement selectedMap = null;

        project.SetAttributeValue("module", mName);
        if (project.Attribute("module").Value == "")
            project.Attribute("module").Remove();

        project.SetAttributeValue("cgID", cgID);
        if (project.Attribute("cgID").Value == "")
            project.Attribute("cgID").Remove();
        if (fName != "")
        {
            var fields = project.Descendants("fields");
            foreach (var field in fields)
            {
                if (field.Attribute("map").Value == fName)
                {
                    selectedMap = field;
                }
            }

            selectedMap.SetAttributeValue("tfs", tfsName);
            if (selectedMap.Attribute("tfs").Value == "")
                selectedMap.Attribute("tfs").Remove();

            selectedMap.SetAttributeValue("cg", cgName);
            if (selectedMap.Attribute("cg").Value == "")
                selectedMap.Attribute("cg").Remove();

            selectedMap.SetAttributeValue("tfsval", tfsVal);
            if (selectedMap.Attribute("tfsval").Value == "")
                selectedMap.Attribute("tfsval").Remove();

            selectedMap.SetAttributeValue("cgval", cgVal);
            if (selectedMap.Attribute("cgval").Value == "")
                selectedMap.Attribute("cgval").Remove();

            selectedMap.SetAttributeValue("max", max);
            if (selectedMap.Attribute("max").Value == "")
                selectedMap.Attribute("max").Remove();

            selectedMap.SetAttributeValue("dir", dir);
            if (selectedMap.Attribute("dir").Value == "")
                selectedMap.Attribute("dir").Remove();

            selectedMap.SetAttributeValue("type", type);
            if (selectedMap.Attribute("type").Value == "")
                selectedMap.Attribute("type").Remove();
        }
        doc.Save(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
    }
    [WebMethod]
    public static List<String> GetAllMappings(string projects, string number)
    {
        //PREPARES LIST OF STRINGS TO RETURN TO FRONT-END
        List<string> sends = new List<string>();
        XElement selectedMap = null;
        //LOADS XML FILE AS AN XDOCUMENT DOC
        XDocument doc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
        //LOADS THE SINGULAR MAPPING FIELD TO DISPLAY
        var project = doc.XPathSelectElement("//project[@name = '" + projects + "']");
        if (project == null)
        {
            return sends;
        }
        var mappings = project.Element("mappings");
        var fields = mappings.Elements();
        foreach (var field in fields)
        {
            if (field.Attribute("map").Value.ToString() == number)
            {
                selectedMap = field;
            }
        }
        //PUTS ALL ATTRIBUTE FIELDS INTO LIST, EMPTY FIELDS ARE BLANKS IN LIST  //project[@name='QA']/fields[
        if (selectedMap != null)
        {
            sends.Add(AttributeValueOrEmpty(selectedMap, "tfs"));
            sends.Add(AttributeValueOrEmpty(selectedMap, "cg"));
            sends.Add(AttributeValueOrEmpty(selectedMap, "tfsval"));
            sends.Add(AttributeValueOrEmpty(selectedMap, "cgval"));
            sends.Add(AttributeValueOrEmpty(selectedMap, "max"));
            sends.Add(AttributeValueOrEmpty(selectedMap, "dir"));
            sends.Add(AttributeValueOrEmpty(selectedMap, "type"));
        }
        return sends;
    }
    [WebMethod]
    public static void CreateProject(string name)
    {
        XDocument doc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
        var root = doc.XPathSelectElement("//CGIntConfiguration");
        XElement project = new XElement("project");
        project.SetAttributeValue("name", name);
        project.SetAttributeValue("module", "incidentrequest");
        project.SetAttributeValue("cgID", "codebehind");
        XElement mappings = new XElement("mappings");
        XElement fields = new XElement("fields");
        fields.SetAttributeValue("map", "1");
        project.Add(mappings);
        mappings.Add(fields);
        root.Add(project);
        doc.Save(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
    }
    [WebMethod]
    public static void DeleteProject(string pName)
    {
        XDocument doc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
        var project = doc.XPathSelectElement("//project[@name='" + pName + "']");
        project.RemoveNodes();
        project.Remove();
        doc.Save(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
    }
    [WebMethod]
    public static void DeleteMap(string pName, string fName)
    {
        XDocument doc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
        var project = doc.XPathSelectElement("//project[@name='" + pName + "']");
        var mappings = project.Element("mappings");
        var fields = mappings.Elements("fields");
        foreach (var field in fields)
        {
            if (field.Attribute("map").Value == fName)
                field.Remove();
        }
        doc.Save(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
    }
    [WebMethod]
    public static void CreateMap(string pName, List<string> fNames)
    {
        XDocument doc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
        var project = doc.XPathSelectElement("//project[@name='" + pName + "']");
        var mappings = project.Element("mappings");
        var fields = project.Descendants("fields");
        XElement newField = new XElement("fields");
        int i = 1;
        foreach (var field in fields)
        {
            if (field.Attribute("map").Value != i.ToString())
            {
                newField.SetAttributeValue("map", i);
                field.AddBeforeSelf(newField);
                doc.Save(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
                return;
            }
            i++;
        }
        newField.SetAttributeValue("map", i);
        mappings.Add(newField);
        doc.Save(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
    }

    public static string AttributeValueOrEmpty(XElement element, string s)
    {
        XAttribute attribute = element.Attribute(s);
        if (attribute == null)
            return "";
        else
        {
            return attribute.Value;
        }
    }

    public XDocument loadXML()
    {
        string relPath = "~/App_Code/XMLFile.xml";
        XDocument doc = XDocument.Load(relPath);
        return doc;
    }

    [WebMethod]
    public static List<string> GetUserInfo()
    {
        List<string> sends = new List<string>();
        XDocument doc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
        var cguser = doc.XPathSelectElement("//appSettings/add[@key='CGuser']");
        var cgpass = doc.XPathSelectElement("//appSettings/add[@key='CGpass']");
        var tfsuser = doc.XPathSelectElement("//appSettings/add[@key='TFSuser']");
        var tfspass = doc.XPathSelectElement("//appSettings/add[@key='TFSpass']");
        var cgpath = doc.XPathSelectElement("//appSettings/add[@key='CGapiPath']");

        sends.Add(tfsuser.Attribute("value").Value);
        sends.Add(tfspass.Attribute("value").Value);
        sends.Add(cguser.Attribute("value").Value);
        sends.Add(cgpass.Attribute("value").Value);
        sends.Add(cgpath.Attribute("value").Value);
        return sends;

    }
    [WebMethod]
    public static void saveUserInfo(string cguser, string cgpass, string tfsuser, string tfspass, string cgpath)
    {
        XDocument doc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
        doc.XPathSelectElement("//appSettings/add[@key='CGuser']").Attribute("value").SetValue(cguser);
        doc.XPathSelectElement("//appSettings/add[@key='CGpass']").Attribute("value").SetValue(cgpass);
        doc.XPathSelectElement("//appSettings/add[@key='TFSuser']").Attribute("value").SetValue(tfsuser);
        doc.XPathSelectElement("//appSettings/add[@key='TFSpass']").Attribute("value").SetValue(tfspass);
        doc.XPathSelectElement("//appSettings/add[@key='CGapiPath']").Attribute("value").SetValue(cgpath);
        doc.Save(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
    }
    [WebMethod]
    public static List<string> getAppend()
    {
        List<string> sends = new List<string>();
        XDocument doc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
        var appender = doc.XPathSelectElement("//log4net/appender");
        var temp = appender.Element("file").Attribute("value").Value;
        sends.Add(temp);
        temp = appender.Element("appendToFile").Attribute("value").Value;
        sends.Add(temp);
        temp = appender.Element("maximumFileSize").Attribute("value").Value;
        sends.Add(temp);
        temp = appender.Element("maxSizeRollBackups").Attribute("value").Value;
        sends.Add(temp);
        appender = doc.XPathSelectElement("//log4net/root");
        temp = appender.Element("level").Attribute("value").Value;
        sends.Add(temp);
        return sends;
    }
    [WebMethod]
    public static void saveAppend(string file, string append, string size, string roll, string level)
    {
        XDocument doc = XDocument.Load(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
        var appender = doc.XPathSelectElement("//log4net/appender");
        appender.Element("file").Attribute("value").SetValue(file);
        appender.Element("appendToFile").Attribute("value").SetValue(append);
        appender.Element("maximumFileSize").Attribute("value").SetValue(size);
        appender.Element("maxSizeRollBackups").Attribute("value").SetValue(roll);
        appender = doc.XPathSelectElement("//log4net/root");
        appender.Element("level").Attribute("value").SetValue(level);
        doc.Save(System.Web.HttpContext.Current.Server.MapPath(".") + "/XMLFile.xml");
    }

}