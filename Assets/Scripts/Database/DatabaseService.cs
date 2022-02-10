using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace FootTactic
{
	public static class DatabaseService
	{
		private static IDatabaseProvider database = new FileProvider();

		public static IEnumerator OpenTactic(string filePath, Action<string> onSuccess, Action<string> onError)
        {
			return database.OpenTactic(filePath, onSuccess, onError);
        }

        public static void SaveTactic(string filePath, string contents, Action<string> onSuccess, Action<string> onError)
        {
            database.SaveTactic(filePath, contents, onSuccess, onError);
        }

    }
}