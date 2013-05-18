using System.IO;
using System.IO.IsolatedStorage;

namespace Coding4Fun.Toolkit.Storage
{
    public class PlatformFileAccess
    {
	    public static void DeleteFile(string fileName)
	    {
		    using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
		    {
			    storageFile.DeleteFile(fileName);
		    }
	    }

	    public static FileStream GetOpenFileStream(string fileName)
        {
            return GetFileStream(fileName, FileMode.OpenOrCreate);
        }

        public static FileStream GetSaveFileStream(string fileName)
        {
			// will create or overwrite file

            return GetFileStream(fileName, FileMode.Create);
        }

        private static FileStream GetFileStream(string fileName, FileMode mode)
        {
            using (var storageFile = IsolatedStorageFile.GetUserStoreForApplication())
            {
                return new IsolatedStorageFileStream(fileName, mode, storageFile);
            }
        }
    }
}