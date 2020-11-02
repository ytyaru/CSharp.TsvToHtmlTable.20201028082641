# Logger

　ログ出力について。

## package

package|公式|
-------|----|
`System.Diagnostics`|o
`log4net`|x
`NLog`|x

### install

* ./src/install.sh

　MONO, NuGet.exe を使ってインストールする。

### build

* ./src/run.sh

　`csc -r:NLog.dll`のように参照追加する。パスは実行ファイルパスと同じディレクトリにないとダメっぽい。

## config

* ./src/NLog.config

　実行ファイルパスと同じディレクトリにないとエラーになる。

## code

```cs
using NLog;
class MyClass
{
    private Logger logger = LogManager.GetCurrentClassLogger();
    private Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
}
```

```cs
using NLog;
class MyClass
{
    private Logger logger = LogManager.GetCurrentClassLogger();
    public MyClass() {
        logger.Fatal("Fatal");
        logger.Error("Error");
        logger.Warn("Warn");
        logger.Info("Info");
        logger.Debug("Debug");
        logger.Trace("Trace");
    }
    public void Errot() {
        try
        {
            throw new Exception("Some Error");
        }
        catch (Exception e)
        {
            Logger logger = NLog.LogManager.GetLogger("AppErrorLogger");
            logger.Error(e, "Error !!");
        }
    }
}
```

