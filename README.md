What's this? / これは何か?
======================

This is  the sample code of ASP.NET SignalR OWIN Self Hosting console app which can run on Raspberry Pi.

これは、Raspberry Pi 上でも実行可能な、OWIN セルフホスティングによる ASP.NET SignalR コンソールアプリのサンプルコードです。

How to build? / ビルド方法
===========================

Requirements / 要件
---------------------
- Windows OS
- Visual Studio 2013

> * It may can build other OS/build tools, but I don't have tried it.  
> ※ 他のOS/ビルドツールでもビルド可能かと思いますが、自分は試していません。

Steps / 手順
----------------

Enter follow commands in command prompt.

コマンドプロンプトで、以下のコマンドを実行します。

```
> git clone git@github.com:sample-by-jsakamoto/SignalR-on-RaspberryPi.git app2
> cd app2
> start app2.sln
```

Then Visual Studio is lauched, type ```Ctrl + Shift + B``` key.  
After that, you get out put files at app2/bin/debug folder.

Visual Studio が起動するので、```Ctrl + Shift + B``` キーを押します。  
そうすると、app2/bin/debug フォルダに出力ファイルが得られます。

How to deploy to Raspberry pi? / Raspberry pi への配置方法
===========================

Requirements / 要件
---------------------
- Raspbian
- mono (```sudo apt-get install mono-complete```)
- Network and SSH connected

Steps / 手順
----------------
Transfer all out put files to Raspberry pi.

すべての出力ファイルを Raspberry pi に転送します。

Example/例
```
> cd app2/bin/debug
> scp -r * pi@<IP address of RaspPi>:/home/pi/
```

> * Tips - you can ran scp command and ssh command if you installed git for windows. These commands will be on ```C:\Program Files\Gin\bin``` folder.
> ※Tips - git for Windows をインストール済みなら、scp コマンドや ssh コマンドを使えます。それらのコマンドは、```C:\Program Files\Gin\bin``` フォルダにあることでしょう。

Next, at the console of on Raspberry pi, enter follow commands.

次に、Raspberry pi のコンソール上で、以下のコマンドを入力します。

```
$ cd /home/pi
$ sudo mono app2.exe
```

After that, you can access ```http://<IP address of Raspberry pi>/index.html``` with any modern web browsers.

以上で、任意のモダン Web ブラウザで ```http://<IP address of Raspberry pi>/index.html``` にアクセスすることが可能です。
