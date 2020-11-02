# Usecase-Pattern

* TSV
    * cat matrix_3.tsv
    * cat col_3.tsv
    * cat row_3.tsv
    * cat matrix_1.tsv
    * cat col_1.tsv
    * cat row_1.tsv
* CMD
    * g
    * i
    * n

```sh
cat matrix_3.tsv | tsv2table
cat matrix_3.tsv | tsv2table -r b
cat matrix_3.tsv | tsv2table -r B
cat matrix_3.tsv | tsv2table -c r
cat matrix_3.tsv | tsv2table -c B
cat matrix_3.tsv | tsv2table -r b -c r
cat matrix_3.tsv | tsv2table -r b -c B
cat matrix_3.tsv | tsv2table -r B -c r
cat matrix_3.tsv | tsv2table -r B -c B

cat col_3.tsv | tsv2table -H c
cat col_3.tsv | tsv2table -H c -c r
cat col_3.tsv | tsv2table -H c -c B

cat row_3.tsv | tsv2table
cat row_3.tsv | tsv2table -r b
cat row_3.tsv | tsv2table -r B

cat matrix_1.tsv | tsv2table
cat matrix_1.tsv | tsv2table -r b
cat matrix_1.tsv | tsv2table -r B
cat matrix_1.tsv | tsv2table -c r
cat matrix_1.tsv | tsv2table -c B
cat matrix_1.tsv | tsv2table -r b -c r
cat matrix_1.tsv | tsv2table -r b -c B
cat matrix_1.tsv | tsv2table -r B -c r
cat matrix_1.tsv | tsv2table -r B -c B

cat col_1.tsv | tsv2table -H c
cat col_1.tsv | tsv2table -H c -c r
cat col_1.tsv | tsv2table -H c -c B

cat row_1.tsv | tsv2table
cat row_1.tsv | tsv2table -r b
cat row_1.tsv | tsv2table -r B
```

