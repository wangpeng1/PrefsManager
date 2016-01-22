# PrefsManager
自由な型で保存できて、さらに保存データが暗号化されるように拡張したPlayerPrefsです。
 
#### 自プロジェクトでの使用
**Assets/PrefsManager.cs**が本体です。自プロジェクトで使用する場合は**PrefsManager.cs**のみを取り出してお使いください。  

<br>
# 使い方
初回に**Initialize**をコールし、アセットバンドルが含まれるサーバ上のディレクトリとアセットバンドルのバージョンを指定します。  
一度設定すれば以降、値の変更しない限り設定する必要はありません。

`AssetBundleManager.Instance.Initialize (bundleDirURL, 1);`

<br>
#### アセットバンドルのダウンロード
**DownloadAssetBundle**でダウンロードするアセットバンドル名を指定します。  
コールバックデリゲートでダウンロードの進捗値を取得できます。

    using UnityEngine;
    using System.Collections;
    // 追加
    using PrefsManager;
    
    public class TestClass : MonoBehaviour {
        
        // Use this for initialization
        void Start () {
                // ロード
                bool isTapButton = UserInfo.Load<bool> ("KEY_ON_TAP_BUTTON");
        }
        
        // ボタンが押された
        public void OnTapButton () {
                // セーブ
                UserInfo.Save<bool> ("KEY_ON_TAP_BUTTON", true);
        }
    }

<br>
#### クラスごと保存する
クラスをSerializableでシリアライズ化すればクラスを丸ごと保存できます。

    using UnityEngine;
    using System.Collections;
    using PrefsManager;
    // 追加
    using System;
    
    public class TestClass : MonoBehaviour {
    
        // Use this for initialization
        void Start () {
                // ロード
                PlayerStatus status = UserInfo.Load<PlayerStatus> ("KEY_PLAYER_STATUS");
                Debug.Log("名前 : "+status.name);
                Debug.Log("レベル : "+status.level);
        }

        // ゲームをセーブする
        public void SaveGame (PlayerStatus status) {
                // セーブ
                UserInfo.Save<PlayerStatus> ("KEY_PLAYER_STATUS", status);
        }

        // ステータスクラス
        [Serializable]
        public class PlayerStatus {
                public string name = "勇者";
                public int level = 1;
        }
}