# Stream Deck Icon Converter

(日本語版は後半に記載しています。)

## Summary

Converts a video file into an "[Elgato Stream Deck](https://www.elgato.com/ja/stream-deck)" icon file (animated GIF).  
(Generates an icon image without the border area between buttons.)

"[FFmpeg](https://www.ffmpeg.org/)" is required to run this application.

## How to use

- Prepare
1. Download this application from "[Releases](https://github.com/TatsuyaJp/StreamDeckIconConverter/releases)".
2. Extract the file from step 1.
3. Download "[FFmpeg](https://www.gyan.dev/ffmpeg/builds/)".
   - If you already have "[7-Zip](https://www.7-zip.org/)" installed, download `ffmpeg-release-essentials.7z`.
   - Otherwise, download `ffmpeg-release-essentials.zip`.
4. Extract the file from step 3.
5. Place `ffmpeg.exe` in the same folder as `StreamDeckIconConverter.exe`.

- Convert
1. Launch 'StreamDeckIconConverter.exe.'
2. Click `Browse` for the `Input Video File`.
3. Open the video file.
4. Change the `Icon Layout`.
5. Change the `Start Time` and `End Time`.
6. Click `Generate Icon (GIF)`.
7. Choose a location to save the file.
8. A sequentially numbered GIF file will be generated.

- Set to "Stream Deck"
1. Launch the `Stream Deck` application.
2. Click "New Profile" to create a profile. (Example: `Profile 1`).
3. Place any icons in all the locations. (Example: `Switch Profile`).
4. Click on the top left icon among the placed icons.
5. The icon you clicked will appear at the bottom of the window, click `v`.
6. Click `Set from File`.
7. Select `(FileName)_01.gif` from the sequentially numbered GIF files.
8. Set the GIF file for all the icons.
9. To synchronize the animation, select a profile other than `Profile 1`.
10. Select `Profile 1` and check the display of the icons.

- Example of "Stream Deck Mini (3x2)"

| Row / Col | 1 | 2 | 3 |
| :---: | --- | --- | --- |
| 1 | (FileName)\_01.gif | (FileName)\_02.gif | (FileName)\_03.gif |
| 2 | (FileName)\_04.gif | (FileName)\_05.gif | (FileName)\_06.gif |

## Author

TatsuyaJp ([Twitter](https://twitter.com/TatsuyaJp))

---

## 概要

動画ファイルを "[Elgato Stream Deck](https://www.elgato.com/ja/stream-deck)" のアイコンファイル(アニメーションGIF)へ変換します。  
(ボタン間の境界領域を除いたアイコン画像を生成します。)

本アプリケーションの動作には "[FFmpeg](https://www.ffmpeg.org/)" が必要です。

## 使用方法

- 準備
1. "[Releases](https://github.com/TatsuyaJp/StreamDeckIconConverter/releases)" から本アプリケーションをダウンロードします。
2. 手順1.のファイルを展開します。
3. "[FFmpeg](https://www.gyan.dev/ffmpeg/builds/)" をダウンロードします。
   - "[7-Zip](https://sevenzip.osdn.jp/)" がインストール済みなら `ffmpeg-release-essentials.7z` をダウンロードします。
   - そうでなければ `ffmpeg-release-essentials.zip` をダウンロードします。
4. 手順3.のファイルを展開します。
5. `StreamDeckIconConverter.exe` と同じフォルダへ `ffmpeg.exe` を置きます。

- 変換
1. `StreamDeckIconConverter.exe` を起動します。
2. `入力動画ファイル` の `参照` をクリックします。
3. 動画ファイルを開きます。
4. `アイコンレイアウト` を変更します。
5. `開始時間` と `終了時間` を変更します。
6. `アイコン生成 (GIF)` をクリックします。
7. ファイルの保存先を選びます。
8. 連番のGIFファイルが生成されます。

- "Stream Deck" へ設定
1. `Stream Deck` アプリケーションを起動します。
2. `新規プロファイル` をクリックしてプロファイルを作成します。 (例: `Profile 1`)
3. 全ての場所へ任意のアイコンを置きます。(例: `プロファイルを切り替え`)
4. 置いたアイコンの中で、最も左上のアイコンをクリックします。
5. ウィンドウの下側にクリックしたアイコンが表示されるので、`v`をクリックします。
6. `ファイルから設定` をクリックします。
7. 連番のGIFファイルから `(FileName)_01.gif` を選択します。
8. 全てのアイコンについて、GIFファイルを設定します。
9. アニメーションを同期させるため、`Profile 1` 以外のプロファイルを選択します。
10. `Profile 1` を選択して、アイコンの表示を確認します。

- "Stream Deck Mini (3x2)" の例

| 行 / 列 | 1 | 2 | 3 |
| :---: | --- | --- | --- |
| 1 | (FileName)\_01.gif | (FileName)\_02.gif | (FileName)\_03.gif |
| 2 | (FileName)\_04.gif | (FileName)\_05.gif | (FileName)\_06.gif |

## 作者

TatsuyaJp ([Twitter](https://twitter.com/TatsuyaJp))
