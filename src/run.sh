#!/usr/bin/env bash
set -Ceu
#---------------------------------------------------------------------------
# TSVをHTMLのtableに変換する。
# CreatedAt: 2020-10-28
#---------------------------------------------------------------------------
Run() {
	THIS="$(realpath "${BASH_SOURCE:-0}")"; HERE="$(dirname "$THIS")"; PARENT="$(dirname "$HERE")"; THIS_NAME="$(basename "$THIS")"; APP_ROOT="$PARENT";
	cd "$HERE"
	. "$HERE/install.sh"
#	csc -nologo Program.cs Sub.cs
#	csc -nologo *.cs -out:prog.exe
	csc -nologo \
		-recurse:*.cs \
		-nullable:enable \
		-langversion:latest \
		-r:CommandLine.dll \
		-r:NLog.dll \
		-r:Microsoft.VisualBasic.dll
	chmod +x Program.exe
	mono Program.exe -l t "$APP_ROOT/res/tsv/matrix_3.tsv"
}
Run "$@"
