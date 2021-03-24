using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml.Serialization;

using DSharpPlus;

using InsanityBot.Util.FileMeta;

namespace InsanityBot.Util
{
    class Serializer
    {
        public static void CreateNew(UInt64 UserID, String Name)
        {
            Directory.CreateDirectory($"./data/{UserID}");
            String filename = $"./data/{UserID}/modlog.xml";
            User user = new User
            {
                Username = Name
            };

            StreamWriter writer = new StreamWriter(filename);
            XmlSerializer serializer = new XmlSerializer(typeof(User));

            serializer.Serialize(writer, user);
            writer.Close();
        }

        public static void Serialize(User user, UInt64 ID)
        {
            String filename = $"./data/{ID}/modlog.xml";

            XmlSerializer serializer = new XmlSerializer(typeof(User));
            StreamWriter writer = new StreamWriter(filename);

            serializer.Serialize(writer, user);
            writer.Close();
        }

        public static User Deserialize(UInt64 ID)
        {
                String filename = $"./data/{ID}/modlog.xml";

                XmlSerializer deserializer = new XmlSerializer(typeof(User));
                FileStream reader = new FileStream(filename, FileMode.Open);

                User returnValue = (User)deserializer.Deserialize(reader);
                reader.Close();
                return returnValue;
        }
    }
}
