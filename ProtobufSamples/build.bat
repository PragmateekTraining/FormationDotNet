COPY LogMessage.proto LoggingServer
CD LoggingServer
C:\Users\mick\Documents\GitHub\FormationDotNet\Tools\protoc\protoc.exe LogMessage.proto --java_out=.
javac LoggingServer.java