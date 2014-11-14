using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Search;
using Windows.Storage.Streams;

namespace Coding4Fun.Toolkit.Storage
{
	public class PlatformFileAccess
	{
		public static async Task DeleteFile(string fileName)
		{
			var file = await GetFileAccess(fileName);
			
			await file.DeleteAsync();
		}

		public async static Task<IInputStream> GetOpenFileSequentialStream(string fileName)
		{
			var file = await GetFileAccess(fileName);

			return await file.OpenSequentialReadAsync();
		}

		public async static Task<IInputStream> GetOpenFileRandomAccesStream(string fileName)
		{
			var data = await GetFileRandomAccessStream(fileName, FileAccessMode.Read);

			return data.GetInputStreamAt(0);
		}

		public async static Task<IOutputStream> GetSaveFileStream(string fileName)
		{
			var data = await GetFileRandomAccessStream(fileName, FileAccessMode.ReadWrite);

			return data.GetOutputStreamAt(0);
		}

		public async static Task<IRandomAccessStream> GetFileRandomAccessStream(string fileName, FileAccessMode accessMode)
		{
			var file = await GetFileAccess(fileName);

			return await file.OpenAsync(accessMode);
		}

		public async static Task<StorageFile> GetFileAccess(string fileName)
		{
            var storageFolder = ApplicationData.Current.LocalFolder;

            StorageFile file = null;
            
            try
            {
                file = await storageFolder.GetFileAsync(fileName);
            }
            catch(Exception)
            {
                
            }

            if (file == null)
            {
                file = await storageFolder.CreateFileAsync(fileName, CreationCollisionOption.ReplaceExisting);
            }

			return file;
		}
	}
}
