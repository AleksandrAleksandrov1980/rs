$sript_name = "tst_smzu.py"
$results_name = "_calc_.html"
$results_subfolder = "!res"
$reference_results_subfolder = "res_24_09_2024__09_35_37.563_ref"
$windiff = "C:\Windiff\WinDiff.exe"
$python = "C:\Program Files (x86)\Python312-32\python.exe"
$lastresults = "lastresults.txt"

$this_path = Split-Path $MyInvocation.MyCommand.Path -Parent
$script_path = Join-Path -Path $this_path -ChildPath $sript_name
python $script_path #python after installation should be a system variable

$lastresults_path = Join-Path -Path $this_path -ChildPath $lastresults
$new_results_subfolder = Get-Content -Path $lastresults_path
Write-Host $new_results_subfolder 

$results_path = Join-Path -Path $($this_path | Split-Path) -ChildPath $results_subfolder
$reference_results_path = Join-Path -Path $results_path -ChildPath $reference_results_subfolder
$new_results_path = Join-Path -Path $results_path -ChildPath $new_results_subfolder
$reference_results = Join-Path -Path $reference_results_path -ChildPath $results_name
$new_results = Join-Path -Path $new_results_path -ChildPath $results_name

Start-Process -FilePath "$windiff" -ArgumentList "$reference_results $new_results" -Wait

Read-Host