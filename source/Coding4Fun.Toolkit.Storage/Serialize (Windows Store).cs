using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using System.Xml;
using System.IO;
using Windows.Storage.Streams;
using System.Runtime.InteropServices.WindowsRuntime;

namespace Coding4Fun.Toolkit.Storage
{
    public static class Serialize
    {
        public async static Task<T> Open<T>(string filePath) where T : class, new()
        {
            return await Open<T>(filePath, false);
        }

        public async static Task<T> Open<T>(string filePath, bool useBinary) where T : class, new()
        {
			var loadedObject = default(T);

			using (var inputStream = await PlatformFileAccess.GetOpenFileSequentialStream(filePath))
			{
				using (var stream = inputStream.AsStreamForRead())
				{

					using (
						var reader = (useBinary
							              ? XmlDictionaryReader.CreateBinaryReader(stream, XmlDictionaryReaderQuotas.Max)
							              : XmlReader.Create(stream)))
					{
						if (stream.Length > 0)
						{
							var serializer = new DataContractSerializer(typeof (T));

							loadedObject = (T) serializer.ReadObject(reader);
						}
					}
				}
			}

			return loadedObject ?? new T();
        }

		public static void Save<T>(string filePath, T objectToSave)
        {
            Save(filePath, objectToSave, false);
        }

		public async static void Save<T>(string filePath, T objectToSave, bool useBinary)
		{
//			Task.Run(async () =>
//				{
					using (var stream = await PlatformFileAccess.GetSaveFileStream(filePath))
					{
						var serializer = new DataContractSerializer(typeof (T),
						                                            new DataContractSerializerSettings() {PreserveObjectReferences = true});
						serializer.WriteObject(stream.AsStreamForWrite(), objectToSave);

						await stream.FlushAsync();
					}
//				}).Wait();
		}
    }
}
