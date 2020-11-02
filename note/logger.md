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

## ロガーの共通化

　例外表示とそれ以外のロガーを共通化できなかった。`<target>`の属性`layout`の値`${exception:format=tostring}`があるかないかだけの違い。

　というか、`Error`,`Fatal`などのレベルであれば例外表示するものだと思うのだが。それくらいイチイチ設定を作り込まずとも表示できるようにしてほしい。

```xml
    <targets>
        <target name="log-stderr" 
                xsi:type="ColoredConsole" 
                errorStream="true" 
                enableAnsiOutput="true" 
                layout="[${level:upperCase=true}] ${message}">
            <highlight-row condition="level == LogLevel.Trace" foregroundColor="Gray" />
            <highlight-row condition="level == LogLevel.Debug" foregroundColor="Blue" />
            <highlight-row condition="level == LogLevel.Info" foregroundColor="Green" />
            <highlight-row condition="level == LogLevel.Warn" foregroundColor="Yellow" />
            <highlight-row condition="level == LogLevel.Error" foregroundColor="Red" />
            <highlight-row condition="level == LogLevel.Fatal" foregroundColor="Magenta" />
        </target>
        <target name="log-stderr-exception" 
                xsi:type="ColoredConsole" 
                errorStream="true" 
                enableAnsiOutput="true" 
                layout="[${level:upperCase=true}] ${message} ${exception:format=tostring}">
            <highlight-row condition="level == LogLevel.Trace" foregroundColor="Gray" />
            <highlight-row condition="level == LogLevel.Debug" foregroundColor="Blue" />
            <highlight-row condition="level == LogLevel.Info" foregroundColor="Green" />
            <highlight-row condition="level == LogLevel.Warn" foregroundColor="Yellow" />
            <highlight-row condition="level == LogLevel.Error" foregroundColor="Red" />
            <highlight-row condition="level == LogLevel.Fatal" foregroundColor="Magenta" />
        </target>
    </targets>
    <rules>
        <logger name="AppDefaultLogger" levels="Trace,Debug,Info,Warn" writeTo="log-stderr" />
        <logger name="AppErrorLogger" levels="Error,Fatal" writeTo="log-stderr-exception" />
    </rules>
```

　呼出元でもロガー名を区別して取得・使用せねばならない。

```cs
Logger logger = NLog.LogManager.GetLogger("AppDefaultLogger");
Logger logger = NLog.LogManager.GetLogger("AppErrorLogger");
```

## 行数表示byリリースビルド

* https://github.com/NLog/NLog/wiki/Callsite-line-number-layout-renderer

　行番号を取得するにはビルドモードをreleaseでなくdebugで行わねばならない。さもなくば行数はすべて0になる。

```xml
${callsite-linenumber}
```
```xml
<target name="log-stderr" 
        xsi:type="ColoredConsole" 
        errorStream="true" 
        enableAnsiOutput="true" 
        layout="[${level:upperCase=true}] ${message} ${callsite-linenumber}">
```

## 色付き＆改行なし

* https://github.com/nlog/NLog/wiki/ColoredConsole-target

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

　仕方ないのでStringBuilderで1行分の文字列を組み立てるようプログラミングした。余計な手間が生じてしまった。だが、よく考えればログに`[DEBUG]`などのプレフィクスがついてしまうなら1行単位でしか出力すべきでない。おそらく最初はそういう想定だったのだろう。だが、プレフィクスが何もないレイアウトにもできる。だったら改行なし出力という要望もあると考えて然るべき。なのに無い。なぜファイル出力だけにはあるのか謎。絶対にとってつけただけだろ。

# バックアップ

　コメントアウトを含めたNLog.config。

