using System.Runtime.Serialization;
using System.Xml;

namespace Coding4Fun.Phone.Storage
{
    public static class Serializer
    {
        public static T Open<T>(string filePath) where T : class, new()
        {
            return Open<T>(filePath, false);
        }

        public static T Open<T>(string filePath, bool useBinary) where T : class, new()
        {
            var loadedObject = default(T);

            using (var stream = PlatformFileAccess.GetOpenFileStream(filePath))
            {
                using (var reader = (useBinary ? XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max) : XmlReader.Create(stream)))
                {
                    if (stream.Length > 0)
                    {
                        var serializer = new DataContractSerializer(typeof (T));

                        loadedObject = (T) serializer.ReadObject(reader);
                    }
                }

                stream.Close();
            }
            
            return loadedObject ?? new T();
        }

        public static void Save<T>(string filePath, T objectToSave)
        {
            Save(filePath, objectToSave, false);
        }

        public static void Save<T>(string filePath, T objectToSave, bool useBinary)
        {
            using (var stream = PlatformFileAccess.GetSaveFileStream(filePath))
            {
                using (var writer = (useBinary ? XmlDictionaryWriter.CreateBinaryWriter(stream) : XmlWriter.Create(stream)))
                {
                    var serializer = new DataContractSerializer(typeof (T));

                    serializer.WriteObject(writer, objectToSave);

                    writer.Flush();
                }

                stream.Close();
            }
        }
    }
}
