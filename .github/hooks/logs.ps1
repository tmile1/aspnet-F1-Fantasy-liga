$data = [Console]::In.ReadToEnd()

Add-Content -Path "C:\Users\Hp\Desktop\F1-Fantasy-liga\.github\hooks\agent_log.txt" -Value "`n--- LOG ENTRY ---`n"
Add-Content -Path "C:\Users\Hp\Desktop\F1-Fantasy-liga\.github\hooks\agent_log.txt" -Value $data