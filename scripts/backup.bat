set D=%date%
set DA=%D:/=-%
mysqldump -u root --databases mediatek86 --single-transaction > "C:\savebdd\bddbackup_%DA%.sql"