#!/usr/bin/env bash
set -Ceu
#---------------------------------------------------------------------------
# TSVをHTMLのtableに変換する。
# CreatedAt: 2020-10-28
#---------------------------------------------------------------------------
Run() {
	THIS="$(realpath "${BASH_SOURCE:-0}")"; HERE="$(dirname "$THIS")"; PARENT="$(dirname "$HERE")"; THIS_NAME="$(basename "$THIS")"; APP_ROOT="$PARENT";
	. "$HERE/install.sh"
	. "$HERE/run.sh"

	cd "$APP_ROOT"
	TSV="./res/tsv"
	CMD="./bin"
	echo "========== TEST =========="
	cat $TSV/matrix_3.tsv | $CMD/tsv2table --version
	cat $TSV/matrix_3.tsv | $CMD/tsv2table --help
	cat $TSV/matrix_3.tsv | $CMD/tsv2table g --help
	cat $TSV/matrix_3.tsv | $CMD/tsv2table i --help
	cat $TSV/matrix_3.tsv | $CMD/tsv2table n --help
	echo "----- group-header -----"
	set -x
	cat $TSV/matrix_3.tsv | $CMD/tsv2table
	cat $TSV/matrix_3.tsv | $CMD/tsv2table -r b
	cat $TSV/matrix_3.tsv | $CMD/tsv2table -r B
	cat $TSV/matrix_3.tsv | $CMD/tsv2table -c r
	cat $TSV/matrix_3.tsv | $CMD/tsv2table -c B
	cat $TSV/matrix_3.tsv | $CMD/tsv2table -r b -c r
	cat $TSV/matrix_3.tsv | $CMD/tsv2table -r b -c B
	cat $TSV/matrix_3.tsv | $CMD/tsv2table -r B -c r
	cat $TSV/matrix_3.tsv | $CMD/tsv2table -r B -c B
	cat $TSV/col_3.tsv | $CMD/tsv2table -H c
	cat $TSV/col_3.tsv | $CMD/tsv2table -H c -c r
	cat $TSV/col_3.tsv | $CMD/tsv2table -H c -c B
	cat $TSV/row_3.tsv | $CMD/tsv2table
	cat $TSV/row_3.tsv | $CMD/tsv2table -r b
	cat $TSV/row_3.tsv | $CMD/tsv2table -r B
	cat $TSV/matrix_1.tsv | $CMD/tsv2table
	cat $TSV/matrix_1.tsv | $CMD/tsv2table -r b
	cat $TSV/matrix_1.tsv | $CMD/tsv2table -r B
	cat $TSV/matrix_1.tsv | $CMD/tsv2table -c r
	cat $TSV/matrix_1.tsv | $CMD/tsv2table -c B
	cat $TSV/matrix_1.tsv | $CMD/tsv2table -r b -c r
	cat $TSV/matrix_1.tsv | $CMD/tsv2table -r b -c B
	cat $TSV/matrix_1.tsv | $CMD/tsv2table -r B -c r
	cat $TSV/matrix_1.tsv | $CMD/tsv2table -r B -c B
	cat $TSV/col_1.tsv | $CMD/tsv2table -H c
	cat $TSV/col_1.tsv | $CMD/tsv2table -H c -c r
	cat $TSV/col_1.tsv | $CMD/tsv2table -H c -c B
	cat $TSV/row_1.tsv | $CMD/tsv2table
	cat $TSV/row_1.tsv | $CMD/tsv2table -r b
	cat $TSV/row_1.tsv | $CMD/tsv2table -r B
	set +x
	echo "----- inner-header -----"
	cat $TSV/matrix_3.tsv | $CMD/tsv2table i
	cat $TSV/matrix_3.tsv | $CMD/tsv2table i -H r
	cat $TSV/matrix_3.tsv | $CMD/tsv2table i -s 2
	cat $TSV/matrix_3.tsv | $CMD/tsv2table i -S 2
	cat $TSV/matrix_3.tsv | $CMD/tsv2table i -s 2 -S 3
	echo "----- none-header -----"
	cat $TSV/matrix_3.tsv | $CMD/tsv2table n
}
Run "$@"
