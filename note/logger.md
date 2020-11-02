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

# できなかったこと

## 色付き＆改行なし

```sh
NLog.NLogConfigurationException: Exception when parsing /tmp/work/CSharp.TsvToHtmlTable.20201028082641/bin/NLog.config.  ---> System.NotSupportedException: Parameter lineEnding not supported on ColoredConsoleTarget
```

NLog.config
```xml
<nlog>
    <targets>
        <target name="log-stderr-none-new-line" 
                xsi:type="ColoredConsole" 
                errorStream="true" 
                enableAnsiOutput="true" 
                lineEnding="None" 
                layout="[${level:upperCase=true}] ${message}">
            <highlight-row condition="level == LogLevel.Trace" foregroundColor="Gray" />
            <highlight-row condition="level == LogLevel.Debug" foregroundColor="Blue" />
            <highlight-row condition="level == LogLevel.Info" foregroundColor="Green" />
            <highlight-row condition="level == LogLevel.Warn" foregroundColor="Yellow" />
            <highlight-row condition="level == LogLevel.Error" foregroundColor="Red" />
            <highlight-row condition="level == LogLevel.Fatal" foregroundColor="Magenta" />
        </target>
    </targets>
    <rules>
        <logger name="NoneNewLineLogger" levels="Trace,Debug,Info,Warn" writeTo="log-stderr-none-new-line" />
    </rules>
</nlog>
```

* https://github.com/nlog/NLog/wiki/File-target

　どうやらファイル用ログでないと改行なしにできないようだ……。

