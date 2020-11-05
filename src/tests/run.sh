#!/usr/bin/env bash
set -Ceu
#---------------------------------------------------------------------------
# TSVをHTMLのtableに変換する。
# CreatedAt: 2020-10-28
#---------------------------------------------------------------------------
Run() {
	THIS="$(realpath "${BASH_SOURCE:-0}")"; HERE="$(dirname "$THIS")"; PARENT="$(dirname "$HERE")"; THIS_NAME="$(basename "$THIS")"; APP_ROOT="$(dirname "$PARENT")";
	cd "$HERE"
	. "$PARENT/install.sh"
	local OUT="$APP_ROOT/bin/tests.dll"
	csc -nologo \
		-target:library \
		-recurse:*.cs \
		-nullable:enable \
		-out:"$OUT" \
		-langversion:latest \
		-r:"$APP_ROOT/bin/CommandLine.dll" \
		-r:"$APP_ROOT/bin/NLog.dll" \
		-r:"$APP_ROOT/bin/nunit.framework.dll"
#	nunit-console "$OUT"
#	$APP_ROOT/bin/packages/NUnit.ConsoleRunner.3.11.1/tools/nunit3-console.exe
#	"$APP_ROOT/bin/packages/NUnit.ConsoleRunner.*/tools/nunit3-console.exe" "$OUT"
	"$APP_ROOT/bin/packages/NUnit.ConsoleRunner.3.11.1/tools/nunit3-console.exe" "$OUT"
}
Run "$@"