```xml
<?xml version="1.0" encoding="utf-8" ?>
<nlog xmlns="http://www.nlog-project.org/schemas/NLog.xsd"
      xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance"
      throwConfigExceptions="true">

    <targets>
        <!--
        <target name="log-stderr" xsi:type="Console" error="true" />
        <target name="log-file" xsi:type="File" fileName="app.log" />
        <target name="log-stdout" xsi:type="Console" />
        -->
        <!-- https://github.com/nlog/NLog/wiki/ColoredConsole-target -->
        <!-- 行番号を取得するにはビルドモードをreleaseでなくdebugで行わねばならない。さもなくば行数はすべて0になる。 -->
        <!--   https://github.com/NLog/NLog/wiki/Callsite-line-number-layout-renderer -->
        <!--   ${callsite-linenumber} -->
        <target name="log-stderr" 
                xsi:type="ColoredConsole" 
                errorStream="true" 
                enableAnsiOutput="true" 
                layout="[${level:upperCase=true}] ${message}">
            <highlight-row condition="level == LogLevel.Trace" foregroundColor="Gray" />
            <highlight-row condition="level == LogLevel.Debug" foregroundColor="Blue" />
            <highlight-row condition="level == LogLevel.Info" foregroundColor="Green" />
            <highlight-row condition="level == LogLevel.Warn" foregroundColor="Yellow" />
            <highlight-row condition="level == LogLevel.Error" foregroundColor="Red" />
            <highlight-row condition="level == LogLevel.Fatal" foregroundColor="Magenta" />
        </target>
        <target name="log-stderr-exception" 
                xsi:type="ColoredConsole" 
                errorStream="true" 
                enableAnsiOutput="true" 
                layout="[${level:upperCase=true}] ${message} ${exception:format=tostring}">
            <highlight-row condition="level == LogLevel.Trace" foregroundColor="Gray" />
            <highlight-row condition="level == LogLevel.Debug" foregroundColor="Blue" />
            <highlight-row condition="level == LogLevel.Info" foregroundColor="Green" />
            <highlight-row condition="level == LogLevel.Warn" foregroundColor="Yellow" />
            <highlight-row condition="level == LogLevel.Error" foregroundColor="Red" />
            <highlight-row condition="level == LogLevel.Fatal" foregroundColor="Magenta" />
        </target>
        <!--
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
        -->
        <!--
        <target name="log-stderr" xsi:type="Console" error="true">
            <layout xsi:type="JsonLayout">
                <attribute name="Level" layout="${level}" />
                <attribute name="Timestamp" layout="${longdate}" />
                <attribute name="Message" layout="${message}" />
                <attribute name="Exception" layout="${exception}" />
            </layout>
        </target>
        -->
        <!--
        <target name="log-stderr" xsi:type="Console" error="true">
            <layout xsi:type="CsvLayout" delimiter="Space" withHeader="false">
                <column name="level" layout="[${level:upperCase=true}]"/>
                <column name="message" layout="${message}" />
            </layout>
        </target>
        -->
        <!-- https://nlog-project.org/config/?tab=layouts -->
        <!-- https://github.com/NLog/NLog/wiki/CsvLayout -->
        <!--
        <target name="log-stderr" xsi:type="Console" error="true">
            <layout xsi:type="CsvLayout" delimiter="Tab" withHeader="false">
                <column name="time" layout="${longdate}" />
                <column name="level" layout="${level:upperCase=true}"/>
                <column name="message" layout="${message}" />
                <column name="callsite" layout="${callsite:includeSourcePath=true}" />
                <column name="stacktrace" layout="${stacktrace:topFrames=10}" />
                <column name="exception" layout="${exception:format=ToString}"/>
                <column name="property1" layout="${event-properties:property1}"/>
            </layout>
        </target>
        -->
    </targets>
    <rules>
        <!--<logger name="AppDefaultLogger" minlevel="Debug" writeTo="log-stderr" />-->
        <!--
        <logger name="AppDefaultLogger" levels="Trace,Debug,Info,Warn" writeTo="log-stderr" />
        <logger name="AppDefaultLogger" levels="Error,Fatal" writeTo="log-stderr-exception" />
        <logger name="*" levels="Trace,Debug,Info,Warn" writeTo="log-stderr" />
        <logger name="*" levels="Error,Fatal" writeTo="log-stderr-exception" />
        -->
        <logger name="AppDefaultLogger" levels="Trace,Debug,Info,Warn" writeTo="log-stderr" />
        <logger name="AppErrorLogger" levels="Error,Fatal" writeTo="log-stderr-exception" />
<!--        <logger name="NoneNewLineLogger" levels="Trace,Debug,Info,Warn" writeTo="log-stderr-none-new-line" />-->
    </rules>
    <!--
    <rules>
        <logger name="*" levels="Info,Warn" writeTo="event"/>
        <logger name="*" levels="Error,Fatal" writeTo="error"/>
    </rules>
    -->
</nlog>
```

