using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace PrefsManager {

	/// <summary>
	/// UserInfoに対応させるキー群です。
	/// </summary>
	public class Keys
	{
		/// <summary>
		/// 単眼か双眼のカメラ設定情報
		/// "MONO"	: 双眼
		/// "BOTH"	: 単眼
		/// </summary>
		public const string CAMERA_TYPE = "CAMERA_TYPE";

		/// <summary>
		/// マーカーリングを使用するかどうか
		/// true	: 使用する
		/// false	: 使用しない
		/// </summary>
		public const string IS_USE_MARKERRING = "IS_USE_MARKERRING";

		/// <summary>
		/// 資料室のデータをダウンロードをお知らせするアラートを表示したかどうか
		/// true	: 表示した
		/// false 	: 表示していない
		/// </summary>
		public const string IS_SHOW_GALLERY_ALERT = "IS_SHOW_GALLERY_ALERT";
	}



	/// <summary>
	/// 型を自由化した拡張PlayerPrefsです。
	/// </summary>
	public class UserInfo
	{
		// 保存先のディレクトリ名
		private const string dirName = "userinfo";
		// 初期化が済んでいるかどうか
		private static bool isInit = false;

		// 保存先のディレクトリ
		private static string SaveDir {
			get {
				// 保存先のディレクトリパスを取得
				#if UNITY_EDITOR
				string dir = Application.streamingAssetsPath + "/";
				#else
				string dir = Application.persistentDataPath + "/";
				#endif
				return dir;
			}
		}


		/// <summary>
		/// データの保存を行います。
		/// </summary>
		/// <param name="data">保存するデータ</param>
		/// <param name="key">データに対応したキー</param>
		public static void Save<T> (string key, T data) {
			// 有効なキーかどうかチェック
			if (!KeyCheck(key)) return;

			// パスを取得
			string path = GetSavePath (key);

			// ファイルを開く
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (path, FileMode.Create);
			// シリアライズして保存
			bf.Serialize (file, data);
			// ファイルを閉じる
			file.Close ();
		}


		/// <summary>
		/// データのロードを行います。
		/// </summary>
		/// <param name="key">取得するデータに対応したキー</param>
		public static T Load<T> (string key) {
			// 有効なキーかどうかチェック
			if (!KeyCheck(key)) return default (T);

			// パスを取得
			string path = GetSavePath (key);

			// ファイルが存在するかどうか
			if (File.Exists (path)) {
				// ファイルを開く
				BinaryFormatter bf = new BinaryFormatter ();
				FileStream file = File.Open (path, FileMode.Open);

				// デシリアライズして取得
				T data = (T)bf.Deserialize (file);
				// ファイルを閉じる
				file.Close ();

				// ロードしたデータを返す
				return data;
			}
			Debug.Log ("キーに対応するファイルが存在しません。");
			// 規定値を返す
			return default (T);
		}

		/// <summary>
		/// キーに対応したデータを削除します。
		/// </summary>
		/// <param name="key">削除するデータに対応したキー</param>
		/// <returns><c>true</c>, 該当のデータが正しく削除された場合 <c>false</c> 該当するファイルが存在しなかった場合</returns>
		public static bool DeleteKey (string key) {
			// 有効なキーかどうかチェック
			if (!KeyCheck(key)) return false;

			// パスを取得
			string path = GetSavePath (key);

			// ファイルが存在するかどうか
			if (File.Exists (path)) {
				// ファイルを削除
				File.Delete (path);
				// 削除成功
				return true;
			}
			Debug.Log ("キーに対応するファイルが存在しません。");
			// ファイルが存在しない
			return false;
		}


		/// <summary>
		/// すべてのデータを削除します。
		/// </summary>
		/// <returns><c>true</c>, すべてのデータが正しく削除された場合 <c>false</c> 該当するファイルが存在しなかった場合</returns>
		public static bool DeleteAll () {
			// 保存先のファイルを設定
			string path = SaveDir + dirName;

			// ファイルが存在するかどうか
			if (Directory.Exists (path)) {
				// ファイルをすべて取得
				string[] filePaths = Directory.GetFiles (path+"/");

				if (filePaths.Length > 0) {

					// ディレクトリ配下のファイルをすべて削除
					foreach (string filePath in filePaths) {
						File.SetAttributes (filePath, FileAttributes.Normal);
						File.Delete (filePath);
					}

					// 削除成功
					return true;
				}
				Debug.Log ("キーに対応するファイルが存在しません。");
				// 削除するファイルが存在しない
				return false;
			}
			Debug.Log (dirName + "ディレクトリが存在しません。");
			// ディレクトリが存在しない
			return false;
		}


		// 有効なキーかどうかチェック
		private static bool KeyCheck (string key) {
			// キーチェック
			if (key == string.Empty) {
				Debug.LogError ("キーが設定されていません。");
				return false;
			}
			else {
				return true;
			}
		}


		// 保存先パスを返す
		private static string GetSavePath (string key) {
			
			// BinaryFormatterをiOSで使用するための処理
			#if UNITY_IOS && !UNITY_EDITOR
			if (!isInit) {
				Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");
				isInit = true;
			}
			#endif

			// ディレクトリが存在するかどうか
			if (!Directory.Exists(SaveDir+dirName)) {
				// 存在しない場合は作成する
				Directory.CreateDirectory(SaveDir+dirName);
			}

			// キーをbyte化して暗号化
			byte[] bytes = Encoding.UTF8.GetBytes(key);
			string encryptionKey = BitConverter.ToString (bytes);

			// 保存先のファイルを設定
			string path = SaveDir + dirName +"/" + encryptionKey + ".dat";

			// パスを返す
			return path;
		}
	}
}