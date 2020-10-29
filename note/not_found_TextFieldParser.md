# MONOにはTextFieldParserがない

　TSVパーサに`TextFieldParser`を使おうとした。しかし、存在しない。

```sh
error CS0246: The type or namespace name 'TextFieldParser' could not be found (are you missing a using directive or an assembly reference?)
```

　`TextFieldParser`は`Microsoft.VisualBasic.dll`ファイルに存在する。このファイルがあればよい。だが、MONOにはないし、NuGet.exeで取得する方法も見つけられなかった。

* https://stackoverflow.com/questions/6644165/cant-get-microsoft-visualbasic-dll-for-mono-2-10

　以下のようにダウンロードして参照設定を追加してもダメ。存在しない。

```sh
mono nuget.exe install Microsoft.VisualBasic
cp ./Microsoft.VisualBasic.*/lib/netcore50/Microsoft.VisualBasic.dll "$HERE/Microsoft.VisualBasic.dll"
```
```sh
csc \
  -recurse:*.cs \
  -r:Microsoft.VisualBasic.dll
```

　以下のように`mono-vbnc`をインストールしてもダメ。

```sh
sudo apt-get install -y mono-vbnc
```

　もし使えるなら以下のように書く。

```cs
using Microsoft.VisualBasic;

TextFieldParser parser = new TextFieldParser(opt.File, System.IO.Text.Encoding.GetEncoding("UTF-8"));
parser.TextFieldType = Microsoft.VisualBasic.FileIO.FieldType.Delimited;
parser.SetDelimiters(opt.Delimiter);
while (false == parser.EndOfData) {
    opt.SourceList.append();
    string[] column = parser.ReadFields();
    for (int i = 0; i < column.Length; i++) {
        opt.SourceList[-1].append(new Cell { Text=column[i] });
    }
}
```

