Sub test()
    Dim logger As COMManagedLogger.IManagedLogger
    Set logger = New COMManagedLogger.ManagedLogger
    logger.Path = "C:\tmp\vbalogs.log"
    Call logger.Log("Hello from VBA")
End Sub
