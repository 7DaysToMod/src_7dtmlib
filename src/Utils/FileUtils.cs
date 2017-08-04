using System;
using System.IO;

namespace SDTM
{
	public static class FileUtils
	{
		public static string GetCurrentWorldSaveDir(){
			return GamePrefs.GetString (EnumGamePrefs.SaveGameFolder)+Path.DirectorySeparatorChar
			+ GamePrefs.GetString (EnumGamePrefs.GameWorld)+Path.DirectorySeparatorChar
			+ GamePrefs.GetString (EnumGamePrefs.GameName);
		}

		public static string GetCurrentWorldModSaveDir(){
			return GamePrefs.GetString (EnumGamePrefs.SaveGameFolder)+Path.DirectorySeparatorChar
			+ GamePrefs.GetString (EnumGamePrefs.GameWorld)+Path.DirectorySeparatorChar
			+ GamePrefs.GetString (EnumGamePrefs.GameName)+Path.DirectorySeparatorChar+"7dtm";
		}

		public static bool ensureDirectoryExists(String pathName){
			if (!Directory.Exists (pathName)) {
				try{
					Directory.CreateDirectory (pathName);
				}catch(Exception e){
					Log.Error (string.Format("[FileUtils] Could not Create Directory {0}: {1}", pathName, e.Message));
					Log.Exception (e);
					return false;
				}
			}

			return true;
		}

		private static void DirectoryCopy(string sourceDirName, string destDirName, bool copySubDirs)
		{
			// Get the subdirectories for the specified directory.
			DirectoryInfo dir = new DirectoryInfo(sourceDirName);

			if (!dir.Exists)
			{
				throw new DirectoryNotFoundException(
					"Source directory does not exist or could not be found: "
					+ sourceDirName);
			}

			DirectoryInfo[] dirs = dir.GetDirectories();
			// If the destination directory doesn't exist, create it.
			if (!Directory.Exists(destDirName))
			{
				Directory.CreateDirectory(destDirName);
			}

			// Get the files in the directory and copy them to the new location.
			FileInfo[] files = dir.GetFiles();
			foreach (FileInfo file in files)
			{
				string temppath = Path.Combine(destDirName, file.Name);
				file.CopyTo(temppath, false);
			}

			// If copying subdirectories, copy them and their contents to new location.
			if (copySubDirs)
			{
				foreach (DirectoryInfo subdir in dirs)
				{
					string temppath = Path.Combine(destDirName, subdir.Name);
					DirectoryCopy(subdir.FullName, temppath, copySubDirs);
				}
			}
		}
	}
}

