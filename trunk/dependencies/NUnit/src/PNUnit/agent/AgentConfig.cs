// ****************************************************************
// This is free software licensed under the NUnit license. You may
// obtain a copy of the license at http://nunit.org.
// ****************************************************************

using System;
using System.IO;
using System.Xml.Serialization;


namespace PNUnit.Agent
{
    [Serializable]
    public class AgentConfig
    {
        public int Port;
        public string PathToAssemblies;
        public string ShadowCopyCache;
    }

    public class AgentConfigLoader
    {
        public static AgentConfig LoadFromFile(string file)
        {
            FileStream reader = new FileStream(file, FileMode.Open, FileAccess.Read);
            XmlSerializer ser= new XmlSerializer(typeof(AgentConfig));
            return (AgentConfig)ser.Deserialize(reader);
        }
    }

}
