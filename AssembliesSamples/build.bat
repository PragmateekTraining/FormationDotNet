CD Dependency
ECHO "Compiling Deep Thought..."
CSC /target:library DeepThought.cs
ECHO "Deep Thought compiled."

CD ..
ECHO "Compiling application..."
CSC /r:Dependency\DeepThought.dll Application.cs
ECHO "Application compiled"