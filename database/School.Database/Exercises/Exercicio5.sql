--5. LISTE OS PROFESSORES COM DATA DE ADMISS�O SUPERIOR AO ANO DE 1998

SELECT
	*
FROM
	[Teacher]
WHERE
	year(AdmitionDate) > '1998';