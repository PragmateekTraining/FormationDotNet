sub log(message)
	WScript.echo message
	call logger.Log(message)
end sub

set logger = CreateObject("Production.Logger")
logger.Path = "C:\tmp\ProductionLogs.logs"
call log("Starting system...")
WScript.Sleep 2000
call log("Running system...")
WScript.Sleep 2000
call log("Stopping system...")
WScript.Sleep 2000
call log("System stopped.")