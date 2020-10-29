#!/usr/bin/env bash
set -Ceu
#---------------------------------------------------------------------------
# TSVをHTMLのtableに変換する。
# CreatedAt: 2020-10-28
#---------------------------------------------------------------------------
Run() {
	THIS="$(realpath "${BASH_SOURCE:-0}")"; HERE="$(dirname "$THIS")"; PARENT="$(dirname "$HERE")"; THIS_NAME="$(basename "$THIS")"; APP_ROOT="$PARENT";
	cd "$HERE"
	IsExistCmd() { type "$1" > /dev/null 2>&1; }
	InstallMono() {
		IsExistCmd mono && return
		sudo apt install apt-transport-https dirmngr gnupg ca-certificates
		sudo apt-key adv --keyserver hkp://keyserver.ubuntu.com:80 --recv-keys 3FA7E0328081BFF6A14DA29AA6A19B38D3D831EF
		echo "deb https://download.mono-project.com/repo/debian stable-raspbianbuster main" | sudo tee /etc/apt/sources.list.d/mono-official-stable.list
		sudo apt update
		time sudo apt -y install mono-devel mono-complete mono-dbg referenceassemblies-pcl ca-certificates-mono  mono-xsp4 > /tmp/work/mono_install.log
		sudo apt full-upgrade -y
	}
	InstallNuGet() {
		IsExistCmd nuget.exe && return
		wget https://dist.nuget.org/win-x86-commandline/latest/nuget.exe
	}
	InstallCommandLineParser() {
		[ -f "$HERE/CommandLine.dll" ] && return
		mono $(which nuget.exe) install CommandLineParser
		cp ./CommandLineParser.*/lib/net461/CommandLine.dll "$HERE/CommandLine.dll"
	}
	InstallNLog() {
		[ -f "$HERE/NLog.dll" ] && return
		mono $(which nuget.exe) install NLog
		cp ./NLog.*/lib/net45/NLog.dll "$HERE/NLog.dll"
	}
	InstallMicrosoft_VisualBasic() {
		[ -f "$HERE/Microsoft.VisualBasic.dll" ] && return
		mono $(which nuget.exe) install Microsoft.VisualBasic
		cp ./Microsoft.VisualBasic.*/lib/netcore50/Microsoft.VisualBasic.dll "$HERE/Microsoft.VisualBasic.dll"
/tmp/work/CSharp.TsvToHtmlTable.20201028082641/src/packages/Microsoft.VisualBasic.10.3.0/lib/netcore50/Microsoft.VisualBasic.dll
	}
	AllInstall() {
		InstallMono
		InstallNuGet
		mkdir -p packages
		cd packages
		InstallNLog
		InstallCommandLineParser
		InstallMicrosoft_VisualBasic
		cd "$HERE"
		sleep 1
	}
	AllInstall
}
Run "$@"
